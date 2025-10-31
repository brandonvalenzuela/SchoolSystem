namespace SchoolSystem.Domain.Enums.Auditoria
{
    /// <summary>
    /// Dirección de la sincronización
    /// </summary>
    public enum DireccionSincronizacion
    {
        Subida = 1,          // Del cliente al servidor
        Bajada = 2,          // Del servidor al cliente
        Bidireccional = 3    // Ambas direcciones
    }
}