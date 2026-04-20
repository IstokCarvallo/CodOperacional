using Microsoft.Data.SqlClient;

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
            var traceId = context.TraceIdentifier;
            var path = context.Request.Path;

            try
            {
                await _next(context);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL ERROR TraceId: {TraceId} Path: {Path}", traceId, path);

                await HandleException(context, traceId, "Error de base de datos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UNHANDLED ERROR TraceId: {TraceId}", traceId);

                await HandleException(context, traceId, "Error interno del servidor");
            }
        }

        private static async Task HandleException(HttpContext context, string traceId, string message)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message,
                traceId
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
