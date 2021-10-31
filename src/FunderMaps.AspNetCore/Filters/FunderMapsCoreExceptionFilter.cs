using FunderMaps.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace FunderMaps.AspNetCore.Middleware
{
    /// <summary>
    ///     Filter <see cref="FunderMapsCoreException"/> and convert into a <see cref="ProblemDetails"/> response.
    /// </summary>
    public class FunderMapsCoreExceptionFilter : IExceptionFilter
    {
        private static readonly Dictionary<Type, HttpStatusCode> exceptionMap = new()
        {
            { typeof(AuthenticationException), HttpStatusCode.Unauthorized },
            { typeof(AuthorizationException), HttpStatusCode.Forbidden },
            { typeof(EntityNotFoundException), HttpStatusCode.NotFound },
            { typeof(EntityReadOnlyException), HttpStatusCode.Locked },
            { typeof(InvalidCredentialException), HttpStatusCode.Forbidden },
            { typeof(InvalidIdentifierException), HttpStatusCode.BadRequest },
            { typeof(OperationAbortedException), HttpStatusCode.BadRequest },
            { typeof(ProcessException), HttpStatusCode.InternalServerError },
            { typeof(QueueOverflowException), HttpStatusCode.InternalServerError },
            { typeof(ReferenceNotFoundException), HttpStatusCode.NotFound },
            { typeof(ServiceUnavailableException), HttpStatusCode.ServiceUnavailable },
            { typeof(StateTransitionException), HttpStatusCode.Forbidden },
            { typeof(StorageException), HttpStatusCode.InternalServerError },
            { typeof(UnhandledTaskException), HttpStatusCode.InternalServerError },
        };

        private readonly ProblemDetailsFactory _problemDetailsFactory;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FunderMapsCoreExceptionFilter(ProblemDetailsFactory problemDetailsFactory)
            => _problemDetailsFactory = problemDetailsFactory;

        /// <summary>
        ///     Called after an action has thrown an <see cref="Exception"/>.
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is FunderMapsCoreException exception)
            {
                if (exceptionMap.TryGetValue(exception.GetType(), out HttpStatusCode statusCode))
                {
                    ProblemDetails problemDetails = _problemDetailsFactory.CreateProblemDetails(
                        context.HttpContext,
                        statusCode: (int)statusCode,
                        title: exception.Title);

                    context.Result = new ObjectResult(problemDetails)
                    {
                        StatusCode = problemDetails.Status,
                    };
                    context.ExceptionHandled = true;
                }
            }
        }
    }
}
