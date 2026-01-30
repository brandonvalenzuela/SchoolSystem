using Microsoft.Extensions.Logging;

namespace SchoolSystem.API.Middleware
{
    /// <summary>
    /// Middleware para propagar CorrelationId en toda la solicitud.
    /// Extrae o genera un CorrelationId único para vincular logs distribuidos.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        private const string CorrelationIdHeader = "X-Correlation-Id";
        private const string CorrelationIdKey = "CorrelationId";

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Procesa la solicitud, extrae o genera CorrelationId y lo guarda en HttpContext.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            // Intentar obtener CorrelationId del header
            var correlationId = ExtractOrGenerateCorrelationId(context);

            // Guardar en HttpContext.Items para acceso en controllers/services
            context.Items[CorrelationIdKey] = correlationId;

            // Agregar a response headers
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeader] = correlationId;
                return Task.CompletedTask;
            });

            // Usar logging scope para que todos los logs incluyan el CorrelationId
            using (_logger.BeginScope(new Dictionary<string, object> { [CorrelationIdKey] = correlationId }))
            {
                await _next(context);
            }
        }

        /// <summary>
        /// Extrae CorrelationId del header o genera uno nuevo.
        /// </summary>
        private string ExtractOrGenerateCorrelationId(HttpContext context)
        {
            const string headerKey = CorrelationIdHeader;

            if (context.Request.Headers.TryGetValue(headerKey, out var correlationId) && 
                !string.IsNullOrWhiteSpace(correlationId))
            {
                return correlationId.ToString();
            }

            // Generar nuevo CorrelationId
            var newCorrelationId = Guid.NewGuid().ToString("N");
            _logger.LogDebug("CorrelationId generado: {CorrelationId}", newCorrelationId);
            return newCorrelationId;
        }
    }
}
