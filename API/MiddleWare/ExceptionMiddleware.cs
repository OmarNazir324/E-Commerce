using Application.Exceptions;
using Application.Responses;
using System.Net;
using System.Text.Json;

namespace API.MiddleWare;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, ex, _env);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        IHostEnvironment env)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

        var response = new ApiResponse<string>()
        {
            Success = false,
            Message = "An error occurred",
            Errors = new List<string>(),
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        switch (exception)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                response.Message = exception.Message;
                response.StatusCode = (int)statusCode;
                break;

            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                response.Message = exception.Message;
                response.StatusCode = (int)statusCode;
                break;

            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                response.Message = exception.Message;
                response.StatusCode = (int)statusCode;
                break;

            default:
                response.Message = env.IsDevelopment()
                    ? exception.Message
                    : "Internal Server Error";

                response.Errors = env.IsDevelopment()
                    ? new List<string> { exception.StackTrace ?? "" }
                    : null;

                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}
