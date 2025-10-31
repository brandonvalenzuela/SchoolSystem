using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Enums.Medico;

namespace SchoolSystem.Domain.Entities.Medico
{
    /// <summary>
    /// Expediente médico de los alumnos
    /// </summary>
    [Table("ExpedientesMedicos")]
    public class ExpedienteMedico : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Alumno relacionado
        /// </summary>
        [Required]
        public int AlumnoId { get; set; }

        #endregion

        #region Información Médica Básica

        /// <summary>
        /// Tipo de sangre
        /// </summary>
        public TipoSangre? TipoSangre { get; set; }

        /// <summary>
        /// Peso en kilogramos
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Peso { get; set; }

        /// <summary>
        /// Estatura en centímetros
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Estatura { get; set; }

        /// <summary>
        /// Índice de Masa Corporal (calculado)
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? IMC { get; set; }

        #endregion

        #region Condiciones Médicas

        /// <summary>
        /// Alergias conocidas (descripción general)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Alergias { get; set; }

        /// <summary>
        /// Condiciones médicas crónicas o importantes
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string CondicionesMedicas { get; set; }

        /// <summary>
        /// Medicamentos que toma regularmente
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string MedicamentosRegulares { get; set; }

        /// <summary>
        /// Restricciones o limitaciones físicas
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Restricciones { get; set; }

        /// <summary>
        /// Indica si requiere atención especial
        /// </summary>
        public bool RequiereAtencionEspecial { get; set; }

        /// <summary>
        /// Detalles de la atención especial requerida
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DetallesAtencionEspecial { get; set; }

        #endregion

        #region Contacto de Emergencia

        /// <summary>
        /// Nombre del contacto de emergencia
        /// </summary>
        [StringLength(200)]
        public string ContactoEmergenciaNombre { get; set; }

        /// <summary>
        /// Teléfono del contacto de emergencia
        /// </summary>
        [StringLength(20)]
        public string ContactoEmergenciaTelefono { get; set; }

        /// <summary>
        /// Teléfono alternativo del contacto de emergencia
        /// </summary>
        [StringLength(20)]
        public string ContactoEmergenciaTelefonoAlt { get; set; }

        /// <summary>
        /// Parentesco del contacto de emergencia
        /// </summary>
        [StringLength(50)]
        public string ContactoEmergenciaParentesco { get; set; }

        #endregion

        #region Seguro Médico

        /// <summary>
        /// Tiene seguro médico
        /// </summary>
        public bool TieneSeguro { get; set; }

        /// <summary>
        /// Nombre de la aseguradora
        /// </summary>
        [StringLength(200)]
        public string SeguroNombre { get; set; }

        /// <summary>
        /// Número de póliza
        /// </summary>
        [StringLength(100)]
        public string SeguroNumeroPoliza { get; set; }

        /// <summary>
        /// Vigencia del seguro
        /// </summary>
        public DateTime? SeguroVigencia { get; set; }

        /// <summary>
        /// Teléfono de la aseguradora
        /// </summary>
        [StringLength(20)]
        public string SeguroTelefono { get; set; }

        #endregion

        #region Médico Particular

        /// <summary>
        /// Nombre del médico particular
        /// </summary>
        [StringLength(200)]
        public string MedicoNombre { get; set; }

        /// <summary>
        /// Especialidad del médico
        /// </summary>
        [StringLength(100)]
        public string MedicoEspecialidad { get; set; }

        /// <summary>
        /// Teléfono del médico
        /// </summary>
        [StringLength(20)]
        public string MedicoTelefono { get; set; }

        /// <summary>
        /// Dirección del consultorio
        /// </summary>
        [StringLength(300)]
        public string MedicoDireccion { get; set; }

        #endregion

        #region Vacunación

        /// <summary>
        /// Indica si tiene esquema de vacunación completo
        /// </summary>
        public bool VacunacionCompleta { get; set; }

        /// <summary>
        /// Observaciones sobre vacunación
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string VacunacionObservaciones { get; set; }

        #endregion

        #region Control y Auditoría

        /// <summary>
        /// Fecha de última actualización del expediente
        /// </summary>
        public DateTime? FechaUltimaActualizacion { get; set; }

        /// <summary>
        /// Fecha de última revisión médica
        /// </summary>
        public DateTime? FechaUltimaRevision { get; set; }

        /// <summary>
        /// Observaciones generales
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        /// <summary>
        /// Indica si el expediente está completo
        /// </summary>
        public bool ExpedienteCompleto { get; set; }

        /// <summary>
        /// Indica si está activo
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Alumno relacionado
        /// </summary>
        [ForeignKey("AlumnoId")]
        public virtual Alumno Alumno { get; set; }

        /// <summary>
        /// Vacunas aplicadas
        /// </summary>
        public virtual ICollection<Vacuna> Vacunas { get; set; }

        /// <summary>
        /// Alergias registradas
        /// </summary>
        public virtual ICollection<Alergia> AlergiasRegistradas { get; set; }

        /// <summary>
        /// Medicamentos registrados
        /// </summary>
        public virtual ICollection<Medicamento> Medicamentos { get; set; }

