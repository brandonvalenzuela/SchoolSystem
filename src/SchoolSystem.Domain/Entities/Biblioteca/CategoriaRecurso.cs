using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;

namespace SchoolSystem.Domain.Entities.Biblioteca
{
    /// <summary>
    /// Categorías para clasificar recursos de la biblioteca
    /// </summary>
    [Table("CategoriasRecurso")]
    public class CategoriaRecurso : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Nombre de la categoría
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción de la categoría
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Descripcion { get; set; }

        #endregion

        #region Configuración

        /// <summary>
        /// Color para identificación visual (hexadecimal)
        /// </summary>
        [StringLength(20)]
        public string Color { get; set; }

        /// <summary>
        /// Icono de la categoría
        /// </summary>
        [StringLength(50)]
        public string Icono { get; set; }

        /// <summary>
        /// Código de la categoría (para referencia rápida)
        /// </summary>
        [StringLength(20)]
        public string Codigo { get; set; }

        /// <summary>
        /// Orden de visualización
        /// </summary>
        public int Orden { get; set; }

        /// <summary>
        /// Indica si está activa
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        #endregion

        #region Estadísticas

        /// <summary>
        /// Cantidad de recursos en esta categoría
        /// </summary>
        public int CantidadRecursos { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Recursos en esta categoría
        /// </summary>
        public virtual ICollection<Libro> Recursos { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public CategoriaRecurso()
        {
            Activo = true;
            Orden = 0;
            CantidadRecursos = 0;
            Recursos = new HashSet<Libro>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si tiene recursos asignados
        /// </summary>
        public bool TieneRecursos => CantidadRecursos > 0;

        /// <summary>
        /// Nombre completo con código
        /// </summary>
        public string NombreCompleto => string.IsNullOrWhiteSpace(Codigo) ? Nombre : $"{Codigo} - {Nombre}";

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Activa la categoría
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
        /// Desactiva la categoría
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
        /// Actualiza la cantidad de recursos
        /// </summary>
        public void ActualizarCantidadRecursos(int cantidad)
        {
            CantidadRecursos = cantidad;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Incrementa el contador de recursos
        /// </summary>
        public void IncrementarRecursos()
        {
            CantidadRecursos++;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Decrementa el contador de recursos
        /// </summary>
        public void DecrementarRecursos()
        {
            if (CantidadRecursos > 0)
            {
                CantidadRecursos--;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Establece el orden de visualización
        /// </summary>
        public void EstablecerOrden(int orden)
        {
            Orden = orden;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida que la categoría sea correcta
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre es requerido");

            if (Nombre?.Length > 100)
                errores.Add("El nombre no puede exceder 100 caracteres");

            if (CantidadRecursos < 0)
                errores.Add("La cantidad de recursos no puede ser negativa");

            if (Orden < 0)
                errores.Add("El orden no puede ser negativo");

            return errores;
        }

        #endregion
    }
}