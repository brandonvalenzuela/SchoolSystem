using Blazored.LocalStorage;
using SchoolSystem.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SchoolSystem.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

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

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            await SetTokenAsync();
            var response = await _httpClient.GetAsync(endpoint);

            // Manejo básico de errores no controlados
            if (!response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new ApiResponse<T> { Succeeded = false, Message = "No autorizado" };
            }

            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        }

        // Método específico para Paginación
        public async Task<ApiResponse<PagedResult<T>>> GetPagedAsync<T>(string endpoint)
        {
            await SetTokenAsync();
            return await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<T>>>(endpoint);
        }

        public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            await SetTokenAsync();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
        }

        public async Task<ApiResponse<bool>> PutAsync<TRequest>(string endpoint, TRequest data)
        {
            await SetTokenAsync();
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string endpoint)
        {
            await SetTokenAsync();
            var response = await _httpClient.DeleteAsync(endpoint);
            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        }
    }
}
