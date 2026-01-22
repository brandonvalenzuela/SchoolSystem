using Blazored.LocalStorage;
using SchoolSystem.Web.Models;
using System.Text.Json;

namespace SchoolSystem.Web.Services
{
    /// <summary>
    /// Provee contexto académico transversal (p.ej. ciclo escolar actual).
    /// Resuelve desde API y cachea en memoria + localStorage (TTL).
    /// </summary>
    public class AcademicContextService
    {
        private const string LsKeyData = "academicContext:cicloActual:data";
        private const string LsKeyAtUtc = "academicContext:cicloActual:cachedAtUtc";

        private readonly ApiService _api;
        private readonly ILocalStorageService _localStorage;

        private CicloEscolarActualDto? _cached;
        private DateTime _cachedAtUtc;

        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);

        public AcademicContextService(ApiService api, ILocalStorageService localStorage)
        {
            _api = api;
            _localStorage = localStorage;
        }

        public async Task<ApiResponse<CicloEscolarActualDto>> GetCicloActualAsync(bool forceRefresh = false)
        {
            // 1) Cache en memoria
            if (!forceRefresh && _cached != null && (DateTime.UtcNow - _cachedAtUtc) < CacheTtl)
            {
                return new ApiResponse<CicloEscolarActualDto>
                {
                    Succeeded = true,
                    Data = _cached,
                    Message = "Ciclo actual (cache memoria)."
                };
            }

            // 2) Cache en localStorage
            if (!forceRefresh)
            {
                var ls = await TryGetFromLocalStorageAsync();
                if (ls != null)
                {
                    _cached = ls.Value.data;
                    _cachedAtUtc = ls.Value.cachedAtUtc;

                    return new ApiResponse<CicloEscolarActualDto>
                    {
                        Succeeded = true,
                        Data = _cached,
                        Message = "Ciclo actual (cache localStorage)."
                    };
                }
            }

            // 3) API
            var resp = await _api.GetAsync<CicloEscolarActualDto>("api/Ciclos/actual");

            if (resp.Succeeded && resp.Data != null)
            {
                _cached = resp.Data;
                _cachedAtUtc = DateTime.UtcNow;

                await TrySaveToLocalStorageAsync(_cached, _cachedAtUtc);
            }

            return resp;
        }

        public async Task ClearCacheAsync()
        {
            _cached = null;
            _cachedAtUtc = default;

            try
            {
                await _localStorage.RemoveItemAsync(LsKeyData);
                await _localStorage.RemoveItemAsync(LsKeyAtUtc);
            }
            catch
            {
                // no-op: si falla localStorage no debe romper la app
            }
        }

        private async Task<(CicloEscolarActualDto data, DateTime cachedAtUtc)?> TryGetFromLocalStorageAsync()
        {
            try
            {
                var cachedAtStr = await _localStorage.GetItemAsync<string>(LsKeyAtUtc);
                if (string.IsNullOrWhiteSpace(cachedAtStr))
                    return null;

                if (!DateTime.TryParse(cachedAtStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var cachedAtUtc))
                    return null;

                if ((DateTime.UtcNow - cachedAtUtc) >= CacheTtl)
                    return null;

                var json = await _localStorage.GetItemAsync<string>(LsKeyData);
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                var data = JsonSerializer.Deserialize<CicloEscolarActualDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data == null)
                    return null;

                return (data, cachedAtUtc);
            }
            catch
            {
                return null;
            }
        }

        private async Task TrySaveToLocalStorageAsync(CicloEscolarActualDto data, DateTime cachedAtUtc)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                await _localStorage.SetItemAsync(LsKeyData, json);
                await _localStorage.SetItemAsync(LsKeyAtUtc, cachedAtUtc.ToString("O")); // ISO 8601
            }
            catch
            {
                // no-op
            }
        }
    }
}
