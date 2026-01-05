using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Finanzas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Finanzas
{
    /// <summary>
    /// Pagos realizados por alumnos
    /// </summary>
    [Table("Pagos")]
    public class Pago : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Cargo al que se aplica el pago
        /// </summary>
        [Required]
        public int CargoId { get; set; }

        /// <summary>
        /// Alumno que realiza el pago
        /// </summary>
        [Required]
        public int? AlumnoId { get; set; }

        /// <summary>
        /// Monto del pago
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }

        #endregion

        #region Método de Pago

        /// <summary>
        /// Método de pago utilizado
        /// </summary>
        [Required]
        public MetodoPago MetodoPago { get; set; }

        /// <summary>
        /// Referencia bancaria o número de cheque
        /// </summary>
        [StringLength(100)]
        public string Referencia { get; set; }

        /// <summary>
        /// Banco (si aplica)
        /// </summary>
        [StringLength(100)]
        public string Banco { get; set; }

        /// <summary>
        /// Últimos 4 dígitos de tarjeta (si aplica)
        /// </summary>
        [StringLength(4)]
        public string UltimosDigitosTarjeta { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha y hora del pago
        /// </summary>
        [Required]
        public DateTime FechaPago { get; set; }

        /// <summary>
        /// Fecha de aplicación del pago (puede diferir de fecha de pago)
        /// </summary>
        public DateTime? FechaAplicacion { get; set; }

        #endregion

        #region Recibo

        /// <summary>
        /// Folio único del recibo
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FolioRecibo { get; set; }

        /// <summary>
        /// Serie del recibo
        /// </summary>
        [StringLength(10)]
        public string SerieRecibo { get; set; }

        /// <summary>
        /// URL del PDF del recibo
        /// </summary>
        [StringLength(500)]
        public string ReciboUrl { get; set; }

        #endregion

        #region Control

        /// <summary>
        /// Usuario que recibió el pago
        /// </summary>
        [Required]
        public int RecibidoPorId { get; set; }

        /// <summary>
        /// Observaciones del pago
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Cancelación

        /// <summary>
        /// Indica si el pago fue cancelado
        /// </summary>
        [Required]
        public bool Cancelado { get; set; }

        /// <summary>
        /// Fecha de cancelación
        /// </summary>
        public DateTime? FechaCancelacion { get; set; }

        /// <summary>
        /// Motivo de cancelación
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string MotivoCancelacion { get; set; }

        /// <summary>
        /// Usuario que canceló
        /// </summary>
        public int? CanceladoPorId { get; set; }

        #endregion

        #region Facturación

        /// <summary>
        /// Indica si el pago fue facturado
        /// </summary>
        public bool Facturado { get; set; }

        /// <summary>
        /// UUID de la factura (CFDI en México)
        /// </summary>
        [StringLength(100)]
        public string UuidFactura { get; set; }

        /// <summary>
        /// URL del XML de la factura
        /// </summary>
        [StringLength(500)]
        public string FacturaXmlUrl { get; set; }

        /// <summary>
        /// URL del PDF de la factura
        /// </summary>
        [StringLength(500)]
        public string FacturaPdfUrl { get; set; }

        /// <summary>
        /// Fecha de facturación
        /// </summary>
        public DateTime? FechaFacturacion { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Referencia externa (para integraciones)
        /// </summary>
        [StringLength(100)]
        public string ReferenciaExterna { get; set; }

        /// <summary>
        /// Datos adicionales en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DatosAdicionales { get; set; }

        /// <summary>
        /// IP desde donde se realizó el pago (si aplica)
        /// </summary>
        [StringLength(45)]
        public string DireccionIp { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Cargo al que se aplica
        /// </summary>
        [ForeignKey("CargoId")]
        public virtual Cargo Cargo { get; set; }

        /// <summary>
        /// Alumno que pagó
        /// </summary>
        [ForeignKey("AlumnoId")]
        public virtual Alumno? Alumno { get; set; }

        /// <summary>
        /// Usuario que recibió el pago
        /// </summary>
        [ForeignKey("RecibidoPorId")]
        public virtual Usuario RecibidoPor { get; set; }

        /// <summary>
        /// Usuario que canceló
        /// </summary>
        [ForeignKey("CanceladoPorId")]
        public virtual Usuario CanceladoPor { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Soft Delete (ISoftDeletable)

        /// <summary>
        /// Indica si el alumno ha sido eliminado lógicamente
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Fecha de eliminación lógica
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// ID del usuario que eliminó el registro
        /// </summary>
        public int? DeletedBy { get; set; }

        #endregion

        #region Constructor

        public Pago()
        {
            FechaPago = DateTime.Now;
            Cancelado = false;
            Facturado = false;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si el pago está activo (no cancelado)
        /// </summary>
        public bool EstaActivo => !Cancelado;

        /// <summary>
        /// Indica si fue con tarjeta
        /// </summary>
        public bool EsConTarjeta => MetodoPago == MetodoPago.Tarjeta;

        /// <summary>
        /// Indica si fue en efectivo
        /// </summary>
        public bool EsEfectivo => MetodoPago == MetodoPago.Efectivo;

        /// <summary>
        /// Indica si fue transferencia
        /// </summary>
        public bool EsTransferencia => MetodoPago == MetodoPago.Transferencia;

        /// <summary>
        /// Días desde el pago
        /// </summary>
        public int DiasDesdeElPago => (DateTime.Now.Date - FechaPago.Date).Days;

        /// <summary>
        /// Indica si tiene recibo generado
        /// </summary>
        public bool TieneRecibo => !string.IsNullOrWhiteSpace(ReciboUrl);

        /// <summary>
        /// Indica si se puede cancelar
        /// </summary>
        public bool PuedeCancelarse => !Cancelado && !Facturado;

        /// <summary>
        /// Nombre del método de pago
        /// </summary>
        public string NombreMetodoPago
        {
            get
            {
                return MetodoPago switch
                {
                    MetodoPago.Efectivo => "Efectivo",
                    MetodoPago.Transferencia => "Transferencia Bancaria",
                    MetodoPago.Tarjeta => "Tarjeta",
                    MetodoPago.Cheque => "Cheque",
                    MetodoPago.Otro => "Otro",
                    _ => "Desconocido"
                };
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Genera el folio del recibo
        /// </summary>
        /// <param name="serie">Serie del recibo</param>
        /// <param name="consecutivo">Número consecutivo</param>
        public void GenerarFolioRecibo(string serie, int consecutivo)
        {
            SerieRecibo = serie;
            FolioRecibo = $"{serie}{consecutivo:D6}";
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Asocia el recibo generado
        /// </summary>
        /// <param name="urlRecibo">URL del PDF del recibo</param>
        public void AsociarRecibo(string urlRecibo)
        {
            if (string.IsNullOrWhiteSpace(urlRecibo))
                throw new ArgumentException("La URL del recibo no puede estar vacía");

            ReciboUrl = urlRecibo;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Cancela el pago
        /// </summary>
        /// <param name="motivo">Motivo de cancelación</param>
        /// <param name="usuarioId">ID del usuario que cancela</param>
        public void Cancelar(string motivo, int usuarioId)
        {
            if (Cancelado)
                throw new InvalidOperationException("El pago ya está cancelado");

            if (Facturado)
                throw new InvalidOperationException("No se puede cancelar un pago facturado. Primero debe cancelar la factura.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("Debe especificar el motivo de cancelación");

            Cancelado = true;
            FechaCancelacion = DateTime.Now;
            MotivoCancelacion = motivo;
            CanceladoPorId = usuarioId;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Revierte la cancelación del pago
        /// </summary>
        public void RevertirCancelacion()
        {
            if (!Cancelado)
                throw new InvalidOperationException("El pago no está cancelado");

            Cancelado = false;
            FechaCancelacion = null;
            MotivoCancelacion = null;
            CanceladoPorId = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Registra la facturación del pago
        /// </summary>
        /// <param name="uuid">UUID de la factura</param>
        /// <param name="xmlUrl">URL del XML</param>
        /// <param name="pdfUrl">URL del PDF</param>
        public void RegistrarFacturacion(string uuid, string xmlUrl, string pdfUrl)
        {
            if (Cancelado)
                throw new InvalidOperationException("No se puede facturar un pago cancelado");

            if (Facturado)
                throw new InvalidOperationException("El pago ya está facturado");

            if (string.IsNullOrWhiteSpace(uuid))
                throw new ArgumentException("El UUID de la factura es requerido");

            Facturado = true;
            UuidFactura = uuid;
            FacturaXmlUrl = xmlUrl;
            FacturaPdfUrl = pdfUrl;
            FechaFacturacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Cancela la factura asociada
        /// </summary>
        public void CancelarFactura()
        {
            if (!Facturado)
                throw new InvalidOperationException("El pago no está facturado");

            Facturado = false;
            UuidFactura = null;
            FacturaXmlUrl = null;
            FacturaPdfUrl = null;
            FechaFacturacion = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece la fecha de aplicación del pago
        /// </summary>
        public void EstablecerFechaAplicacion(DateTime fechaAplicacion)
        {
            if (fechaAplicacion < FechaPago)
                throw new ArgumentException("La fecha de aplicación no puede ser anterior a la fecha de pago");

            FechaAplicacion = fechaAplicacion;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Agrega información de tarjeta
        /// </summary>
        /// <param name="ultimosDigitos">Últimos 4 dígitos</param>
        /// <param name="banco">Nombre del banco</param>
        public void AgregarInfoTarjeta(string ultimosDigitos, string banco = null)
        {
            if (MetodoPago != MetodoPago.Tarjeta)
                throw new InvalidOperationException("El método de pago no es tarjeta");

            if (string.IsNullOrWhiteSpace(ultimosDigitos) || ultimosDigitos.Length != 4)
                throw new ArgumentException("Debe proporcionar los últimos 4 dígitos");

            UltimosDigitosTarjeta = ultimosDigitos;
            Banco = banco;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Agrega información de transferencia/cheque
        /// </summary>
        /// <param name="referencia">Referencia o número</param>
        /// <param name="banco">Nombre del banco</param>
        public void AgregarInfoBancaria(string referencia, string banco)
        {
            if (MetodoPago != MetodoPago.Transferencia && MetodoPago != MetodoPago.Cheque)
                throw new InvalidOperationException("El método de pago no requiere información bancaria");

            if (string.IsNullOrWhiteSpace(referencia))
                throw new ArgumentException("La referencia es requerida");

            Referencia = referencia;
            Banco = banco;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Agrega observaciones al pago
        /// </summary>
        public void AgregarObservaciones(string observaciones)
        {
            if (string.IsNullOrWhiteSpace(observaciones))
                return;

            Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                ? observaciones
                : $"{Observaciones}\n{observaciones}";

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida que el pago sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (Monto <= 0)
                errores.Add("El monto del pago debe ser mayor a cero");

            if (string.IsNullOrWhiteSpace(FolioRecibo))
                errores.Add("El folio del recibo es requerido");

            if (FechaAplicacion.HasValue && FechaAplicacion.Value < FechaPago)
                errores.Add("La fecha de aplicación no puede ser anterior a la fecha de pago");

            if (Cancelado && string.IsNullOrWhiteSpace(MotivoCancelacion))
                errores.Add("Debe especificar el motivo de cancelación");

            if (Cancelado && !CanceladoPorId.HasValue)
                errores.Add("Debe especificar quién canceló el pago");

            if (Facturado && string.IsNullOrWhiteSpace(UuidFactura))
                errores.Add("El UUID de la factura es requerido si está facturado");

            if (MetodoPago == MetodoPago.Tarjeta && !string.IsNullOrWhiteSpace(UltimosDigitosTarjeta))
            {
                if (UltimosDigitosTarjeta.Length != 4)
                    errores.Add("Los últimos dígitos de tarjeta deben ser 4");
            }

            return errores;
        }

        #endregion
    }
}