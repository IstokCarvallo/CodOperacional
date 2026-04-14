using System.Net;
using System.Text.Json;

namespace APISegura.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;

                _logger.LogError(ex,
                    "Unhandled exception {TraceId} at {Path}",
                    traceId,
                    context.Request.Path);

                await HandleExceptionAsync(context, ex, traceId);
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context,
            Exception ex,
            string traceId)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Error interno del servidor";

            // 🔧 Mapeo básico (puedes extenderlo después)
            if (ex is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
            }

            var response = new
            {
                success = false,
                message,
                traceId
            };

            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
