namespace SchoolSystem.Domain.Enums.Conducta
{
    /// <summary>
    /// Tipos de sanción disciplinaria
    /// </summary>
    public enum TipoSancion
    {
        /// <summary>
        /// Amonestación verbal
        /// </summary>
        AmonestacionVerbal = 1,

        /// <summary>
        /// Amonestación escrita
        /// </summary>
        AmonestacionEscrita = 2,

        /// <summary>
        /// Suspensión temporal
        /// </summary>
        Suspension = 3,

        /// <summary>
        /// Expulsión definitiva
        /// </summary>
        Expulsion = 4,

        /// <summary>
        /// Trabajo comunitario o servicio social
        /// </summary>
        TrabajoComunitario = 5,

        /// <summary>
        /// Citatorio a padres de familia
        /// </summary>
        CitaPadres = 6,

        /// <summary>
        /// Reposición de daños
        /// </summary>
        ReposicionDanos = 7,

        /// <summary>
        /// Pérdida de privilegios
        /// </summary>
        PerdidaPrivilegios = 8
    }
}
