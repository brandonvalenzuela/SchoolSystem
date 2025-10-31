namespace SchoolSystem.Domain.Enums.Finanzas
{
    /// <summary>
    /// Método de pago
    /// </summary>
    public enum MetodoPago
    {
        /// <summary>
        /// Pago en efectivo
        /// </summary>
        Efectivo = 1,

        /// <summary>
        /// Transferencia bancaria
        /// </summary>
        Transferencia = 2,

        /// <summary>
        /// Tarjeta de crédito/débito
        /// </summary>
        Tarjeta = 3,

        /// <summary>
        /// Cheque
        /// </summary>
        Cheque = 4,

        /// <summary>
        /// Otro método
        /// </summary>
        Otro = 5
    }
}