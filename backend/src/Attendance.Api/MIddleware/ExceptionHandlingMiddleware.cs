using System.Text.Json;
using FluentValidation;

namespace Attendance.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log using ASP.NET logging
            _logger.LogError(ex, ex.Message);

            // TEMPORARY: Print full exception to console for debugging
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("============= EXCEPTION =============");
            Console.WriteLine(ex);
            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.ResetColor();

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        object response;

        switch (exception)
        {
            case ValidationException validationException:

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                response = new
                {
                    StatusCode = 400,
                    Message = validationException.Message,
                    Errors = validationException.Errors.Any()
                        ? validationException.Errors.Select(error => new
                        {
                            error.PropertyName,
                            error.ErrorMessage
                        })
                        : null
                };

                break;

            case KeyNotFoundException:

                context.Response.StatusCode = StatusCodes.Status404NotFound;

                response = new
                {
                    StatusCode = 404,
                    Message = exception.Message
                };

                break;

            case UnauthorizedAccessException:

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                response = new
                {
                    StatusCode = 401,
                    Message = exception.Message
                };

                break;

            default:

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                response = new
                {
                    StatusCode = 500,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace
                };

                break;
        }

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
