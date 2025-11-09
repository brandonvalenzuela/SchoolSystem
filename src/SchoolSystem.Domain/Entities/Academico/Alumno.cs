using SchoolSystem.Domain.Entities.Common;
using System;
using SchoolSystem.Domain.Entities.Conducta;
using SchoolSystem.Domain.Enums.Academico;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad Alumno - Representa a un estudiante inscrito en la escuela
    /// </summary>
    public class Alumno : BaseEntity, IAuditableEntity, ISoftDeletable
    {
        #region Propiedades de la Escuela (Multi-tenant)

        /// <summary>
        /// ID de la escuela a la que pertenece el alumno
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// Escuela asociada (Navigation Property)
        /// </summary>
        public virtual Escuelas.Escuela Escuela { get; set; }

        #endregion

        #region Vinculación con Usuario

        /// <summary>
        /// ID del usuario asociado (opcional, si el alumno tiene acceso al sistema)
        /// </summary>
        public int? UsuarioId { get; set; }

        /// <summary>
        /// Usuario asociado (Navigation Property)
        /// </summary>
        public virtual Usuarios.Usuario Usuario { get; set; }

        #endregion

        #region Datos de Identificación

        /// <summary>
        /// Matrícula única del alumno en la escuela
        /// </summary>
        public string Matricula { get; set; }

        /// <summary>
        /// CURP del alumno (Clave Única de Registro de Población - México)
        /// </summary>
        public string CURP { get; set; }

        #endregion

        #region Datos Personales

        /// <summary>
        /// Nombre(s) del alumno
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido paterno
        /// </summary>
        public string ApellidoPaterno { get; set; }

        /// <summary>
        /// Apellido materno
        /// </summary>
        public string ApellidoMaterno { get; set; }

        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Género del alumno
        /// </summary>
        public Genero Genero { get; set; }

        /// <summary>
        /// URL de la foto del alumno
        /// </summary>
        public string? FotoUrl { get; set; }

        #endregion

        #region Datos de Contacto

        /// <summary>
        /// Dirección de residencia
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Teléfono del alumno (si aplica)
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Correo electrónico del alumno (si aplica)
        /// </summary>
        public string Email { get; set; }

        #endregion

        #region Información Médica

        /// <summary>
        /// Tipo de sangre del alumno
        /// </summary>
        public string TipoSangre { get; set; }

        /// <summary>
        /// Alergias conocidas
        /// </summary>
        public string? Alergias { get; set; }

        /// <summary>
        /// Condiciones médicas relevantes
        /// </summary>
        public string? CondicionesMedicas { get; set; }

        /// <summary>
        /// Medicamentos que toma regularmente
        /// </summary>
        public string? Medicamentos { get; set; }

        #endregion

        #region Contacto de Emergencia

        /// <summary>
        /// Nombre completo del contacto de emergencia
        /// </summary>
        public string ContactoEmergenciaNombre { get; set; }

        /// <summary>
        /// Teléfono del contacto de emergencia
        /// </summary>
        public string ContactoEmergenciaTelefono { get; set; }

        /// <summary>
        /// Relación con el alumno (padre, madre, tutor, etc.)
        /// </summary>
        public string ContactoEmergenciaRelacion { get; set; }

        #endregion

        #region Control Académico

        /// <summary>
        /// Fecha de ingreso a la escuela
        /// </summary>
        public DateTime FechaIngreso { get; set; }

        /// <summary>
        /// Fecha de baja de la escuela (si aplica)
        /// </summary>
        public DateTime? FechaBaja { get; set; }

        /// <summary>
        /// Motivo de baja (si aplica)
        /// </summary>
        public string? MotivoBaja { get; set; }

        /// <summary>
        /// Estatus actual del alumno
        /// </summary>
        public EstatusAlumno? Estatus { get; set; }

        /// <summary>
        /// Observaciones generales sobre el alumno
        /// </summary>
        public string? Observaciones { get; set; }

        #endregion

        #region Propiedades de Auditoría (IAuditableEntity)

        /// <summary>
        /// Fecha y hora de creación del registro
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha y hora de la última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// ID del usuario que creó el registro
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// ID del usuario que realizó la última actualización
        /// </summary>
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

        #region Propiedades Calculadas

        /// <summary>
        /// Nombre completo del alumno
        /// </summary>
        public string NombreCompleto => $"{Nombre} {ApellidoPaterno} {ApellidoMaterno}".Trim();

        /// <summary>
        /// Edad del alumno
        /// </summary>
        public int Edad
        {
            get
            {
                var hoy = DateTime.Today;
                var edad = hoy.Year - FechaNacimiento.Year;

                if (FechaNacimiento.Date > hoy.AddYears(-edad))
                    edad--;

                return edad;
            }
        }

        /// <summary>
        /// Años de antigüedad en la escuela
        /// </summary>
        public int AntiguedadEnAños
        {
            get
            {
                var hoy = DateTime.Today;
                var años = hoy.Year - FechaIngreso.Year;

                if (FechaIngreso.Date > hoy.AddYears(-años))
                    años--;

                return años;
            }
        }

        #endregion

        #region Navigation Properties (Relaciones)

        /// <summary>
        /// Inscripciones del alumno (historial por ciclo escolar)
        /// </summary>
        public virtual ICollection<Inscripcion> Inscripciones { get; set; }

        /// <summary>
        /// Padres/tutores asociados al alumno
        /// </summary>
        public virtual ICollection<AlumnoPadre> AlumnoPadres { get; set; }

        /// <summary>
        /// Asistencias del alumno
        /// </summary>
        public virtual ICollection<Evaluacion.Asistencia> Asistencias { get; set; }

        /// <summary>
        /// Calificaciones del alumno
        /// </summary>
        public virtual ICollection<Evaluacion.Calificacion> Calificaciones { get; set; }

        /// <summary>
        /// Registros de conducta del alumno
        /// </summary>
        public virtual ICollection<RegistroConducta> RegistrosConducta { get; set; }

        /// <summary>
        /// Puntos del alumno (gamificación)
        /// </summary>
        public virtual ICollection<AlumnoPuntos> Puntos { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Alumno()
        {
            FechaIngreso = DateTime.Now;
            Estatus = EstatusAlumno.Activo;
            IsDeleted = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            // Inicializar colecciones
            Inscripciones = new HashSet<Inscripcion>();
            AlumnoPadres = new HashSet<AlumnoPadre>();
            Asistencias = new HashSet<Evaluacion.Asistencia>();
            Calificaciones = new HashSet<Evaluacion.Calificacion>();
            RegistrosConducta = new HashSet<RegistroConducta>();
            Puntos = new HashSet<AlumnoPuntos>();
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si el alumno está activo
        /// </summary>
        public bool EstaActivo()
        {
            return Estatus == EstatusAlumno.Activo && !IsDeleted;
        }

        /// <summary>
        /// Verifica si el alumno es menor de edad (menor de 18 años)
        /// </summary>
        public bool EsMenorDeEdad()
        {
            return Edad < 18;
        }

        /// <summary>
        /// Da de baja al alumno
        /// </summary>
        public void DarDeBaja(string motivo, int usuarioId)
        {
            Estatus = EstatusAlumno.Baja;
            FechaBaja = DateTime.Now;
            MotivoBaja = motivo;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Reactiva al alumno
        /// </summary>
        public void Reactivar(int usuarioId)
        {
            Estatus = EstatusAlumno.Activo;
            FechaBaja = null;
            MotivoBaja = null;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Marca al alumno como egresado
        /// </summary>
        public void MarcarComoEgresado(int usuarioId)
        {
            Estatus = EstatusAlumno.Egresado;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si el alumno tiene información médica registrada
        /// </summary>
        public bool TieneInformacionMedica()
        {
            return !string.IsNullOrWhiteSpace(TipoSangre) ||
                   !string.IsNullOrWhiteSpace(Alergias) ||
                   !string.IsNullOrWhiteSpace(CondicionesMedicas);
        }

        /// <summary>
        /// Verifica si el alumno tiene contacto de emergencia registrado
        /// </summary>
        public bool TieneContactoEmergencia()
        {
            return !string.IsNullOrWhiteSpace(ContactoEmergenciaNombre) &&
                   !string.IsNullOrWhiteSpace(ContactoEmergenciaTelefono);
        }

        /// <summary>
        /// Soft delete - Elimina lógicamente al alumno
        /// </summary>
        public void EliminarLogicamente(int usuarioId)
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
            DeletedBy = usuarioId;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Restaura un alumno eliminado lógicamente
        /// </summary>
        public void Restaurar()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
            UpdatedAt = DateTime.Now;
        }

        #endregion
    }

   
}