using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SchoolSystem.Web.Services
{
    public class CurrentUserContext
    {
        public int UserId { get; set; }
        public int EscuelaId { get; set; }
        public string? Role { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }

    /// <summary>
    /// Lee el contexto del usuario actual desde los Claims del AuthenticationState.
    /// (No hace llamadas a API; es inmediato y consistente con JWT)
    /// </summary>
    public class CurrentUserContextService
    {
        private readonly AuthenticationStateProvider _auth;

        public CurrentUserContextService(AuthenticationStateProvider auth)
        {
            _auth = auth;
        }

        public async Task<CurrentUserContext> GetAsync()
        {
            var state = await _auth.GetAuthenticationStateAsync();
            var user = state.User;

            if (user?.Identity?.IsAuthenticated != true)
                throw new InvalidOperationException("Usuario no autenticado.");

            var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var escuelaIdStr = user.FindFirst("EscuelaId")?.Value;

            if (!int.TryParse(userIdStr, out var userId))
                throw new InvalidOperationException("Claim NameIdentifier inválido o faltante.");

            if (!int.TryParse(escuelaIdStr, out var escuelaId))
                throw new InvalidOperationException("Claim EscuelaId inválido o faltante.");

            return new CurrentUserContext
            {
                UserId = userId,
                EscuelaId = escuelaId,
                Role = user.FindFirst(ClaimTypes.Role)?.Value,
                UserName = user.FindFirst(ClaimTypes.Name)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
            };
        }
    }
}
