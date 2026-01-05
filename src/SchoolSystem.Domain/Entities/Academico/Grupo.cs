using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums;
using SchoolSystem.Domain.Enums.Academico;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad Grupo - Representa un grupo o sección dentro de un grado
    /// Ejemplos: 3° de Primaria Grupo A, 2° de Secundaria Grupo B
    /// </summary>
    public class Grupo : BaseEntity, IAuditableEntity
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

        #region Vinculación con Grado

        /// <summary>
        /// ID del grado al que pertenece
        /// </summary>
        public int GradoId { get; set; }

        /// <summary>
        /// Grado asociado (Navigation Property)
        /// </summary>
        public virtual Grado Grado { get; set; }

        #endregion

        #region Propiedades Básicas

        /// <summary>
        /// Nombre del grupo
        /// Ejemplos: "A", "B", "C" o "Matutino", "Vespertino"
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Ciclo escolar al que corresponde
        /// Ejemplo: "2024-2025", "2025-2026"
        /// </summary>
        public string CicloEscolar { get; set; }

        /// <summary>
        /// Descripción del grupo (opcional)
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Capacidad máxima de alumnos
        /// </summary>
        public int CapacidadMaxima { get; set; }

        /// <summary>
        /// Indica si el grupo está activo
        /// </summary>
        public bool Activo { get; set; }

        #endregion

        #region Asignaciones

        /// <summary>
        /// ID del maestro titular del grupo
        /// </summary>
        public int? MaestroTitularId { get; set; }

        /// <summary>
        /// Maestro titular (Navigation Property)
        /// </summary>
        public virtual Maestro MaestroTitular { get; set; }

        /// <summary>
        /// Aula asignada al grupo
        /// Ejemplo: "Aula 101", "Salón A-2"
        /// </summary>
        public string? Aula { get; set; }

        /// <summary>
        /// Turno del grupo
        /// </summary>
        public Turno? Turno { get; set; }

        #endregion

        #region Horarios

        /// <summary>
        /// Hora de inicio de clases
        /// </summary>
        public TimeSpan? HoraInicio { get; set; }

        /// <summary>
        /// Hora de fin de clases
        /// </summary>
        public TimeSpan? HoraFin { get; set; }

        /// <summary>
        /// Días de la semana que tiene clases
        /// Ejemplo: "Lunes,Martes,Miércoles,Jueves,Viernes"
        /// </summary>
        public string DiasClase { get; set; }

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

        #region Navigation Properties (Relaciones)

        /// <summary>
        /// Inscripciones de alumnos a este grupo
        /// </summary>
        public virtual ICollection<Inscripcion> Inscripciones { get; set; }

        /// <summary>
        /// Asignaciones de materias y maestros para este grupo. 
        /// CORRECCIÓN: Nombre ajustado a 'GrupoMateriaMaestros' para coincidir con la configuración de EF Core.
        /// </summary>
        public virtual ICollection<GrupoMateriaMaestro> GrupoMateriaMaestros { get; set; }

        /// <summary>
        /// Asistencias registradas en este grupo
        /// </summary>
        public virtual ICollection<Evaluacion.Asistencia> Asistencias { get; set; }

        /// <summary>
        /// Calificaciones de alumnos de este grupo
        /// </summary>
        public virtual ICollection<Evaluacion.Calificacion> Calificaciones { get; set; }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Nombre completo del grupo (incluye grado y nivel)
        /// Ejemplo: "3° de Primaria - Grupo A"
        /// </summary>
        public string NombreCompleto
        {
            get
            {
                if (Grado?.NivelEducativo != null)
                    return $"{Grado.Nombre} de {Grado.NivelEducativo.Nombre} - {Nombre}";

                if (Grado != null)
                    return $"{Grado.Nombre} - {Nombre}";

                return $"{Nombre}";
            }
        }

        /// <summary>
        /// Cantidad de alumnos inscritos actualmente
        /// </summary>
        public int CantidadAlumnos => Inscripciones?.Count(i => i.Estatus == EstatusInscripcion.Inscrito) ?? 0;

        /// <summary>
        /// Cantidad de espacios disponibles
        /// </summary>
        public int EspaciosDisponibles => CapacidadMaxima - CantidadAlumnos;

        /// <summary>
        /// Porcentaje de ocupación del grupo
        /// </summary>
        public decimal PorcentajeOcupacion
        {
            get
            {
                if (CapacidadMaxima == 0)
                    return 0;

                return (decimal)CantidadAlumnos / CapacidadMaxima * 100;
            }
        }

        /// <summary>
        /// Cantidad de materias asignadas
        /// </summary>
        public int CantidadMaterias => GrupoMateriaMaestros?.Count ?? 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Grupo()
        {
            Activo = true;
            CapacidadMaxima = 30;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            // Inicializar colecciones
            Inscripciones = new HashSet<Inscripcion>();
            GrupoMateriaMaestros = new HashSet<GrupoMateriaMaestro>(); // CORREGIDO
            Asistencias = new HashSet<Evaluacion.Asistencia>();
            Calificaciones = new HashSet<Evaluacion.Calificacion>();
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si el grupo está activo
        /// </summary>
        public bool EstaActivo()
        {
            return Activo && (Grado?.Activo ?? true);
        }

        /// <summary>
        /// Verifica si el grupo tiene capacidad disponible
        /// </summary>
        public bool TieneCapacidadDisponible()
        {
            return CantidadAlumnos < CapacidadMaxima;
        }

        /// <summary>
        /// Verifica si el grupo está lleno
        /// </summary>
        public bool EstaLleno()
        {
            return CantidadAlumnos >= CapacidadMaxima;
        }

        /// <summary>
        /// Verifica si puede inscribir una cantidad específica de alumnos
        /// </summary>
        public bool PuedeInscribir(int cantidad)
        {
            return (CantidadAlumnos + cantidad) <= CapacidadMaxima;
        }

        /// <summary>
        /// Activa el grupo
        /// </summary>
        public void Activar(int usuarioId)
        {
            Activo = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Desactiva el grupo
        /// </summary>
        public void Desactivar(int usuarioId)
        {
            Activo = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si el grupo tiene maestro titular asignado
        /// </summary>
        public bool TieneMaestroTitular()
        {
            return MaestroTitularId.HasValue;
        }

        /// <summary>
        /// Verifica si el grupo tiene alumnos inscritos
        /// </summary>
        public bool TieneAlumnos()
        {
            return Inscripciones != null && Inscripciones.Any(i => i.Estatus == EstatusInscripcion.Inscrito);
        }

        /// <summary>
        /// Obtiene los alumnos inscritos actualmente
        /// </summary>
        public IEnumerable<Alumno> ObtenerAlumnosInscritos()
        {
            if (Inscripciones == null)
                return Enumerable.Empty<Alumno>();

            return Inscripciones
                .Where(i => i.Estatus == EstatusInscripcion.Inscrito)
                .OrderBy(i => i.NumeroLista)
                .Select(i => i.Alumno)
                .Where(a => a != null);
        }

        /// <summary>
        /// Obtiene las materias que se imparten en el grupo
        /// </summary>
        public IEnumerable<Materia> ObtenerMaterias()
        {
            if (GrupoMateriaMaestros == null) // CORREGIDO
                return Enumerable.Empty<Materia>();

            return GrupoMateriaMaestros // CORREGIDO
                .Select(am => am.Materia)
                .Where(m => m != null)
                .Distinct();
        }

        /// <summary>
        /// Obtiene los maestros que imparten clases en el grupo
        /// </summary>
        public IEnumerable<Maestro> ObtenerMaestros()
        {
            var maestros = new List<Maestro>();

            // Agregar maestro titular si existe
            if (MaestroTitular != null)
                maestros.Add(MaestroTitular);

            // Agregar maestros de materias
            if (GrupoMateriaMaestros != null) // CORREGIDO
            {
                maestros.AddRange(
                    GrupoMateriaMaestros // CORREGIDO
                        .Select(am => am.Maestro)
                        .Where(m => m != null && !m.Id.Equals(MaestroTitularId))
                );
            }

            return maestros.Distinct();
        }

        /// <summary>
        /// Verifica si un maestro imparte alguna materia en el grupo
        /// </summary>
        public bool TieneMaestro(int maestroId)
        {
            // Comparar MaestroTitularId con maestroId solo si MaestroTitularId tiene valor y el tipo es compatible
            if (MaestroTitularId.HasValue && MaestroTitularId.Value.ToString() == maestroId.ToString())
                return true;

            return GrupoMateriaMaestros != null && // CORREGIDO
                   GrupoMateriaMaestros.Any(am => am.MaestroId == maestroId);
        }

        /// <summary>
        /// Verifica si una materia está asignada al grupo
        /// </summary>
        public bool TieneMateria(int materiaId)
        {
            return GrupoMateriaMaestros != null && // CORREGIDO
                   GrupoMateriaMaestros.Any(am => am.MateriaId == materiaId);
        }

        /// <summary>
        /// Obtiene el maestro que imparte una materia específica
        /// </summary>
        public Maestro? ObtenerMaestroDe(int materiaId) => GrupoMateriaMaestros? // CORREGIDO
                .FirstOrDefault(am => am.MateriaId == materiaId)?
                .Maestro;

        /// <summary>
        /// Asigna un maestro titular al grupo
        /// </summary>
        public void AsignarMaestroTitular(int maestroId, int usuarioId)
        {
            MaestroTitularId = maestroId;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Remueve el maestro titular
        /// </summary>
        public void RemoverMaestroTitular(int usuarioId)
        {
            MaestroTitularId = null;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Valida que la configuración del grupo sea consistente
        /// </summary>
        public bool ConfiguracionEsValida(out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                mensajeError = "El nombre del grupo es requerido";
                return false;
            }

            if (string.IsNullOrWhiteSpace(CicloEscolar))
            {
                mensajeError = "El ciclo escolar es requerido";
                return false;
            }

            if (CapacidadMaxima <= 0)
            {
                mensajeError = "La capacidad máxima debe ser mayor a 0";
                return false;
            }

            if (HoraInicio.HasValue && HoraFin.HasValue && HoraInicio.Value >= HoraFin.Value)
            {
                mensajeError = "La hora de inicio debe ser menor que la hora de fin";
                return false;
            }

            return true;
        }

        #endregion
    }
}
