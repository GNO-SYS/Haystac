using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var result = JsonSerializer.Serialize(new { message = err?.Message });
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
}