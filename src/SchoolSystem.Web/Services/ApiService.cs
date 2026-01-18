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
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
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
            // Leer el contenido como string primero (Evita errores de stream)
            var content = await response.Content.ReadAsStringAsync();

            // Si no hay contenido, devolvemos instancia vacía
            if (string.IsNullOrWhiteSpace(content))
                return Activator.CreateInstance<T>();

            // 1) Intento normal: deserializar al tipo esperado
            try
            {
                var result = JsonSerializer.Deserialize<T>(content, _jsonOptions);
                if (result != null)
                    return result;
            }
            catch (Exception ex)
            {
                // Si falla la deserialización (ej: Data es null pero T es int)
                // Intentamos al menos devolver un objeto con el mensaje de error si existe
                Console.WriteLine($"Error de deserialización: {ex.Message}");
            }

            // 2) Fallback: si NO es success, y T parece ser ApiResponse<>,
            // construimos un ApiResponse con mensaje más útil.
            if (!response.IsSuccessStatusCode)
            {
                var fallback = Activator.CreateInstance<T>();

                // Mensaje base: status code + razón
                var message = $"{(int)response.StatusCode} {response.ReasonPhrase}";

                // Intentar extraer un "Message" del JSON si existe
                try
                {
                    using var doc = JsonDocument.Parse(content);

                    if (doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        if (doc.RootElement.TryGetProperty("message", out var msgLower))
                            message = msgLower.GetString() ?? message;

                        if (doc.RootElement.TryGetProperty("Message", out var msgUpper))
                            message = msgUpper.GetString() ?? message;

                        // También podrías extraer "errors" si tu backend manda validaciones tipo ModelState
                        if (doc.RootElement.TryGetProperty("errors", out var errorsProp) &&
                            errorsProp.ValueKind == JsonValueKind.Object)
                        {
                            message += " (validación)";
                        }
                    }
                }
                catch
                {
                    // Si no es JSON (por ejemplo HTML), recortamos para no inundar logs
                    var snippet = content.Length > 200 ? content.Substring(0, 200) + "..." : content;
                    message += $" | Respuesta: {snippet}";
                }

                // Asignar Succeeded=false y Message si esas propiedades existen en T
                typeof(T).GetProperty("Succeeded")?.SetValue(fallback, false);
                typeof(T).GetProperty("Message")?.SetValue(fallback, message);

                return fallback;
            }

            // 3) Si fue success pero no se pudo deserializar, devolvemos instancia vacía
            // (esto evita crashear la UI)
            return Activator.CreateInstance<T>();
        }
    }
}