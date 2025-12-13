using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums.Escuelas;

namespace SchoolSystem.Domain.Entities.Escuelas
{
    /// <summary>
    /// Entidad Escuela - Representa una institución educativa en el sistema
    /// Esta es la entidad principal del modelo multi-tenant
    /// </summary>
    public class Escuela : BaseEntity, IAuditableEntity
    {
        #region Propiedades Básicas
        
        /// <summary>
        /// Código único identificador de la escuela
        /// </summary>
        public string Codigo { get; set; }
        
        /// <summary>
        /// Nombre comercial de la escuela
        /// </summary>
        public string Nombre { get; set; }
        
        /// <summary>
        /// Razón social de la institución
        /// </summary>
        public string RazonSocial { get; set; }
        
        /// <summary>
        /// RFC de la institución (México) o equivalente
        /// </summary>
        public string RFC { get; set; }
        
        #endregion
        
        #region Datos de Contacto
        
        /// <summary>
        /// Dirección física de la escuela
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Ciudad donde se encuentra ubicada la escuela
        /// </summary>
        public string Ciudad { get; set; }

        /// <summary>
        /// Estado o entidad federativa donde se ubica la escuela
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Código postal de la ubicación de la escuela
        /// </summary>
        public string CodigoPostal { get; set; }

        /// <summary>
        /// País donde se encuentra la escuela
        /// </summary>
        public string Pais { get; set; }

        /// <summary>
        /// Teléfono de contacto principal
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Teléfono alternativo o secundario de contacto
        /// </summary>
        public string TelefonoAlternativo { get; set; }


        /// <summary>
        /// Correo electrónico institucional
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// URL del sitio web de la escuela
        /// </summary>
        public string SitioWeb { get; set; }
        
        /// <summary>
        /// URL del logo de la escuela
        /// </summary>
        public string LogoUrl { get; set; }

        #endregion

        #region Suscripción y Plan

        /// <summary>
        /// ID del plan de suscripción (1=Básico, 2=Profesional, 3=Enterprise)
        /// </summary>
        public TipoPlan? TipoPlan { get; set; }
        
        /// <summary>
        /// Fecha de registro en el sistema
        /// </summary>
        public DateTime FechaRegistro { get; set; }
        
        /// <summary>
        /// Fecha de expiración de la suscripción
        /// </summary>
        public DateTime? FechaExpiracion { get; set; }
        
        /// <summary>
        /// Indica si la escuela está activa en el sistema
        /// </summary>
        public bool Activo { get; set; }
        
        #endregion
        
        #region Configuración Personalizada

        /// <summary>
        /// Número máximo de alumnos permitidos según el plan
        /// </summary>
        public int? MaxAlumnos { get; set; }
        
        /// <summary>
        /// Número máximo de maestros permitidos según el plan
        /// </summary>
        public int? MaxMaestros { get; set; }
        
        /// <summary>
        /// Espacio de almacenamiento en MB
        /// </summary>
        public int? EspacioAlmacenamiento { get; set; }
        
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
        
        #region Navigation Properties (Relaciones)
        
        /// <summary>
        /// Usuarios asociados a esta escuela
        /// </summary>
        public virtual ICollection<Usuarios.Usuario> Usuarios { get; set; }
        
        /// <summary>
        /// Alumnos de esta escuela
        /// </summary>
        public virtual ICollection<Academico.Alumno> Alumnos { get; set; }
        
        /// <summary>
        /// Niveles educativos de esta escuela
        /// </summary>
        public virtual ICollection<Academico.NivelEducativo> NivelesEducativos { get; set; }
        
        /// <summary>
        /// Grupos de esta escuela
        /// </summary>
        public virtual ICollection<Academico.Grupo> Grupos { get; set; }
        
        /// <summary>
        /// Materias de esta escuela
        /// </summary>
        public virtual ICollection<Academico.Materia> Materias { get; set; }
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Escuela()
        {
            FechaRegistro = DateTime.Now;
            Activo = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            TipoPlan = Enums.Escuelas.TipoPlan.Prueba;


            // Inicializar colecciones
            Usuarios = new HashSet<Usuarios.Usuario>();
            Alumnos = new HashSet<Academico.Alumno>();
            NivelesEducativos = new HashSet<Academico.NivelEducativo>();
            Grupos = new HashSet<Academico.Grupo>();
            Materias = new HashSet<Academico.Materia>();
        }
        
        #endregion
        
        #region Métodos de Negocio
        
        /// <summary>
        /// Verifica si la suscripción de la escuela está activa y vigente
        /// </summary>
        public bool TieneSuscripcionVigente()
        {
            if (!Activo)
                return false;
            
            if (!FechaExpiracion.HasValue)
                return true; // Suscripción sin límite de tiempo
            
            return FechaExpiracion.Value > DateTime.Now;
        }
        
        /// <summary>
        /// Verifica si la escuela ha alcanzado el límite de alumnos
        /// </summary>
        public bool HaAlcanzadoLimiteAlumnos()
        {
            if (!MaxAlumnos.HasValue)
                return false; // Sin límite
            
            return Alumnos?.Count >= MaxAlumnos.Value;
        }
        
        /// <summary>
        /// Verifica si la escuela ha alcanzado el límite de maestros
        /// </summary>
        public bool HaAlcanzadoLimiteMaestros()
        {
            if (!MaxMaestros.HasValue)
                return false; // Sin límite
            
            var cantidadMaestros = 0;
            if (Usuarios != null)
            {
                foreach (var usuario in Usuarios)
                {
                    if (usuario.Rol == RolUsuario.Maestro)
                        cantidadMaestros++;
                }
            }
            
            return cantidadMaestros >= MaxMaestros.Value;
        }
        
        /// <summary>
        /// Activa la escuela
        /// </summary>
        public void Activar()
        {
            Activo = true;
            UpdatedAt = DateTime.Now;
        }
        
        /// <summary>
        /// Desactiva la escuela
        /// </summary>
        public void Desactivar()
        {
            Activo = false;
            UpdatedAt = DateTime.Now;
        }
        
        /// <summary>
        /// Renueva la suscripción por un período de meses
        /// </summary>
        public void RenovarSuscripcion(int meses)
        {
            if (!FechaExpiracion.HasValue || FechaExpiracion.Value < DateTime.Now)
            {
                FechaExpiracion = DateTime.Now.AddMonths(meses);
            }
            else
            {
                FechaExpiracion = FechaExpiracion.Value.AddMonths(meses);
            }
            
            UpdatedAt = DateTime.Now;
        }
        
        #endregion
    }
}