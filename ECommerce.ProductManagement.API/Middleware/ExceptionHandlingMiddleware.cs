using System;
using System.Net;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ECommerce.ProductManagement.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _log;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // If downstream set a non-success status without a body, write a simple JSON body
                if (context.Response.StatusCode >= 400 && context.Response.ContentLength == null && !context.Response.HasStarted)
                {
                    var simple = CreateErrorResponse(context.Response.StatusCode, ReasonPhrase(context.Response.StatusCode), Activity.Current?.Id);
                    context.Response.ContentType = "application/json";
                    var json = JsonSerializer.Serialize(simple);
                    await context.Response.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static string ReasonPhrase(int status) =>
            status switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                422 => "Unprocessable Entity",
                _ => "Internal Server Error"
            };

        private static object CreateErrorResponse(int statusCode, string message, string? traceId) =>
            new
            {
                status = statusCode,
                error = message,
                message = message,
                traceId
            };

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var status = (int)HttpStatusCode.InternalServerError;
            var response = CreateErrorResponse(status, "An unexpected error occurred.", Activity.Current?.Id);
            var payload = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = status;
            return context.Response.WriteAsync(payload);
        }
    }
}
