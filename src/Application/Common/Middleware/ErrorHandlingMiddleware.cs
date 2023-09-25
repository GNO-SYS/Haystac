using Microsoft.AspNetCore.Http;

namespace Haystac.Application.Common.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception err)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = GetStatusCode(err);

            var errorMessage = GetErrorMessageJson(err);           
            var result = JsonSerializer.Serialize(new { message = errorMessage });
            await response.WriteAsync(result);
        }
    }

    static int GetStatusCode(Exception e)
    {
        return e switch
        {
            AppException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            ForbiddenAccessException => (int)HttpStatusCode.Forbidden,
            ValidationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError,
        };
    }

    static string GetErrorMessageJson(Exception e)
    {
        return e switch
        {
            AppException 
            or NotFoundException 
            or ForbiddenAccessException 
            or ValidationException => $"{e.Message}",
            _ => "Internal server error",
        };
    }
}