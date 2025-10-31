namespace SchoolSystem.Domain.Enums.Biblioteca
{
    /// <summary>
    /// Estados de un préstamo de biblioteca
    /// </summary>
    public enum EstadoPrestamo
    {
        Activo = 1,
        Devuelto = 2,
        Vencido = 3,
        Extraviado = 4,
        Cancelado = 5
    }
}