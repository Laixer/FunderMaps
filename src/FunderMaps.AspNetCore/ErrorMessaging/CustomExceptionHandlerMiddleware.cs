using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace FunderMaps.AspNetCore.ErrorMessaging
{
    /// <summary>
    ///     Custom exception handler which only catches exceptions that 
    ///     are of type of inherit from <typeparamref name="TException"/>.
    /// </summary>
    /// <remarks>
    ///     This redirects to <see cref="CustomExceptionHandlerOptions.ErrorControllerPath"/>
    ///     for error handling.
    /// </remarks>
    /// <typeparam name="TException"><see cref="Exception"/> base</typeparam>
    public class CustomExceptionHandlerMiddleware<TException>
        where TException : Exception
    {
        private readonly RequestDelegate _next;
        private readonly CustomExceptionHandlerOptions _options;
        private readonly ILogger<CustomExceptionHandlerMiddleware<TException>> _logger;
        private readonly IExceptionMapper<TException> _mapper;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public CustomExceptionHandlerMiddleware(RequestDelegate next,
            IOptions<CustomExceptionHandlerOptions> options,
            ILoggerFactory loggerFactory,
            IExceptionMapper<TException> mapper)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = loggerFactory?.CreateLogger<CustomExceptionHandlerMiddleware<TException>>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Invoke this middleware.
        /// </summary>
        /// <remarks>
        ///     This functions performs no async work itself.
        /// </remarks>
        /// <param name="context"><see cref="HttpContext"/></param>
        /// <returns><see cref="Task"/></returns>
        public virtual Task InvokeAsync(HttpContext context)
        {
            ExceptionDispatchInfo edi;

            try
            {
                var task = _next(context);
                if (task.IsCompletedSuccessfully)
                {
                    return Task.CompletedTask;
                }
                else if (task.IsCanceled)
                {
                    edi = ExceptionDispatchInfo.Capture(new OperationAbortedException());
                }
                else if (task.IsFaulted)
                {
                    edi = ExceptionDispatchInfo.Capture(task.Exception);
                }
                else
                {
                    return ProcessAsync(context, task);
                }
            }
            catch (TException e)
            {
                edi = ExceptionDispatchInfo.Capture(e);
            }

            // Perform execution outside of try/catch block.
            return HandleExceptionAsync(edi, context);
        }

        /// <summary>
        ///     Process a task asynchronously.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/></param>
        /// <param name="task"><see cref="Task"/></param>
        private async Task ProcessAsync(HttpContext context, Task task)
        {
            ExceptionDispatchInfo edi;

            try
            {
                await task;
                return;
            }
            catch (TException e)
            {
                edi = ExceptionDispatchInfo.Capture(e);
            }

            // Perform execution outside of try/catch block.
            await HandleExceptionAsync(edi, context);
        }

        /// <summary>
        ///     Handle the thrown exception by appending a
        ///     <see cref="ProblemDetails"/> to the <see cref="HttpContext.Response"/>.
        /// </summary>
        /// <param name="edi"><see cref="ExceptionDispatchInfo"/></param>
        /// <param name="context"><see cref="HttpContext"/></param>
        protected virtual async Task HandleExceptionAsync(ExceptionDispatchInfo edi, HttpContext context)
        {
            // We're only interested in the root cause of the exception, so if we're dealing with an
            // aggregate then unpack the exception stack and replace the exception dispatcher with the
            // exception base.
            if (edi.SourceException is AggregateException)
            {
                edi = ExceptionDispatchInfo.Capture((edi.SourceException as AggregateException).GetBaseException());
            }

            _logger.LogDebug($"The exception message: {edi.SourceException.Message}");

            // If we conclude the exception as not one of TException then abort and
            // throw the exception upwards.
            if (!(edi.SourceException is TException))
            {
                edi.Throw();
            }

            // We can't do anything if the response has already started, just abort.
            // This means headers have already been sent to the client.
            if (context.Response.HasStarted)
            {
                _logger.LogError(edi.SourceException, "Could not do anything, response has already started");

                edi.Throw();
            }

            // Save original context path for later restoration in case we can't handle the exception.
            var originalPath = context.Request.Path;

            try
            {
                ClearHttpContext(context);

                context.Features.Set(_mapper.Map(edi.SourceException as TException));

                // Pass back up the chain to reach the error handling controller.
                context.Request.Path = _options.ErrorControllerPath ?? throw new InvalidOperationException("No error handler path specified");
                await _next(context);

                return;
            }
            catch (Exception e)
            {
                // Only log, we re-throw the original exception after this.
                _logger.LogError(e, "Could not handle exception, re-throwing original");
            }
            finally
            {
                // Set the context path to what it was.
                context.Request.Path = originalPath;
            }

            // We couldn't handle the exception so re-throw.
            edi.Throw();
        }

        /// <summary>
        ///     Prepares the <paramref name="context"/> for re-usage.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/></param>
        private void ClearHttpContext(HttpContext context)
        {
            _logger.LogDebug("Clear response");

            // Clear any response properties which have been set.
            context.Response.Clear();

            // An endpoint may have already been set. Since we're going to re-
            // invoke the middleware pipeline we need to reset the endpoint and
            // route values to ensure things are re-calculated.
            context.SetEndpoint(endpoint: null);
            context.Features.Get<IRouteValuesFeature>()?.RouteValues?.Clear();
        }
    }
}
