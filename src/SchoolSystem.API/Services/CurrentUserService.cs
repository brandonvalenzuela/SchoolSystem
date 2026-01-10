using SchoolSystem.Application.Services.Interfaces;
using System.Security.Claims;

namespace SchoolSystem.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var idClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                return idClaim != null && int.TryParse(idClaim.Value, out int userId) ? userId : null;
            }
        }

        public int? EscuelaId
        {
            get
            {
                // Asumiendo que guardaste "EscuelaId" en el token al hacer login
                var escuelaClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("EscuelaId");
                return escuelaClaim != null && int.TryParse(escuelaClaim.Value, out int id) ? id : null;
            }
        }

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
        }

        public string Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            }
        }
    }
}
