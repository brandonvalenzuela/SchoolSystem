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
            try
            {
                // Lee el contenido. Si es null o vacío, devuelve default.
                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                return result;
            }
            catch
            {
                // Si falla la deserialización o el body está vacío en un error 400/500
                if (!response.IsSuccessStatusCode)
                {
                    // Intentar leer como string para ver el error (opcional para debug)
                    // var error = await response.Content.ReadAsStringAsync();

                    // Retornamos un objeto "fallido" genérico. 
                    // Nota: Esto asume que T es ApiResponse<Something>
                    // Si T fuera otro tipo, esto podría fallar, pero en tu arquitectura siempre es ApiResponse.
                    return Activator.CreateInstance<T>();
                }
                throw;
            }
        }
    }
}