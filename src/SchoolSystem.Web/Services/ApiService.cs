using Blazored.LocalStorage;
using SchoolSystem.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SchoolSystem.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        // Opciones para que no importe si el JSON viene en Mayúsculas o minúsculas
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        // Método para adjuntar el token a cada petición
        private async Task SetTokenAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // --- MÉTODO HELPER PARA PROCESAR RESPUESTAS ---
        private async Task<ApiResponse<T>> ProcessResponseAsync<T>(HttpResponseMessage response)
        {
            // Intentamos leer el JSON independientemente del Status Code
            try
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(_jsonOptions);
                return result ?? new ApiResponse<T> { Succeeded = false, Message = "Respuesta vacía del servidor." };
            }
            catch (Exception)
            {
                // Si falla al leer el JSON (ej: error 500 de IIS o proxy sin cuerpo JSON)
                return new ApiResponse<T>
                {
                    Succeeded = false,
                    Message = $"Error del servidor: {response.StatusCode}"
                };
            }
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            await SetTokenAsync();
            var response = await _httpClient.GetAsync(endpoint);

            // Manejo básico de errores no controlados
            if (!response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new ApiResponse<T> { Succeeded = false, Message = "No autorizado" };
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return result ?? new ApiResponse<T> { Succeeded = false, Message = "Respuesta vacía del servidor." };
        }

        // Método específico para Paginación
        public async Task<ApiResponse<PagedResult<T>>> GetPagedAsync<T>(string endpoint)
        {
            await SetTokenAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<T>>>(endpoint);
            return result ?? new ApiResponse<PagedResult<T>> { Succeeded = false, Message = "Respuesta vacía del servidor." };
        }

        public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            await SetTokenAsync();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
            return result ?? new ApiResponse<TResponse> { Succeeded = false, Message = "Respuesta vacía del servidor." };
        }

        public async Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            await SetTokenAsync();
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
            return result ?? new ApiResponse<TResponse> { Succeeded = false, Message = "Respuesta vacía del servidor." };
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
        {
            await SetTokenAsync();
            var response = await _httpClient.DeleteAsync(endpoint);

            // Aquí está la clave: ReadFromJsonAsync ahora usa <ApiResponse<T>>
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return result ?? new ApiResponse<T> { Succeeded = false, Message = "Respuesta vacía del servidor." };
        }
    }
}
