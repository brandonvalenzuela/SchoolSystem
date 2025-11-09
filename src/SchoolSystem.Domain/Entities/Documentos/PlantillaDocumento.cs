using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums.Documentos;

namespace SchoolSystem.Domain.Entities.Documentos
{
    /// <summary>
    /// Plantillas para generación de documentos
    /// </summary>
    [Table("PlantillasDocumento")]
    public class PlantillaDocumento : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Nombre de la plantilla
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        [Required]
        public TipoDocumento TipoDocumento { get; set; }

        /// <summary>
        /// Descripción de la plantilla
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Descripcion { get; set; }

        #endregion

        #region Contenido

        /// <summary>
        /// Contenido HTML de la plantilla
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ContenidoHtml { get; set; }

        /// <summary>
        /// CSS personalizado para la plantilla
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string EstilosCSS { get; set; }

        /// <summary>
        /// Variables disponibles en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string VariablesDisponibles { get; set; }

        #endregion

        #region Configuración de Página

        /// <summary>
        /// Tamaño de página
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TamanioPagina { get; set; }

        /// <summary>
        /// Orientación de la página
        /// </summary>
        [Required]
        public OrientacionPagina Orientacion { get; set; }

        /// <summary>
        /// Margen superior en mm
        /// </summary>
        public int MargenSuperior { get; set; }

        /// <summary>
        /// Margen inferior en mm
        /// </summary>
        public int MargenInferior { get; set; }

        /// <summary>
        /// Margen izquierdo en mm
        /// </summary>
        public int MargenIzquierdo { get; set; }

        /// <summary>
        /// Margen derecho en mm
        /// </summary>
        public int MargenDerecho { get; set; }

        #endregion

        #region Encabezado y Pie de Página

        /// <summary>
        /// Tiene encabezado
        /// </summary>
        public bool TieneEncabezado { get; set; }

        /// <summary>
        /// Contenido HTML del encabezado
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string EncabezadoHtml { get; set; }

        /// <summary>
        /// Altura del encabezado en mm
        /// </summary>
        public int? AlturaEncabezado { get; set; }

        /// <summary>
        /// Tiene pie de página
        /// </summary>
        public bool TienePiePagina { get; set; }

        /// <summary>
        /// Contenido HTML del pie de página
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string PiePaginaHtml { get; set; }

        /// <summary>
        /// Altura del pie de página en mm
        /// </summary>
        public int? AlturaPiePagina { get; set; }

        #endregion

        #region Marca de Agua

        /// <summary>
        /// Tiene marca de agua
        /// </summary>
        public bool TieneMarcaAgua { get; set; }

        /// <summary>
        /// Texto de la marca de agua
        /// </summary>
        [StringLength(100)]
        public string TextoMarcaAgua { get; set; }

        /// <summary>
        /// URL de imagen para marca de agua
        /// </summary>
        [StringLength(500)]
        public string ImagenMarcaAguaUrl { get; set; }

        /// <summary>
        /// Opacidad de la marca de agua (0-100)
        /// </summary>
        public int? OpacidadMarcaAgua { get; set; }

        #endregion

        #region Configuración

        /// <summary>
        /// Es plantilla por defecto para este tipo de documento
        /// </summary>
        public bool EsPlantillaPorDefecto { get; set; }

        /// <summary>
        /// Requiere firma digital
        /// </summary>
        public bool RequiereFirmaDigital { get; set; }

        /// <summary>
        /// Requiere folio consecutivo
        /// </summary>
        public bool RequiereFolio { get; set; }

        /// <summary>
        /// Prefijo del folio
        /// </summary>
        [StringLength(20)]
        public string PrefijoFolio { get; set; }

