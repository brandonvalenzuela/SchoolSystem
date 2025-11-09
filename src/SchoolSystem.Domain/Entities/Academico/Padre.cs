using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad Padre - Representa a un padre de familia o tutor
    /// Extiende la información de un Usuario con rol de Padre
    /// </summary>
    public class Padre : BaseEntity
    {
        #region Propiedades de la Escuela (Multi-tenant)

        /// <summary>
        /// ID de la escuela a la que pertenece
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// Escuela asociada (Navigation Property)
        /// </summary>
        public virtual Escuelas.Escuela Escuela { get; set; }

        #endregion

        #region Vinculación con Usuario

        /// <summary>
        /// ID del usuario asociado (obligatorio)
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Usuario asociado (Navigation Property)
        /// Contiene los datos personales, contacto y credenciales
        /// </summary>
        public virtual Usuario Usuario { get; set; }

        #endregion

        #region Información Laboral

        /// <summary>
        /// Ocupación o profesión del padre/tutor
        /// </summary>
        public string Ocupacion { get; set; }

        /// <summary>
        /// Nombre del lugar donde trabaja
        /// </summary>
        public string LugarTrabajo { get; set; }

        /// <summary>
        /// Teléfono del trabajo
        /// </summary>
        public string TelefonoTrabajo { get; set; }

        /// <summary>
        /// Dirección del trabajo
        /// </summary>
        public string DireccionTrabajo { get; set; }

        /// <summary>
        /// Puesto o cargo que desempeña
        /// </summary>
        public string Puesto { get; set; }

        #endregion

        #region Información Educativa

        /// <summary>
        /// Nivel de estudios del padre/tutor
        /// (Primaria, Secundaria, Preparatoria, Licenciatura, Posgrado, etc.)
        /// </summary>
        public string NivelEstudios { get; set; }

        /// <summary>
        /// Carrera o especialidad (si aplica)
        /// </summary>
        public string Carrera { get; set; }

        #endregion

        #region Información Adicional

        /// <summary>
        /// Estado civil del padre/tutor
        /// </summary>
        public string EstadoCivil { get; set; }

        /// <summary>
        /// Observaciones generales sobre el padre/tutor
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Indica si el padre/tutor acepta recibir notificaciones por SMS
        /// (puede generar costos)
        /// </summary>
        public bool AceptaSMS { get; set; }

        /// <summary>
        /// Indica si el padre/tutor acepta recibir notificaciones por email
        /// </summary>
        public bool AceptaEmail { get; set; }

        /// <summary>
        /// Indica si el padre/tutor acepta notificaciones push en la app
        /// </summary>
        public bool AceptaPush { get; set; }

        #endregion

        #region Navigation Properties (Relaciones)

        /// <summary>
        /// Relaciones con alumnos (hijos/tutelados)
        /// Un padre puede tener múltiples hijos en la escuela
        /// </summary>
        public virtual ICollection<AlumnoPadre> AlumnoPadres { get; set; }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Cantidad de hijos/tutelados en la escuela
        /// </summary>
        public int CantidadHijos => AlumnoPadres?.Count ?? 0;

        /// <summary>
        /// Lista de IDs de los alumnos asociados
        /// </summary>
        public IEnumerable<int> AlumnosIds => 
            AlumnoPadres?.Select(ap => ap.AlumnoId).ToList() ?? Enumerable.Empty<int>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Padre()
        {
            AceptaSMS = true;
            AceptaEmail = true;
            AceptaPush = true;

            // Inicializar colecciones
            AlumnoPadres = new HashSet<AlumnoPadre>();
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si el padre tiene al menos un hijo en la escuela
        /// </summary>
        public bool TieneHijos()
        {
            return AlumnoPadres != null && AlumnoPadres.Any();
        }

        /// <summary>
        /// Verifica si el padre es tutor principal de algún alumno
        /// </summary>
        public bool EsTutorPrincipalDeAlguno()
        {
            return AlumnoPadres != null && AlumnoPadres.Any(ap => ap.EsTutorPrincipal);
        }

        /// <summary>
        /// Verifica si el padre es tutor principal de un alumno específico
        /// </summary>
        public bool EsTutorPrincipalDe(int alumnoId)
        {
            return AlumnoPadres != null &&
                   AlumnoPadres.Any(ap => ap.AlumnoId == alumnoId && ap.EsTutorPrincipal);
        }

        /// <summary>
        /// Verifica si el padre está autorizado para recoger a un alumno específico
        /// </summary>
        public bool EstaAutorizadoParaRecoger(int alumnoId)
        {
            return AlumnoPadres != null &&
                   AlumnoPadres.Any(ap => ap.AlumnoId == alumnoId && ap.AutorizadoRecoger);
        }

        /// <summary>
        /// Verifica si el padre debe recibir notificaciones sobre un alumno
        /// </summary>
        public bool DebeRecibirNotificacionesDe(int alumnoId)
        {
            return AlumnoPadres != null &&
                   AlumnoPadres.Any(ap => ap.AlumnoId == alumnoId && ap.RecibeNotificaciones);
        }

        /// <summary>
        /// Obtiene todos los alumnos de los que es tutor principal
        /// </summary>
        public IEnumerable<Alumno> ObtenerAlumnosPrincipales()
        {
            if (AlumnoPadres == null)
                return Enumerable.Empty<Alumno>();

            return AlumnoPadres
                .Where(ap => ap.EsTutorPrincipal)
                .Select(ap => ap.Alumno)
                .Where(a => a != null);
        }

        /// <summary>
        /// Obtiene todos los alumnos asociados (principales y secundarios)
        /// </summary>
        public IEnumerable<Alumno> ObtenerTodosLosAlumnos()
        {
            if (AlumnoPadres == null)
                return Enumerable.Empty<Alumno>();

            return AlumnoPadres
                .Select(ap => ap.Alumno)
                .Where(a => a != null);
        }

        /// <summary>
        /// Verifica si el padre tiene información laboral completa
        /// </summary>
        public bool TieneInformacionLaboral()
        {
            return !string.IsNullOrWhiteSpace(Ocupacion) &&
                   !string.IsNullOrWhiteSpace(LugarTrabajo);
        }

        /// <summary>
        /// Habilita todos los canales de notificación
        /// </summary>
        public void HabilitarTodasLasNotificaciones()
        {
            AceptaSMS = true;
            AceptaEmail = true;
            AceptaPush = true;
        }

        /// <summary>
        /// Deshabilita todos los canales de notificación
        /// </summary>
        public void DeshabilitarTodasLasNotificaciones()
        {
            AceptaSMS = false;
            AceptaEmail = false;
            AceptaPush = false;
        }

        /// <summary>
        /// Verifica si tiene al menos un canal de notificación habilitado
        /// </summary>
        public bool TieneAlgunCanalNotificacionHabilitado()
        {
            return AceptaSMS || AceptaEmail || AceptaPush;
        }

        #endregion
    }
}