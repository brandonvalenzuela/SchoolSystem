using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SchoolSystem.Web.Auth;
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
        private readonly NavigationManager _navigation;
        private readonly AuthenticationStateProvider _authProvider;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiService(HttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigation, AuthenticationStateProvider authProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _navigation = navigation;
            _authProvider = authProvider;
        }

        private async Task HandleUnauthorized()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authProvider).NotifyUserLogout();

            // Redirigir al login guardando la URL actual por si acaso
            // (Aunque el componente RedirectToLogin lo hace, es bueno tener el fallback aquí)
            _navigation.NavigateTo("login");
        }

        private async Task SetTokenAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // --- GET ---
        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            await SetTokenAsync();
            var response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await HandleUnauthorized();
                return new ApiResponse<T> { Succeeded = false, Message = "Sesión expirada" };
            }

            return await ProcessResponseAsync<ApiResponse<T>>(response);
        }

        // --- GET PAGINADO (Corregido para no usar GetFromJsonAsync directo) ---
        public async Task<ApiResponse<PagedResult<T>>> GetPagedAsync<T>(string endpoint)
        {
            await SetTokenAsync();

            // Usamos GetAsync para poder inspeccionar el código de estado antes de deserializar
            var response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await HandleUnauthorized();
                return new ApiResponse<PagedResult<T>> { Succeeded = false, Message = "Sesión expirada" };
            }

            return await ProcessResponseAsync<ApiResponse<PagedResult<T>>>(response);
        }

        // --- POST ---
        public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            await SetTokenAsync();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await HandleUnauthorized();
                return new ApiResponse<TResponse> { Succeeded = false, Message = "Sesión expirada" };
            }

            return await ProcessResponseAsync<ApiResponse<TResponse>>(response);
        }

        // --- PUT ---
        public async Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            await SetTokenAsync();
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await HandleUnauthorized();
                return new ApiResponse<TResponse> { Succeeded = false, Message = "Sesión expirada" };
            }

            return await ProcessResponseAsync<ApiResponse<TResponse>>(response);
        }

        // --- DELETE ---
        public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
        {
            await SetTokenAsync();
            var response = await _httpClient.DeleteAsync(endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await HandleUnauthorized();
                return new ApiResponse<T> { Succeeded = false, Message = "Sesión expirada" };
            }

            return await ProcessResponseAsync<ApiResponse<T>>(response);
        }

        // --- HELPER PARA DESERIALIZAR ---
        private async Task<T> ProcessResponseAsync<T>(HttpResponseMessage response)
        {
            // 1. Leer el contenido como string primero (Evita errores de stream)
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return Activator.CreateInstance<T>();
                }

                // 2. Deserializar usando las opciones de minúsculas/mayúsculas
                var result = JsonSerializer.Deserialize<T>(content, _jsonOptions);
                return result;
            }
            catch (Exception ex)
            {
                // Si falla la deserialización (ej: Data es null pero T es int)
                // Intentamos al menos devolver un objeto con el mensaje de error si existe
                Console.WriteLine($"Error de Deserialización: {ex.Message}");

                var errorFallback = Activator.CreateInstance<T>();

                // Intentamos extraer el mensaje manualmente del string si es posible
                if (content.Contains("\"Message\":"))
                {
                    // Un truco rápido para extraer el mensaje sin fallar por el tipo de Data
                    using var doc = JsonDocument.Parse(content);
                    if (doc.RootElement.TryGetProperty("Message", out var msgProp))
                    {
                        var message = msgProp.GetString();
                        // Asignamos el mensaje al objeto creado vía reflexión
                        typeof(T).GetProperty("Message")?.SetValue(errorFallback, message);
                    }
                }

                return errorFallback;
            }
        }
    }
}