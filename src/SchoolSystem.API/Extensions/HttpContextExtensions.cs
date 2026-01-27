using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace SchoolSystem.API.Extensions
{
    /// <summary>
    /// Extensiones para HttpContext para extraer información del usuario desde claims.
    /// Útil para hardening de seguridad en endpoints.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Obtiene el UserId desde los claims del usuario autenticado.
        /// </summary>
        /// <returns>ID del usuario, o 0 si no está disponible</returns>
        public static int GetUserId(this HttpContext context)
        {
            var userIdClaim = context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return 0;

            return userId;
        }

        /// <summary>
        /// Obtiene el EscuelaId desde los claims del usuario autenticado.
        /// Este claim se setea durante la autenticación y es específico de SchoolSystem.
        /// </summary>
        /// <returns>ID de la escuela, o 0 si no está disponible</returns>
        public static int GetEscuelaId(this HttpContext context)
        {
            var escuelaClaim = context?.User?.FindFirst("EscuelaId")?.Value;
            
            if (string.IsNullOrEmpty(escuelaClaim) || !int.TryParse(escuelaClaim, out var escuelaId))
                return 0;

            return escuelaId;
        }

        /// <summary>
        /// Obtiene el nombre de usuario desde los claims.
        /// </summary>
        /// <returns>Nombre de usuario, o null si no está disponible</returns>
        public static string GetUsername(this HttpContext context)
        {
            return context?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }

        /// <summary>
        /// Verifica si el usuario tiene un rol específico.
        /// </summary>
        public static bool HasRole(this HttpContext context, string role)
        {
            return context?.User?.IsInRole(role) ?? false;
        }

        /// <summary>
        /// Obtiene el GrupoId desde los claims si existe (grupo actual del maestro).
        /// No siempre está disponible; se usa si está asociado.
        /// </summary>
        public static int GetGrupoId(this HttpContext context)
        {
            var grupoClaim = context?.User?.FindFirst("GrupoId")?.Value;
            
            if (string.IsNullOrEmpty(grupoClaim) || !int.TryParse(grupoClaim, out var grupoId))
                return 0;

            return grupoId;
        }
    }
}
