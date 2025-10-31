using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums.Medico;

namespace SchoolSystem.Domain.Entities.Medico
{
    /// <summary>
    /// Registro de medicamentos que toman los alumnos
    /// </summary>
    [Table("Medicamentos")]
    public class Medicamento : BaseEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Expediente médico relacionado
        /// </summary>
        [Required]
        public int ExpedienteMedicoId { get; set; }

        /// <summary>
        /// Nombre del medicamento
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NombreMedicamento { get; set; }

        /// <summary>
        /// Nombre genérico (principio activo)
        /// </summary>
        [StringLength(200)]
        public string NombreGenerico { get; set; }

        #endregion

        #region Prescripción

        /// <summary>
        /// Dosis prescrita
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Dosis { get; set; }

        /// <summary>
        /// Frecuencia de administración
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Frecuencia { get; set; }

        /// <summary>
        /// Vía de administración
        /// </summary>
        [Required]
        public ViaAdministracion Via { get; set; }

        /// <summary>
        /// Motivo o indicación médica
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Indicacion { get; set; }

        #endregion

        #region Médico Prescriptor

        /// <summary>
        /// Médico que prescribió el medicamento
        /// </summary>
        [StringLength(200)]
        public string MedicoPrescriptor { get; set; }

        /// <summary>
        /// Especialidad del médico
        /// </summary>
        [StringLength(100)]
        public string EspecialidadMedico { get; set; }

        /// <summary>
        /// Número de cédula profesional
        /// </summary>
        [StringLength(50)]
        public string CedulaMedico { get; set; }

        #endregion

        #region Vigencia

