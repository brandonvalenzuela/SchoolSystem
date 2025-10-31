using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums.Comunicacion;

namespace SchoolSystem.Domain.Entities.Comunicacion
{
    /// <summary>
    /// Log de notificaciones SMS enviadas
    /// </summary>
    [Table("NotificacionSmsLog")]
    public class NotificacionSmsLog : BaseEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Notificación relacionada
        /// </summary>
        [Required]
        public int NotificacionId { get; set; }

        /// <summary>
        /// Número de teléfono destinatario
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Telefono { get; set; }

        /// <summary>
        /// Mensaje enviado
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Mensaje { get; set; }

        #endregion

        #region Proveedor

        /// <summary>
        /// Proveedor de SMS (Twilio, Nexmo, etc.)
        /// </summary>
        [StringLength(50)]
        public string Proveedor { get; set; }

        /// <summary>
        /// Estado del envío
        /// </summary>
        [Required]
        public EstatusSms Estatus { get; set; }

        /// <summary>
        /// SID o ID del proveedor
        /// </summary>
        [StringLength(100)]
        public string SidProveedor { get; set; }

        #endregion

        #region Costos y Fechas

        /// <summary>
        /// Costo del SMS
        /// </summary>
        [Column(TypeName = "decimal(10,4)")]
        public decimal? Costo { get; set; }

        /// <summary>
        /// Moneda del costo
        /// </summary>
        [StringLength(10)]
        public string Moneda { get; set; }

        /// <summary>
        /// Fecha de envío
        /// </summary>
        public DateTime? FechaEnvio { get; set; }

        /// <summary>
        /// Fecha de entrega confirmada
        /// </summary>
        public DateTime? FechaEntrega { get; set; }

        #endregion

        #region Error y Reintentos

        /// <summary>
        /// Mensaje de error (si falló)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ErrorMensaje { get; set; }

        /// <summary>
        /// Código de error del proveedor
        /// </summary>
        [StringLength(50)]
        public string CodigoError { get; set; }

        /// <summary>
        /// Número de intentos de envío
        /// </summary>
        public int NumeroIntentos { get; set; }

        /// <summary>
        /// Fecha del último intento
        /// </summary>
        public DateTime? FechaUltimoIntento { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Metadata adicional en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Metadata { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Notificación relacionada
        /// </summary>
        [ForeignKey("NotificacionId")]
        public virtual Notificacion Notificacion { get; set; }

        #endregion

        #region Constructor

        public NotificacionSmsLog()
        {
            Estatus = EstatusSms.Pendiente;
            NumeroIntentos = 0;
            Moneda = "MXN";
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si el SMS fue enviado exitosamente
        /// </summary>
        public bool Enviado => Estatus == EstatusSms.Enviado || Estatus == EstatusSms.Entregado;

        /// <summary>
        /// Indica si el SMS falló
        /// </summary>
        public bool Fallido => Estatus == EstatusSms.Fallido;

        /// <summary>
        /// Indica si está pendiente
        /// </summary>
        public bool Pendiente => Estatus == EstatusSms.Pendiente;

        /// <summary>
        /// Tiempo desde el envío
        /// </summary>
        public TimeSpan? TiempoDesdeEnvio => FechaEnvio.HasValue ? DateTime.Now - FechaEnvio.Value : null;

        /// <summary>
        /// Tiempo de entrega (desde envío hasta entrega)
        /// </summary>
        public TimeSpan? TiempoEntrega
        {
            get
            {
                if (FechaEnvio.HasValue && FechaEntrega.HasValue)
                    return FechaEntrega.Value - FechaEnvio.Value;
                return null;
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Marca el SMS como enviado
        /// </summary>
        public void MarcarComoEnviado(string sidProveedor, decimal? costo = null)
        {
            Estatus = EstatusSms.Enviado;
            FechaEnvio = DateTime.Now;
            SidProveedor = sidProveedor;
            Costo = costo;
            NumeroIntentos++;
            FechaUltimoIntento = DateTime.Now;
        }

        /// <summary>
        /// Marca el SMS como entregado
        /// </summary>
        public void MarcarComoEntregado()
        {
            Estatus = EstatusSms.Entregado;
            FechaEntrega = DateTime.Now;
        }

        /// <summary>
        /// Marca el SMS como fallido
        /// </summary>
        public void MarcarComoFallido(string errorMensaje, string codigoError = null)
        {
            Estatus = EstatusSms.Fallido;
            ErrorMensaje = errorMensaje;
            CodigoError = codigoError;
            NumeroIntentos++;
            FechaUltimoIntento = DateTime.Now;
        }

        /// <summary>
        /// Reintentar envío
        /// </summary>
        public void Reintentar()
        {
            Estatus = EstatusSms.Pendiente;
            ErrorMensaje = null;
            CodigoError = null;
        }

        /// <summary>
        /// Actualiza metadata
        /// </summary>
        public void ActualizarMetadata(string metadata)
        {
            Metadata = metadata;
        }

        /// <summary>
        /// Valida el log
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Telefono))
                errores.Add("El teléfono es requerido");

            if (string.IsNullOrWhiteSpace(Mensaje))
                errores.Add("El mensaje es requerido");

            if (Telefono?.Length > 20)
                errores.Add("El teléfono no puede exceder 20 caracteres");

            return errores;
        }

        #endregion
    }
}