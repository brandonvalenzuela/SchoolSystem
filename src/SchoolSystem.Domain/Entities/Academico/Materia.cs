using SchoolSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad Materia - Representa una asignatura o materia que se imparte
    /// Ejemplos: Matemáticas, Español, Historia, Ciencias Naturales
    /// </summary>
    public class Materia : BaseEntity, IAuditableEntity
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

        #region Propiedades Básicas

        /// <summary>
        /// Nombre de la materia
        /// Ejemplo: "Matemáticas", "Español", "Historia"
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Clave o código de la materia
        /// Ejemplo: "MAT", "ESP", "HIST"
        /// </summary>
        public string Clave { get; set; }

        /// <summary>
        /// Descripción de la materia
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Color representativo de la materia (código hexadecimal para UI)
        /// Ejemplo: "#FF5733"
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Indica si la materia está activa
        /// </summary>
        public bool Activo { get; set; }

        #endregion

        #region Configuración de la Materia

        /// <summary>
        /// Área o departamento al que pertenece
        /// Ejemplo: "Ciencias", "Humanidades", "Artes"
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Tipo de materia
        /// Ejemplo: "Teórica", "Práctica", "Taller", "Laboratorio"
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Nivel de dificultad (1-5)
        /// </summary>
        public int? NivelDificultad { get; set; }

        /// <summary>
        /// Indica si requiere materiales especiales
        /// </summary>
        public bool RequiereMateriales { get; set; }

        /// <summary>
        /// Lista de materiales requeridos
        /// </summary>
        public string MaterialesRequeridos { get; set; }

        /// <summary>
        /// Indica si requiere instalaciones especiales (laboratorio, taller, etc.)
        /// </summary>
        public bool RequiereInstalacionesEspeciales { get; set; }

        /// <summary>
        /// Instalaciones requeridas
        /// </summary>
        public string InstalacionesRequeridas { get; set; }

        #endregion

        #region Objetivos y Contenido

        /// <summary>
        /// Objetivos generales de la materia
        /// </summary>
        public string Objetivos { get; set; }

        /// <summary>
        /// Competencias que desarrolla la materia
        /// </summary>
        public string Competencias { get; set; }

        /// <summary>
        /// Contenido temático general
        /// </summary>
        public string ContenidoTematico { get; set; }

        /// <summary>
        /// Bibliografía recomendada
        /// </summary>
        public string Bibliografia { get; set; }

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
        /// Relación con grados (en qué grados se imparte esta materia)
        /// </summary>
        public virtual ICollection<GradoMateria> GradoMaterias { get; set; }

        /// <summary>
        /// Asignaciones de esta materia a grupos con maestros
        /// </summary>
        public virtual ICollection<GrupoMateriaMaestro> AsignacionesGrupos { get; set; }

        /// <summary>
        /// Calificaciones de alumnos en esta materia
        /// </summary>
        public virtual ICollection<Evaluacion.Calificacion> Calificaciones { get; set; }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Cantidad de grados en los que se imparte
        /// </summary>
        public int CantidadGrados => GradoMaterias?.Count ?? 0;

        /// <summary>
        /// Cantidad de grupos en los que se imparte actualmente
        /// </summary>
        public int CantidadGrupos => AsignacionesGrupos?.Select(ag => ag.GrupoId).Distinct().Count() ?? 0;

        /// <summary>
        /// Cantidad de maestros que la imparten
        /// </summary>
        public int CantidadMaestros => AsignacionesGrupos?.Select(ag => ag.MaestroId).Distinct().Count() ?? 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Materia()
        {
            Activo = true;
            RequiereMateriales = false;
            RequiereInstalacionesEspeciales = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            // Inicializar colecciones
            GradoMaterias = new HashSet<GradoMateria>();
            AsignacionesGrupos = new HashSet<GrupoMateriaMaestro>();
            Calificaciones = new HashSet<Evaluacion.Calificacion>();
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si la materia está activa
        /// </summary>
        public bool EstaActiva()
        {
            return Activo;
        }

        /// <summary>
        /// Activa la materia
        /// </summary>
        public void Activar(int usuarioId)
        {
            Activo = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Desactiva la materia
        /// </summary>
        public void Desactivar(int usuarioId)
        {
            Activo = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si la materia se imparte en algún grado
        /// </summary>
        public bool SeImparteEnAlgunGrado()
        {
            return GradoMaterias != null && GradoMaterias.Any();
        }

        /// <summary>
        /// Verifica si la materia se imparte en un grado específico
        /// </summary>
        public bool SeImparteEn(int gradoId)
        {
            return GradoMaterias != null && GradoMaterias.Any(gm => gm.GradoId == gradoId);
        }

        /// <summary>
        /// Obtiene los grados en los que se imparte la materia
        /// </summary>
        public IEnumerable<Grado> ObtenerGrados()
        {
            if (GradoMaterias == null)
                return Enumerable.Empty<Grado>();

            return GradoMaterias
                .Select(gm => gm.Grado)
                .Where(g => g != null && g.Activo)
                .OrderBy(g => g.NivelEducativoId)
                .ThenBy(g => g.Orden);
        }

        /// <summary>
        /// Obtiene los grupos en los que se imparte actualmente
        /// </summary>
        public IEnumerable<Grupo> ObtenerGrupos()
        {
            if (AsignacionesGrupos == null)
                return Enumerable.Empty<Grupo>();

            return AsignacionesGrupos
                .Select(ag => ag.Grupo)
                .Where(g => g != null && g.Activo)
                .Distinct()
                .OrderBy(g => g.GradoId)
                .ThenBy(g => g.Nombre);
        }

        /// <summary>
        /// Obtiene los maestros que imparten la materia
        /// </summary>
        public IEnumerable<Maestro> ObtenerMaestros()
        {
            if (AsignacionesGrupos == null)
                return Enumerable.Empty<Maestro>();

            return AsignacionesGrupos
                .Select(ag => ag.Maestro)
                .Where(m => m != null)
                .Distinct();
        }

        /// <summary>
        /// Verifica si un maestro imparte esta materia
        /// </summary>
        public bool LaImparteMaestro(int maestroId)
        {
            return AsignacionesGrupos != null &&
                   AsignacionesGrupos.Any(ag => ag.MaestroId == maestroId);
        }

        /// <summary>
        /// Obtiene las horas semanales totales de la materia en un grado específico
        /// </summary>
        public int? ObtenerHorasSemanalesEn(int gradoId)
        {
            return GradoMaterias?
                .FirstOrDefault(gm => gm.GradoId == gradoId)?
                .HorasSemanales;
        }

        /// <summary>
        /// Verifica si la materia es obligatoria en un grado específico
        /// </summary>
        public bool EsObligatoriaEn(int gradoId)
        {
            var gradoMateria = GradoMaterias?.FirstOrDefault(gm => gm.GradoId == gradoId);
            return gradoMateria?.Obligatoria ?? false;
        }

        /// <summary>
        /// Verifica si la materia requiere recursos especiales
        /// </summary>
        public bool RequiereRecursosEspeciales()
        {
            return RequiereMateriales || RequiereInstalacionesEspeciales;
        }

        /// <summary>
        /// Verifica si tiene objetivos y contenido definidos
        /// </summary>
        public bool TieneContenidoDefinido()
        {
            return !string.IsNullOrWhiteSpace(Objetivos) ||
                   !string.IsNullOrWhiteSpace(ContenidoTematico);
        }

        /// <summary>
        /// Obtiene el total de alumnos que cursan la materia actualmente
        /// </summary>
        public int ObtenerTotalAlumnos()
        {
            if (AsignacionesGrupos == null)
                return 0;

            return AsignacionesGrupos
                .Select(ag => ag.Grupo)
                .Where(g => g != null)
                .Sum(g => g.CantidadAlumnos);
        }

        /// <summary>
        /// Calcula la carga académica total de la materia (grupos * horas)
        /// </summary>
        public int CalcularCargaAcademica()
        {
            if (AsignacionesGrupos == null)
                return 0;

            return AsignacionesGrupos.Count *
                   (GradoMaterias?.FirstOrDefault()?.HorasSemanales ?? 0);
        }

        /// <summary>
        /// Valida que la configuración de la materia sea consistente
        /// </summary>
        public bool ConfiguracionEsValida(out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                mensajeError = "El nombre de la materia es requerido";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Clave))
            {
                mensajeError = "La clave de la materia es requerida";
                return false;
            }

            if (NivelDificultad.HasValue && (NivelDificultad.Value < 1 || NivelDificultad.Value > 5))
            {
                mensajeError = "El nivel de dificultad debe estar entre 1 y 5";
                return false;
            }

            if (RequiereMateriales && string.IsNullOrWhiteSpace(MaterialesRequeridos))
            {
                mensajeError = "Debe especificar los materiales requeridos";
                return false;
            }

            if (RequiereInstalacionesEspeciales && string.IsNullOrWhiteSpace(InstalacionesRequeridas))
            {
                mensajeError = "Debe especificar las instalaciones requeridas";
                return false;
            }

            return true;
        }

        #endregion
    }
}