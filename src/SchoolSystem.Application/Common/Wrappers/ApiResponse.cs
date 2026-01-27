using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Common.Wrappers
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }

        /// <summary>
        /// ID de correlación para trazar la solicitud en logs.
        /// Útil para debugging y soporte.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Código HTTP de estado. Opcional, se usa principalmente para errores (409, 400, 500, etc.)
        /// Útil en el frontend para tomar decisiones sobre cómo manejar el error.
        /// </summary>
        public int? StatusCode { get; set; }

        public ApiResponse()
        {
            CorrelationId = Guid.NewGuid().ToString();
        }

        // Constructor para respuestas exitosas
        public ApiResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Errors = null;
            CorrelationId = Guid.NewGuid().ToString();
        }

        // Constructor para respuestas fallidas
        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
            Errors = new List<string> { message };
            CorrelationId = Guid.NewGuid().ToString();
        }

        // Constructor para múltiples errores
        public ApiResponse(IEnumerable<string> errors, string message = "Ocurrieron errores")
        {
            Succeeded = false;
            Message = message;
            Errors = new List<string>(errors);
            CorrelationId = Guid.NewGuid().ToString();
        }

        // Constructor para errores de validación
        public ApiResponse(List<string> errors)
        {
            Succeeded = false;
            Message = "Se encontraron errores de validación";
            Errors = errors;
            Data = default;
            CorrelationId = Guid.NewGuid().ToString();
        }
    }
}
