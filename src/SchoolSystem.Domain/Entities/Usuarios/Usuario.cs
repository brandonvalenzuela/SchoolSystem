using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Enums;
using SchoolSystem.Domain.Enums.Academico;
using SchoolSystem.Domain.Enums.Escuelas;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Domain.Entities.Usuarios
{
    /// <summary>
    /// Entidad Usuario - Representa a cualquier persona con acceso al sistema
    /// Incluye: Directores, Subdirectores, Maestros, Administrativos, Padres, Alumnos
    /// </summary>
    public class Usuario : BaseEntity, IAuditableEntity, ISoftDeletable
    {
        #region Propiedades de la Escuela (Multi-tenant)

        /// <summary>
        /// ID de la escuela a la que pertenece el usuario
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// Escuela asociada (Navigation Property)
        /// </summary>
        public virtual Escuela Escuela { get; set; }

        #endregion

        #region Credenciales de Autenticación

        /// <summary>
        /// Nombre de usuario para login (único en el sistema)
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Correo electrónico (único en el sistema)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Hash de la contraseña (nunca almacenar en texto plano)
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Rol del usuario en el sistema
        /// </summary>
        public RolUsuario Rol { get; set; }

        #endregion

        #region Datos Personales

        /// <summary>
        /// Nombre(s) de la persona
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
        /// Teléfono principal de contacto
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Teléfono de emergencia
        /// </summary>
        public string TelefonoEmergencia { get; set; }

        /// <summary>
        /// URL de la foto de perfil
        /// </summary>
        public string FotoUrl { get; set; }

        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        public DateTime? FechaNacimiento { get; set; }

        /// <summary>
        /// Género de la persona
        /// </summary>
        public Genero? Genero { get; set; }

        #endregion

        #region Control de Estado y Sesión

        /// <summary>
        /// Indica si el usuario está activo en el sistema
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Fecha y hora del último acceso al sistema
        /// </summary>
        public DateTime? UltimoAcceso { get; set; }

        /// <summary>
        /// Token para recuperación de contraseña
        /// </summary>
        public string? TokenRecuperacion { get; set; }

        /// <summary>
        /// Fecha de expiración del token de recuperación
        /// </summary>
        public DateTime? TokenExpiracion { get; set; }

        /// <summary>
        /// Número de intentos fallidos de login
        /// </summary>
        public int IntentosFallidos { get; set; }

        /// <summary>
        /// Fecha hasta la cual el usuario está bloqueado
        /// </summary>
        public DateTime? BloqueadoHasta { get; set; }

        #endregion

        #region Propiedades de Auditoría (IAuditableEntity)

        /// <summary>
        /// Fecha y hora de creación del usuario
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha y hora de la última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// ID del usuario que creó este registro
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
        /// Nombre completo del usuario
        /// </summary>
        public string NombreCompleto => $"{Nombre} {ApellidoPaterno} {ApellidoMaterno}".Trim();

        /// <summary>
        /// Edad de la persona (si tiene fecha de nacimiento)
        /// </summary>
        public int? Edad
        {
            get
            {
                if (!FechaNacimiento.HasValue)
                    return null;

                var hoy = DateTime.Today;
                var edad = hoy.Year - FechaNacimiento.Value.Year;

                if (FechaNacimiento.Value.Date > hoy.AddYears(-edad))
                    edad--;

                return edad;
            }
        }

        #endregion

        #region Navigation Properties (Relaciones)

        /// <summary>
        /// Información adicional si el usuario es un Maestro
        /// </summary>
        public virtual Maestro Maestro { get; set; }

        /// <summary>
        /// Información adicional si el usuario es un Padre
        /// </summary>
        public virtual Padre Padre { get; set; }

        /// <summary>
        /// Información adicional si el usuario es un Alumno
        /// </summary>
        public virtual Alumno Alumno { get; set; }

        /// <summary>
        /// Dispositivos registrados para este usuario
        /// </summary>
        public virtual ICollection<Dispositivo> Dispositivos { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Usuario()
        {
            Activo = true;
            IntentosFallidos = 0;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            // Inicializar colecciones
            Dispositivos = new HashSet<Dispositivo>();
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si el usuario está bloqueado
        /// </summary>
        public bool EstaBloqueado()
        {
            if (!BloqueadoHasta.HasValue)
                return false;

            if (BloqueadoHasta.Value <= DateTime.Now)
            {
                // El bloqueo ya expiró, desbloqueamos
                BloqueadoHasta = null;
                IntentosFallidos = 0;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Registra un intento fallido de login
        /// Bloquea al usuario después de 5 intentos
        /// </summary>
        public void RegistrarIntentoFallido()
        {
            IntentosFallidos++;

            // Bloquear después de 5 intentos fallidos por 30 minutos
            if (IntentosFallidos >= 5)
            {
                BloqueadoHasta = DateTime.Now.AddMinutes(30);
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Registra un login exitoso
        /// </summary>
        public void RegistrarLoginExitoso()
        {
            UltimoAcceso = DateTime.Now;
            IntentosFallidos = 0;
            BloqueadoHasta = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Genera un token para recuperación de contraseña
        /// Válido por 24 horas
        /// </summary>
        public void GenerarTokenRecuperacion()
        {
            TokenRecuperacion = Guid.NewGuid().ToString();
            TokenExpiracion = DateTime.Now.AddHours(24);
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Verifica si el token de recuperación es válido
        /// </summary>
        public bool TokenRecuperacionEsValido(string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(TokenRecuperacion))
                return false;

            if (!TokenExpiracion.HasValue || TokenExpiracion.Value < DateTime.Now)
                return false;

            return TokenRecuperacion == token;
        }

        /// <summary>
        /// Invalida el token de recuperación después de usarlo
        /// </summary>
        public void InvalidarTokenRecuperacion()
        {
            TokenRecuperacion = null;
            TokenExpiracion = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Activa el usuario
        /// </summary>
        public void Activar()
        {
            Activo = true;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Desactiva el usuario
        /// </summary>
        public void Desactivar()
        {
            Activo = false;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Verifica si el usuario tiene un rol específico
        /// </summary>
        public bool TieneRol(RolUsuario rol)
        {
            return Rol == rol;
        }

        /// <summary>
        /// Verifica si el usuario es un administrador (Director o SuperAdmin)
        /// </summary>
        public bool EsAdministrador()
        {
            return Rol == RolUsuario.SuperAdmin ||
                   Rol == RolUsuario.Director ||
                   Rol == RolUsuario.Subdirector;
        }

        /// <summary>
        /// Verifica si el usuario es un maestro
        /// </summary>
        public bool EsMaestro()
        {
            return Rol == RolUsuario.Maestro;
        }

        /// <summary>
        /// Verifica si el usuario es un padre de familia
        /// </summary>
        public bool EsPadre()
        {
            return Rol == RolUsuario.Padre;
        }

        /// <summary>
        /// Verifica si el usuario es un alumno
        /// </summary>
        public bool EsAlumno()
        {
            return Rol == RolUsuario.Alumno;
        }

        #region Validaciones de Integridad de Rol

        /// <summary>
        /// Valida si el usuario tiene los datos de perfil correspondientes a su rol.
        /// </summary>
        public bool PerfilEstaCompleto()
        {
            switch (Rol)
            {
                case RolUsuario.Maestro:
                    return Maestro != null; // Requiere registro en tabla Maestros
                case RolUsuario.Padre:
                    return Padre != null;   // Requiere registro en tabla Padres
                case RolUsuario.Alumno:
                    return Alumno != null;  // Requiere registro en tabla Alumnos
                default:
                    return true; // Admin, Director, etc. solo requieren tabla Usuario
            }
        }

        /// <summary>
        /// Verifica si el usuario tiene permisos para operar en una escuela específica.
        /// </summary>
        public bool TieneAccesoAEscuela(int escuelaIdSolicitada)
        {
            if (Rol == RolUsuario.SuperAdmin)
                return true; // SuperAdmin entra a todas
            return EscuelaId == escuelaIdSolicitada;
        }

        #endregion



        #endregion
    }
}