        /// <summary>
        /// Fecha de inicio del tratamiento
        /// </summary>
        [Required]
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha de fin del tratamiento (si aplica)
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Indica si es un tratamiento crónico
        /// </summary>
        public bool TratamientoCronico { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Estado del medicamento
        /// </summary>
        [Required]
        public EstadoMedicamento Estado { get; set; }

        /// <summary>
        /// Fecha de suspensión (si aplica)
        /// </summary>
        public DateTime? FechaSuspension { get; set; }

        /// <summary>
        /// Motivo de suspensión
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string MotivoSuspension { get; set; }

        #endregion

        #region Instrucciones

        /// <summary>
        /// Instrucciones especiales de administración
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string InstruccionesEspeciales { get; set; }

        /// <summary>
        /// Precauciones o advertencias
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Precauciones { get; set; }

        /// <summary>
        /// Efectos secundarios conocidos
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string EfectosSecundarios { get; set; }

        #endregion

        #region Control Escolar

        /// <summary>
        /// Indica si debe administrarse en horario escolar
        /// </summary>
        public bool AdministrarEnEscuela { get; set; }

        /// <summary>
        /// Horario de administración en la escuela
        /// </summary>
        [StringLength(100)]
        public string HorarioEscolar { get; set; }

        /// <summary>
        /// Indica si requiere supervisión especial
        /// </summary>
        public bool RequiereSupervision { get; set; }

        /// <summary>
        /// Indica si el alumno puede auto-administrarlo
        /// </summary>
        public bool PuedeAutoAdministrar { get; set; }

        #endregion

        #region Documentación

        /// <summary>
        /// URL de la receta médica
        /// </summary>
        [StringLength(500)]
        public string RecetaUrl { get; set; }

        /// <summary>
        /// Observaciones adicionales
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Expediente médico relacionado
        /// </summary>
        [ForeignKey("ExpedienteMedicoId")]
        public virtual ExpedienteMedico ExpedienteMedico { get; set; }

        #endregion

        #region Constructor

        public Medicamento()
        {
            FechaInicio = DateTime.Now;
            Estado = EstadoMedicamento.Activo;
            TratamientoCronico = false;
            AdministrarEnEscuela = false;
            RequiereSupervision = false;
            PuedeAutoAdministrar = false;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si está activo
        /// </summary>
        public bool EstaActivo => Estado == EstadoMedicamento.Activo;

        /// <summary>
        /// Indica si está suspendido
        /// </summary>
        public bool EstaSuspendido => Estado == EstadoMedicamento.Suspendido;

        /// <summary>
        /// Indica si el tratamiento finalizó
        /// </summary>
        public bool TratamientoFinalizado => Estado == EstadoMedicamento.Finalizado;

        /// <summary>
        /// Indica si el tratamiento está vigente
        /// </summary>
        public bool EstaVigente
        {
            get
            {
                if (!EstaActivo) return false;
                if (TratamientoCronico) return true;
                if (!FechaFin.HasValue) return true;
                return DateTime.Now.Date <= FechaFin.Value.Date;
            }
        }

        /// <summary>
        /// Indica si el tratamiento ha vencido
        /// </summary>
        public bool TratamientoVencido => FechaFin.HasValue && DateTime.Now.Date > FechaFin.Value.Date && EstaActivo;

        /// <summary>
        /// Días de tratamiento
        /// </summary>
        public int DiasDesdeInicio => (DateTime.Now.Date - FechaInicio.Date).Days;

        /// <summary>
        /// Días hasta el fin del tratamiento
        /// </summary>
        public int? DiasHastaFin
        {
            get
            {
                if (!FechaFin.HasValue) return null;
                return (FechaFin.Value.Date - DateTime.Now.Date).Days;
            }
        }

        /// <summary>
        /// Duración total del tratamiento en días
        /// </summary>
        public int? DuracionTratamiento
        {
            get
            {
                if (!FechaFin.HasValue) return null;
                return (FechaFin.Value.Date - FechaInicio.Date).Days;
            }
        }

        /// <summary>
        /// Indica si tiene receta adjunta
        /// </summary>
        public bool TieneReceta => !string.IsNullOrWhiteSpace(RecetaUrl);

        /// <summary>
        /// Descripción completa del medicamento
        /// </summary>
        public string DescripcionCompleta => $"{NombreMedicamento} - {Dosis} - {Frecuencia}";

        /// <summary>
        /// Indica si está próximo a vencer (menos de 7 días)
        /// </summary>
        public bool ProximoAVencer
        {
            get
            {
                var dias = DiasHastaFin;
                return dias.HasValue && dias.Value > 0 && dias.Value <= 7;
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Suspende el medicamento
        /// </summary>
        public void Suspender(string motivo, DateTime? fechaSuspension = null)
        {
            if (!EstaActivo)
                throw new InvalidOperationException("Solo se pueden suspender medicamentos activos");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("El motivo de suspensión es requerido");

            Estado = EstadoMedicamento.Suspendido;
            FechaSuspension = fechaSuspension ?? DateTime.Now;
            MotivoSuspension = motivo;
        }

        /// <summary>
        /// Reactiva el medicamento
        /// </summary>
        public void Reactivar(string motivo = null)
        {
            if (EstaActivo)
                throw new InvalidOperationException("El medicamento ya está activo");

            Estado = EstadoMedicamento.Activo;
            FechaSuspension = null;
            MotivoSuspension = null;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                AgregarObservaciones($"[REACTIVADO] {motivo}");
            }
        }

        /// <summary>
        /// Finaliza el tratamiento
        /// </summary>
        public void FinalizarTratamiento(DateTime? fechaFin = null, string observaciones = null)
        {
            if (!EstaActivo)
                throw new InvalidOperationException("Solo se pueden finalizar medicamentos activos");

            Estado = EstadoMedicamento.Finalizado;
            FechaFin = fechaFin ?? DateTime.Now;

            if (!string.IsNullOrWhiteSpace(observaciones))
            {
                AgregarObservaciones($"[FINALIZADO] {observaciones}");
            }
        }

        /// <summary>
        /// Extiende la fecha de fin del tratamiento
        /// </summary>
        public void ExtenderTratamiento(DateTime nuevaFechaFin, string motivo = null)
        {
            if (!EstaActivo)
                throw new InvalidOperationException("Solo se pueden extender medicamentos activos");

            if (nuevaFechaFin <= (FechaFin ?? FechaInicio))
                throw new ArgumentException("La nueva fecha debe ser posterior a la fecha actual de fin");

            FechaFin = nuevaFechaFin;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                AgregarObservaciones($"[EXTENDIDO] {motivo}");
            }
        }

        /// <summary>
        /// Marca como tratamiento crónico
        /// </summary>
        public void MarcarComoCronico()
        {
            TratamientoCronico = true;
            FechaFin = null;
        }

        /// <summary>
        /// Configura la administración en escuela
        /// </summary>
        public void ConfigurarAdministracionEscolar(bool administrarEnEscuela, string horario = null,
                                                     bool requiereSupervision = false, bool puedeAutoAdministrar = false)
        {
            AdministrarEnEscuela = administrarEnEscuela;

            if (administrarEnEscuela)
            {
                HorarioEscolar = horario;
                RequiereSupervision = requiereSupervision;
                PuedeAutoAdministrar = puedeAutoAdministrar;
            }
            else
            {
                HorarioEscolar = null;
                RequiereSupervision = false;
                PuedeAutoAdministrar = false;
            }
        }

        /// <summary>
        /// Adjunta receta médica
        /// </summary>
        public void AdjuntarReceta(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("La URL de la receta es requerida");

            RecetaUrl = url;
        }

        /// <summary>
        /// Actualiza la dosis
        /// </summary>
        public void ActualizarDosis(string nuevaDosis, string motivo = null)
        {
            if (string.IsNullOrWhiteSpace(nuevaDosis))
                throw new ArgumentException("La nueva dosis es requerida");

            var dosisAnterior = Dosis;
            Dosis = nuevaDosis;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                AgregarObservaciones($"[CAMBIO DOSIS: {dosisAnterior} → {nuevaDosis}] {motivo}");
            }
        }

        /// <summary>
        /// Actualiza la frecuencia
        /// </summary>
        public void ActualizarFrecuencia(string nuevaFrecuencia, string motivo = null)
        {
            if (string.IsNullOrWhiteSpace(nuevaFrecuencia))
                throw new ArgumentException("La nueva frecuencia es requerida");

            var frecuenciaAnterior = Frecuencia;
            Frecuencia = nuevaFrecuencia;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                AgregarObservaciones($"[CAMBIO FRECUENCIA: {frecuenciaAnterior} → {nuevaFrecuencia}] {motivo}");
            }
        }

