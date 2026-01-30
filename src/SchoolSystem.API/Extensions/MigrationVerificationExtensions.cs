using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Persistence.Context;

namespace SchoolSystem.API.Extensions
{
    /// <summary>
    /// ✅ EXTENSIÓN: Verificación de migraciones pendientes al iniciar la API
    /// 
    /// Detecta si hay migraciones pendientes en la base de datos.
    /// Loguea errores y puede detener la aplicación en producción si está configurado.
    /// 
    /// Uso en Program.cs:
    /// await app.VerifyPendingMigrationsAsync();
    /// </summary>
    public static class MigrationVerificationExtensions
    {
        /// <summary>
        /// Verifica si hay migraciones pendientes y actúa según la configuración por environment.
        /// </summary>
        /// <param name="app">WebApplication instance</param>
        /// <returns>Task completado</returns>
        public static async Task VerifyPendingMigrationsAsync(this WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<SchoolSystemDbContext>>();
            var configuration = app.Configuration;
            var environment = app.Environment;

            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<SchoolSystemDbContext>();

                    // Obtener migraciones pendientes
                    var pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync()).ToList();

                    if (pendingMigrations.Any())
                    {
                        var migrationsText = string.Join(", ", pendingMigrations);

                        // Loguear el error de manera clara
                        var errorMessage = $"❌ MIGRACIONES PENDIENTES DETECTADAS: {migrationsText}";
                        logger.LogError(errorMessage);
                        logger.LogError("⚠️ La base de datos no está sincronizada con el código. Ejecuta: dotnet ef database update");

                        // Obtener configuración por environment
                        var config = GetMigrationVerificationConfig(configuration, environment);

                        if (config.ThrowExceptionInProduction && !environment.IsDevelopment())
                        {
                            logger.LogError("🛑 STOP: Modo Producción detectado. Deteniendo aplicación debido a migraciones pendientes.");
                            throw new InvalidOperationException(
                                $"Migraciones pendientes detectadas en Producción: {migrationsText}. " +
                                "Por favor, ejecuta 'dotnet ef database update' antes de iniciar la aplicación.");
                        }

                        if (config.WarningOnlyInDevelopment && environment.IsDevelopment())
                        {
                            logger.LogWarning("⚠️ ADVERTENCIA: Migraciones pendientes detectadas en Desarrollo.");
                            logger.LogWarning($"   Migraciones pendientes: {migrationsText}");
                            logger.LogWarning("   Próximamente se aplicarán automáticamente o requieren ejecución manual.");
                        }

                        // Aplicar migraciones automáticamente si está configurado
                        if (config.AutoApplyMigrationsInDevelopment && environment.IsDevelopment())
                        {
                            logger.LogWarning("🔄 APLICANDO MIGRACIONES AUTOMÁTICAMENTE (Desarrollo)...");
                            await dbContext.Database.MigrateAsync();
                            logger.LogInformation("✅ Migraciones aplicadas exitosamente.");
                        }
                    }
                    else
                    {
                        logger.LogInformation("✅ Base de datos sincronizada. No hay migraciones pendientes.");
                    }

                    // Loguear estado de la BD
                    var appliedMigrations = (await dbContext.Database.GetAppliedMigrationsAsync()).ToList();
                    logger.LogInformation($"📊 Total de migraciones aplicadas: {appliedMigrations.Count}");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "❌ ERROR CRÍTICO al verificar migraciones pendientes");
                
                // En producción, siempre relanzar
                if (!environment.IsDevelopment())
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Obtiene la configuración de verificación de migraciones según el environment.
        /// </summary>
        private static MigrationVerificationConfig GetMigrationVerificationConfig(
            IConfiguration configuration, 
            IWebHostEnvironment environment)
        {
            var config = new MigrationVerificationConfig();

            // Leer configuración por environment
            var section = configuration.GetSection($"Database:MigrationVerification");
            
            if (environment.IsProduction())
            {
                config.ThrowExceptionInProduction = section.GetValue("ThrowExceptionInProduction", true);
                config.WarningOnlyInDevelopment = false;
                config.AutoApplyMigrationsInDevelopment = false;
            }
            else if (environment.IsStaging())
            {
                config.ThrowExceptionInProduction = section.GetValue("ThrowExceptionInProduction", false);
                config.WarningOnlyInDevelopment = section.GetValue("WarnInStaging", true);
                config.AutoApplyMigrationsInDevelopment = false;
            }
            else // Development
            {
                config.ThrowExceptionInProduction = false;
                config.WarningOnlyInDevelopment = section.GetValue("WarnInDevelopment", true);
                config.AutoApplyMigrationsInDevelopment = section.GetValue("AutoApplyInDevelopment", false);
            }

            return config;
        }

        /// <summary>
        /// Clase interna para mantener la configuración de verificación de migraciones.
        /// </summary>
        private class MigrationVerificationConfig
        {
            /// <summary>
            /// Si true, lanza excepción en Producción cuando hay migraciones pendientes.
            /// Si false, solo loguea una advertencia.
            /// </summary>
            public bool ThrowExceptionInProduction { get; set; } = true;

            /// <summary>
            /// Si true, solo advierte en Desarrollo sin detener la aplicación.
            /// </summary>
            public bool WarningOnlyInDevelopment { get; set; } = true;

            /// <summary>
            /// Si true, aplica migraciones automáticamente en Desarrollo.
            /// </summary>
            public bool AutoApplyMigrationsInDevelopment { get; set; } = false;
        }
    }
}
