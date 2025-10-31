namespace SchoolSystem.Domain.Enums.Finanzas
{
    /// <summary>
    /// Estado de un cargo/pago
    /// </summary>
    public enum EstatusCargo
    {
        /// <summary>
        /// Pendiente de pago
        /// </summary>
        Pendiente = 1,

        /// <summary>
        /// Pagado completamente
        /// </summary>
        Pagado = 2,

        /// <summary>
        /// Pago parcial realizado
        /// </summary>
        Parcial = 3,

        /// <summary>
        /// Vencido (no pagado a tiempo)
        /// </summary>
        Vencido = 4,

        /// <summary>
        /// Cancelado
        /// </summary>
        Cancelado = 5
    }
}