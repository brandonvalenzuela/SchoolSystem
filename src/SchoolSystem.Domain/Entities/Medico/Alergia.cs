using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums.Medico;

namespace SchoolSystem.Domain.Entities.Medico
{
    /// <summary>
    /// Registro de alergias de alumnos
    /// </summary>
    [Table("Alergias")]
    public class Alergia : BaseEntity
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
        /// Tipo de alergia
        /// </summary>
        [Required]
        public TipoAlergia Tipo { get; set; }

        /// <summary>
        /// Nombre del alérgeno
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NombreAlergeno { get; set; }

        #endregion

        #region Gravedad y Síntomas

        /// <summary>
        /// Nivel de gravedad
        /// </summary>
        [Required]
        public NivelGravedad Gravedad { get; set; }

        /// <summary>
        /// Síntomas que presenta
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Sintomas { get; set; }

        /// <summary>
        /// Indica si puede causar anafilaxia
        /// </summary>
        public bool PuedeSerAnafilactica { get; set; }

        #endregion

        #region Diagnóstico

        /// <summary>
        /// Fecha de diagnóstico
        /// </summary>
        public DateTime? FechaDiagnostico { get; set; }

        /// <summary>
        /// Médico que diagnosticó
        /// </summary>
        [StringLength(200)]
        public string MedicoDiagnostico { get; set; }

        /// <summary>
        /// Tipo de prueba diagnóstica realizada
        /// </summary>
        [StringLength(200)]
        public string TipoPrueba { get; set; }

        #endregion

        #region Tratamiento

        /// <summary>
        /// Tratamiento recomendado
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string TratamientoRecomendado { get; set; }

        /// <summary>
        /// Medicamento de emergencia
        /// </summary>
        [StringLength(200)]
        public string MedicamentoEmergencia { get; set; }

        /// <summary>
        /// Indica si debe tener autoinyector (epinefrina)
        /// </summary>
        public bool RequiereAutoinyector { get; set; }

        /// <summary>
        /// Instrucciones de emergencia
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string InstruccionesEmergencia { get; set; }

        #endregion

        #region Control

        /// <summary>
        /// Indica si está activa (algunas alergias pueden superarse)
        /// </summary>
        [Required]
        public bool Activa { get; set; }

        /// <summary>
        /// Fecha en que se superó la alergia (si aplica)
        /// </summary>
        public DateTime? FechaSuperacion { get; set; }

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

        public Alergia()
        {
            Activa = true;
            PuedeSerAnafilactica = false;
            RequiereAutoinyector = false;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si es de alta gravedad
        /// </summary>
        public bool EsGrave => Gravedad == NivelGravedad.Grave;

        /// <summary>
        /// Indica si es alergia alimentaria
        /// </summary>
        public bool EsAlimentaria => Tipo == TipoAlergia.Alimento;

        /// <summary>
        /// Indica si es alergia a medicamento
        /// </summary>
        public bool EsMedicamento => Tipo == TipoAlergia.Medicamento;

        /// <summary>
        /// Indica si es crítica (anafiláctica o grave)
        /// </summary>
        public bool EsCritica => PuedeSerAnafilactica || EsGrave;

        /// <summary>
        /// Años desde el diagnóstico
        /// </summary>
        public int? AniosDesdeDignostico
        {
            get
            {
                if (!FechaDiagnostico.HasValue) return null;
                return DateTime.Now.Year - FechaDiagnostico.Value.Year;
            }
        }

        /// <summary>
        /// Descripción completa de la alergia
        /// </summary>
        public string DescripcionCompleta => $"{NombreAlergeno} ({Tipo}) - {Gravedad}";

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Marca la alergia como superada
        /// </summary>
        public void MarcarComoSuperada(DateTime fechaSuperacion, string observaciones = null)
        {
            if (!Activa)
                throw new InvalidOperationException("La alergia ya está marcada como superada");

            if (fechaSuperacion < FechaDiagnostico)
                throw new ArgumentException("La fecha de superación no puede ser anterior al diagnóstico");

            Activa = false;
            FechaSuperacion = fechaSuperacion;

            if (!string.IsNullOrWhiteSpace(observaciones))
            {
                Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                    ? observaciones
                    : $"{Observaciones}\n[SUPERADA] {observaciones}";
            }
        }

        /// <summary>
        /// Reactiva la alergia
        /// </summary>
        public void Reactivar(string motivo = null)
        {
            if (Activa)
                throw new InvalidOperationException("La alergia ya está activa");

            Activa = true;
            FechaSuperacion = null;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                    ? $"[REACTIVADA] {motivo}"
                    : $"{Observaciones}\n[REACTIVADA] {motivo}";
            }
        }

        /// <summary>
        /// Actualiza el nivel de gravedad
        /// </summary>
        public void ActualizarGravedad(NivelGravedad nuevaGravedad, string motivo = null)
        {
            var gravedadAnterior = Gravedad;
            Gravedad = nuevaGravedad;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                var nota = $"[CAMBIO GRAVEDAD: {gravedadAnterior} → {nuevaGravedad}] {motivo}";
                Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                    ? nota
                    : $"{Observaciones}\n{nota}";
            }
        }

        /// <summary>
        /// Establece si puede ser anafiláctica
        /// </summary>
        public void ConfigurarAnafilaxia(bool puedeSerAnafilactica, bool requiereAutoinyector = false)
        {
            PuedeSerAnafilactica = puedeSerAnafilactica;
            RequiereAutoinyector = puedeSerAnafilactica && requiereAutoinyector;

            if (puedeSerAnafilactica && Gravedad != NivelGravedad.Grave)
            {
                Gravedad = NivelGravedad.Grave;
            }
        }

        /// <summary>
        /// Establece el tratamiento de emergencia
        /// </summary>
        public void EstablecerTratamientoEmergencia(string medicamento, string instrucciones)
        {
            MedicamentoEmergencia = medicamento;
            InstruccionesEmergencia = instrucciones;
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
        }

        /// <summary>
        /// Valida que la alergia sea correcta
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(NombreAlergeno))
                errores.Add("El nombre del alérgeno es requerido");

            if (string.IsNullOrWhiteSpace(Sintomas))
                errores.Add("Los síntomas son requeridos");

            if (FechaDiagnostico.HasValue && FechaDiagnostico.Value > DateTime.Now)
                errores.Add("La fecha de diagnóstico no puede ser futura");

            if (!Activa && !FechaSuperacion.HasValue)
                errores.Add("Si la alergia está inactiva, debe tener fecha de superación");

            if (FechaSuperacion.HasValue && FechaDiagnostico.HasValue && FechaSuperacion.Value < FechaDiagnostico.Value)
                errores.Add("La fecha de superación no puede ser anterior a la fecha de diagnóstico");

            if (PuedeSerAnafilactica && string.IsNullOrWhiteSpace(InstruccionesEmergencia))
                errores.Add("Las alergias anafilácticas deben tener instrucciones de emergencia");

            if (RequiereAutoinyector && string.IsNullOrWhiteSpace(MedicamentoEmergencia))
                errores.Add("Si requiere autoinyector, debe especificar el medicamento de emergencia");

            return errores;
        }

        #endregion
    }
}