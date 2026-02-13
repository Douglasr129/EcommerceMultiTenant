using Identity.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Identity.Api.Middlewares
{
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 por padrão

            // Mapeia suas exceções para Status Codes HTTP
            code = exception switch
            {
                ArgumentNullException => HttpStatusCode.BadRequest, // 400
                ArgumentException => HttpStatusCode.BadRequest,     // 400
                UnauthorizedAccessException => HttpStatusCode.Forbidden, // 403
                NotFoundException => HttpStatusCode.NotFound,       // 404
                ConflictException => HttpStatusCode.Conflict,          // 409
                _ => HttpStatusCode.InternalServerError             // 500
            };

            var result = JsonSerializer.Serialize(new
            {
                error = exception.Message,
                status = (int)code
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
