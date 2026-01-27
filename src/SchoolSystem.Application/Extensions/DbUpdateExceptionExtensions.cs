using Microsoft.EntityFrameworkCore;
using System;

namespace SchoolSystem.Application.Extensions
{
    /// <summary>
    /// Extensiones para detectar y clasificar excepciones de actualización de base de datos.
    /// Principalmente para identificar violaciones de índices UNIQUE (concurrencia).
    /// </summary>
    public static class DbUpdateExceptionExtensions
    {
        /// <summary>
        /// Detecta si una DbUpdateException es causada por una violación de índice UNIQUE en MySQL.
        /// 
        /// En MySQL, el error de entrada duplicada es 1062.
        /// Esto ocurre cuando múltiples usuarios intentan insertar el mismo registro simultáneamente.
        /// </summary>
        /// <param name="ex">La excepción DbUpdateException a verificar</param>
        /// <returns>true si es un error de clave duplicada/UNIQUE, false en caso contrario</returns>
        public static bool IsDuplicateKeyError(this DbUpdateException ex)
        {
            if (ex?.InnerException == null)
                return false;

            var innerEx = ex.InnerException;

            // Intentar obtener el código de error de MySQL (MySqlException)
            try
            {
                var numberProperty = innerEx.GetType().GetProperty("Number");
                if (numberProperty != null)
                {
                    var number = (int?)numberProperty.GetValue(innerEx);
                    if (number == 1062) // MySQL Duplicate entry error
                        return true;
                }
            }
            catch
            {
                // Si falla la reflexión, continuar con búsqueda en mensaje
            }

            // Fallback: buscar en el mensaje si contiene indicadores de duplicado
            if (innerEx.Message != null)
            {
                return innerEx.Message.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) ||
                       innerEx.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
                       innerEx.Message.Contains("UNIQUE constraint", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Obtiene un mensaje amigable para el usuario cuando hay un error de duplicado.
        /// </summary>
        /// <returns>Mensaje listo para mostrar al cliente</returns>
        public static string GetConflictMessage(this DbUpdateException ex)
        {
            return "Conflicto de concurrencia: algunas calificaciones ya fueron capturadas por otro usuario. " +
                   "Por favor, recarga la página e intenta de nuevo.";
        }
    }
}
