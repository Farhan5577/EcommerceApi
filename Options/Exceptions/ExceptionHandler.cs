using EcommerceApi.Options.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EcommerceApi.Options.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, title) = exception switch
            {
                BadRequestException     => (StatusCodes.Status400BadRequest, "Invalid Request"),
                UnauthorizedException   => (StatusCodes.Status401Unauthorized, "Acces Unauthorized"),
                ForbiddenException      => (StatusCodes.Status403Forbidden, "Restricted Acess"),
                NotFoundException       => (StatusCodes.Status404NotFound, "Thing Not Found"),
                ConflictException       => (StatusCodes.Status409Conflict, "Conflict Occur"),
                _                       => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };
            httpContext.Response.StatusCode = statusCode;
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
