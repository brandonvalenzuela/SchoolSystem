using SchoolSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad NivelEducativo - Representa un nivel educativo en la escuela
    /// Ejemplos: Kinder, Primaria, Secundaria, Preparatoria, Universidad
    /// </summary>
    public class NivelEducativo : BaseEntity, IAuditableEntity
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
        /// Nombre del nivel educativo
        /// Ejemplos: "Kinder", "Primaria", "Secundaria", "Preparatoria"
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Abreviatura del nivel educativo
        /// Ejemplos: "PREES", "K", "PRIM", "SEC", "PREP"
        /// </summary>
        public string? Abreviatura { get; set; }

        /// <summary>
        /// Descripción del nivel educativo
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Orden de visualización y jerarquía
        /// Ejemplo: 1=Kinder, 2=Primaria, 3=Secundaria, 4=Preparatoria
        /// </summary>
        public int Orden { get; set; }

        /// <summary>
        /// Indica si el nivel está activo
        /// </summary>
        public bool Activo { get; set; }

        #endregion

        #region Configuración del Nivel

        /// <summary>
        /// Edad mínima requerida para ingresar al nivel
        /// </summary>
        public int? EdadMinima { get; set; }

        /// <summary>
        /// Edad máxima permitida en el nivel
        /// </summary>
        public int? EdadMaxima { get; set; }

        /// <summary>
        /// Duración en años del nivel educativo
        /// Ejemplo: Primaria = 6 años, Secundaria = 3 años
        /// </summary>
        public int? DuracionAños { get; set; }

        /// <summary>
        /// Color representativo del nivel (código hexadecimal para UI)
        /// Ejemplo: "#FF5733"
        /// </summary>
        public string? Color { get; set; }

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
        /// Grados que pertenecen a este nivel educativo
        /// Ejemplo: Primaria tiene grados 1°, 2°, 3°, 4°, 5°, 6°
        /// </summary>
        public virtual ICollection<Grado> Grados { get; set; }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Cantidad de grados en este nivel
        /// </summary>
        public int CantidadGrados => Grados?.Count ?? 0;

        /// <summary>
        /// Cantidad de grados activos en este nivel
        /// </summary>
        public int CantidadGradosActivos => Grados?.Count(g => g.Activo) ?? 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public NivelEducativo()
        {
            Activo = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            // Inicializar colecciones
            Grados = new HashSet<Grado>();
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si el nivel está activo
        /// </summary>
        public bool EstaActivo()
        {
            return Activo;
        }

        /// <summary>
        /// Verifica si un alumno tiene la edad adecuada para este nivel
        /// </summary>
        public bool EdadEsApropiada(int edad)
        {
            if (!EdadMinima.HasValue && !EdadMaxima.HasValue)
                return true; // Sin restricción de edad

            if (EdadMinima.HasValue && edad < EdadMinima.Value)
                return false;

            if (EdadMaxima.HasValue && edad > EdadMaxima.Value)
                return false;

            return true;
        }

        /// <summary>
        /// Verifica si un alumno tiene la edad adecuada basándose en su fecha de nacimiento
        /// </summary>
        public bool EdadEsApropiada(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento.Date > hoy.AddYears(-edad))
                edad--;

            return EdadEsApropiada(edad);
        }

        /// <summary>
        /// Activa el nivel educativo
        /// </summary>
        public void Activar(int usuarioId)
        {
            Activo = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Desactiva el nivel educativo
        /// </summary>
        public void Desactivar(int usuarioId)
        {
            Activo = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si el nivel tiene grados asociados
        /// </summary>
        public bool TieneGrados()
        {
            return Grados != null && Grados.Any();
        }

        /// <summary>
        /// Verifica si el nivel tiene grados activos
        /// </summary>
        public bool TieneGradosActivos()
        {
            return Grados != null && Grados.Any(g => g.Activo);
        }

        /// <summary>
        /// Obtiene los grados activos del nivel ordenados
        /// </summary>
        public IEnumerable<Grado> ObtenerGradosActivos()
        {
            if (Grados == null)
                return Enumerable.Empty<Grado>();

            return Grados
                .Where(g => g.Activo)
                .OrderBy(g => g.Orden);
        }

        /// <summary>
        /// Obtiene el primer grado del nivel (por orden)
        /// </summary>
        public Grado? ObtenerPrimerGrado()
        {
            return Grados?
                .Where(g => g.Activo)
                .OrderBy(g => g.Orden)
                .FirstOrDefault();
        }

        /// <summary>
        /// Obtiene el último grado del nivel (por orden)
        /// </summary>
        public Grado? ObtenerUltimoGrado()
        {
            return Grados?
                .Where(g => g.Activo)
                .OrderByDescending(g => g.Orden)
                .FirstOrDefault();
        }

        /// <summary>
        /// Valida que la configuración del nivel sea consistente
        /// </summary>
        public bool ConfiguracionEsValida(out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                mensajeError = "El nombre del nivel es requerido";
                return false;
            }

            if (EdadMinima.HasValue && EdadMaxima.HasValue && EdadMinima.Value > EdadMaxima.Value)
            {
                mensajeError = "La edad mínima no puede ser mayor que la edad máxima";
                return false;
            }

            if (DuracionAños.HasValue && DuracionAños.Value <= 0)
            {
                mensajeError = "La duración en años debe ser mayor a 0";
                return false;
            }

            if (Orden < 0)
            {
                mensajeError = "El orden no puede ser negativo";
                return false;
            }

            return true;
        }

        #endregion
    }
}