namespace SchoolSystem.Infrastructure.Constants
{
    /// <summary>
    /// Paleta de colores consistente para Materias y elementos académicos
    /// Diseñada para ser visibles, profesionales y educativos
    /// </summary>
    public static class AcademicPalette
    {
        /// <summary>
        /// Color fallback si no hay color especificado o es inválido
        /// Gris neutral - funciona con cualquier tema
        /// </summary>
        public const string DefaultMateriaColor = "#9E9E9E";

        /// <summary>
        /// Colores hexadecimales sugeridos para materias (#RRGGBB)
        /// Seleccionados para ser visibles, profesionales y educativos
        /// </summary>
        public static readonly IReadOnlyList<string> MateriaColors = new List<string>
        {
            "#FF6B6B", // Rojo - Ciencias Sociales, Historia
            "#4ECDC4", // Turquesa - Ciencias Naturales, Sostenibilidad
            "#45B7D1", // Azul - Matemáticas, Lógica
            "#FFA502", // Naranja - Arte, Creatividad
            "#95E1D3", // Verde Menta - Educación Física, Movimiento
            "#C7CEEA", // Lavanda - Idiomas, Comunicación
            "#B19CD9", // Púrpura - Tecnología, Innovación
            "#FFD93D", // Amarillo - Educación, Tutoría
            "#6BCF7F", // Verde - Biología, Naturaleza
            "#FF6B9D", // Rosa - Artes Escénicas, Expresión
            "#A8E6CF", // Verde Pastel - Ecología, Medio Ambiente
            "#FFD3B6"  // Peach - Diseño, Estética
        }.AsReadOnly();

        /// <summary>
        /// Obtiene un color de la paleta por índice
        /// Si el índice excede la cantidad, usa modulo para rotar
        /// </summary>
        /// <param name="index">Índice base (0 o superior)</param>
        /// <returns>Color hexadecimal válido (#RRGGBB)</returns>
        public static string PickColorByIndex(int index)
        {
            if (MateriaColors.Count == 0)
                return DefaultMateriaColor;

            int safeIndex = Math.Abs(index) % MateriaColors.Count;
            return MateriaColors[safeIndex];
        }

        /// <summary>
        /// Obtiene un color determinístico basado en el hash de una clave
        /// Asegura que el mismo nombre siempre tenga el mismo color
        /// </summary>
        /// <param name="key">Clave o nombre (ej: nombre de materia)</param>
        /// <returns>Color hexadecimal válido (#RRGGBB) o DefaultMateriaColor si key es null/empty</returns>
        public static string PickColorFor(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return DefaultMateriaColor;

            // Usar GetHashCode de string para determinismo
            // Math.Abs para evitar índices negativos
            int hash = Math.Abs(key.GetHashCode());
            return PickColorByIndex(hash);
        }

        /// <summary>
        /// Valida si un color hexadecimal tiene formato válido (#RGB o #RRGGBB)
        /// </summary>
        /// <param name="hexColor">Color en formato hex (ej: #FF5733)</param>
        /// <returns>true si es válido, false en caso contrario</returns>
        public static bool IsValidHexColor(string hexColor)
        {
            if (string.IsNullOrWhiteSpace(hexColor))
                return false;

            // Validar formato #RGB (4 chars) o #RRGGBB (7 chars)
            return System.Text.RegularExpressions.Regex.IsMatch(
                hexColor,
                @"^#([A-Fa-f0-9]{3}|[A-Fa-f0-9]{6})$"
            );
        }

        /// <summary>
        /// Obtiene un color seguro: si es válido lo devuelve, sino el fallback
        /// </summary>
        /// <param name="hexColor">Color a validar</param>
        /// <returns>Color válido o DefaultMateriaColor</returns>
        public static string GetValidColorOrDefault(string hexColor)
        {
            if (IsValidHexColor(hexColor))
                return hexColor;

            return DefaultMateriaColor;
        }
    }
}
