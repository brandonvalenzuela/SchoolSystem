namespace SchoolSystem.Web.Models
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T? Data { get; set; }

        /// <summary>
        /// Código de estado HTTP de la respuesta (ej: 200, 409, 400, 500)
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// CorrelationId para rastrear solicitudes en logs distribuidos
        /// </summary>
        public string? CorrelationId { get; set; }
    }
}
