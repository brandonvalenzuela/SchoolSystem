namespace SchoolSystem.Domain.Enums.Academico
{
    /// <summary>
    /// Tipo de contrato del maestro
    /// </summary>
    public enum TipoContrato
    {
        /// <summary>
        /// Maestro de base (planta permanente)
        /// </summary>
        Base = 1,

        /// <summary>
        /// Maestro interino (temporal)
        /// </summary>
        Interino = 2,

        /// <summary>
        /// Por honorarios
        /// </summary>
        Honorarios = 3,

        /// <summary>
        /// Medio tiempo
        /// </summary>
        MedioTiempo = 4,

        /// <summary>
        /// Por horas
        /// </summary>
        PorHoras = 5
    }
}