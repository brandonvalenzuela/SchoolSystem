using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Biblioteca;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Biblioteca
{
    /// <summary>
    /// Préstamos de recursos de la biblioteca
    /// </summary>
    [Table("Prestamos")]
    public class Prestamo : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Libro o recurso prestado
        /// </summary>
        [Required]
        public int LibroId { get; set; }

        /// <summary>
        /// Alumno que solicitó el préstamo (si aplica)
        /// </summary>
        public int? AlumnoId { get; set; }

        /// <summary>
        /// Maestro que solicitó el préstamo (si aplica)
        /// </summary>
        public int? MaestroId { get; set; }

        /// <summary>
        /// Usuario que registró el préstamo
        /// </summary>
        [Required]
        public int RegistradoPorId { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha y hora del préstamo
        /// </summary>
        [Required]
        public DateTime FechaPrestamo { get; set; }

        /// <summary>
        /// Fecha programada de devolución
        /// </summary>
        [Required]
        public DateTime FechaDevolucionProgramada { get; set; }

        /// <summary>
        /// Fecha real de devolución
        /// </summary>
        public DateTime? FechaDevolucionReal { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Estado del préstamo
        /// </summary>
        [Required]
        public EstadoPrestamo Estado { get; set; }

        /// <summary>
        /// Observaciones del préstamo
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        /// <summary>
        /// Observaciones de la devolución
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ObservacionesDevolucion { get; set; }

        #endregion

        #region Multas

        /// <summary>
        /// Monto de multa por retraso
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoMulta { get; set; }

        /// <summary>
        /// Indica si la multa fue pagada
        /// </summary>
        public bool MultaPagada { get; set; }

        /// <summary>
        /// Fecha de pago de la multa
        /// </summary>
        public DateTime? FechaPagoMulta { get; set; }

        #endregion

        #region Control de Devolución

        /// <summary>
        /// Usuario que recibió la devolución
        /// </summary>
        public int? DevueltoPorId { get; set; }

        /// <summary>
        /// Condición del recurso al devolverlo
        /// </summary>
        [StringLength(50)]
        public string CondicionDevolucion { get; set; }

        /// <summary>
        /// Indica si se reportó como extraviado
        /// </summary>
        public bool ReportadoExtraviado { get; set; }

        /// <summary>
        /// Fecha en que se reportó como extraviado
        /// </summary>
        public DateTime? FechaReporteExtravio { get; set; }

        /// <summary>
        /// Indica si se reportó como dañado
        /// </summary>
        public bool ReportadoDaniado { get; set; }

        #endregion

        #region Renovaciones

        /// <summary>
        /// Cantidad de veces que se renovó el préstamo
        /// </summary>
        public int CantidadRenovaciones { get; set; }

        /// <summary>
        /// Fecha de la última renovación
        /// </summary>
        public DateTime? FechaUltimaRenovacion { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Folio del préstamo
        /// </summary>
        [StringLength(50)]
        public string Folio { get; set; }

        /// <summary>
        /// Indica si fue un préstamo urgente
        /// </summary>
        public bool PrestamoUrgente { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Libro prestado
        /// </summary>
        [ForeignKey("LibroId")]
        public virtual Libro Libro { get; set; }

        /// <summary>
        /// Alumno relacionado
        /// </summary>
        [ForeignKey("AlumnoId")]
        public virtual Alumno Alumno { get; set; }

        /// <summary>
        /// Maestro relacionado
        /// </summary>
        [ForeignKey("MaestroId")]
        public virtual Maestro Maestro { get; set; }

        /// <summary>
        /// Usuario que registró
        /// </summary>
        [ForeignKey("RegistradoPorId")]
        public virtual Usuario RegistradoPor { get; set; }

        /// <summary>
        /// Usuario que recibió la devolución
        /// </summary>
        [ForeignKey("DevueltoPorId")]
        public virtual Usuario DevueltoPor { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public Prestamo()
        {
            FechaPrestamo = DateTime.Now;
            Estado = EstadoPrestamo.Activo;
            MontoMulta = 0;
            MultaPagada = false;
            ReportadoExtraviado = false;
            ReportadoDaniado = false;
            CantidadRenovaciones = 0;
            PrestamoUrgente = false;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si el préstamo está activo
        /// </summary>
        public bool EstaActivo => Estado == EstadoPrestamo.Activo;

        /// <summary>
        /// Indica si fue devuelto
        /// </summary>
        public bool EstaDevuelto => Estado == EstadoPrestamo.Devuelto;

        /// <summary>
        /// Indica si está vencido
        /// </summary>
        public bool EstaVencido => Estado == EstadoPrestamo.Vencido || (EstaActivo && DateTime.Now > FechaDevolucionProgramada);

        /// <summary>
        /// Días de préstamo programados
        /// </summary>
        public int DiasPrestamoProgramado => (FechaDevolucionProgramada.Date - FechaPrestamo.Date).Days;

        /// <summary>
        /// Días de retraso
        /// </summary>
        public int DiasRetraso
        {
            get
            {
                if (!EstaActivo || !EstaVencido) return 0;
                return (DateTime.Now.Date - FechaDevolucionProgramada.Date).Days;
            }
        }

        /// <summary>
        /// Días hasta la devolución programada
        /// </summary>
        public int DiasHastaDevolucion
        {
            get
            {
                if (!EstaActivo) return 0;
                return (FechaDevolucionProgramada.Date - DateTime.Now.Date).Days;
            }
        }

        /// <summary>
        /// Días que estuvo prestado (al momento de devolverlo)
        /// </summary>
        public int? DiasPrestadoReal
        {
            get
            {
                if (!FechaDevolucionReal.HasValue) return null;
                return (FechaDevolucionReal.Value.Date - FechaPrestamo.Date).Days;
            }
        }

        /// <summary>
        /// Indica si fue devuelto con retraso
        /// </summary>
        public bool DevueltoConRetraso
        {
            get
            {
                if (!FechaDevolucionReal.HasValue) return false;
                return FechaDevolucionReal.Value > FechaDevolucionProgramada;
            }
        }

        /// <summary>
        /// Indica si tiene multa pendiente
        /// </summary>
        public bool TieneMultaPendiente => MontoMulta > 0 && !MultaPagada;

        /// <summary>
        /// Indica si es préstamo a alumno
        /// </summary>
        public bool EsPrestamoAlumno => AlumnoId.HasValue;

        /// <summary>
        /// Indica si es préstamo a maestro
        /// </summary>
        public bool EsPrestamoMaestro => MaestroId.HasValue;

        /// <summary>
        /// Nombre del solicitante
        /// </summary>
        public string NombreSolicitante
        {
            get
            {
                if (Alumno != null) return Alumno.NombreCompleto;
                if (Maestro != null) return Maestro.Usuario.NombreCompleto;
                return "No especificado";
            }
        }

        /// <summary>
        /// Indica si se puede renovar
        /// </summary>
        public bool PuedeRenovarse => EstaActivo && !EstaVencido && CantidadRenovaciones < 3;

        /// <summary>
        /// Indica si está próximo a vencer (menos de 3 días)
        /// </summary>
        public bool ProximoAVencer => EstaActivo && DiasHastaDevolucion >= 0 && DiasHastaDevolucion <= 3;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Registra la devolución del libro
        /// </summary>
        /// <param name="usuarioId">ID del usuario que recibe la devolución</param>
        /// <param name="condicion">Condición del recurso</param>
        /// <param name="observaciones">Observaciones de la devolución</param>
        public void RegistrarDevolucion(int usuarioId, string condicion = "Bueno", string observaciones = null)
        {
            if (!EstaActivo)
                throw new InvalidOperationException("Solo se pueden devolver préstamos activos");

            FechaDevolucionReal = DateTime.Now;
            DevueltoPorId = usuarioId;
            CondicionDevolucion = condicion;
            ObservacionesDevolucion = observaciones;
            Estado = EstadoPrestamo.Devuelto;

            // Calcular multa si hay retraso
            if (DevueltoConRetraso)
            {
                var diasRetraso = (FechaDevolucionReal.Value.Date - FechaDevolucionProgramada.Date).Days;
                CalcularMulta(diasRetraso);
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Calcula la multa por días de retraso
        /// </summary>
        /// <param name="diasRetraso">Días de retraso</param>
        /// <param name="montoPorDia">Monto de multa por día (default: 5)</param>
        public void CalcularMulta(int diasRetraso, decimal montoPorDia = 5)
        {
            if (diasRetraso <= 0)
            {
                MontoMulta = 0;
                return;
            }

            MontoMulta = diasRetraso * montoPorDia;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Registra el pago de la multa
        /// </summary>
        public void RegistrarPagoMulta()
        {
            if (MontoMulta <= 0)
                throw new InvalidOperationException("No hay multa para pagar");

            if (MultaPagada)
                throw new InvalidOperationException("La multa ya fue pagada");

            MultaPagada = true;
            FechaPagoMulta = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Renueva el préstamo
        /// </summary>
        /// <param name="diasAdicionales">Días adicionales de préstamo</param>
        public void Renovar(int diasAdicionales = 7)
        {
            if (!PuedeRenovarse)
                throw new InvalidOperationException("Este préstamo no puede renovarse");

            if (diasAdicionales <= 0)
                throw new ArgumentException("Los días adicionales deben ser mayores a cero");

            FechaDevolucionProgramada = FechaDevolucionProgramada.AddDays(diasAdicionales);
            CantidadRenovaciones++;
            FechaUltimaRenovacion = DateTime.Now;

            // Si estaba vencido, volver a activo
            if (Estado == EstadoPrestamo.Vencido)
                Estado = EstadoPrestamo.Activo;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca el préstamo como vencido
        /// </summary>
        public void MarcarComoVencido()
        {
            if (!EstaActivo)
                throw new InvalidOperationException("Solo se pueden marcar como vencidos los préstamos activos");

            Estado = EstadoPrestamo.Vencido;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Reporta el recurso como extraviado
        /// </summary>
        public void ReportarExtravio()
        {
            if (!EstaActivo)
                throw new InvalidOperationException("Solo se pueden reportar extravíos en préstamos activos");

            ReportadoExtraviado = true;
            FechaReporteExtravio = DateTime.Now;
            Estado = EstadoPrestamo.Extraviado;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Reporta el recurso como dañado
        /// </summary>
        public void ReportarDanio()
        {
            ReportadoDaniado = true;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Cancela el préstamo
        /// </summary>
        /// <param name="motivo">Motivo de cancelación</param>
        public void Cancelar(string motivo)
        {
            if (!EstaActivo)
                throw new InvalidOperationException("Solo se pueden cancelar préstamos activos");

            Estado = EstadoPrestamo.Cancelado;
            ObservacionesDevolucion = $"Cancelado: {motivo}";
            FechaDevolucionReal = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Extiende la fecha de devolución programada
        /// </summary>
        public void ExtenderFechaDevolucion(DateTime nuevaFecha, string motivo = null)
        {
            if (!EstaActivo)
                throw new InvalidOperationException("Solo se puede extender la fecha de préstamos activos");

            if (nuevaFecha <= FechaDevolucionProgramada)
                throw new ArgumentException("La nueva fecha debe ser posterior a la fecha actual");

            FechaDevolucionProgramada = nuevaFecha;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                    ? $"Fecha extendida: {motivo}"
                    : $"{Observaciones}\nFecha extendida: {motivo}";
            }

            // Si estaba vencido, volver a activo
            if (Estado == EstadoPrestamo.Vencido)
                Estado = EstadoPrestamo.Activo;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Genera un folio para el préstamo
        /// </summary>
        public void GenerarFolio(string prefijo, int consecutivo)
        {
            Folio = $"{prefijo}{consecutivo:D6}";
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida que el préstamo sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (!AlumnoId.HasValue && !MaestroId.HasValue)
                errores.Add("Debe especificar un alumno o un maestro para el préstamo");

            if (AlumnoId.HasValue && MaestroId.HasValue)
                errores.Add("No puede tener alumno y maestro al mismo tiempo");

            if (FechaDevolucionProgramada <= FechaPrestamo)
                errores.Add("La fecha de devolución programada debe ser posterior a la fecha de préstamo");

            if (FechaDevolucionReal.HasValue && FechaDevolucionReal.Value < FechaPrestamo)
                errores.Add("La fecha de devolución real no puede ser anterior a la fecha de préstamo");

            if (MontoMulta < 0)
                errores.Add("El monto de la multa no puede ser negativo");

            if (MultaPagada && !FechaPagoMulta.HasValue)
                errores.Add("Si la multa está pagada, debe tener fecha de pago");

            if (CantidadRenovaciones < 0)
                errores.Add("La cantidad de renovaciones no puede ser negativa");

            if (ReportadoExtraviado && !FechaReporteExtravio.HasValue)
                errores.Add("Si está reportado como extraviado, debe tener fecha de reporte");

            return errores;
        }

        #endregion
    }
}