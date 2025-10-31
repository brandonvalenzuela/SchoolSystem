namespace SchoolSystem.Domain.Enums.Auditoria
{
    /// <summary>
    /// Modo de sincronización
    /// </summary>
    public enum ModoSincronizacion
    {
        Completa = 1,      // Sincroniza todo
        Incremental = 2,   // Solo cambios desde última sincronización
        Selectiva = 3      // Solo entidades específicas
    }
}