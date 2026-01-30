using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Enums.Academico;
using SchoolSystem.Infrastructure.Constants;
using SchoolSystem.Infrastructure.Persistence.Context;

namespace SchoolSystem.Infrastructure.Seeding
{
    /// <summary>
    /// Seeder idempotente para datos académicos iniciales
    /// Ejecutado en el startup de la API para garantizar consistencia
    /// </summary>
    public static class AcademicSeed
    {
        /// <summary>
        /// Semilla de materias básicas para cada escuela
        /// Se ejecuta solo si no existen ya
        /// </summary>
        private static readonly List<(string Nombre, string Clave, string Descripcion, AreaAcademica Area, TipoMateria Tipo)> MateriasSeed = new()
        {
            ("Matemáticas", "MAT-001", "Fundamentos de álgebra, geometría y aritmética", AreaAcademica.Ciencias, TipoMateria.Teorica),
            ("Español", "ESP-001", "Lenguaje, gramática y literatura", AreaAcademica.Lenguajes, TipoMateria.Teorica),
            ("Ciencias Naturales", "CIE-001", "Biología, química y física básica", AreaAcademica.CienciasNaturales, TipoMateria.TeoricoPractica),
            ("Historia", "HIS-001", "Historia de México y universal", AreaAcademica.Humanidades, TipoMateria.Teorica),
            ("Geografía", "GEO-001", "Geografía física y humana", AreaAcademica.CienciasNaturales, TipoMateria.Teorica),
            ("Inglés", "ING-001", "Idioma inglés nivel básico e intermedio", AreaAcademica.Lenguajes, TipoMateria.Teorica),
            ("Educación Física", "EDF-001", "Deportes y acondicionamiento físico", AreaAcademica.Deportes, TipoMateria.Practica),
            ("Artes", "ART-001", "Artes visuales y expresión artística", AreaAcademica.Artes, TipoMateria.Taller),
            ("Formación Cívica y Ética", "FCE-001", "Ciudadanía, derechos y responsabilidades", AreaAcademica.FormacionCivica, TipoMateria.Teorica),
            ("Tecnología", "TEC-001", "Informática y herramientas digitales", AreaAcademica.Tecnologia, TipoMateria.Practica),
            ("Computación", "COM-001", "Programación y software", AreaAcademica.Tecnologia, TipoMateria.Practica),
            ("Música", "MUS-001", "Teoría musical e interpretación", AreaAcademica.Artes, TipoMateria.Teorica),
            ("Biología", "BIO-001", "Estudio de los seres vivos", AreaAcademica.CienciasNaturales, TipoMateria.TeoricoPractica),
            ("Química", "QUI-001", "Elementos, compuestos y reacciones", AreaAcademica.Ciencias, TipoMateria.Laboratorio),
            ("Física", "FIS-001", "Mecánica, energía y fuerzas", AreaAcademica.Ciencias, TipoMateria.TeoricoPractica),
            ("Economía", "ECO-001", "Conceptos básicos de economía", AreaAcademica.Humanidades, TipoMateria.Teorica)
        };

        /// <summary>
        /// Ejecuta el seed de materias de forma idempotente
        /// </summary>
        /// <param name="db">Contexto de base de datos</param>
        /// <param name="logger">Logger para auditoría</param>
        /// <param name="escuelaId">ID de la escuela (default: 1)</param>
        /// <param name="ct">CancellationToken</param>
        public static async Task SeedMateriasAsync(
            SchoolSystemDbContext db,
            ILogger<SchoolSystemDbContext> logger,
            int escuelaId = 1,
            CancellationToken ct = default)
        {
            try
            {
                var existingCount = await db.Materias
                    .Where(m => m.EscuelaId == escuelaId && !m.IsDeleted)
                    .CountAsync(ct);

                if (existingCount >= MateriasSeed.Count)
                {
                    logger?.LogInformation("✅ Materias seed ya existe para escuela {EscuelaId}. Saltando seeder.", escuelaId);
                    return;
                }

                logger?.LogInformation("🌱 Iniciando seed de materias para escuela {EscuelaId}...", escuelaId);

                int insertedCount = 0;
                int updatedCount = 0;

                using (var transaction = await db.Database.BeginTransactionAsync(ct))
                {
                    try
                    {
                        foreach (var (nombre, clave, descripcion, area, tipo) in MateriasSeed)
                        {
                            // Verificar idempotencia por (EscuelaId, Nombre)
                            var existingMateria = await db.Materias
                                .FirstOrDefaultAsync(m =>
                                    m.EscuelaId == escuelaId &&
                                    m.Nombre == nombre &&
                                    !m.IsDeleted,
                                ct);

                            if (existingMateria == null)
                            {
                                // Nueva materia
                                var materia = new Materia
                                {
                                    EscuelaId = escuelaId,
                                    Nombre = nombre,
                                    Clave = clave,
                                    Descripcion = descripcion,
                                    Area = area,
                                    Tipo = tipo,
                                    ColorHex = AcademicPalette.PickColorFor(nombre),
                                    Icono = IconoMateria.Book,
                                    Activo = true,
                                    RequiereMateriales = false,
                                    RequiereInstalacionesEspeciales = false,
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow,
                                    IsDeleted = false
                                };

                                db.Materias.Add(materia);
                                insertedCount++;
                            }
                            else if (string.IsNullOrWhiteSpace(existingMateria.ColorHex))
                            {
                                // Materia existente sin color: asignar color
                                existingMateria.ColorHex = AcademicPalette.PickColorFor(nombre);
                                db.Materias.Update(existingMateria);
                                updatedCount++;
                            }
                            else if (!existingMateria.Activo)
                            {
                                // Materia existente pero inactiva: activar
                                existingMateria.Activo = true;
                                db.Materias.Update(existingMateria);
                                updatedCount++;
                            }
                        }

                        // Guardar cambios dentro de la transacción
                        await db.SaveChangesAsync(ct);
                        await transaction.CommitAsync(ct);

                        logger?.LogInformation(
                            "✅ Seed de materias completado. Insertadas: {Insertadas}, Actualizadas: {Actualizadas}, Escuela: {EscuelaId}",
                            insertedCount, updatedCount, escuelaId);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(ct);
                        logger?.LogError(ex, "❌ Error en transacción de seed. Rollback ejecutado.");
                        throw;
                    }
                }
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("Duplicate") ?? false)
            {
                // Concurrencia: otro proceso insertó simultáneamente
                logger?.LogWarning(
                    "⚠️ Conflicto de concurrencia en seed de materias (Duplicate key). " +
                    "Es seguro ignorar si el seed está siendo ejecutado en paralelo. Escuela: {EscuelaId}",
                    escuelaId);
                // No relanzar, es idempotente
            }
            catch (DbUpdateConcurrencyException concEx)
            {
                // Concurrencia: otro proceso modificó simultáneamente
                logger?.LogWarning(
                    "⚠️ Conflicto de concurrencia en seed de materias (ConcurrencyException). " +
                    "Reintentando en próximo startup. Escuela: {EscuelaId}",
                    escuelaId);
                // No relanzar, reintentará en próximo startup
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "❌ Error inesperado en seed de materias. Escuela: {EscuelaId}", escuelaId);
                throw;
            }
        }

        /// <summary>
        /// Sobrecarga alternativa sin logger para compatibilidad
        /// </summary>
        public static async Task SeedMateriasAsync(
            SchoolSystemDbContext db,
            int escuelaId = 1,
            CancellationToken ct = default)
        {
            await SeedMateriasAsync(db, null, escuelaId, ct);
        }
    }
}
