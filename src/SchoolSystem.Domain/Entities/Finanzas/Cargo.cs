using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Finanzas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SchoolSystem.Domain.Entities.Finanzas
{
    /// <summary>
    /// Cargos aplicados a alumnos
    /// </summary>
    public class Cargo : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Alumno al que se aplica el cargo
        /// </summary>
        [Required]
        public int? AlumnoId { get; set; }

        /// <summary>
        /// Concepto de pago aplicado
        /// </summary>
        [Required]
        public int ConceptoPagoId { get; set; }

        /// <summary>
        /// Ciclo escolar
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CicloEscolar { get; set; }

        #endregion

        #region Montos

        /// <summary>
        /// Monto original del cargo
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }

        /// <summary>
        /// Descuento aplicado
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Descuento { get; set; }

        /// <summary>
        /// Porcentaje de descuento aplicado
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? PorcentajeDescuento { get; set; }

        /// <summary>
        /// Monto final después de descuento
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoFinal { get; set; }

        /// <summary>
        /// Mora acumulada por retraso
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal Mora { get; set; }

        /// <summary>
        /// Monto total pagado
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoPagado { get; set; }

        /// <summary>
        /// Saldo pendiente
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal SaldoPendiente { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha de creación del cargo
        /// </summary>
        [Required]
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        [Required]
        public DateTime FechaVencimiento { get; set; }

        /// <summary>
        /// Fecha en que se pagó completamente (si aplica)
        /// </summary>
        public DateTime? FechaPagoCompleto { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Estado del cargo
        /// </summary>
        [Required]
        public EstatusCargo Estatus { get; set; }

        /// <summary>
        /// Observaciones del cargo
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        /// <summary>
        /// Motivo de cancelación (si aplica)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string MotivoCancelacion { get; set; }

        /// <summary>
        /// Fecha de cancelación
        /// </summary>
        public DateTime? FechaCancelacion { get; set; }

        /// <summary>
        /// Usuario que canceló
        /// </summary>
        public int? CanceladoPorId { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Número de recibo o folio
        /// </summary>
        [StringLength(50)]
        public string NumeroRecibo { get; set; }

        /// <summary>
        /// Referencia externa (para integraciones)
        /// </summary>
        [StringLength(100)]
        public string ReferenciaExterna { get; set; }

        /// <summary>
        /// Indica si fue generado automáticamente
        /// </summary>
        public bool GeneradoAutomaticamente { get; set; }

        /// <summary>
        /// Mes al que corresponde (para colegiaturas)
        /// </summary>
        public int? MesCorrespondiente { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Alumno relacionado
        /// </summary>
        [ForeignKey("AlumnoId")]
        public virtual Alumno? Alumno { get; set; }

        /// <summary>
        /// Concepto de pago relacionado
        /// </summary>
        [ForeignKey("ConceptoPagoId")]
        public virtual ConceptoPago ConceptoPago { get; set; }

        /// <summary>
        /// Usuario que canceló
        /// </summary>
        [ForeignKey("CanceladoPorId")]
        public virtual Usuario CanceladoPor { get; set; }

        /// <summary>
        /// Pagos aplicados a este cargo
        /// </summary>
        public virtual ICollection<Pago> Pagos { get; set; }

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

        public Cargo()
        {
            FechaCreacion = DateTime.Now;
            Descuento = 0;
            Mora = 0;
            MontoPagado = 0;
            SaldoPendiente = 0;
            Estatus = EstatusCargo.Pendiente;
            GeneradoAutomaticamente = false;
            Pagos = new HashSet<Pago>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si el cargo está vencido
        /// </summary>
        public bool EstaVencido => DateTime.Now.Date > FechaVencimiento.Date && Estatus == EstatusCargo.Pendiente;

        /// <summary>
        /// Indica si está pagado completamente
        /// </summary>
        public bool EstaPagado => Estatus == EstatusCargo.Pagado;

        /// <summary>
        /// Indica si está parcialmente pagado
        /// </summary>
        public bool PagoParcial => Estatus == EstatusCargo.Parcial;

        /// <summary>
        /// Indica si está cancelado
        /// </summary>
        public bool EstaCancelado => Estatus == EstatusCargo.Cancelado;

        /// <summary>
        /// Indica si está pendiente
        /// </summary>
        public bool EstaPendiente => Estatus == EstatusCargo.Pendiente;

        /// <summary>
        /// Días de retraso
        /// </summary>
        public int DiasRetraso
        {
            get
            {
                if (EstaPagado || !EstaVencido) return 0;
                return (DateTime.Now.Date - FechaVencimiento.Date).Days;
            }
        }

        /// <summary>
        /// Días para vencer
        /// </summary>
        public int DiasParaVencer => (FechaVencimiento.Date - DateTime.Now.Date).Days;

        /// <summary>
        /// Monto total con mora
        /// </summary>
        public decimal MontoTotalConMora => MontoFinal + Mora;

        /// <summary>
        /// Porcentaje pagado
        /// </summary>
        public decimal PorcentajePagado
        {
            get
            {
                if (MontoTotalConMora == 0) return 100;
                return (MontoPagado / MontoTotalConMora) * 100;
            }
        }

        /// <summary>
        /// Total de pagos realizados
        /// </summary>
        public int TotalPagos => Pagos?.Count ?? 0;

        /// <summary>
        /// Indica si tiene descuento aplicado
        /// </summary>
        public bool TieneDescuento => Descuento > 0;

        /// <summary>
        /// Indica si tiene mora
        /// </summary>
        public bool TieneMora => Mora > 0;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Aplica un descuento al cargo
        /// </summary>
        /// <param name="porcentajeDescuento">Porcentaje de descuento</param>
        /// <param name="motivoDescuento">Motivo del descuento</param>
        public void AplicarDescuento(decimal porcentajeDescuento, string motivoDescuento = null)
        {
            if (EstaPagado)
                throw new InvalidOperationException("No se puede aplicar descuento a un cargo ya pagado");

            if (EstaCancelado)
                throw new InvalidOperationException("No se puede aplicar descuento a un cargo cancelado");

            if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                throw new ArgumentException("El porcentaje debe estar entre 0 y 100");

            PorcentajeDescuento = porcentajeDescuento;
            Descuento = Monto * (porcentajeDescuento / 100);
            MontoFinal = Monto - Descuento;
            SaldoPendiente = MontoFinal - MontoPagado;

            if (!string.IsNullOrWhiteSpace(motivoDescuento))
            {
                Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                    ? $"Descuento aplicado: {motivoDescuento}"
                    : $"{Observaciones}\nDescuento aplicado: {motivoDescuento}";
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Calcula y aplica mora por días de retraso
        /// </summary>
        public void CalcularMora()
        {
            if (!EstaVencido || EstaPagado || EstaCancelado)
            {
                Mora = 0;
                return;
            }

            if (ConceptoPago?.TieneMora == true)
            {
                var moraCalculada = ConceptoPago.CalcularMora(DiasRetraso);
                Mora = moraCalculada;
                SaldoPendiente = MontoFinal + Mora - MontoPagado;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Registra un pago aplicado a este cargo
        /// </summary>
        /// <param name="montoPago">Monto del pago</param>
        public void RegistrarPago(decimal montoPago)
        {
            if (EstaCancelado)
                throw new InvalidOperationException("No se puede registrar pago en un cargo cancelado");

            if (montoPago <= 0)
                throw new ArgumentException("El monto del pago debe ser mayor a cero");

            MontoPagado += montoPago;
            SaldoPendiente = MontoTotalConMora - MontoPagado;

            if (SaldoPendiente <= 0)
            {
                Estatus = EstatusCargo.Pagado;
                FechaPagoCompleto = DateTime.Now;
                SaldoPendiente = 0;
            }
            else if (MontoPagado > 0)
            {
                Estatus = EstatusCargo.Parcial;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Cancela el cargo
        /// </summary>
        /// <param name="motivo">Motivo de cancelación</param>
        /// <param name="usuarioId">ID del usuario que cancela</param>
        public void Cancelar(string motivo, int usuarioId)
        {
            if (EstaPagado)
                throw new InvalidOperationException("No se puede cancelar un cargo ya pagado");

            if (EstaCancelado)
                throw new InvalidOperationException("El cargo ya está cancelado");

            if (MontoPagado > 0)
                throw new InvalidOperationException("No se puede cancelar un cargo con pagos aplicados. Primero debe revertir los pagos.");

            Estatus = EstatusCargo.Cancelado;
            MotivoCancelacion = motivo;
            FechaCancelacion = DateTime.Now;
            CanceladoPorId = usuarioId;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Reactiva un cargo cancelado
        /// </summary>
        public void Reactivar()
        {
            if (!EstaCancelado)
                throw new InvalidOperationException("Solo se pueden reactivar cargos cancelados");

            Estatus = MontoPagado > 0 ? EstatusCargo.Parcial : EstatusCargo.Pendiente;
            MotivoCancelacion = null;
            FechaCancelacion = null;
            CanceladoPorId = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza el estado del cargo según pagos y fechas
        /// </summary>
        public void ActualizarEstatus()
        {
            if (EstaCancelado) return;

            CalcularMora();

            if (SaldoPendiente <= 0 && MontoPagado >= MontoTotalConMora)
            {
                Estatus = EstatusCargo.Pagado;
                if (!FechaPagoCompleto.HasValue)
                    FechaPagoCompleto = DateTime.Now;
            }
            else if (MontoPagado > 0)
            {
                Estatus = EstatusCargo.Parcial;
            }
            else if (EstaVencido)
            {
                Estatus = EstatusCargo.Vencido;
            }
            else
            {
                Estatus = EstatusCargo.Pendiente;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Extiende la fecha de vencimiento
        /// </summary>
        /// <param name="nuevaFecha">Nueva fecha de vencimiento</param>
        /// <param name="motivo">Motivo de la extensión</param>
        public void ExtenderVencimiento(DateTime nuevaFecha, string motivo = null)
        {
            if (EstaPagado)
                throw new InvalidOperationException("No se puede extender el vencimiento de un cargo pagado");

            if (EstaCancelado)
                throw new InvalidOperationException("No se puede extender el vencimiento de un cargo cancelado");

            if (nuevaFecha <= FechaVencimiento)
                throw new ArgumentException("La nueva fecha debe ser posterior a la fecha actual de vencimiento");

            FechaVencimiento = nuevaFecha;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                    ? $"Vencimiento extendido: {motivo}"
                    : $"{Observaciones}\nVencimiento extendido: {motivo}";
            }

            // Recalcular mora con nueva fecha
            CalcularMora();
            ActualizarEstatus();
        }

        /// <summary>
        /// Establece un número de recibo
        /// </summary>
        public void EstablecerNumeroRecibo(string numeroRecibo)
        {
            if (string.IsNullOrWhiteSpace(numeroRecibo))
                throw new ArgumentException("El número de recibo no puede estar vacío");

            NumeroRecibo = numeroRecibo;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida que el cargo sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (Monto < 0)
                errores.Add("El monto no puede ser negativo");

            if (Descuento < 0)
                errores.Add("El descuento no puede ser negativo");

            if (Descuento > Monto)
                errores.Add("El descuento no puede ser mayor al monto");

            if (MontoFinal < 0)
                errores.Add("El monto final no puede ser negativo");

            if (MontoFinal != (Monto - Descuento))
                errores.Add("El monto final debe ser igual al monto menos el descuento");

            if (FechaVencimiento < FechaCreacion)
                errores.Add("La fecha de vencimiento no puede ser anterior a la fecha de creación");

            if (string.IsNullOrWhiteSpace(CicloEscolar))
                errores.Add("El ciclo escolar es requerido");

            if (MontoPagado < 0)
                errores.Add("El monto pagado no puede ser negativo");

            if (SaldoPendiente < 0)
                errores.Add("El saldo pendiente no puede ser negativo");

            return errores;
        }

        #endregion
    }
}