        /// <summary>
        /// Historial médico
        /// </summary>
        public virtual ICollection<HistorialMedico> HistorialMedico { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public ExpedienteMedico()
        {
            RequiereAtencionEspecial = false;
            TieneSeguro = false;
            VacunacionCompleta = false;
            ExpedienteCompleto = false;
            Activo = true;
            Vacunas = new HashSet<Vacuna>();
            AlergiasRegistradas = new HashSet<Alergia>();
            Medicamentos = new HashSet<Medicamento>();
            HistorialMedico = new HashSet<HistorialMedico>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si tiene alergias registradas
        /// </summary>
        public bool TieneAlergias => !string.IsNullOrWhiteSpace(Alergias) || (AlergiasRegistradas?.Count > 0);

        /// <summary>
        /// Indica si tiene condiciones médicas
        /// </summary>
        public bool TieneCondicionesMedicas => !string.IsNullOrWhiteSpace(CondicionesMedicas);

        /// <summary>
        /// Indica si toma medicamentos regulares
        /// </summary>
        public bool TomaMedicamentos => !string.IsNullOrWhiteSpace(MedicamentosRegulares) || (Medicamentos?.Count > 0);

        /// <summary>
        /// Indica si tiene contacto de emergencia completo
        /// </summary>
        public bool TieneContactoEmergencia =>
            !string.IsNullOrWhiteSpace(ContactoEmergenciaNombre) &&
            !string.IsNullOrWhiteSpace(ContactoEmergenciaTelefono);

        /// <summary>
        /// Indica si el seguro está vigente
        /// </summary>
        public bool SeguroVigente => TieneSeguro && SeguroVigencia.HasValue && SeguroVigencia.Value >= DateTime.Now;

        /// <summary>
        /// Días hasta vencimiento del seguro
        /// </summary>
        public int? DiasHastaVencimientoSeguro
        {
            get
            {
                if (!SeguroVigencia.HasValue) return null;
                return (SeguroVigencia.Value.Date - DateTime.Now.Date).Days;
            }
        }

        /// <summary>
        /// Indica si el seguro está próximo a vencer (menos de 30 días)
        /// </summary>
        public bool SeguroProximoVencer
        {
            get
            {
                var dias = DiasHastaVencimientoSeguro;
                return dias.HasValue && dias.Value > 0 && dias.Value <= 30;
            }
        }

        /// <summary>
        /// Clasificación del IMC
        /// </summary>
        public string ClasificacionIMC
        {
            get
            {
                if (!IMC.HasValue) return "No calculado";

                if (IMC < 18.5m) return "Bajo peso";
                if (IMC < 25m) return "Normal";
                if (IMC < 30m) return "Sobrepeso";
                return "Obesidad";
            }
        }

        /// <summary>
        /// Días desde la última actualización
        /// </summary>
        public int? DiasSinActualizar
        {
            get
            {
                if (!FechaUltimaActualizacion.HasValue) return null;
                return (DateTime.Now.Date - FechaUltimaActualizacion.Value.Date).Days;
            }
        }

        /// <summary>
        /// Indica si necesita actualización (más de 180 días sin actualizar)
        /// </summary>
        public bool NecesitaActualizacion
        {
            get
            {
                var dias = DiasSinActualizar;
                return !dias.HasValue || dias.Value > 180;
            }
        }

        /// <summary>
        /// Total de vacunas aplicadas
        /// </summary>
        public int TotalVacunas => Vacunas?.Count ?? 0;

        /// <summary>
        /// Total de alergias registradas
        /// </summary>
        public int TotalAlergias => AlergiasRegistradas?.Count ?? 0;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Actualiza el peso y estatura, y recalcula el IMC
        /// </summary>
        public void ActualizarPesoEstatura(decimal peso, decimal estatura)
        {
            if (peso <= 0)
                throw new ArgumentException("El peso debe ser mayor a cero");

            if (estatura <= 0)
                throw new ArgumentException("La estatura debe ser mayor a cero");

            Peso = peso;
            Estatura = estatura;

            // Calcular IMC: peso (kg) / (estatura (m))²
            var estaturaMetros = estatura / 100;
            IMC = Math.Round(peso / (estaturaMetros * estaturaMetros), 2);

            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece el contacto de emergencia
        /// </summary>
        public void EstablecerContactoEmergencia(string nombre, string telefono, string parentesco, string telefonoAlt = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del contacto es requerido");

            if (string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("El teléfono del contacto es requerido");

            ContactoEmergenciaNombre = nombre;
            ContactoEmergenciaTelefono = telefono;
            ContactoEmergenciaParentesco = parentesco;
            ContactoEmergenciaTelefonoAlt = telefonoAlt;

            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura el seguro médico
        /// </summary>
        public void ConfigurarSeguro(bool tieneSeguro, string nombre = null, string numeroPoliza = null,
                                     DateTime? vigencia = null, string telefono = null)
        {
            TieneSeguro = tieneSeguro;

            if (tieneSeguro)
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre de la aseguradora es requerido");

                SeguroNombre = nombre;
                SeguroNumeroPoliza = numeroPoliza;
                SeguroVigencia = vigencia;
                SeguroTelefono = telefono;
            }
            else
            {
                SeguroNombre = null;
                SeguroNumeroPoliza = null;
                SeguroVigencia = null;
                SeguroTelefono = null;
            }

            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece información del médico particular
        /// </summary>
        public void EstablecerMedicoParticular(string nombre, string especialidad = null,
                                               string telefono = null, string direccion = null)
        {
            MedicoNombre = nombre;
            MedicoEspecialidad = especialidad;
            MedicoTelefono = telefono;
            MedicoDireccion = direccion;

            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca si requiere atención especial
        /// </summary>
        public void ConfigurarAtencionEspecial(bool requiere, string detalles = null)
        {
            RequiereAtencionEspecial = requiere;
            DetallesAtencionEspecial = requiere ? detalles : null;

            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza las condiciones médicas
        /// </summary>
        public void ActualizarCondicionesMedicas(string condiciones)
        {
            CondicionesMedicas = condiciones;
            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza las alergias
        /// </summary>
        public void ActualizarAlergias(string alergias)
        {
            Alergias = alergias;
            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza los medicamentos regulares
        /// </summary>
        public void ActualizarMedicamentos(string medicamentos)
        {
            MedicamentosRegulares = medicamentos;
            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza las restricciones físicas
        /// </summary>
        public void ActualizarRestricciones(string restricciones)
        {
            Restricciones = restricciones;
            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca el esquema de vacunación como completo o incompleto
        /// </summary>
        public void ActualizarEstadoVacunacion(bool completa, string observaciones = null)
        {
            VacunacionCompleta = completa;
            VacunacionObservaciones = observaciones;
            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Registra una revisión médica
        /// </summary>
        public void RegistrarRevisionMedica(string observaciones = null)
        {
            FechaUltimaRevision = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(observaciones))
            {
                Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                    ? observaciones
                    : $"{Observaciones}\n[{DateTime.Now:dd/MM/yyyy}] {observaciones}";
            }

            FechaUltimaActualizacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Verifica si el expediente está completo
        /// </summary>
        public void VerificarCompletitud()
        {
            var completo = true;

            // Verificar información básica
            if (!TipoSangre.HasValue) completo = false;
            if (!TieneContactoEmergencia) completo = false;

            ExpedienteCompleto = completo;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Activa el expediente
        /// </summary>
        public void Activar()
        {
            if (!Activo)
            {
                Activo = true;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Desactiva el expediente
        /// </summary>
        public void Desactivar()
        {
            if (Activo)
            {
                Activo = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Obtiene un resumen del expediente médico
        /// </summary>
        public string ObtenerResumen()
        {
            var resumen = "=== EXPEDIENTE MÉDICO ===\n\n";

            if (TipoSangre.HasValue)
                resumen += $"Tipo de Sangre: {TipoSangre}\n";

            if (Peso.HasValue && Estatura.HasValue)
                resumen += $"Peso: {Peso} kg | Estatura: {Estatura} cm | IMC: {IMC} ({ClasificacionIMC})\n";

            if (TieneAlergias)
                resumen += $"\n⚠️ ALERGIAS: {Alergias}\n";

            if (TieneCondicionesMedicas)
                resumen += $"\n📋 CONDICIONES: {CondicionesMedicas}\n";

            if (TomaMedicamentos)
                resumen += $"\n💊 MEDICAMENTOS: {MedicamentosRegulares}\n";

            if (RequiereAtencionEspecial)
                resumen += $"\n🔔 ATENCIÓN ESPECIAL: {DetallesAtencionEspecial}\n";

            if (TieneContactoEmergencia)
                resumen += $"\n🚨 EMERGENCIA: {ContactoEmergenciaNombre} ({ContactoEmergenciaParentesco}) - {ContactoEmergenciaTelefono}\n";

            if (TieneSeguro)
                resumen += $"\n🏥 SEGURO: {SeguroNombre} | Póliza: {SeguroNumeroPoliza}\n";

            return resumen;
        }

        /// <summary>
        /// Valida que el expediente sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (Peso.HasValue && Peso <= 0)
                errores.Add("El peso debe ser mayor a cero");

            if (Estatura.HasValue && Estatura <= 0)
                errores.Add("La estatura debe ser mayor a cero");

            if (Peso.HasValue && Estatura.HasValue && !IMC.HasValue)
                errores.Add("Debe calcular el IMC si tiene peso y estatura");

            if (TieneSeguro)
            {
                if (string.IsNullOrWhiteSpace(SeguroNombre))
                    errores.Add("El nombre de la aseguradora es requerido");
            }

            if (RequiereAtencionEspecial && string.IsNullOrWhiteSpace(DetallesAtencionEspecial))
                errores.Add("Debe especificar los detalles de la atención especial requerida");

            if (SeguroVigencia.HasValue && SeguroVigencia.Value < DateTime.Now)
                errores.Add("El seguro está vencido");

            return errores;
        }

        #endregion
    }
}