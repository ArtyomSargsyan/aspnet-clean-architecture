using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Exceptions;

namespace ToDoApi.Extensions.Middleware;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, type) = Classify(exception);

        Log(exception, statusCode, title, httpContext);

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext    = httpContext,
            Exception      = exception,
            ProblemDetails = Build(exception, statusCode, title, type, httpContext)
        });
    }

    // Maps exception types → (status, title, RFC type URI).
    // 4xx = client errors, 5xx = server errors.
    private static (int StatusCode, string Title, string Type) Classify(Exception exception) =>
        exception switch
        {
            NotFoundException           => (StatusCodes.Status404NotFound,
                                           "Resource not found",
                                           "https://tools.ietf.org/html/rfc9110#section-15.5.5"),

            ConflictException           => (StatusCodes.Status409Conflict,
                                           "Conflict",
                                           "https://tools.ietf.org/html/rfc9110#section-15.5.10"),

            ForbiddenException          => (StatusCodes.Status403Forbidden,
                                           "Forbidden",
                                           "https://tools.ietf.org/html/rfc9110#section-15.5.4"),

            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,
                                           "Unauthorized",
                                           "https://tools.ietf.org/html/rfc9110#section-15.5.2"),

            ValidationException         => (StatusCodes.Status422UnprocessableEntity,
                                           "Validation failed",
                                           "https://tools.ietf.org/html/rfc4918#section-11.2"),

            OperationCanceledException  => (499,
                                           "Request was cancelled",
                                           "about:blank"),

            _                           => (StatusCodes.Status500InternalServerError,
                                           "An unexpected error occurred",
                                           "https://tools.ietf.org/html/rfc9110#section-15.6.1")
        };

    // 5xx → Error  |  4xx → Warning  |  cancelled → Information
    private void Log(Exception exception, int statusCode, string title, HttpContext ctx)
    {
        if (exception is OperationCanceledException)
        {
            logger.LogInformation(
                "Request cancelled — {Method} {Path}",
                ctx.Request.Method, ctx.Request.Path);
            return;
        }

        if (statusCode >= StatusCodes.Status500InternalServerError)
            logger.LogError(exception,
                "[{StatusCode}] {Title} — {Method} {Path}",
                statusCode, title, ctx.Request.Method, ctx.Request.Path);
        else
            logger.LogWarning(exception,
                "[{StatusCode}] {Title} — {Method} {Path}",
                statusCode, title, ctx.Request.Method, ctx.Request.Path);
    }

    private ProblemDetails Build(
        Exception exception,
        int statusCode,
        string title,
        string type,
        HttpContext ctx)
    {
        var problem = new ProblemDetails
        {
            Status   = statusCode,
            Title    = title,
            Type     = type,
            Instance = ctx.Request.Path
        };

        switch (exception)
        {
            case ValidationException valEx:
                problem.Detail = "One or more validation errors occurred.";
                problem.Extensions["errors"] = valEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());
                break;

            case AppException appEx:
                // Domain exceptions always carry a safe, user-facing message.
                problem.Detail = appEx.Message;
                break;

            default:
                // Only expose raw exception details in Development to avoid info leakage.
                if (environment.IsDevelopment())
                    problem.Detail = exception.Message;
                break;
        }

        return problem;
    }
}
