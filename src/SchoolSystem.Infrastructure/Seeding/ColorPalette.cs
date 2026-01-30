namespace SchoolSystem.Infrastructure.Seeding
{
    /// <summary>
    /// Paleta de colores consistente para Materias
    /// Diseñada para ser visibles, profesionales y educativos
    /// </summary>
    public static class ColorPalette
    {
        /// <summary>
        /// Colores hexadecimales sugeridos para materias (#RRGGBB)
        /// </summary>
        public static readonly List<string> Colors = new List<string>
        {
            "#FF6B6B", // Rojo - Ciencias Sociales/Historia
            "#4ECDC4", // Turquesa - Ciencias Naturales
            "#45B7D1", // Azul - Matemáticas
            "#FFA502", // Naranja - Arte/Artes Plásticas
            "#95E1D3", // Verde Menta - Educación Física
            "#C7CEEA", // Lavanda - Idiomas
            "#B19CD9", // Púrpura - Tecnología
            "#FFD93D", // Amarillo - Educación/Tutoría
            "#6BCF7F", // Verde - Biología
            "#FF6B9D", // Rosa - Artes Escénicas
            "#A8E6CF", // Verde Pastel - Ecología
            "#FFD3B6"  // Peach - Diseño
        };

        /// <summary>
        /// Color fallback si no hay color especificado
        /// </summary>
        public const string Fallback = "#9E9E9E";

        /// <summary>
        /// Obtiene un color de la paleta por índice
        /// Si el índice excede la cantidad, usa modulo para rotar
        /// </summary>
        public static string GetColorByIndex(int index)
        {
            if (Colors.Count == 0)
                return Fallback;

            return Colors[Math.Abs(index) % Colors.Count];
        }

        /// <summary>
        /// Obtiene un color determinístico basado en el hash del nombre
        /// Asegura que materias con el mismo nombre siempre tengan el mismo color
        /// </summary>
        public static string GetColorByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Fallback;

            var hash = Math.Abs(name.GetHashCode());
            return GetColorByIndex(hash);
        }
    }
}
