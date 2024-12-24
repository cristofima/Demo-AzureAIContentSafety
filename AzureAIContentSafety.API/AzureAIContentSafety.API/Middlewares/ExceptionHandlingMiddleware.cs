using AzureAIContentSafety.API.Exceptions;
using AzureAIContentSafety.API.Utils;

namespace AzureAIContentSafety.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, 404, ex.Message);
        }
        catch (HarmfulContentException ex)
        {
            await HandleExceptionAsync(context, 400, ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, 500, "An unexpected error occurred.");
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        int statusCode,
        string message,
        Dictionary<string, string[]> errors = null
    )
    {
        var response = ErrorResponseUtil.CreateErrorResponse(context, statusCode, message, errors);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
