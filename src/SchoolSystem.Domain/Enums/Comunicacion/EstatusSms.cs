namespace SchoolSystem.Domain.Enums.Comunicacion
{
    /// <summary>
    /// Estados de envío de SMS
    /// </summary>
    public enum EstatusSms
    {
        Pendiente = 1,
        Enviado = 2,
        Entregado = 3,
        Fallido = 4
    }
}