using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Enums.Academico;

namespace SchoolSystem.Infrastructure.Seeding
{
    /// <summary>
    /// Datos iniciales de Materias para seed
    /// Incluye nombre, descripción y asignación automática de color
    /// </summary>
    public static class MateriasSeedData
    {
        public static List<MateriaDefinition> GetMateriasDefinitions()
        {
            return new List<MateriaDefinition>
            {
                // Ciencias (0)
                new("MAT-101", "Matemáticas I", "Fundamentos de álgebra, geometría y trigonometría. Desarrolla pensamiento lógico y resolución de problemas.", AreaAcademica.Ciencias, TipoMateria.Teorica),
                new("MAT-102", "Matemáticas II", "Cálculo diferencial e integral. Aplicaciones en física y economía.", AreaAcademica.Ciencias, TipoMateria.Teorica),
                new("FIS-101", "Física I", "Mecánica clásica, movimiento, fuerzas y energía.", AreaAcademica.Ciencias, TipoMateria.TeoricoPractica),
                new("FIS-102", "Física II", "Termodinámica, óptica, electromagnetismo.", AreaAcademica.Ciencias, TipoMateria.TeoricoPractica),
                new("QUI-101", "Química Inorgánica", "Estructura del átomo, tabla periódica, enlaces químicos.", AreaAcademica.Ciencias, TipoMateria.Laboratorio),

                // Ciencias Naturales (6)
                new("BIO-101", "Biología General", "Célula, genética, evolución y diversidad de vida.", AreaAcademica.CienciasNaturales, TipoMateria.TeoricoPractica),
                new("BIO-102", "Biología Humana", "Anatomía y fisiología del cuerpo humano.", AreaAcademica.CienciasNaturales, TipoMateria.Teorica),
                new("GEO-101", "Geografía", "Geografía física y humana. Clima, relieve, población.", AreaAcademica.CienciasNaturales, TipoMateria.Teorica),
                new("ECO-101", "Ecología", "Ecosistemas, biodiversidad y sostenibilidad.", AreaAcademica.CienciasNaturales, TipoMateria.Teorica),

                // Humanidades (1)
                new("ESP-101", "Español I", "Literatura, gramática y redacción. Análisis de textos clásicos.", AreaAcademica.Humanidades, TipoMateria.Teorica),
                new("ESP-102", "Español II", "Literatura moderna. Producción de textos académicos.", AreaAcademica.Humanidades, TipoMateria.Teorica),
                new("HIS-101", "Historia de México", "Periodos prehispánico, colonial, independencia y modernidad.", AreaAcademica.Humanidades, TipoMateria.Teorica),
                new("HIS-102", "Historia Universal", "Civilizaciones antiguas, medievales, modernas y contemporáneas.", AreaAcademica.Humanidades, TipoMateria.Teorica),
                new("CIV-101", "Educación Cívica", "Derechos humanos, participación ciudadana, responsabilidad social.", AreaAcademica.FormacionCivica, TipoMateria.Teorica),
                new("ECN-101", "Economía", "Conceptos básicos de microeconomía y macroeconomía.", AreaAcademica.Humanidades, TipoMateria.Teorica),

                // Lenguajes (2)
                new("ING-101", "Inglés I", "Gramática, vocabulario y conversación nivel básico-intermedio.", AreaAcademica.Lenguajes, TipoMateria.Teorica),
                new("ING-102", "Inglés II", "Lectura y escritura avanzada. Preparación para exámenes internacionales.", AreaAcademica.Lenguajes, TipoMateria.Teorica),

                // Artes (3)
                new("ARV-101", "Artes Visuales I", "Dibujo, pintura y teoría del color.", AreaAcademica.Artes, TipoMateria.Taller),
                new("ARV-102", "Artes Visuales II", "Escultura, fotografía y arte digital.", AreaAcademica.Artes, TipoMateria.Taller),
                new("MUS-101", "Música", "Historia de la música, teoría musical y apreciación.", AreaAcademica.Artes, TipoMateria.Teorica),
                new("DAN-101", "Danza", "Técnicas de danza, expresión corporal y coreografía.", AreaAcademica.Artes, TipoMateria.Taller),

                // Deportes (4)
                new("EDF-101", "Educación Física I", "Deportes de equipo, atletismo y acondicionamiento físico.", AreaAcademica.Deportes, TipoMateria.Practica),
                new("EDF-102", "Educación Física II", "Gimnasia, deporte individual y teoría de movimiento.", AreaAcademica.Deportes, TipoMateria.Practica),

                // Tecnología (5)
                new("TEC-101", "Informática I", "Introducción a computadoras, sistemas operativos y ofimática.", AreaAcademica.Tecnologia, TipoMateria.Teorica),
                new("TEC-102", "Programación Básica", "Lógica de programación y lenguajes introductorios.", AreaAcademica.Tecnologia, TipoMateria.Practica)
            };
        }

        /// <summary>
        /// Crea entidades Materia a partir de las definiciones
        /// Asigna colores automáticamente usando GetColorByName
        /// </summary>
        public static List<Materia> CreateMaterias(int escuelaId)
        {
            var materias = new List<Materia>();

            foreach (var def in GetMateriasDefinitions())
            {
                var materia = new Materia
                {
                    EscuelaId = escuelaId,
                    Nombre = def.Nombre,
                    Clave = def.Clave,
                    Descripcion = def.Descripcion,
                    Area = def.Area,
                    Tipo = def.Tipo,
                    ColorHex = ColorPalette.GetColorByName(def.Nombre),
                    Icono = IconoMateria.Book,
                    Activo = true,
                    RequiereMateriales = false,
                    RequiereInstalacionesEspeciales = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                materias.Add(materia);
            }

            return materias;
        }
    }

    /// <summary>
    /// Definición de una materia para seed (sin ID)
    /// </summary>
    public class MateriaDefinition
    {
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public AreaAcademica Area { get; set; }
        public TipoMateria Tipo { get; set; }

        public MateriaDefinition(string clave, string nombre, string descripcion, AreaAcademica area, TipoMateria tipo)
        {
            Clave = clave;
            Nombre = nombre;
            Descripcion = descripcion;
            Area = area;
            Tipo = tipo;
        }
    }
}