        /// <summary>
        /// Agrega observaciones
        /// </summary>
        public void AgregarObservaciones(string observaciones)
        {
            if (string.IsNullOrWhiteSpace(observaciones))
                return;

            Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                ? $"[{DateTime.Now:dd/MM/yyyy}] {observaciones}"
                : $"{Observaciones}\n[{DateTime.Now:dd/MM/yyyy}] {observaciones}";
        }

        /// <summary>
        /// Valida que el medicamento sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(NombreMedicamento))
                errores.Add("El nombre del medicamento es requerido");

            if (string.IsNullOrWhiteSpace(Dosis))
                errores.Add("La dosis es requerida");

            if (string.IsNullOrWhiteSpace(Frecuencia))
                errores.Add("La frecuencia es requerida");

            if (string.IsNullOrWhiteSpace(Indicacion))
                errores.Add("La indicación médica es requerida");

            if (FechaInicio > DateTime.Now)
                errores.Add("La fecha de inicio no puede ser futura");

            if (FechaFin.HasValue && FechaFin.Value < FechaInicio)
                errores.Add("La fecha de fin no puede ser anterior a la fecha de inicio");

            if (EstaSuspendido)
            {
                if (!FechaSuspension.HasValue)
                    errores.Add("Si está suspendido, debe tener fecha de suspensión");

                if (string.IsNullOrWhiteSpace(MotivoSuspension))
                    errores.Add("Si está suspendido, debe tener motivo de suspensión");
            }

            if (AdministrarEnEscuela && string.IsNullOrWhiteSpace(HorarioEscolar))
                errores.Add("Si se administra en escuela, debe especificar el horario");

            if (RequiereSupervision && !AdministrarEnEscuela)
                errores.Add("Si requiere supervisión, debe administrarse en escuela");

            if (PuedeAutoAdministrar && RequiereSupervision)
                errores.Add("No puede requerir supervisión y auto-administración al mismo tiempo");

            return errores;
        }

        #endregion
    }
}