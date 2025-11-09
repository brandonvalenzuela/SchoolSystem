using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums.Biblioteca;

namespace SchoolSystem.Domain.Entities.Biblioteca
{
    /// <summary>
    /// Libros y recursos de la biblioteca escolar
    /// </summary>
    [Table("Libros")]
    public class Libro : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Título del libro o recurso
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        /// <summary>
        /// Autor(es) del libro
        /// </summary>
        [StringLength(200)]
        public string Autor { get; set; }

        /// <summary>
        /// ISBN del libro
        /// </summary>
        [StringLength(20)]
        public string ISBN { get; set; }

        /// <summary>
        /// Editorial
        /// </summary>
        [StringLength(100)]
        public string Editorial { get; set; }

        /// <summary>
        /// Año de publicación
        /// </summary>
        public int? AnioPublicacion { get; set; }

        #endregion

        #region Clasificación

        /// <summary>
        /// Tipo de recurso
        /// </summary>
        [Required]
        public TipoRecurso Tipo { get; set; }

        /// <summary>
        /// Categoría del recurso
        /// </summary>
        public int? CategoriaId { get; set; }

        /// <summary>
        /// Código de clasificación (Dewey, LC, etc.)
        /// </summary>
        [StringLength(50)]
        public string CodigoClasificacion { get; set; }

        #endregion

        #region Detalles Físicos

        /// <summary>
        /// Número de páginas
        /// </summary>
        public int? NumeroPaginas { get; set; }

        /// <summary>
        /// Edición
        /// </summary>
        [StringLength(50)]
        public string Edicion { get; set; }

        /// <summary>
        /// Idioma
        /// </summary>
        [StringLength(50)]
        public string Idioma { get; set; }

        /// <summary>
        /// Descripción del libro
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Descripcion { get; set; }

        #endregion

        #region Inventario

        /// <summary>
        /// Cantidad total de ejemplares
        /// </summary>
        [Required]
        public int CantidadTotal { get; set; }

        /// <summary>
        /// Cantidad disponible para préstamo
        /// </summary>
        [Required]
        public int CantidadDisponible { get; set; }

        /// <summary>
        /// Cantidad actualmente prestada
        /// </summary>
        [Required]
        public int CantidadPrestada { get; set; }

        /// <summary>
        /// Cantidad extraviada
        /// </summary>
        public int CantidadExtraviada { get; set; }

        /// <summary>
        /// Cantidad dañada
        /// </summary>
        public int CantidadDaniada { get; set; }

        #endregion

        #region Ubicación y Estado

        /// <summary>
        /// Ubicación física en la biblioteca
        /// </summary>
        [StringLength(100)]
        public string Ubicacion { get; set; }

        /// <summary>
        /// Estante específico
        /// </summary>
        [StringLength(50)]
        public string Estante { get; set; }

        /// <summary>
        /// Estado general del recurso
        /// </summary>
        [Required]
        public EstadoRecurso? Estado { get; set; }

        /// <summary>
        /// Indica si está disponible para préstamo
        /// </summary>
        [Required]
        public bool DisponiblePrestamo { get; set; }

        #endregion

        #region Multimedia

        /// <summary>
        /// URL de la imagen de portada
        /// </summary>
        [StringLength(500)]
        public string ImagenPortadaUrl { get; set; }

        /// <summary>
        /// URL del recurso digital (si aplica)
        /// </summary>
        [StringLength(500)]
        public string RecursoDigitalUrl { get; set; }

        #endregion

        #region Valoración

        /// <summary>
        /// Calificación promedio (1-5)
        /// </summary>
        [Column(TypeName = "decimal(3,2)")]
        public decimal? CalificacionPromedio { get; set; }

        /// <summary>
        /// Cantidad de veces que ha sido prestado
        /// </summary>
        public int VecesPrestado { get; set; }

