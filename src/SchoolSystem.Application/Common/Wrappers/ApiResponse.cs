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

        public ApiResponse()
        {
        }

        // Constructor para respuestas exitosas
        public ApiResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Errors = null;
        }

        // Constructor para respuestas fallidas
        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
            Errors = new List<string> { message };
        }

        // Constructor para múltiples errores
        public ApiResponse(IEnumerable<string> errors, string message = "Ocurrieron errores")
        {
            Succeeded = false;
            Message = message;
            Errors = new List<string>(errors);
        }
    }
}
