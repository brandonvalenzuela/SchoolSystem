using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Enums.Conducta
{
    /// <summary>
    /// Tipos de notificación para registros de conducta
    /// Define el método por el cual se notifica a padres o tutores
    /// </summary>
    public enum TipoNotificacion
    {
        /// <summary>
        /// Sin notificación - Solo se registra internamente
        /// </summary>
        Ninguna = 0,

        /// <summary>
        /// Notificación por correo electrónico
        /// </summary>
        Email = 1,

        /// <summary>
        /// Notificación por mensaje SMS
        /// </summary>
        SMS = 2,

        /// <summary>
        /// Notificación push en la aplicación móvil
        /// </summary>
        PushNotification = 3,

        /// <summary>
        /// Llamada telefónica directa a los padres
        /// </summary>
        LlamadaTelefonica = 4,

        /// <summary>
        /// Citatorio físico entregado al alumno
        /// </summary>
        CitatorioFisico = 5,

        /// <summary>
        /// Notificación por WhatsApp
        /// </summary>
        WhatsApp = 6,

        /// <summary>
        /// Reunión presencial con padres
        /// </summary>
        ReunionPresencial = 7,

        /// <summary>
        /// Nota en el cuaderno de tareas
        /// </summary>
        NotaCuaderno = 8,

        /// <summary>
        /// Todas las vías disponibles (Email + SMS + Push)
        /// </summary>
        Todas = 9
    }
}
