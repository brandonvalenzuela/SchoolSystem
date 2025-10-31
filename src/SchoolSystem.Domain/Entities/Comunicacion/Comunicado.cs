using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Comunicacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Comunicacion
{
    /// <summary>
    /// Comunicados generales publicados por la escuela
    /// </summary>
    [Table("Comunicados")]
    public class Comunicado : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Título del comunicado
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        /// <summary>
        /// Contenido del comunicado
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Contenido { get; set; }

        /// <summary>
        /// Destinatarios del comunicado
        /// </summary>
        [Required]
        public DestinatarioComunicado Destinatarios { get; set; }

        /// <summary>
        /// Grupo específico (si aplica)
        /// </summary>
        public int? GrupoId { get; set; }

        #endregion

        #region Archivos Adjuntos

        /// <summary>
        /// URL del archivo adjunto
        /// </summary>
        [StringLength(500)]
        public string ArchivoAdjuntoUrl { get; set; }

        /// <summary>
        /// Nombre del archivo adjunto
        /// </summary>
        [StringLength(200)]
        public string ArchivoAdjuntoNombre { get; set; }

        /// <summary>
        /// Tamaño del archivo en bytes
        /// </summary>
        public long? ArchivoAdjuntoTamano { get; set; }

        /// <summary>
        /// Tipo MIME del archivo
        /// </summary>
        [StringLength(100)]
        public string ArchivoAdjuntoTipo { get; set; }

        #endregion

        #region Publicación

        /// <summary>
        /// Usuario que publicó el comunicado
        /// </summary>
        [Required]
        public int PublicadoPorId { get; set; }

        /// <summary>
        /// Fecha de publicación
        /// </summary>
        [Required]
        public DateTime FechaPublicacion { get; set; }

        /// <summary>
        /// Fecha de expiración (opcional)
        /// </summary>
        public DateTime? FechaExpiracion { get; set; }

        #endregion

        #region Configuración

        /// <summary>
        /// Requiere confirmación de lectura
        /// </summary>
        [Required]
        public bool RequiereConfirmacion { get; set; }

        /// <summary>
        /// Estado del comunicado
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        /// <summary>
        /// Prioridad del comunicado
        /// </summary>
        public PrioridadNotificacion Prioridad { get; set; }

        /// <summary>
        /// Categoría del comunicado
        /// </summary>
        [StringLength(50)]
        public string Categoria { get; set; }

        /// <summary>
        /// Permite comentarios
        /// </summary>
        public bool PermiteComentarios { get; set; }

        #endregion

        #region Estadísticas

        /// <summary>
        /// Total de destinatarios
        /// </summary>
        public int TotalDestinatarios { get; set; }

        /// <summary>
        /// Total de lecturas
        /// </summary>
        public int TotalLecturas { get; set; }

        /// <summary>
        /// Total de confirmaciones
        /// </summary>
        public int TotalConfirmaciones { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Grupo relacionado (si aplica)
        /// </summary>
        [ForeignKey("GrupoId")]
        public virtual Grupo Grupo { get; set; }

        /// <summary>
        /// Usuario que publicó
        /// </summary>
        [ForeignKey("PublicadoPorId")]
        public virtual Usuario PublicadoPor { get; set; }

        /// <summary>
        /// Lecturas del comunicado
        /// </summary>
        public virtual ICollection<ComunicadoLectura> Lecturas { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public Comunicado()
        {
            FechaPublicacion = DateTime.Now;
            Activo = true;
            RequiereConfirmacion = false;
            Prioridad = PrioridadNotificacion.Normal;
            PermiteComentarios = false;
            TotalDestinatarios = 0;
            TotalLecturas = 0;
            TotalConfirmaciones = 0;
            Lecturas = new HashSet<ComunicadoLectura>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si el comunicado está expirado
        /// </summary>
        public bool EstaExpirado => FechaExpiracion.HasValue && DateTime.Now > FechaExpiracion.Value;

        /// <summary>
        /// Indica si es para todos
        /// </summary>
        public bool EsParaTodos => Destinatarios == DestinatarioComunicado.Todos;

        /// <summary>
        /// Indica si es para un grupo específico
        /// </summary>
        public bool EsParaGrupoEspecifico => Destinatarios == DestinatarioComunicado.GrupoEspecifico && GrupoId.HasValue;

        /// <summary>
        /// Porcentaje de lecturas
        /// </summary>
        public decimal PorcentajeLecturas
        {
            get
            {
                if (TotalDestinatarios == 0) return 0;
                return (decimal)TotalLecturas / TotalDestinatarios * 100;
            }
        }

        /// <summary>
        /// Porcentaje de confirmaciones
        /// </summary>
        public decimal PorcentajeConfirmaciones
        {
            get
            {
                if (!RequiereConfirmacion || TotalDestinatarios == 0) return 0;
                return (decimal)TotalConfirmaciones / TotalDestinatarios * 100;
            }
        }

        /// <summary>
        /// Indica si tiene archivo adjunto
        /// </summary>
        public bool TieneArchivo => !string.IsNullOrWhiteSpace(ArchivoAdjuntoUrl);

        /// <summary>
        /// Tiempo desde la publicación
        /// </summary>
        public TimeSpan TiempoDesdePublicacion => DateTime.Now - FechaPublicacion;

        /// <summary>
        /// Días desde la publicación
        /// </summary>
        public int DiasDesdePublicacion => (int)TiempoDesdePublicacion.TotalDays;

        /// <summary>
        /// Indica si es urgente
        /// </summary>
        public bool EsUrgente => Prioridad == PrioridadNotificacion.Urgente;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Publica el comunicado
        /// </summary>
        public void Publicar()
        {
            if (!Activo)
            {
                Activo = true;
                FechaPublicacion = DateTime.Now;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Despublica el comunicado
        /// </summary>
        public void Despublicar()
        {
            if (Activo)
            {
                Activo = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Establece fecha de expiración
        /// </summary>
        public void EstablecerExpiracion(DateTime fechaExpiracion)
        {
            if (fechaExpiracion <= FechaPublicacion)
                throw new InvalidOperationException("La fecha de expiración debe ser posterior a la fecha de publicación");

            FechaExpiracion = fechaExpiracion;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Remueve la fecha de expiración
        /// </summary>
        public void RemoverExpiracion()
        {
            FechaExpiracion = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Adjunta un archivo al comunicado
        /// </summary>
        public void AdjuntarArchivo(string url, string nombreArchivo, long tamano, string tipoMime)
        {
            ArchivoAdjuntoUrl = url;
            ArchivoAdjuntoNombre = nombreArchivo;
            ArchivoAdjuntoTamano = tamano;
            ArchivoAdjuntoTipo = tipoMime;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Remueve el archivo adjunto
        /// </summary>
        public void RemoverArchivo()
        {
            ArchivoAdjuntoUrl = null;
            ArchivoAdjuntoNombre = null;
            ArchivoAdjuntoTamano = null;
            ArchivoAdjuntoTipo = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza las estadísticas del comunicado
        /// </summary>
        public void ActualizarEstadisticas(int totalDestinatarios, int totalLecturas, int totalConfirmaciones)
        {
            TotalDestinatarios = totalDestinatarios;
            TotalLecturas = totalLecturas;
            TotalConfirmaciones = totalConfirmaciones;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Incrementa el contador de lecturas
        /// </summary>
        public void IncrementarLecturas()
        {
            TotalLecturas++;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Incrementa el contador de confirmaciones
        /// </summary>
        public void IncrementarConfirmaciones()
        {
            TotalConfirmaciones++;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Cambia los destinatarios del comunicado
        /// </summary>
        public void CambiarDestinatarios(DestinatarioComunicado destinatarios, int? grupoId = null)
        {
            if (destinatarios == DestinatarioComunicado.GrupoEspecifico && !grupoId.HasValue)
                throw new InvalidOperationException("Debe especificar un grupo cuando los destinatarios son 'GrupoEspecifico'");

            Destinatarios = destinatarios;
            GrupoId = destinatarios == DestinatarioComunicado.GrupoEspecifico ? grupoId : null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Habilita/deshabilita confirmación de lectura
        /// </summary>
        public void ConfigurarConfirmacion(bool requiereConfirmacion)
        {
            RequiereConfirmacion = requiereConfirmacion;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida que el comunicado sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Titulo))
                errores.Add("El título es requerido");

            if (string.IsNullOrWhiteSpace(Contenido))
                errores.Add("El contenido es requerido");

            if (Titulo?.Length > 200)
                errores.Add("El título no puede exceder 200 caracteres");

            if (FechaExpiracion.HasValue && FechaExpiracion.Value <= FechaPublicacion)
                errores.Add("La fecha de expiración debe ser posterior a la fecha de publicación");

            if (Destinatarios == DestinatarioComunicado.GrupoEspecifico && !GrupoId.HasValue)
                errores.Add("Debe especificar un grupo cuando los destinatarios son 'GrupoEspecifico'");

            return errores;
        }

        #endregion
    }
}