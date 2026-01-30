using Microsoft.EntityFrameworkCore;
using System;

namespace SchoolSystem.Application.Extensions
{
    /// <summary>
    /// Extensiones para detectar y clasificar excepciones de actualización de base de datos.
    /// Principalmente para identificar violaciones de índices UNIQUE (concurrencia).
    /// Compatible con Pomelo MySQL y MySqlConnector.
    /// </summary>
    public static class DbUpdateExceptionExtensions
    {
        /// <summary>
        /// Detecta si una DbUpdateException es causada por una violación de índice UNIQUE en MySQL.
        /// 
        /// En MySQL, el error de entrada duplicada es 1062 (ER_DUP_ENTRY).
        /// Esto ocurre cuando múltiples usuarios intentan insertar/actualizar el mismo registro simultáneamente.
        /// 
        /// Compatible con:
        /// - Pomelo.EntityFrameworkCore.MySql (usa MySqlException con propiedad Number)
        /// - MySqlConnector (usa MySqlException con propiedad Number)
        /// </summary>
        /// <param name="ex">La excepción DbUpdateException a verificar</param>
        /// <returns>true si es un error de clave duplicada/UNIQUE, false en caso contrario</returns>
        public static bool IsDuplicateKeyError(this DbUpdateException ex)
        {
            if (ex?.InnerException == null)
                return false;

            return IsUniqueViolation(ex.InnerException);
        }

        /// <summary>
        /// Helper privado que detecta violaciones UNIQUE en diferentes tipos de excepciones MySQL.
        /// </summary>
        private static bool IsUniqueViolation(Exception innerEx)
        {
            if (innerEx == null)
                return false;

            // ============================================================
            // ESTRATEGIA 1: Reflexión para obtener código de error MySQL
            // ============================================================
            // Funciona con MySqlException (Pomelo y MySqlConnector)
            try
            {
                var numberProperty = innerEx.GetType().GetProperty("Number");
                if (numberProperty != null)
                {
                    var number = (int?)numberProperty.GetValue(innerEx);
                    if (number == 1062) // MySQL ER_DUP_ENTRY
                        return true;
                }
            }
            catch
            {
                // Falló la reflexión, continuar con búsqueda en mensaje
            }

            // ============================================================
            // ESTRATEGIA 2: Búsqueda en mensaje (fallback robusto)
            // ============================================================
            // Busca palabras clave que indican violación de UNIQUE
            if (innerEx.Message != null)
            {
                var message = innerEx.Message;

                // Indicadores comunes de violación UNIQUE en MySQL
                return message.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) ||
                       message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
                       message.Contains("UNIQUE constraint", StringComparison.OrdinalIgnoreCase) ||
                       message.Contains("Duplicate Index", StringComparison.OrdinalIgnoreCase) ||
                       message.Contains("ER_DUP_ENTRY", StringComparison.OrdinalIgnoreCase) ||
                       (message.Contains("1062", StringComparison.OrdinalIgnoreCase) && 
                        message.Contains("duplicate", StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        /// <summary>
        /// Obtiene un mensaje amigable para el usuario cuando hay un error de duplicado.
        /// Este mensaje es específico para captura masiva de calificaciones.
        /// </summary>
        /// <returns>Mensaje listo para mostrar al cliente (sin detalles técnicos)</returns>
        public static string GetConflictMessage(this DbUpdateException ex)
        {
            return "Conflicto de concurrencia: algunas calificaciones ya fueron capturadas por otro usuario. " +
                   "Por favor, recarga la página e intenta de nuevo.";
        }
    }
}
