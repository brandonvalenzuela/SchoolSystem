namespace SchoolSystem.Domain.Enums.Auditoria
{
    /// <summary>
    /// Estados de una sincronización
    /// </summary>
    public enum EstadoSincronizacion
    {
        Pendiente = 1,
        EnProceso = 2,
        Completada = 3,
        Error = 4,
        Cancelada = 5,
        Parcial = 6
    }
}