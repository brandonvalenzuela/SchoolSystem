using SchoolSystem.Application.Common.Wrappers;
using System.Net;
using System.Text.Json;

namespace SchoolSystem.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ApiResponse<string>() { Succeeded = false, Message = error.Message };

                switch (error)
                {
                    case KeyNotFoundException e:
                        // No encontrado (404)
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case ArgumentException e:
                        // Bad Request (400) - Validaciones de negocio
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case UnauthorizedAccessException e:
                        // No autorizado (401)
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    case FluentValidation.ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        var errors = e.Errors.Select(x => x.ErrorMessage).ToList();
                        responseModel = new ApiResponse<string>(errors);
                        break;

                    default:
                        // Error interno (500)
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = "Ocurrió un error interno en el servidor.";
                        // En desarrollo, podrías querer ver el error real:
                        // responseModel.Errors = new List<string> { error.Message };
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
