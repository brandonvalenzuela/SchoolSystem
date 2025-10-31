namespace SchoolSystem.Domain.Enums.Conducta
{
    /// <summary>
    /// Categorías de conducta para clasificar registros
    /// </summary>
    public enum CategoriaConducta
    {
        // Positivas
        Responsabilidad = 1,
        Respeto = 2,
        Colaboracion = 3,
        Liderazgo = 4,
        Excelencia = 5,
        Creatividad = 6,
        Solidaridad = 7,
        Perseverancia = 8,

        // Negativas
        Indisciplina = 10,
        Irrespeto = 11,
        Agresion = 12,
        Deshonestidad = 13,
        Impuntualidad = 14,
        InclumplimientoTareas = 15,
        DañoMaterial = 16,
        Bullying = 17,

        // Otras
        Otro = 99
    }
}