        /// <summary>
        /// Popularidad del recurso
        /// </summary>
        public int Popularidad { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Precio de adquisición
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioAdquisicion { get; set; }

        /// <summary>
        /// Fecha de adquisición
        /// </summary>
        public DateTime? FechaAdquisicion { get; set; }

        /// <summary>
        /// Proveedor
        /// </summary>
        [StringLength(100)]
        public string Proveedor { get; set; }

        /// <summary>
        /// Notas adicionales
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Notas { get; set; }

        /// <summary>
        /// Indica si está activo en el catálogo
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Categoría relacionada
        /// </summary>
        [ForeignKey("CategoriaId")]
        public virtual CategoriaRecurso Categoria { get; set; }

        /// <summary>
        /// Préstamos de este libro
        /// </summary>
        public virtual ICollection<Prestamo> Prestamos { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public Libro()
        {
            CantidadTotal = 1;
            CantidadDisponible = 1;
            CantidadPrestada = 0;
            CantidadExtraviada = 0;
            CantidadDaniada = 0;
            DisponiblePrestamo = true;
            Estado = EstadoRecurso.Disponible;
            Activo = true;
            VecesPrestado = 0;
            Popularidad = 0;
            Idioma = "Español";
            Prestamos = new HashSet<Prestamo>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si hay ejemplares disponibles
        /// </summary>
        public bool HayDisponibles => CantidadDisponible > 0;

        /// <summary>
        /// Indica si todos los ejemplares están prestados
        /// </summary>
        public bool TodosPrestados => CantidadPrestada == CantidadTotal;

        /// <summary>
        /// Indica si es un libro
        /// </summary>
        public bool EsLibro => Tipo == TipoRecurso.Libro;

        /// <summary>
        /// Indica si es recurso digital
        /// </summary>
        public bool EsDigital => !string.IsNullOrWhiteSpace(RecursoDigitalUrl);

        /// <summary>
        /// Indica si tiene portada
        /// </summary>
        public bool TienePortada => !string.IsNullOrWhiteSpace(ImagenPortadaUrl);

        /// <summary>
        /// Porcentaje de disponibilidad
        /// </summary>
        public decimal PorcentajeDisponibilidad
        {
            get
            {
                if (CantidadTotal == 0) return 0;
                return ((decimal)CantidadDisponible / CantidadTotal) * 100;
            }
        }

        /// <summary>
        /// Información completa del libro
        /// </summary>
        public string InformacionCompleta
        {
            get
            {
                var info = Titulo;
                if (!string.IsNullOrWhiteSpace(Autor))
                    info += $" - {Autor}";
                if (AnioPublicacion.HasValue)
                    info += $" ({AnioPublicacion})";
                return info;
            }
        }

        /// <summary>
        /// Años desde la adquisición
        /// </summary>
        public int? AniosDesdeAdquisicion
        {
            get
            {
                if (!FechaAdquisicion.HasValue) return null;
                return DateTime.Now.Year - FechaAdquisicion.Value.Year;
            }
        }

        /// <summary>
        /// Estado de inventario
        /// </summary>
        public string EstadoInventario
        {
            get
            {
                if (CantidadDisponible > 0) return "Disponible";
                if (CantidadPrestada > 0) return "Prestado";
                if (CantidadExtraviada > 0) return "Extraviado";
                if (CantidadDaniada > 0) return "Dañado";
                return "Sin ejemplares";
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Registra un préstamo del libro
        /// </summary>
        public void RegistrarPrestamo()
        {
            if (CantidadDisponible <= 0)
                throw new InvalidOperationException("No hay ejemplares disponibles para préstamo");

            if (!DisponiblePrestamo)
                throw new InvalidOperationException("Este recurso no está disponible para préstamo");

            CantidadDisponible--;
            CantidadPrestada++;
            VecesPrestado++;
            Popularidad++;

            if (CantidadDisponible == 0)
                Estado = EstadoRecurso.Prestado;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Registra una devolución del libro
        /// </summary>
        public void RegistrarDevolucion()
        {
            if (CantidadPrestada <= 0)
                throw new InvalidOperationException("No hay ejemplares prestados para devolver");

            CantidadPrestada--;
            CantidadDisponible++;

            if (CantidadDisponible > 0 && Estado == EstadoRecurso.Prestado)
                Estado = EstadoRecurso.Disponible;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca un ejemplar como extraviado
        /// </summary>
        public void MarcarComoExtraviado()
        {
            if (CantidadPrestada <= 0)
                throw new InvalidOperationException("No hay ejemplares prestados que puedan estar extraviados");

            CantidadPrestada--;
            CantidadExtraviada++;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca un ejemplar como dañado
        /// </summary>
        /// <param name="desdeDisponible">Si se marca desde disponible (true) o desde prestado (false)</param>
        public void MarcarComoDaniado(bool desdeDisponible = false)
        {
            if (desdeDisponible)
            {
                if (CantidadDisponible <= 0)
                    throw new InvalidOperationException("No hay ejemplares disponibles");
                CantidadDisponible--;
            }
            else
            {
                if (CantidadPrestada <= 0)
                    throw new InvalidOperationException("No hay ejemplares prestados");
                CantidadPrestada--;
            }

            CantidadDaniada++;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Recupera un ejemplar extraviado
        /// </summary>
        public void RecuperarExtraviado()
        {
            if (CantidadExtraviada <= 0)
                throw new InvalidOperationException("No hay ejemplares extraviados para recuperar");

            CantidadExtraviada--;
            CantidadDisponible++;

            if (CantidadDisponible > 0)
                Estado = EstadoRecurso.Disponible;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Repara un ejemplar dañado
        /// </summary>
        public void RepararDaniado()
        {
            if (CantidadDaniada <= 0)
                throw new InvalidOperationException("No hay ejemplares dañados para reparar");

            CantidadDaniada--;
            CantidadDisponible++;

            if (CantidadDisponible > 0)
                Estado = EstadoRecurso.Disponible;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Da de baja un ejemplar permanentemente
        /// </summary>
        /// <param name="origen">De dónde se da de baja (disponible, dañado, extraviado)</param>
        public void DarDeBaja(string origen = "dañado")
        {
            switch (origen.ToLower())
            {
                case "disponible":
                    if (CantidadDisponible <= 0)
                        throw new InvalidOperationException("No hay ejemplares disponibles");
                    CantidadDisponible--;
                    break;
                case "dañado":
                    if (CantidadDaniada <= 0)
                        throw new InvalidOperationException("No hay ejemplares dañados");
                    CantidadDaniada--;
                    break;
                case "extraviado":
                    if (CantidadExtraviada <= 0)
                        throw new InvalidOperationException("No hay ejemplares extraviados");
                    CantidadExtraviada--;
                    break;
                default:
                    throw new ArgumentException("Origen no válido. Use: disponible, dañado o extraviado");
            }

            CantidadTotal--;

            if (CantidadTotal == 0)
            {
                Estado = EstadoRecurso.NoDisponible;
                DisponiblePrestamo = false;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Agrega nuevos ejemplares al inventario
        /// </summary>
        public void AgregarEjemplares(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero");

            CantidadTotal += cantidad;
            CantidadDisponible += cantidad;

            if (CantidadDisponible > 0)
            {
                Estado = EstadoRecurso.Disponible;
                DisponiblePrestamo = true;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza la calificación promedio
        /// </summary>
        public void ActualizarCalificacion(decimal nuevaCalificacion)
        {
            if (nuevaCalificacion < 1 || nuevaCalificacion > 5)
                throw new ArgumentException("La calificación debe estar entre 1 y 5");

            CalificacionPromedio = nuevaCalificacion;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Habilita o deshabilita el préstamo
        /// </summary>
        public void ConfigurarPrestamo(bool disponible)
        {
            DisponiblePrestamo = disponible;

            if (!disponible && CantidadDisponible > 0)
                Estado = EstadoRecurso.NoPrestable;
            else if (disponible && CantidadDisponible > 0)
                Estado = EstadoRecurso.Disponible;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Activa el recurso en el catálogo
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
        /// Desactiva el recurso del catálogo
        /// </summary>
        public void Desactivar()
        {
            if (Activo)
            {
                Activo = false;
                DisponiblePrestamo = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Valida que el libro sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Titulo))
                errores.Add("El título es requerido");

            if (Titulo?.Length > 200)
                errores.Add("El título no puede exceder 200 caracteres");

            if (CantidadTotal < 0)
                errores.Add("La cantidad total no puede ser negativa");

            if (CantidadDisponible < 0)
                errores.Add("La cantidad disponible no puede ser negativa");

            if (CantidadPrestada < 0)
                errores.Add("La cantidad prestada no puede ser negativa");

            if (CantidadExtraviada < 0)
                errores.Add("La cantidad extraviada no puede ser negativa");

            if (CantidadDaniada < 0)
                errores.Add("La cantidad dañada no puede ser negativa");

            var sumaCantidades = CantidadDisponible + CantidadPrestada + CantidadExtraviada + CantidadDaniada;
            if (sumaCantidades != CantidadTotal)
                errores.Add("La suma de las cantidades (disponible + prestada + extraviada + dañada) debe ser igual a la cantidad total");

            if (CalificacionPromedio.HasValue && (CalificacionPromedio < 1 || CalificacionPromedio > 5))
                errores.Add("La calificación promedio debe estar entre 1 y 5");

            if (AnioPublicacion.HasValue && (AnioPublicacion < 1000 || AnioPublicacion > DateTime.Now.Year + 1))
                errores.Add("El año de publicación no es válido");

            if (PrecioAdquisicion.HasValue && PrecioAdquisicion < 0)
                errores.Add("El precio de adquisición no puede ser negativo");

            return errores;
        }

        #endregion
    }
}