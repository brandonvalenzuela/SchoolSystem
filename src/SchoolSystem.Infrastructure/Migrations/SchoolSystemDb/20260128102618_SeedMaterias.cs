using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations.SchoolSystemDb
{
    /// <inheritdoc />
    public partial class SeedMaterias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // IDEMPOTENT: Insertar datos de materias iniciales
            // Solo si no existe una materia con la misma clave en la escuela

            var insertSql = @"
INSERT IGNORE INTO Materias (
    EscuelaId, Nombre, Clave, Descripcion, Area, Tipo, ColorHex, 
    Icono, Activo, RequiereMateriales, RequiereInstalacionesEspeciales,
    CreatedAt, UpdatedAt, IsDeleted
) VALUES
-- Ciencias (Area=0)
(1, 'Matemáticas I', 'MAT-101', 'Fundamentos de álgebra, geometría y trigonometría. Desarrolla pensamiento lógico y resolución de problemas.', 0, 0, '#FF6B6B', 'calculate', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Matemáticas II', 'MAT-102', 'Cálculo diferencial e integral. Aplicaciones en física y economía.', 0, 0, '#4ECDC4', 'calculate', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Física I', 'FIS-101', 'Mecánica clásica, movimiento, fuerzas y energía.', 0, 4, '#45B7D1', 'science', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Física II', 'FIS-102', 'Termodinámica, óptica, electromagnetismo.', 0, 4, '#FFA502', 'science', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Química Inorgánica', 'QUI-101', 'Estructura del átomo, tabla periódica, enlaces químicos.', 0, 3, '#95E1D3', 'science', 1, 0, 0, NOW(), NOW(), 0),
-- Ciencias Naturales (Area=6)
(1, 'Biología General', 'BIO-101', 'Célula, genética, evolución y diversidad de vida.', 6, 4, '#C7CEEA', 'science', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Biología Humana', 'BIO-102', 'Anatomía y fisiología del cuerpo humano.', 6, 0, '#B19CD9', 'science', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Geografía', 'GEO-101', 'Geografía física y humana. Clima, relieve, población.', 6, 0, '#FFD93D', 'public', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Ecología', 'ECO-101', 'Ecosistemas, biodiversidad y sostenibilidad.', 6, 0, '#6BCF7F', 'science', 1, 0, 0, NOW(), NOW(), 0),
-- Humanidades (Area=1)
(1, 'Español I', 'ESP-101', 'Literatura, gramática y redacción. Análisis de textos clásicos.', 1, 0, '#FF6B9D', 'book', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Español II', 'ESP-102', 'Literatura moderna. Producción de textos académicos.', 1, 0, '#A8E6CF', 'book', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Historia de México', 'HIS-101', 'Periodos prehispánico, colonial, independencia y modernidad.', 1, 0, '#FFD3B6', 'history', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Historia Universal', 'HIS-102', 'Civilizaciones antiguas, medievales, modernas y contemporáneas.', 1, 0, '#FF6B6B', 'history', 1, 0, 0, NOW(), NOW(), 0),
-- Formación Cívica (Area=7)
(1, 'Educación Cívica', 'CIV-101', 'Derechos humanos, participación ciudadana, responsabilidad social.', 7, 0, '#4ECDC4', 'gavel', 1, 0, 0, NOW(), NOW(), 0),
-- Humanidades (Area=1)
(1, 'Economía', 'ECN-101', 'Conceptos básicos de microeconomía y macroeconomía.', 1, 0, '#45B7D1', 'trending_up', 1, 0, 0, NOW(), NOW(), 0),
-- Lenguajes (Area=2)
(1, 'Inglés I', 'ING-101', 'Gramática, vocabulario y conversación nivel básico-intermedio.', 2, 0, '#FFA502', 'language', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Inglés II', 'ING-102', 'Lectura y escritura avanzada. Preparación para exámenes internacionales.', 2, 0, '#95E1D3', 'language', 1, 0, 0, NOW(), NOW(), 0),
-- Artes (Area=3)
(1, 'Artes Visuales I', 'ARV-101', 'Dibujo, pintura y teoría del color.', 3, 2, '#C7CEEA', 'palette', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Artes Visuales II', 'ARV-102', 'Escultura, fotografía y arte digital.', 3, 2, '#B19CD9', 'palette', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Música', 'MUS-101', 'Historia de la música, teoría musical y apreciación.', 3, 0, '#FFD93D', 'music_note', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Danza', 'DAN-101', 'Técnicas de danza, expresión corporal y coreografía.', 3, 2, '#6BCF7F', 'directions_dance', 1, 0, 0, NOW(), NOW(), 0),
-- Deportes (Area=4)
(1, 'Educación Física I', 'EDF-101', 'Deportes de equipo, atletismo y acondicionamiento físico.', 4, 1, '#FF6B9D', 'sports_soccer', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Educación Física II', 'EDF-102', 'Gimnasia, deporte individual y teoría de movimiento.', 4, 1, '#A8E6CF', 'sports_gymnastics', 1, 0, 0, NOW(), NOW(), 0),
-- Tecnología (Area=5)
(1, 'Informática I', 'TEC-101', 'Introducción a computadoras, sistemas operativos y ofimática.', 5, 0, '#FFD3B6', 'computer', 1, 0, 0, NOW(), NOW(), 0),
(1, 'Programación Básica', 'TEC-102', 'Lógica de programación y lenguajes introductorios.', 5, 1, '#FF6B6B', 'code', 1, 0, 0, NOW(), NOW(), 0);
";

            migrationBuilder.Sql(insertSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // NO eliminar datos al hacer rollback
            // Esto preserva integridad referencial si hay datos relacionados
        }
    }
}

