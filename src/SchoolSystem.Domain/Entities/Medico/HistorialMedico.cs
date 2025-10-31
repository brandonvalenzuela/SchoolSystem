using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Medico;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Medico
{
    /// <summary>
    /// Historial de incidentes médicos de alumnos
    /// </summary>
    [Table("HistorialMedico")]
    public class HistorialMedico : BaseEntity
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
        /// Tipo de incidente médico
        /// </summary>
        [Required]
        public TipoIncidenteMedico TipoIncidente { get; set; }

        /// <summary>
        /// Fecha y hora del incidente
        /// </summary>
        [Required]
        public DateTime FechaIncidente { get; set; }

        #endregion

        #region Descripción del Incidente

        /// <summary>
        /// Descripción detallada del incidente
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Síntomas presentados
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Sintomas { get; set; }

        /// <summary>
        /// Lugar donde ocurrió el incidente
        /// </summary>
        [StringLength(200)]
        public string LugarIncidente { get; set; }

        /// <summary>
        /// Indica si ocurrió en la escuela
        /// </summary>
        public bool OcurrioEnEscuela { get; set; }

        #endregion

        #region Diagnóstico

        /// <summary>
        /// Diagnóstico médico
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Diagnostico { get; set; }

        /// <summary>
        /// Gravedad del incidente
        /// </summary>
        public NivelGravedad? Gravedad { get; set; }

        #endregion

        #region Atención Médica

        /// <summary>
        /// Tratamiento aplicado
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string TratamientoAplicado { get; set; }

        /// <summary>
        /// Médico que atendió
        /// </summary>
        [StringLength(200)]
        public string MedicoAtencion { get; set; }

        /// <summary>
        /// Especialidad del médico
        /// </summary>
        [StringLength(100)]
        public string EspecialidadMedico { get; set; }

        /// <summary>
        /// Lugar donde fue atendido
        /// </summary>
        [StringLength(200)]
        public string LugarAtencion { get; set; }

        /// <summary>
        /// Institución donde fue atendido
        /// </summary>
        [StringLength(200)]
        public string InstitucionAtencion { get; set; }

        #endregion

        #region Hospitalización

        /// <summary>
        /// Indica si requirió hospitalización
        /// </summary>
        public bool RequirioHospitalizacion { get; set; }

        /// <summary>
        /// Fecha de ingreso hospitalario
        /// </summary>
        public DateTime? FechaIngresoHospital { get; set; }

        /// <summary>
        /// Fecha de alta hospitalaria
        /// </summary>
        public DateTime? FechaAltaHospital { get; set; }

        /// <summary>
        /// Días de hospitalización
        /// </summary>
        public int? DiasHospitalizado { get; set; }

        /// <summary>
        /// Motivo de hospitalización
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string MotivoHospitalizacion { get; set; }

        #endregion

        #region Medicamentos y Procedimientos

        /// <summary>
        /// Medicamentos recetados
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string MedicamentosRecetados { get; set; }

        /// <summary>
        /// Procedimientos realizados
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ProcedimientosRealizados { get; set; }

        /// <summary>
        /// Indica si requirió cirugía
        /// </summary>
        public bool RequirioCirugia { get; set; }

        /// <summary>
        /// Descripción de la cirugía
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DescripcionCirugia { get; set; }

        #endregion

        #region Seguimiento

        /// <summary>
        /// Requiere seguimiento médico
        /// </summary>
        public bool RequiereSeguimiento { get; set; }

        /// <summary>
        /// Fecha de próxima consulta
        /// </summary>
        public DateTime? FechaProximaConsulta { get; set; }

        /// <summary>
        /// Indicaciones de seguimiento
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string IndicacionesSeguimiento { get; set; }

        /// <summary>
        /// Indica si el caso está cerrado
        /// </summary>
        public bool CasoCerrado { get; set; }

        /// <summary>
        /// Fecha de cierre del caso
        /// </summary>
        public DateTime? FechaCierreCaso { get; set; }

        #endregion

        #region Notificación

        /// <summary>
        /// Indica si se notificó a los padres
        /// </summary>
        public bool PadresNotificados { get; set; }

        /// <summary>
        /// Fecha y hora de notificación a padres
        /// </summary>
        public DateTime? FechaNotificacionPadres { get; set; }

        /// <summary>
        /// Persona que notificó
        /// </summary>
        [StringLength(200)]
        public string PersonaQueNotifico { get; set; }

        /// <summary>
        /// Medio de notificación
        /// </summary>
        [StringLength(50)]
        public string MedioNotificacion { get; set; }

        #endregion

        #region Documentación

        /// <summary>
        /// URL de documentos médicos
        /// </summary>
        [StringLength(500)]
        public string DocumentosUrl { get; set; }

        /// <summary>
        /// Observaciones adicionales
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        /// <summary>
        /// Usuario que registró el incidente
        /// </summary>
        public int? RegistradoPorId { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Expediente médico relacionado
        /// </summary>
        [ForeignKey("ExpedienteMedicoId")]
        public virtual ExpedienteMedico ExpedienteMedico { get; set; }

        /// <summary>
        /// Usuario que registró
        /// </summary>
        [ForeignKey("RegistradoPorId")]
        public virtual Usuario RegistradoPor { get; set; }

        #endregion

        #region Constructor

        public HistorialMedico()
        {
            FechaIncidente = DateTime.Now;
            OcurrioEnEscuela = false;
            RequirioHospitalizacion = false;
            RequirioCirugia = false;
            RequiereSeguimiento = false;
            CasoCerrado = false;
            PadresNotificados = false;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si es grave
        /// </summary>
        public bool EsGrave => Gravedad == NivelGravedad.Grave;

        /// <summary>
        /// Indica si fue un accidente
        /// </summary>
        public bool EsAccidente => TipoIncidente == TipoIncidenteMedico.Accidente;

        /// <summary>
        /// Indica si fue una enfermedad
        /// </summary>
        public bool EsEnfermedad => TipoIncidente == TipoIncidenteMedico.Enfermedad;

        /// <summary>
        /// Días desde el incidente
        /// </summary>
        public int DiasDesdeIncidente => (DateTime.Now.Date - FechaIncidente.Date).Days;

        /// <summary>
        /// Indica si está hospitalizado actualmente
        /// </summary>
        public bool EstaHospitalizado => RequirioHospitalizacion &&
                                         FechaIngresoHospital.HasValue &&
                                         !FechaAltaHospital.HasValue;

        /// <summary>
        /// Días de hospitalización calculados
        /// </summary>
        public int? DiasHospitalizacionCalculados
        {
            get
            {
                if (!FechaIngresoHospital.HasValue) return null;

                var fechaFin = FechaAltaHospital ?? DateTime.Now;
                return (fechaFin.Date - FechaIngresoHospital.Value.Date).Days;
            }
        }

        /// <summary>
        /// Indica si tiene documentos adjuntos
        /// </summary>
        public bool TieneDocumentos => !string.IsNullOrWhiteSpace(DocumentosUrl);

        /// <summary>
        /// Indica si tiene seguimiento pendiente
        /// </summary>
        public bool TieneSeguimientoPendiente => RequiereSeguimiento && !CasoCerrado;

        /// <summary>
        /// Días hasta la próxima consulta
        /// </summary>
        public int? DiasHastaProximaConsulta
        {
            get
            {
                if (!FechaProximaConsulta.HasValue) return null;
                return (FechaProximaConsulta.Value.Date - DateTime.Now.Date).Days;
            }
        }

        /// <summary>
        /// Indica si la próxima consulta está próxima (menos de 7 días)
        /// </summary>
        public bool ProximaConsultaCercana
        {
            get
            {
                var dias = DiasHastaProximaConsulta;
                return dias.HasValue && dias.Value > 0 && dias.Value <= 7;
            }
        }

        /// <summary>
        /// Indica si la consulta está vencida
        /// </summary>
        public bool ConsultaVencida => FechaProximaConsulta.HasValue &&
                                       DateTime.Now.Date > FechaProximaConsulta.Value.Date &&
                                       !CasoCerrado;

        /// <summary>
        /// Resumen breve del incidente
        /// </summary>
        public string ResumenBreve
        {
            get
            {
                var resumen = $"{TipoIncidente} - {FechaIncidente:dd/MM/yyyy}";
                if (!string.IsNullOrWhiteSpace(Diagnostico))
                    resumen += $" - {Diagnostico}";
                return resumen;
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Registra la hospitalización
        /// </summary>
        public void RegistrarHospitalizacion(DateTime fechaIngreso, string motivo, string institucion)
        {
            if (fechaIngreso < FechaIncidente)
                throw new ArgumentException("La fecha de ingreso no puede ser anterior al incidente");

            RequirioHospitalizacion = true;
            FechaIngresoHospital = fechaIngreso;
            MotivoHospitalizacion = motivo;
            InstitucionAtencion = institucion;
            Gravedad = NivelGravedad.Grave;
        }

        /// <summary>
        /// Registra el alta hospitalaria
        /// </summary>
        public void RegistrarAlta(DateTime fechaAlta, string indicaciones = null)
        {
            if (!RequirioHospitalizacion)
                throw new InvalidOperationException("No hay hospitalización registrada");

            if (!FechaIngresoHospital.HasValue)
                throw new InvalidOperationException("No hay fecha de ingreso registrada");

            if (fechaAlta < FechaIngresoHospital.Value)
                throw new ArgumentException("La fecha de alta no puede ser anterior a la fecha de ingreso");

            FechaAltaHospital = fechaAlta;
            DiasHospitalizado = (fechaAlta.Date - FechaIngresoHospital.Value.Date).Days;

            if (!string.IsNullOrWhiteSpace(indicaciones))
            {
                IndicacionesSeguimiento = indicaciones;
                RequiereSeguimiento = true;
            }
        }

        /// <summary>
        /// Registra una cirugía
        /// </summary>
        public void RegistrarCirugia(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción de la cirugía es requerida");

            RequirioCirugia = true;
            DescripcionCirugia = descripcion;
            Gravedad = NivelGravedad.Grave;
        }

        /// <summary>
        /// Programa seguimiento médico
        /// </summary>
        public void ProgramarSeguimiento(DateTime fechaProximaConsulta, string indicaciones)
        {
            RequiereSeguimiento = true;
            FechaProximaConsulta = fechaProximaConsulta;
            IndicacionesSeguimiento = indicaciones;
        }

        /// <summary>
        /// Cierra el caso
        /// </summary>
        public void CerrarCaso(string observaciones = null)
        {
            if (CasoCerrado)
                throw new InvalidOperationException("El caso ya está cerrado");

            CasoCerrado = true;
            FechaCierreCaso = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(observaciones))
            {
                AgregarObservaciones($"[CASO CERRADO] {observaciones}");
            }
        }

        /// <summary>
        /// Reabre el caso
        /// </summary>
        public void ReabrirCaso(string motivo)
        {
            if (!CasoCerrado)
                throw new InvalidOperationException("El caso no está cerrado");

            CasoCerrado = false;
            FechaCierreCaso = null;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                AgregarObservaciones($"[CASO REABIERTO] {motivo}");
            }
        }

        /// <summary>
        /// Registra la notificación a padres
        /// </summary>
        public void RegistrarNotificacionPadres(string personaQueNotifico, string medio)
        {
            PadresNotificados = true;
            FechaNotificacionPadres = DateTime.Now;
            PersonaQueNotifico = personaQueNotifico;
            MedioNotificacion = medio;
        }

        /// <summary>
        /// Adjunta documentos médicos
        /// </summary>
        public void AdjuntarDocumentos(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("La URL de los documentos es requerida");

            DocumentosUrl = url;
        }

        /// <summary>
        /// Actualiza el diagnóstico
        /// </summary>
        public void ActualizarDiagnostico(string diagnostico, NivelGravedad? gravedad = null)
        {
            Diagnostico = diagnostico;

            if (gravedad.HasValue)
                Gravedad = gravedad.Value;
        }

        /// <summary>
        /// Agrega observaciones
        /// </summary>
        public void AgregarObservaciones(string observaciones)
        {
            if (string.IsNullOrWhiteSpace(observaciones))
                return;

            Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                ? $"[{DateTime.Now:dd/MM/yyyy HH:mm}] {observaciones}"
                : $"{Observaciones}\n[{DateTime.Now:dd/MM/yyyy HH:mm}] {observaciones}";
        }

        /// <summary>
        /// Obtiene un resumen del incidente
        /// </summary>
        public string ObtenerResumen()
        {
            var resumen = $"=== {TipoIncidente.ToString().ToUpper()} ===\n";
            resumen += $"Fecha: {FechaIncidente:dd/MM/yyyy HH:mm}\n";

            if (!string.IsNullOrWhiteSpace(Descripcion))
                resumen += $"\nDescripción: {Descripcion}\n";

            if (!string.IsNullOrWhiteSpace(Diagnostico))
                resumen += $"\nDiagnóstico: {Diagnostico}\n";

            if (Gravedad.HasValue)
                resumen += $"Gravedad: {Gravedad}\n";

            if (RequirioHospitalizacion)
            {
                resumen += $"\n🏥 HOSPITALIZACIÓN: {DiasHospitalizado ?? 0} días\n";
                if (!string.IsNullOrWhiteSpace(InstitucionAtencion))
                    resumen += $"   Institución: {InstitucionAtencion}\n";
            }

            if (RequirioCirugia)
                resumen += $"\n⚕️ CIRUGÍA: {DescripcionCirugia}\n";

            if (!string.IsNullOrWhiteSpace(TratamientoAplicado))
                resumen += $"\nTratamiento: {TratamientoAplicado}\n";

            if (RequiereSeguimiento && !CasoCerrado)
            {
                resumen += $"\n📋 SEGUIMIENTO PENDIENTE\n";
                if (FechaProximaConsulta.HasValue)
                    resumen += $"   Próxima consulta: {FechaProximaConsulta:dd/MM/yyyy}\n";
            }

            return resumen;
        }

        /// <summary>
        /// Valida que el historial médico sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Descripcion))
                errores.Add("La descripción del incidente es requerida");

            if (FechaIncidente > DateTime.Now)
                errores.Add("La fecha del incidente no puede ser futura");

            if (RequirioHospitalizacion)
            {
                if (!FechaIngresoHospital.HasValue)
                    errores.Add("Si requirió hospitalización, debe tener fecha de ingreso");

                if (FechaAltaHospital.HasValue && FechaAltaHospital.Value < FechaIngresoHospital)
                    errores.Add("La fecha de alta no puede ser anterior a la fecha de ingreso");

                if (string.IsNullOrWhiteSpace(MotivoHospitalizacion))
                    errores.Add("Si requirió hospitalización, debe especificar el motivo");
            }

            if (RequirioCirugia && string.IsNullOrWhiteSpace(DescripcionCirugia))
                errores.Add("Si requirió cirugía, debe especificar la descripción");

            if (RequiereSeguimiento && !CasoCerrado && !FechaProximaConsulta.HasValue)
                errores.Add("Si requiere seguimiento, debe programar la próxima consulta");

            if (CasoCerrado && !FechaCierreCaso.HasValue)
                errores.Add("Si el caso está cerrado, debe tener fecha de cierre");

            if (PadresNotificados && !FechaNotificacionPadres.HasValue)
                errores.Add("Si los padres fueron notificados, debe tener fecha de notificación");

            return errores;
        }

        #endregion
    }
}