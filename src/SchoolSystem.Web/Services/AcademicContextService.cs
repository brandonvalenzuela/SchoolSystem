using Blazored.LocalStorage;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    /// <summary>
    /// Provee contexto académico transversal (p.ej. ciclo escolar actual).
    /// Resuelve desde API y cachea (localStorage) con fallback a appsettings.
    /// </summary>
    public class AcademicContextService
    {
        private readonly ApiService _api;
        private CicloEscolarActualDto? _cached;
        private DateTime _cachedAtUtc;

        // cache simple (ej 10 min)
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);

        public AcademicContextService(ApiService api)
        {
            _api = api;
        }

        public async Task<ApiResponse<CicloEscolarActualDto>> GetCicloActualAsync(bool forceRefresh = false)
        {
            if (!forceRefresh && _cached != null && (DateTime.UtcNow - _cachedAtUtc) < CacheTtl)
            {
                return new ApiResponse<CicloEscolarActualDto>
                {
                    Succeeded = true,
                    Data = _cached,
                    Message = "Ciclo actual (cache)."
                };
            }

            var resp = await _api.GetAsync<CicloEscolarActualDto>("api/Ciclos/actual");
            if (resp.Succeeded && resp.Data != null)
            {
                _cached = resp.Data;
                _cachedAtUtc = DateTime.UtcNow;
            }

            return resp;
        }

        public void ClearCache() => _cached = null;
    }
}
