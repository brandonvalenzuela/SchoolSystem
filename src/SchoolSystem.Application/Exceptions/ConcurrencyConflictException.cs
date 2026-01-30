namespace SchoolSystem.Application.Exceptions
{
    /// <summary>
    /// Excepción de negocio lanzada cuando ocurre un conflicto de concurrencia.
    /// Típicamente cuando múltiples usuarios intentan capturar calificaciones simultáneamente
    /// en el mismo grupo/materia/periodo.
    /// </summary>
    public class ConcurrencyConflictException : Exception
    {
        /// <summary>
        /// CorrelationId para rastrear la solicitud en logs distribuidos.
        /// </summary>
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Información adicional del contexto (EscuelaId, GrupoId, etc).
        /// </summary>
        public Dictionary<string, object>? ContextData { get; set; }

        public ConcurrencyConflictException()
            : base("Se detectó una captura simultánea. Recarga y vuelve a intentar.")
        {
        }

        public ConcurrencyConflictException(string message)
            : base(message)
        {
        }

        public ConcurrencyConflictException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor con CorrelationId y contexto
        /// </summary>
        public ConcurrencyConflictException(
            string message,
            string? correlationId = null,
            Dictionary<string, object>? contextData = null)
            : base(message)
        {
            CorrelationId = correlationId;
            ContextData = contextData;
        }
    }
}
