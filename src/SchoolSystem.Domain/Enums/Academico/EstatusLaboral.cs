namespace SchoolSystem.Domain.Enums.Academico
{
    /// <summary>
    /// Estatus laboral del maestro
    /// </summary>
    public enum EstatusLaboral
    {
        /// <summary>
        /// Maestro activo y trabajando
        /// </summary>
        Activo = 1,

        /// <summary>
        /// Maestro en licencia temporal
        /// </summary>
        Licencia = 2,

        /// <summary>
        /// Maestro dado de baja
        /// </summary>
        Baja = 3,

        /// <summary>
        /// Maestro suspendido
        /// </summary>
        Suspendido = 4
    }
}