namespace SchoolSystem.Domain.Enums.Biblioteca
{
    /// <summary>
    /// Estado de un recurso en la biblioteca
    /// </summary>
    public enum EstadoRecurso
    {
        Disponible = 1,
        Prestado = 2,
        NoPrestable = 3,
        EnReparacion = 4,
        NoDisponible = 5
    }
}