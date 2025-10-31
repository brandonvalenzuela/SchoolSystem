namespace SchoolSystem.Domain.Enums.Academico
{
    /// <summary>
    /// Estados de una entrega de tarea
    /// </summary>
    public enum EstatusEntrega
    {
        /// <summary>
        /// Pendiente de entregar
        /// </summary>
        Pendiente = 1,

        /// <summary>
        /// Entregada, pendiente de revisión
        /// </summary>
        Entregada = 2,

        /// <summary>
        /// Revisada y calificada
        /// </summary>
        Revisada = 3,

        /// <summary>
        /// Entregada fuera de tiempo
        /// </summary>
        Tardia = 4,

        /// <summary>
        /// No fue entregada
        /// </summary>
        NoEntregada = 5
    }
}