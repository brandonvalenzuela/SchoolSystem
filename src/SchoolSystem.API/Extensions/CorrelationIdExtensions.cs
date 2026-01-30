namespace SchoolSystem.API.Extensions
{
    /// <summary>
    /// Extensiones para trabajar con CorrelationId en HttpContext.
    /// </summary>
    public static class CorrelationIdExtensions
    {
        private const string CorrelationIdKey = "CorrelationId";

        /// <summary>
        /// Obtiene el CorrelationId del contexto HTTP actual.
        /// </summary>
        public static string GetCorrelationId(this HttpContext context)
        {
            if (context?.Items.TryGetValue(CorrelationIdKey, out var correlationId) == true)
            {
                return correlationId?.ToString() ?? Guid.NewGuid().ToString("N");
            }

            return Guid.NewGuid().ToString("N");
        }
    }
}
