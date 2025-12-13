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
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                return claim != null && int.TryParse(claim.Value, out int id) ? id : null;
            }
        }

        public int? EscuelaId
        {
            get
            {
                // Busca el claim personalizado "EscuelaId" que pusimos en el token
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("EscuelaId");
                return claim != null && int.TryParse(claim.Value, out int id) ? id : null;
            }
        }

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
        }
    }
}
