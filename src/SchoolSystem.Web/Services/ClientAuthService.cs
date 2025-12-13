using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SchoolSystem.Web.Auth;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class ClientAuthService
    {
        private readonly ApiService _apiService;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public ClientAuthService(ApiService apiService, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _apiService = apiService;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<ApiResponse<string>> LoginAsync(LoginDto loginDto)
        {
            // 1. Llamar al endpoint de tu API
            var result = await _apiService.PostAsync<LoginDto, string>("api/Auth/login", loginDto);

            if (result.Succeeded)
            {
                // 2. Si es exitoso, el Data del response contiene el Token (string)
                string token = result.Data.ToString(); // O result.Data si ApiResponse<string> se deserializa bien

                // 3. Guardar en LocalStorage
                await _localStorage.SetItemAsync("authToken", token);

                // 4. Notificar al proveedor de estado que algo cambió (para refrescar menús)
                await ((CustomAuthStateProvider)_authStateProvider).GetAuthenticationStateAsync();
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await ((CustomAuthStateProvider)_authStateProvider).GetAuthenticationStateAsync();
        }
    }
}
