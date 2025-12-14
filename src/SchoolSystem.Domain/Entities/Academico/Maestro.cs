using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Academico;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad Maestro - Representa a un profesor o docente
    /// Extiende la información de un Usuario con rol de Maestro
    /// </summary>
    public class Maestro : BaseEntity, IAuditableEntity, ISoftDeletable
    {
        #region Propiedades de la Escuela (Multi-tenant)

        /// <summary>
        /// ID de la escuela a la que pertenece
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// Escuela asociada (Navigation Property)
        /// </summary>
        public virtual Escuela Escuela { get; set; }

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
        /// Número de empleado único en la escuela
        /// </summary>
        public string NumeroEmpleado { get; set; }

        /// <summary>
        /// Fecha de ingreso a la institución
        /// </summary>
        public DateTime? FechaIngreso { get; set; }

        /// <summary>
        /// Fecha de baja de la institución (si aplica)
        /// </summary>
        public DateTime? FechaBaja { get; set; }

        /// <summary>
        /// Tipo de contrato del maestro
        /// </summary>
        public TipoContrato? TipoContrato { get; set; }

        /// <summary>
        /// Estatus laboral del maestro
        /// </summary>
        public EstatusLaboral? Estatus { get; set; }

        /// <summary>
        /// Salario mensual del maestro
        /// </summary>
        public decimal? Salario { get; set; }

        #endregion

        #region Información Profesional

        /// <summary>
        /// Cédula profesional del maestro
        /// </summary>
        public string CedulaProfesional { get; set; }

        /// <summary>
        /// Especialidad o área de conocimiento principal
        /// </summary>
        public string Especialidad { get; set; }

        /// <summary>
        /// Título académico (Licenciatura, Maestría, Doctorado)
        /// </summary>
        public string TituloAcademico { get; set; }

        /// <summary>
        /// Universidad donde obtuvo su título
        /// </summary>
        public string Universidad { get; set; }

        /// <summary>
        /// Año de graduación
        /// </summary>
        public int? AñoGraduacion { get; set; }

        /// <summary>
        /// Años de experiencia docente
        /// </summary>
        public int? AñosExperiencia { get; set; }

        #endregion

        #region Certificaciones y Capacitaciones

        /// <summary>
        /// Certificaciones adicionales (JSON o texto delimitado)
        /// </summary>
        public string Certificaciones { get; set; }

        /// <summary>
        /// Cursos de capacitación realizados
        /// </summary>
        public string Capacitaciones { get; set; }

        /// <summary>
        /// Idiomas que habla el maestro
        /// </summary>
        public string Idiomas { get; set; }

        #endregion

        #region Información Adicional

        /// <summary>
        /// Observaciones generales sobre el maestro
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Horario de atención a padres
        /// </summary>
        public string HorarioAtencion { get; set; }

        /// <summary>
        /// Disponibilidad para actividades extracurriculares
        /// </summary>
        public bool DisponibleExtracurriculares { get; set; }

        #endregion

        #region Navigation Properties (Relaciones)

        /// <summary>
        /// Grupos de los que es titular
        /// </summary>
        public virtual ICollection<Grupo> GruposTitular { get; set; }

        /// <summary>
        /// Asignaciones de materias que imparte.
        /// CORRECCIÓN: Nombre ajustado a 'AsignacionesDeGrupoMateria' para coincidir con la configuración de EF Core.
        /// </summary>
        public virtual ICollection<GrupoMateriaMaestro> AsignacionesDeGrupoMateria { get; set; }

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
        /// Cantidad de grupos en los que es titular
        /// </summary>
        public int CantidadGruposTitular => GruposTitular?.Count ?? 0;

        /// <summary>
        /// Cantidad de materias que imparte
        /// </summary>
        public int CantidadMaterias => AsignacionesDeGrupoMateria?.Count ?? 0;

        /// <summary>
        /// Antigüedad en años en la institución
        /// </summary>
        public int? AntiguedadEnAños
        {
            get
            {
                if (!FechaIngreso.HasValue)
                    return null;

                var fechaFin = FechaBaja ?? DateTime.Today;
                var años = fechaFin.Year - FechaIngreso.Value.Year;

                if (FechaIngreso.Value.Date > fechaFin.AddYears(-años))
                    años--;

                return años;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Maestro()
        {
            Estatus = EstatusLaboral.Activo;
            DisponibleExtracurriculares = false;

            // Inicializar colecciones
            GruposTitular = new HashSet<Grupo>();
            AsignacionesDeGrupoMateria = new HashSet<GrupoMateriaMaestro>(); // CORREGIDO
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si el maestro está activo
        /// </summary>
        public bool EstaActivo()
        {
            return Estatus == EstatusLaboral.Activo;
        }

        /// <summary>
        /// Verifica si el maestro está en licencia
        /// </summary>
        public bool EstaEnLicencia()
        {
            return Estatus == EstatusLaboral.Licencia;
        }

        /// <summary>
        /// Verifica si el maestro ha sido dado de baja
        /// </summary>
        public bool EstaDeBaja()
        {
            return Estatus == EstatusLaboral.Baja;
        }

        /// <summary>
        /// Da de baja al maestro
        /// </summary>
        public void DarDeBaja()
        {
            Estatus = EstatusLaboral.Baja;
            FechaBaja = DateTime.Now;
        }

        /// <summary>
        /// Reactiva al maestro
        /// </summary>
        public void Reactivar()
        {
            Estatus = EstatusLaboral.Activo;
            FechaBaja = null;
        }

        /// <summary>
        /// Pone al maestro en licencia
        /// </summary>
        public void PonerEnLicencia()
        {
            Estatus = EstatusLaboral.Licencia;
        }

        /// <summary>
        /// Verifica si el maestro tiene información profesional completa
        /// </summary>
        public bool TieneInformacionProfesional()
        {
            return !string.IsNullOrWhiteSpace(CedulaProfesional) &&
                   !string.IsNullOrWhiteSpace(Especialidad) &&
                   !string.IsNullOrWhiteSpace(TituloAcademico);
        }

        /// <summary>
        /// Verifica si el maestro es titular de algún grupo
        /// </summary>
        public bool EsTitularDeGrupo()
        {
            return GruposTitular != null && GruposTitular.Any();
        }

        /// <summary>
        /// Verifica si el maestro imparte alguna materia
        /// </summary>
        public bool ImparteAlgunaMateria()
        {
            return AsignacionesDeGrupoMateria != null && AsignacionesDeGrupoMateria.Any(); // CORREGIDO
        }

        /// <summary>
        /// Obtiene las materias que imparte en un grupo específico
        /// </summary>
        public IEnumerable<Materia> ObtenerMateriasPorGrupo(int grupoId)
        {
            if (AsignacionesDeGrupoMateria == null) // CORREGIDO
                return Enumerable.Empty<Materia>();

            return AsignacionesDeGrupoMateria // CORREGIDO
                .Where(am => am.GrupoId == grupoId)
                .Select(am => am.Materia)
                .Where(m => m != null);
        }

        /// <summary>
        /// Obtiene todos los grupos donde imparte clases
        /// </summary>
        public IEnumerable<Grupo> ObtenerGruposDondeImparte()
        {
            if (AsignacionesDeGrupoMateria == null) // CORREGIDO
                return Enumerable.Empty<Grupo>();

            return AsignacionesDeGrupoMateria // CORREGIDO
                .Select(am => am.Grupo)
                .Where(g => g != null)
                .Distinct();
        }

        /// <summary>
        /// Verifica si el maestro tiene experiencia mínima requerida
        /// </summary>
        public bool TieneExperienciaMinima(int añosMinimos)
        {
            return AñosExperiencia.HasValue && AñosExperiencia.Value >= añosMinimos;
        }

        /// <summary>
        /// Calcula la carga académica total (cantidad de grupos + materias)
        /// </summary>
        public int CalcularCargaAcademica()
        {
            return CantidadGruposTitular + CantidadMaterias;
        }

        #endregion
    }
}
