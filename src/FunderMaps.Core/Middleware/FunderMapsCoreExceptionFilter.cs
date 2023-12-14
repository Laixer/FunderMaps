using FunderMaps.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace FunderMaps.Core.Middleware;

/// <summary>
///     Filter <see cref="FunderMapsCoreException"/> and convert into a <see cref="ProblemDetails"/> response.
/// </summary>
public class FunderMapsCoreExceptionFilter(ProblemDetailsFactory problemDetailsFactory) : IExceptionFilter
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
        { typeof(DatabaseException), HttpStatusCode.InternalServerError },
        { typeof(QueueOverflowException), HttpStatusCode.InternalServerError },
        { typeof(ReferenceNotFoundException), HttpStatusCode.NotFound },
        { typeof(ServiceUnavailableException), HttpStatusCode.ServiceUnavailable },
        { typeof(StateTransitionException), HttpStatusCode.Forbidden },
        { typeof(StorageException), HttpStatusCode.InternalServerError },
        { typeof(UnhandledTaskException), HttpStatusCode.InternalServerError },
    };

    /// <summary>
    ///     Called after an action has thrown an <see cref="Exception"/>.
    /// </summary>
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is FunderMapsCoreException exception)
        {
            if (exceptionMap.TryGetValue(exception.GetType(), out HttpStatusCode statusCode))
            {
                ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
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
