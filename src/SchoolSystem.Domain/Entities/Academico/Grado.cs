using SchoolSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad Grado - Representa un grado escolar dentro de un nivel educativo
    /// Ejemplos: 1°, 2°, 3° de Primaria | 1°, 2°, 3° de Secundaria
    /// </summary>
    public class Grado : BaseEntity, IAuditableEntity
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

        #region Vinculación con Nivel Educativo

        /// <summary>
        /// ID del nivel educativo al que pertenece
        /// </summary>
        public int NivelEducativoId { get; set; }

        /// <summary>
        /// Nivel educativo asociado (Navigation Property)
        /// </summary>
        public virtual NivelEducativo NivelEducativo { get; set; }

        #endregion

        #region Propiedades Básicas

        /// <summary>
        /// Nombre del grado
        /// Ejemplos: "1°", "2°", "3°" o "A", "B", "C" para kinder
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción del grado (opcional)
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Orden dentro del nivel educativo
        /// Ejemplo: 1=Primer grado, 2=Segundo grado, etc.
        /// </summary>
        public int Orden { get; set; }

        /// <summary>
        /// Indica si el grado está activo
        /// </summary>
        public bool Activo { get; set; }

        #endregion

        #region Configuración del Grado

        /// <summary>
        /// Edad recomendada para el grado
        /// </summary>
        public int? EdadRecomendada { get; set; }

        /// <summary>
        /// Capacidad máxima de alumnos por grupo en este grado
        /// </summary>
        public int? CapacidadMaximaPorGrupo { get; set; }

        /// <summary>
        /// Horas de clase semanales totales
        /// </summary>
        public int? HorasSemanales { get; set; }

        /// <summary>
        /// Requisitos para ingresar a este grado
        /// </summary>
        public string Requisitos { get; set; }

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
        /// Grupos de este grado
        /// Ejemplo: 3° de Primaria tiene grupos A, B, C
        /// </summary>
        public virtual ICollection<Grupo> Grupos { get; set; }

        /// <summary>
        /// Materias asignadas a este grado
        /// </summary>
        public virtual ICollection<GradoMateria> GradoMaterias { get; set; }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Nombre completo del grado (incluye nivel educativo)
        /// Ejemplo: "1° de Primaria", "3° de Secundaria"
        /// </summary>
        public string NombreCompleto
        {
            get
            {
                if (NivelEducativo != null)
                    return $"{Nombre} de {NivelEducativo.Nombre}";

                return Nombre;
            }
        }

        /// <summary>
        /// Cantidad de grupos en este grado
        /// </summary>
        public int CantidadGrupos => Grupos?.Count ?? 0;

        /// <summary>
        /// Cantidad de grupos activos
        /// </summary>
        public int CantidadGruposActivos => Grupos?.Count(g => g.Activo) ?? 0;

        /// <summary>
        /// Cantidad de materias asignadas
        /// </summary>
        public int CantidadMaterias => GradoMaterias?.Count ?? 0;

        /// <summary>
        /// Total de horas de materias asignadas
        /// </summary>
        public int TotalHorasMaterias => GradoMaterias?.Sum(gm => gm.HorasSemanales ?? 0) ?? 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Grado()
        {
            Activo = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            // Inicializar colecciones
            Grupos = new HashSet<Grupo>();
            GradoMaterias = new HashSet<GradoMateria>();
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si el grado está activo
        /// </summary>
        public bool EstaActivo()
        {
            return Activo && (NivelEducativo?.Activo ?? true);
        }

        /// <summary>
        /// Verifica si un alumno tiene la edad adecuada para este grado
        /// </summary>
        public bool EdadEsApropiada(int edad)
        {
            if (!EdadRecomendada.HasValue)
                return true; // Sin restricción de edad

            // Permitir +/- 2 años de la edad recomendada
            return Math.Abs(edad - EdadRecomendada.Value) <= 2;
        }

        /// <summary>
        /// Activa el grado
        /// </summary>
        public void Activar(int usuarioId)
        {
            Activo = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Desactiva el grado
        /// </summary>
        public void Desactivar(int usuarioId)
        {
            Activo = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si el grado tiene grupos
        /// </summary>
        public bool TieneGrupos()
        {
            return Grupos != null && Grupos.Any();
        }

        /// <summary>
        /// Verifica si el grado tiene grupos activos
        /// </summary>
        public bool TieneGruposActivos()
        {
            return Grupos != null && Grupos.Any(g => g.Activo);
        }

        /// <summary>
        /// Verifica si el grado tiene materias asignadas
        /// </summary>
        public bool TieneMaterias()
        {
            return GradoMaterias != null && GradoMaterias.Any();
        }

        /// <summary>
        /// Obtiene los grupos activos ordenados por nombre
        /// </summary>
        public IEnumerable<Grupo> ObtenerGruposActivos()
        {
            if (Grupos == null)
                return Enumerable.Empty<Grupo>();

            return Grupos
                .Where(g => g.Activo)
                .OrderBy(g => g.Nombre);
        }

        /// <summary>
        /// Obtiene las materias ordenadas por orden
        /// </summary>
        public IEnumerable<Materia> ObtenerMaterias()
        {
            if (GradoMaterias == null)
                return Enumerable.Empty<Materia>();

            return GradoMaterias
                .OrderBy(gm => gm.Orden)
                .Select(gm => gm.Materia)
                .Where(m => m != null);
        }

        /// <summary>
        /// Obtiene una materia específica del grado
        /// </summary>
        public GradoMateria ObtenerMateria(int materiaId)
        {
            return GradoMaterias?.FirstOrDefault(gm => gm.MateriaId == materiaId);
        }

        /// <summary>
        /// Verifica si una materia está asignada a este grado
        /// </summary>
        public bool TieneMateria(int materiaId)
        {
            return GradoMaterias != null && GradoMaterias.Any(gm => gm.MateriaId == materiaId);
        }

        /// <summary>
        /// Calcula el total de alumnos en el grado (todos los grupos)
        /// </summary>
        public int CalcularTotalAlumnos()
        {
            if (Grupos == null)
                return 0;

            // Esto requeriría acceso a inscripciones
            // Es solo un placeholder para la lógica
            return 0;
        }

        /// <summary>
        /// Verifica si hay capacidad disponible en algún grupo
        /// </summary>
        public bool TieneCapacidadDisponible()
        {
            if (Grupos == null || !Grupos.Any())
                return true; // Sin grupos, hay capacidad ilimitada

            return Grupos.Any(g => g.Activo && g.TieneCapacidadDisponible());
        }

        /// <summary>
        /// Valida que la configuración del grado sea consistente
        /// </summary>
        public bool ConfiguracionEsValida(out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                mensajeError = "El nombre del grado es requerido";
                return false;
            }

            if (Orden < 0)
            {
                mensajeError = "El orden no puede ser negativo";
                return false;
            }

            if (CapacidadMaximaPorGrupo.HasValue && CapacidadMaximaPorGrupo.Value <= 0)
            {
                mensajeError = "La capacidad máxima debe ser mayor a 0";
                return false;
            }

            if (HorasSemanales.HasValue && HorasSemanales.Value <= 0)
            {
                mensajeError = "Las horas semanales deben ser mayores a 0";
                return false;
            }

            return true;
        }

        #endregion
    }
}