namespace SchoolSystem.Domain.Enums.Comunicacion
{
    /// <summary>
    /// Canal de notificación
    /// </summary>
    public enum CanalNotificacion
    {
        /// <summary>
        /// Notificación dentro de la app
        /// </summary>
        App = 1,

        /// <summary>
        /// SMS (mensaje de texto)
        /// </summary>
        SMS = 2,

        /// <summary>
        /// Email (correo electrónico)
        /// </summary>
        Email = 3,

        /// <summary>
        /// Push notification (notificación push)
        /// </summary>
        Push = 4,

        /// <summary>
        /// Notificación dentro del sistema
        /// </summary>
        Sistema = 5
    }
}