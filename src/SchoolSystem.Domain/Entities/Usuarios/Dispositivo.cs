using SchoolSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Entities.Usuarios
{
    /// <summary>
    /// Entidad Dispositivo - Representa un dispositivo registrado por el usuario
    /// Para notificaciones push y control de sesiones
    /// </summary>
    public class Dispositivo : BaseEntity
    {
        /// <summary>
        /// ID del usuario dueño del dispositivo
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Usuario dueño (Navigation Property)
        /// </summary>
        public virtual Usuario? Usuario { get; set; }

        /// <summary>
        /// Identificador único del dispositivo
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Nombre descriptivo del dispositivo
        /// </summary>
        public string DeviceName { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de dispositivo (web, mobile, desktop)
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Sistema operativo del dispositivo
        /// </summary>
        public string SO { get; set; } = string.Empty;

        /// <summary>
        /// Navegador utilizado (si aplica)
        /// </summary>
        public string Navegador { get; set; } = string.Empty;

        /// <summary>
        /// Token de Firebase Cloud Messaging para notificaciones push
        /// </summary>
        public string TokenFCM { get; set; } = string.Empty;

        /// <summary>
        /// Dirección IP de la última conexión
        /// </summary>
        public string IpUltimaConexion { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de registro del dispositivo
        /// </summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Fecha de última actividad
        /// </summary>
        public DateTime UltimaActividad { get; set; }

        /// <summary>
        /// Indica si el dispositivo está activo
        /// </summary>
        public bool Activo { get; set; }

        public Dispositivo()
        {
            FechaRegistro = DateTime.Now;
            UltimaActividad = DateTime.Now;
            Activo = true;
        }
    }
}