        /// <summary>
        /// Consecutivo actual del folio
        /// </summary>
        public int? ConsecutivoFolio { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Indica si está activa
        /// </summary>
        [Required]
        public bool Activa { get; set; }

        /// <summary>
        /// Versión de la plantilla
        /// </summary>
        [StringLength(20)]
        public string Version { get; set; }

        /// <summary>
        /// Notas de la versión
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string NotasVersion { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Categoría o etiqueta
        /// </summary>
        [StringLength(100)]
        public string Categoria { get; set; }

        /// <summary>
        /// Tags separados por comas
        /// </summary>
        [StringLength(500)]
        public string Tags { get; set; }

        /// <summary>
        /// Cantidad de veces que se ha usado
        /// </summary>
        public int VecesUsada { get; set; }

        /// <summary>
        /// Fecha de última generación
        /// </summary>
        public DateTime? FechaUltimaGeneracion { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Documentos generados con esta plantilla
        /// </summary>
        public virtual ICollection<Documento> Documentos { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public PlantillaDocumento()
        {
            TamanioPagina = "A4";
            Orientacion = OrientacionPagina.Vertical;
            MargenSuperior = 20;
            MargenInferior = 20;
            MargenIzquierdo = 20;
            MargenDerecho = 20;
            TieneEncabezado = false;
            TienePiePagina = false;
            TieneMarcaAgua = false;
            EsPlantillaPorDefecto = false;
            RequiereFirmaDigital = false;
            RequiereFolio = false;
            Activa = true;
            VecesUsada = 0;
            Version = "1.0";
            Documentos = new HashSet<Documento>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si es boleta de calificaciones
        /// </summary>
        public bool EsBoleta => TipoDocumento == TipoDocumento.Boleta;

        /// <summary>
        /// Indica si es constancia
        /// </summary>
        public bool EsConstancia => TipoDocumento == TipoDocumento.Constancia;

        /// <summary>
        /// Indica si es diploma
        /// </summary>
        public bool EsDiploma => TipoDocumento == TipoDocumento.Diploma;

        /// <summary>
        /// Indica si tiene contenido HTML
        /// </summary>
        public bool TieneContenido => !string.IsNullOrWhiteSpace(ContenidoHtml);

        /// <summary>
        /// Indica si ha sido usada
        /// </summary>
        public bool HaSidoUsada => VecesUsada > 0;

        /// <summary>
        /// Días desde la última generación
        /// </summary>
        public int? DiasSinUsar
        {
            get
            {
                if (!FechaUltimaGeneracion.HasValue)
                    return null;
                return (DateTime.Now.Date - FechaUltimaGeneracion.Value.Date).Days;
            }
        }

        /// <summary>
        /// Indica si es plantilla poco usada (menos de 5 usos)
        /// </summary>
        public bool EsPocoUsada => VecesUsada < 5;

        /// <summary>
        /// Nombre completo con versión
        /// </summary>
        public string NombreCompleto => string.IsNullOrWhiteSpace(Version) ? Nombre : $"{Nombre} v{Version}";

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Genera el siguiente folio
        /// </summary>
        public string GenerarSiguienteFolio()
        {
            if (!RequiereFolio)
                throw new InvalidOperationException("Esta plantilla no requiere folio");

            if (!ConsecutivoFolio.HasValue)
                ConsecutivoFolio = 1;
            else
                ConsecutivoFolio++;

            var prefijo = string.IsNullOrWhiteSpace(PrefijoFolio) ? "" : PrefijoFolio;
            return $"{prefijo}{ConsecutivoFolio:D6}";
        }

        /// <summary>
        /// Registra el uso de la plantilla
        /// </summary>
        public void RegistrarUso()
        {
            VecesUsada++;
            FechaUltimaGeneracion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca como plantilla por defecto
        /// </summary>
        public void MarcarComoPorDefecto()
        {
            EsPlantillaPorDefecto = true;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Desmarca como plantilla por defecto
        /// </summary>
        public void DesmarcarComoPorDefecto()
        {
            EsPlantillaPorDefecto = false;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Activa la plantilla
        /// </summary>
        public void Activar()
        {
            if (!Activa)
            {
                Activa = true;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Desactiva la plantilla
        /// </summary>
        public void Desactivar()
        {
            if (Activa)
            {
                Activa = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Configura el encabezado
        /// </summary>
        public void ConfigurarEncabezado(bool tieneEncabezado, string contenidoHtml = null, int? altura = null)
        {
            TieneEncabezado = tieneEncabezado;

            if (tieneEncabezado)
            {
                EncabezadoHtml = contenidoHtml;
                AlturaEncabezado = altura ?? 50;
            }
            else
            {
                EncabezadoHtml = null;
                AlturaEncabezado = null;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura el pie de página
        /// </summary>
        public void ConfigurarPiePagina(bool tienePie, string contenidoHtml = null, int? altura = null)
        {
            TienePiePagina = tienePie;

            if (tienePie)
            {
                PiePaginaHtml = contenidoHtml;
                AlturaPiePagina = altura ?? 30;
            }
            else
            {
                PiePaginaHtml = null;
                AlturaPiePagina = null;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura la marca de agua
        /// </summary>
        public void ConfigurarMarcaAgua(bool tieneMarca, string texto = null, string imagenUrl = null, int? opacidad = null)
        {
            TieneMarcaAgua = tieneMarca;

            if (tieneMarca)
            {
                TextoMarcaAgua = texto;
                ImagenMarcaAguaUrl = imagenUrl;
                OpacidadMarcaAgua = opacidad ?? 20;
            }
            else
            {
                TextoMarcaAgua = null;
                ImagenMarcaAguaUrl = null;
                OpacidadMarcaAgua = null;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura los márgenes de la página
        /// </summary>
        public void ConfigurarMargenes(int superior, int inferior, int izquierdo, int derecho)
        {
            if (superior < 0 || inferior < 0 || izquierdo < 0 || derecho < 0)
                throw new ArgumentException("Los márgenes no pueden ser negativos");

            MargenSuperior = superior;
            MargenInferior = inferior;
            MargenIzquierdo = izquierdo;
            MargenDerecho = derecho;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura el folio
        /// </summary>
        public void ConfigurarFolio(bool requiereFolio, string prefijo = null, int? consecutivoInicial = null)
        {
            RequiereFolio = requiereFolio;

            if (requiereFolio)
            {
                PrefijoFolio = prefijo;
                ConsecutivoFolio = consecutivoInicial ?? 1;
            }
            else
            {
                PrefijoFolio = null;
                ConsecutivoFolio = null;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza la versión de la plantilla
        /// </summary>
        public void ActualizarVersion(string nuevaVersion, string notas = null)
        {
            if (string.IsNullOrWhiteSpace(nuevaVersion))
                throw new ArgumentException("La versión es requerida");

            Version = nuevaVersion;
            NotasVersion = notas;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Duplica la plantilla
        /// </summary>
        public PlantillaDocumento Duplicar(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nuevo nombre es requerido");

            return new PlantillaDocumento
            {
                EscuelaId = this.EscuelaId,
                Nombre = nuevoNombre,
                TipoDocumento = this.TipoDocumento,
                Descripcion = this.Descripcion,
                ContenidoHtml = this.ContenidoHtml,
                EstilosCSS = this.EstilosCSS,
                VariablesDisponibles = this.VariablesDisponibles,
                TamanioPagina = this.TamanioPagina,
                Orientacion = this.Orientacion,
                MargenSuperior = this.MargenSuperior,
                MargenInferior = this.MargenInferior,
                MargenIzquierdo = this.MargenIzquierdo,
                MargenDerecho = this.MargenDerecho,
                TieneEncabezado = this.TieneEncabezado,
                EncabezadoHtml = this.EncabezadoHtml,
                AlturaEncabezado = this.AlturaEncabezado,
                TienePiePagina = this.TienePiePagina,
                PiePaginaHtml = this.PiePaginaHtml,
                AlturaPiePagina = this.AlturaPiePagina,
                TieneMarcaAgua = this.TieneMarcaAgua,
                TextoMarcaAgua = this.TextoMarcaAgua,
                ImagenMarcaAguaUrl = this.ImagenMarcaAguaUrl,
                OpacidadMarcaAgua = this.OpacidadMarcaAgua,
                RequiereFirmaDigital = this.RequiereFirmaDigital,
                RequiereFolio = this.RequiereFolio,
                PrefijoFolio = this.PrefijoFolio,
                Categoria = this.Categoria,
                Tags = this.Tags,
                Version = "1.0",
                Activa = true,
                EsPlantillaPorDefecto = false
            };
        }

        /// <summary>
        /// Valida que la plantilla sea correcta
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(ContenidoHtml))
                errores.Add("El contenido HTML es requerido");

            if (MargenSuperior < 0 || MargenInferior < 0 || MargenIzquierdo < 0 || MargenDerecho < 0)
                errores.Add("Los márgenes no pueden ser negativos");

            if (TieneEncabezado && string.IsNullOrWhiteSpace(EncabezadoHtml))
                errores.Add("Si tiene encabezado, debe proporcionar el contenido HTML");

            if (TienePiePagina && string.IsNullOrWhiteSpace(PiePaginaHtml))
                errores.Add("Si tiene pie de página, debe proporcionar el contenido HTML");

            if (TieneMarcaAgua)
            {
                if (string.IsNullOrWhiteSpace(TextoMarcaAgua) && string.IsNullOrWhiteSpace(ImagenMarcaAguaUrl))
                    errores.Add("Si tiene marca de agua, debe proporcionar texto o imagen");

                if (OpacidadMarcaAgua.HasValue && (OpacidadMarcaAgua < 0 || OpacidadMarcaAgua > 100))
                    errores.Add("La opacidad debe estar entre 0 y 100");
            }

            if (RequiereFolio && ConsecutivoFolio.HasValue && ConsecutivoFolio < 1)
                errores.Add("El consecutivo del folio debe ser mayor a cero");

            return errores;
        }

        #endregion
    }
}