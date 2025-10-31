using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums.Finanzas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Finanzas
{
    /// <summary>
    /// Estado de cuenta financiero de alumnos
    /// </summary>
    [Table("EstadosCuenta")]
    public class EstadoCuenta : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Alumno al que pertenece el estado de cuenta
        /// </summary>
        [Required]
        public int AlumnoId { get; set; }

        /// <summary>
        /// Ciclo escolar
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CicloEscolar { get; set; }

        #endregion

        #region Resumen Financiero

        /// <summary>
        /// Total de cargos aplicados
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalCargos { get; set; }

        /// <summary>
        /// Total de descuentos aplicados
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalDescuentos { get; set; }

        /// <summary>
        /// Total de mora acumulada
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalMora { get; set; }

        /// <summary>
        /// Total de pagos realizados
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPagos { get; set; }

        /// <summary>
        /// Saldo pendiente
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SaldoPendiente { get; set; }

        /// <summary>
        /// Saldo a favor (si existe)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SaldoAFavor { get; set; }

        #endregion

        #region Contadores

        /// <summary>
        /// Cantidad de cargos pendientes
        /// </summary>
        public int CargosPendientes { get; set; }

        /// <summary>
        /// Cantidad de cargos pagados
        /// </summary>
        public int CargosPagados { get; set; }

        /// <summary>
        /// Cantidad de cargos vencidos
        /// </summary>
        public int CargosVencidos { get; set; }

        /// <summary>
        /// Cantidad de cargos parcialmente pagados
        /// </summary>
        public int CargosParciales { get; set; }

        /// <summary>
        /// Total de cargos
        /// </summary>
        public int TotalCargosCantidad { get; set; }

        /// <summary>
        /// Total de pagos realizados (cantidad)
        /// </summary>
        public int TotalPagosCantidad { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Indica si tiene adeudos
        /// </summary>
        [Required]
        public bool TieneAdeudos { get; set; }

        /// <summary>
        /// Indica si tiene cargos vencidos
        /// </summary>
        [Required]
        public bool TieneCargosVencidos { get; set; }

        /// <summary>
        /// Indica si está al corriente (sin adeudos)
        /// </summary>
        [Required]
        public bool AlCorriente { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha del último cargo aplicado
        /// </summary>
        public DateTime? FechaUltimoCargo { get; set; }

        /// <summary>
        /// Fecha del último pago realizado
        /// </summary>
        public DateTime? FechaUltimoPago { get; set; }

        /// <summary>
        /// Fecha de la última actualización del estado de cuenta
        /// </summary>
        [Required]
        public DateTime FechaActualizacion { get; set; }

        /// <summary>
        /// Fecha del próximo vencimiento
        /// </summary>
        public DateTime? FechaProximoVencimiento { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Observaciones generales
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        /// <summary>
        /// Indica si requiere atención especial
        /// </summary>
        public bool RequiereAtencion { get; set; }

        /// <summary>
        /// Notas de atención especial
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string NotasAtencion { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Alumno relacionado
        /// </summary>
        [ForeignKey("AlumnoId")]
        public virtual Alumno Alumno { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public EstadoCuenta()
        {
            TotalCargos = 0;
            TotalDescuentos = 0;
            TotalMora = 0;
            TotalPagos = 0;
            SaldoPendiente = 0;
            SaldoAFavor = 0;
            CargosPendientes = 0;
            CargosPagados = 0;
            CargosVencidos = 0;
            CargosParciales = 0;
            TotalCargosCantidad = 0;
            TotalPagosCantidad = 0;
            TieneAdeudos = false;
            TieneCargosVencidos = false;
            AlCorriente = true;
            RequiereAtencion = false;
            FechaActualizacion = DateTime.Now;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Total a pagar (cargos - descuentos + mora)
        /// </summary>
        public decimal TotalAPagar => TotalCargos - TotalDescuentos + TotalMora;

        /// <summary>
        /// Porcentaje pagado
        /// </summary>
        public decimal PorcentajePagado
        {
            get
            {
                if (TotalAPagar == 0) return 100;
                return (TotalPagos / TotalAPagar) * 100;
            }
        }

        /// <summary>
        /// Porcentaje de morosidad
        /// </summary>
        public decimal PorcentajeMorosidad
        {
            get
            {
                if (TotalCargosCantidad == 0) return 0;
                return ((decimal)CargosVencidos / TotalCargosCantidad) * 100;
            }
        }

        /// <summary>
        /// Días desde el último pago
        /// </summary>
        public int? DiasDesdeUltimoPago
        {
            get
            {
                if (!FechaUltimoPago.HasValue) return null;
                return (DateTime.Now.Date - FechaUltimoPago.Value.Date).Days;
            }
        }

        /// <summary>
        /// Días hasta el próximo vencimiento
        /// </summary>
        public int? DiasHastaProximoVencimiento
        {
            get
            {
                if (!FechaProximoVencimiento.HasValue) return null;
                return (FechaProximoVencimiento.Value.Date - DateTime.Now.Date).Days;
            }
        }

        /// <summary>
        /// Indica si tiene próximo vencimiento cercano (menos de 7 días)
        /// </summary>
        public bool TieneVencimientoCercano
        {
            get
            {
                var dias = DiasHastaProximoVencimiento;
                return dias.HasValue && dias.Value >= 0 && dias.Value <= 7;
            }
        }

        /// <summary>
        /// Promedio de pago por transacción
        /// </summary>
        public decimal? PromedioPago
        {
            get
            {
                if (TotalPagosCantidad == 0) return null;
                return TotalPagos / TotalPagosCantidad;
            }
        }

        /// <summary>
        /// Promedio de cargo
        /// </summary>
        public decimal? PromedioCargo
        {
            get
            {
                if (TotalCargosCantidad == 0) return null;
                return TotalCargos / TotalCargosCantidad;
            }
        }

        /// <summary>
        /// Estado general en texto
        /// </summary>
        public string EstadoGeneral
        {
            get
            {
                if (AlCorriente) return "Al Corriente";
                if (TieneCargosVencidos) return "Moroso";
                if (TieneAdeudos) return "Con Adeudos";
                if (SaldoAFavor > 0) return "Saldo a Favor";
                return "Sin Movimientos";
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Recalcula todos los totales del estado de cuenta
        /// </summary>
        /// <param name="cargos">Lista de cargos del alumno</param>
        /// <param name="pagos">Lista de pagos del alumno</param>
        public void RecalcularEstado(IEnumerable<Cargo> cargos, IEnumerable<Pago> pagos)
        {
            // Reset de valores
            TotalCargos = 0;
            TotalDescuentos = 0;
            TotalMora = 0;
            TotalPagos = 0;
            SaldoPendiente = 0;
            CargosPendientes = 0;
            CargosPagados = 0;
            CargosVencidos = 0;
            CargosParciales = 0;
            TotalCargosCantidad = 0;
            TotalPagosCantidad = 0;
            FechaUltimoCargo = null;
            FechaUltimoPago = null;
            FechaProximoVencimiento = null;

            // Calcular desde cargos
            foreach (var cargo in cargos)
            {
                if (cargo.EstaCancelado) continue;

                TotalCargos += cargo.Monto;
                TotalDescuentos += cargo.Descuento;
                TotalMora += cargo.Mora;
                SaldoPendiente += cargo.SaldoPendiente;
                TotalCargosCantidad++;

                // Actualizar contadores por estado
                switch (cargo.Estatus)
                {
                    case EstatusCargo.Pendiente:
                        CargosPendientes++;
                        break;
                    case EstatusCargo.Pagado:
                        CargosPagados++;
                        break;
                    case EstatusCargo.Vencido:
                        CargosVencidos++;
                        break;
                    case EstatusCargo.Parcial:
                        CargosParciales++;
                        break;
                }

                // Actualizar fecha último cargo
                if (!FechaUltimoCargo.HasValue || cargo.FechaCreacion > FechaUltimoCargo.Value)
                    FechaUltimoCargo = cargo.FechaCreacion;

                // Actualizar próximo vencimiento
                if (!cargo.EstaPagado && cargo.FechaVencimiento >= DateTime.Now.Date)
                {
                    if (!FechaProximoVencimiento.HasValue || cargo.FechaVencimiento < FechaProximoVencimiento.Value)
                        FechaProximoVencimiento = cargo.FechaVencimiento;
                }
            }

            // Calcular desde pagos
            foreach (var pago in pagos)
            {
                if (pago.Cancelado) continue;

                TotalPagos += pago.Monto;
                TotalPagosCantidad++;

                // Actualizar fecha último pago
                if (!FechaUltimoPago.HasValue || pago.FechaPago > FechaUltimoPago.Value)
                    FechaUltimoPago = pago.FechaPago;
            }

            // Calcular saldo a favor
            if (SaldoPendiente < 0)
            {
                SaldoAFavor = Math.Abs(SaldoPendiente);
                SaldoPendiente = 0;
            }
            else
            {
                SaldoAFavor = 0;
            }

            // Actualizar indicadores
            TieneAdeudos = SaldoPendiente > 0;
            TieneCargosVencidos = CargosVencidos > 0;
            AlCorriente = !TieneAdeudos && !TieneCargosVencidos;

            // Determinar si requiere atención
            RequiereAtencion = TieneCargosVencidos || (CargosVencidos >= 3);

            FechaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca como requiere atención
        /// </summary>
        /// <param name="notas">Notas sobre por qué requiere atención</param>
        public void MarcarRequiereAtencion(string notas)
        {
            RequiereAtencion = true;
            NotasAtencion = notas;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Quita la marca de requiere atención
        /// </summary>
        public void QuitarRequiereAtencion()
        {
            RequiereAtencion = false;
            NotasAtencion = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Agrega observaciones
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
        /// Verifica si el estado de cuenta está desactualizado
        /// </summary>
        /// <param name="diasMaximos">Días máximos desde la última actualización</param>
        /// <returns>True si está desactualizado</returns>
        public bool EstaDesactualizado(int diasMaximos = 1)
        {
            var diasDesdeActualizacion = (DateTime.Now.Date - FechaActualizacion.Date).Days;
            return diasDesdeActualizacion > diasMaximos;
        }

        /// <summary>
        /// Obtiene un resumen textual del estado de cuenta
        /// </summary>
        public string ObtenerResumen()
        {
            var resumen = $"Estado de Cuenta - {CicloEscolar}\n";
            resumen += $"Estado: {EstadoGeneral}\n";
            resumen += $"Total a Pagar: ${TotalAPagar:N2}\n";
            resumen += $"Total Pagado: ${TotalPagos:N2}\n";
            resumen += $"Saldo Pendiente: ${SaldoPendiente:N2}\n";

            if (SaldoAFavor > 0)
                resumen += $"Saldo a Favor: ${SaldoAFavor:N2}\n";

            if (TieneCargosVencidos)
                resumen += $"Cargos Vencidos: {CargosVencidos}\n";

            if (FechaProximoVencimiento.HasValue)
                resumen += $"Próximo Vencimiento: {FechaProximoVencimiento.Value:dd/MM/yyyy}\n";

            return resumen;
        }

        /// <summary>
        /// Valida el estado de cuenta
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(CicloEscolar))
                errores.Add("El ciclo escolar es requerido");

            if (TotalCargos < 0)
                errores.Add("El total de cargos no puede ser negativo");

            if (TotalDescuentos < 0)
                errores.Add("El total de descuentos no puede ser negativo");

            if (TotalPagos < 0)
                errores.Add("El total de pagos no puede ser negativo");

            if (TotalDescuentos > TotalCargos)
                errores.Add("Los descuentos no pueden ser mayores a los cargos");

            if (SaldoPendiente < 0 && SaldoAFavor <= 0)
                errores.Add("No puede haber saldo pendiente negativo sin saldo a favor");

            if (CargosPendientes < 0 || CargosPagados < 0 || CargosVencidos < 0 || CargosParciales < 0)
                errores.Add("Los contadores de cargos no pueden ser negativos");

            var totalContadores = CargosPendientes + CargosPagados + CargosVencidos + CargosParciales;
            if (totalContadores != TotalCargosCantidad)
                errores.Add("La suma de los contadores de estado no coincide con el total de cargos");

            return errores;
        }

        #endregion
    }
}