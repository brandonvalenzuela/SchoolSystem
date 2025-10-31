using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Documentos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Documentos
{
    /// <summary>
    /// Documentos generados en el sistema
    /// </summary>
    [Table("Documentos")]
    public class Documento : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Plantilla utilizada para generar el documento
        /// </summary>
        public int? PlantillaDocumentoId { get; set; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        [Required]
        public TipoDocumento TipoDocumento { get; set; }

        /// <summary>
        /// Título del documento
        /// </summary>
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }

        /// <summary>
        /// Folio o número de control
        /// </summary>
        [StringLength(50)]
        public string Folio { get; set; }

        #endregion

        #region Entidad Relacionada

        /// <summary>
        /// Tipo de entidad relacionada
        /// </summary>
        [StringLength(50)]
        public string TipoEntidad { get; set; }

        /// <summary>
        /// ID de la entidad relacionada (Alumno, Maestro, etc.)
        /// </summary>
        public int? EntidadRelacionadaId { get; set; }

        /// <summary>
        /// Nombre de la entidad relacionada
        /// </summary>
        [StringLength(300)]
        public string NombreEntidadRelacionada { get; set; }

        #endregion

        #region Contenido

        /// <summary>
        /// Contenido HTML generado
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ContenidoHtml { get; set; }

        /// <summary>
        /// URL del archivo PDF generado
        /// </summary>
        [StringLength(500)]
        public string ArchivoUrl { get; set; }

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        [StringLength(200)]
        public string NombreArchivo { get; set; }

        /// <summary>
        /// Tamaño del archivo en bytes
        /// </summary>
        public long? TamanioArchivo { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha de generación del documento
        /// </summary>
        [Required]
        public DateTime FechaGeneracion { get; set; }

        /// <summary>
        /// Fecha de vigencia del documento (si aplica)
        /// </summary>
        public DateTime? FechaVigencia { get; set; }

        /// <summary>
        /// Fecha de vencimiento del documento (si aplica)
        /// </summary>
        public DateTime? FechaVencimiento { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Estado del documento
        /// </summary>
        [Required]
        public EstadoDocumento Estado { get; set; }

        /// <summary>
        /// Fecha de envío (si fue enviado)
        /// </summary>
        public DateTime? FechaEnvio { get; set; }

        /// <summary>
        /// Correos a los que se envió
        /// </summary>
        [StringLength(500)]
        public string CorreosEnvio { get; set; }

        #endregion

        #region Firma Digital

        /// <summary>
        /// Tiene firma digital
        /// </summary>
        public bool TieneFirmaDigital { get; set; }

        /// <summary>
        /// Hash de la firma digital
        /// </summary>
        [StringLength(500)]
        public string HashFirma { get; set; }

        /// <summary>
        /// Fecha de firma
        /// </summary>
        public DateTime? FechaFirma { get; set; }

        /// <summary>
        /// Usuario que firmó
        /// </summary>
        public int? FirmadoPorId { get; set; }

        /// <summary>
        /// Certificado utilizado
        /// </summary>
        [StringLength(500)]
        public string CertificadoFirma { get; set; }

        #endregion

        #region Usuario Generador

        /// <summary>
        /// Usuario que generó el documento
        /// </summary>
        [Required]
        public int GeneradoPorId { get; set; }

        /// <summary>
        /// Indica si fue generado automáticamente
        /// </summary>
        public bool GeneradoAutomaticamente { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Descripción o notas del documento
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Tags o etiquetas
        /// </summary>
        [StringLength(500)]
        public string Tags { get; set; }

        /// <summary>
        /// Datos adicionales en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DatosAdicionales { get; set; }

        /// <summary>
        /// Cantidad de descargas
        /// </summary>
        public int CantidadDescargas { get; set; }

        /// <summary>
        /// Fecha de última descarga
        /// </summary>
        public DateTime? FechaUltimaDescarga { get; set; }

        #endregion

        #region Control

        /// <summary>
        /// Es público (visible sin autenticación)
        /// </summary>
        public bool EsPublico { get; set; }

        /// <summary>
        /// Indica si está archivado
        /// </summary>
        public bool Archivado { get; set; }

        /// <summary>
        /// Fecha de archivado
        /// </summary>
        public DateTime? FechaArchivado { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Plantilla utilizada
        /// </summary>
        [ForeignKey("PlantillaDocumentoId")]
        public virtual PlantillaDocumento PlantillaDocumento { get; set; }

        /// <summary>
        /// Usuario que generó
        /// </summary>
        [ForeignKey("GeneradoPorId")]
        public virtual Usuario GeneradoPor { get; set; }

        /// <summary>
        /// Usuario que firmó
        /// </summary>
        [ForeignKey("FirmadoPorId")]
        public virtual Usuario FirmadoPor { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public Documento()
        {
            FechaGeneracion = DateTime.Now;
            Estado = EstadoDocumento.Borrador;
            TieneFirmaDigital = false;
            GeneradoAutomaticamente = false;
            CantidadDescargas = 0;
            EsPublico = false;
            Archivado = false;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si es borrador
        /// </summary>
        public bool EsBorrador => Estado == EstadoDocumento.Borrador;

        /// <summary>
        /// Indica si está generado
        /// </summary>
        public bool EstaGenerado => Estado == EstadoDocumento.Generado;

        /// <summary>
        /// Indica si fue enviado
        /// </summary>
        public bool FueEnviado => Estado == EstadoDocumento.Enviado;

        /// <summary>
        /// Indica si está firmado
        /// </summary>
        public bool EstaFirmado => Estado == EstadoDocumento.Firmado || TieneFirmaDigital;

        /// <summary>
        /// Indica si tiene archivo PDF
        /// </summary>
        public bool TieneArchivo => !string.IsNullOrWhiteSpace(ArchivoUrl);

        /// <summary>
        /// Indica si es una boleta
        /// </summary>
        public bool EsBoleta => TipoDocumento == TipoDocumento.Boleta;

        /// <summary>
        /// Indica si es una constancia
        /// </summary>
        public bool EsConstancia => TipoDocumento == TipoDocumento.Constancia;

        /// <summary>
        /// Indica si está vigente
        /// </summary>
        public bool EstaVigente
        {
            get
            {
                if (!FechaVigencia.HasValue) return true;
                if (!FechaVencimiento.HasValue) return DateTime.Now >= FechaVigencia.Value;
                return DateTime.Now >= FechaVigencia.Value && DateTime.Now <= FechaVencimiento.Value;
            }
        }

        /// <summary>
        /// Indica si está vencido
        /// </summary>
        public bool EstaVencido => FechaVencimiento.HasValue && DateTime.Now > FechaVencimiento.Value;

        /// <summary>
        /// Días hasta el vencimiento
        /// </summary>
        public int? DiasHastaVencimiento
        {
            get
            {
                if (!FechaVencimiento.HasValue) return null;
                return (FechaVencimiento.Value.Date - DateTime.Now.Date).Days;
            }
        }

        /// <summary>
        /// Días desde la generación
        /// </summary>
        public int DiasDesdeGeneracion => (DateTime.Now.Date - FechaGeneracion.Date).Days;

        /// <summary>
        /// Indica si ha sido descargado
        /// </summary>
        public bool HaSidoDescargado => CantidadDescargas > 0;

        /// <summary>
        /// Tamaño del archivo en KB
        /// </summary>
        public decimal? TamanioArchivoKB
        {
            get
            {
                if (!TamanioArchivo.HasValue) return null;
                return Math.Round((decimal)TamanioArchivo.Value / 1024, 2);
            }
        }

        /// <summary>
        /// Tamaño del archivo en MB
        /// </summary>
        public decimal? TamanioArchivoMB
        {
            get
            {
                if (!TamanioArchivo.HasValue) return null;
                return Math.Round((decimal)TamanioArchivo.Value / (1024 * 1024), 2);
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Genera el documento (marca como generado)
        /// </summary>
        public void MarcarComoGenerado(string archivoUrl, string nombreArchivo, long tamanioArchivo)
        {
            if (!EsBorrador)
                throw new InvalidOperationException("Solo se pueden generar documentos en estado borrador");

            if (string.IsNullOrWhiteSpace(archivoUrl))
                throw new ArgumentException("La URL del archivo es requerida");

            Estado = EstadoDocumento.Generado;
            ArchivoUrl = archivoUrl;
            NombreArchivo = nombreArchivo;
            TamanioArchivo = tamanioArchivo;
            FechaGeneracion = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca el documento como enviado
        /// </summary>
        public void MarcarComoEnviado(string correos)
        {
            if (EsBorrador)
                throw new InvalidOperationException("No se puede enviar un documento en borrador");

            Estado = EstadoDocumento.Enviado;
            FechaEnvio = DateTime.Now;
            CorreosEnvio = correos;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Firma digitalmente el documento
        /// </summary>
        public void FirmarDigitalmente(int usuarioId, string hash, string certificado = null)
        {
            if (EsBorrador)
                throw new InvalidOperationException("No se puede firmar un documento en borrador");

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("El hash de la firma es requerido");

            TieneFirmaDigital = true;
            HashFirma = hash;
            FechaFirma = DateTime.Now;
            FirmadoPorId = usuarioId;
            CertificadoFirma = certificado;
            Estado = EstadoDocumento.Firmado;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Registra una descarga del documento
        /// </summary>
        public void RegistrarDescarga()
        {
            CantidadDescargas++;
            FechaUltimaDescarga = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Archiva el documento
        /// </summary>
        public void Archivar(string observaciones = null)
        {
            if (Archivado)
                throw new InvalidOperationException("El documento ya está archivado");

            Archivado = true;
            FechaArchivado = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(observaciones))
            {
                Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                    ? observaciones
                    : $"{Observaciones}\n[ARCHIVADO] {observaciones}";
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Desarchivar el documento
        /// </summary>
        public void Desarchivar(string motivo = null)
        {
            if (!Archivado)
                throw new InvalidOperationException("El documento no está archivado");

            Archivado = false;
            FechaArchivado = null;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                    ? $"[DESARCHIVADO] {motivo}"
                    : $"{Observaciones}\n[DESARCHIVADO] {motivo}";
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece las fechas de vigencia
        /// </summary>
        public void EstablecerVigencia(DateTime fechaVigencia, DateTime? fechaVencimiento = null)
        {
            if (fechaVencimiento.HasValue && fechaVencimiento.Value < fechaVigencia)
                throw new ArgumentException("La fecha de vencimiento no puede ser anterior a la fecha de vigencia");

            FechaVigencia = fechaVigencia;
            FechaVencimiento = fechaVencimiento;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece la entidad relacionada
        /// </summary>
        public void EstablecerEntidadRelacionada(string tipoEntidad, int entidadId, string nombreEntidad)
        {
            TipoEntidad = tipoEntidad;
            EntidadRelacionadaId = entidadId;
            NombreEntidadRelacionada = nombreEntidad;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Hace el documento público
        /// </summary>
        public void HacerPublico()
        {
            EsPublico = true;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Hace el documento privado
        /// </summary>
        public void HacerPrivado()
        {
            EsPublico = false;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Regenera el documento (vuelve a borrador)
        /// </summary>
        public void RegenerarDocumento()
        {
            if (EstaFirmado && TieneFirmaDigital)
                throw new InvalidOperationException("No se puede regenerar un documento firmado digitalmente");

            Estado = EstadoDocumento.Borrador;
            ArchivoUrl = null;
            TamanioArchivo = null;
            FechaEnvio = null;
            CorreosEnvio = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Agrega observaciones
        /// </summary>
        public void AgregarObservaciones(string observaciones)
        {
            if (string.IsNullOrWhiteSpace(observaciones))
                return;

            Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                ? $"[{DateTime.Now:dd/MM/yyyy HH:mm}] {observaciones}"
                : $"{Observaciones}\n[{DateTime.Now:dd/MM/yyyy HH:mm}] {observaciones}";

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida que el documento sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Titulo))
                errores.Add("El título es requerido");

            if (!EsBorrador && string.IsNullOrWhiteSpace(ArchivoUrl))
                errores.Add("Los documentos generados deben tener un archivo asociado");

            if (FechaVencimiento.HasValue && FechaVigencia.HasValue && FechaVencimiento.Value < FechaVigencia.Value)
                errores.Add("La fecha de vencimiento no puede ser anterior a la fecha de vigencia");

            if (TieneFirmaDigital)
            {
                if (string.IsNullOrWhiteSpace(HashFirma))
                    errores.Add("Si tiene firma digital, debe tener hash de firma");

                if (!FirmadoPorId.HasValue)
                    errores.Add("Si tiene firma digital, debe tener usuario que firmó");

                if (!FechaFirma.HasValue)
                    errores.Add("Si tiene firma digital, debe tener fecha de firma");
            }

            if (FueEnviado)
            {
                if (!FechaEnvio.HasValue)
                    errores.Add("Si fue enviado, debe tener fecha de envío");

                if (string.IsNullOrWhiteSpace(CorreosEnvio))
                    errores.Add("Si fue enviado, debe tener correos de envío");
            }

            if (Archivado && !FechaArchivado.HasValue)
                errores.Add("Si está archivado, debe tener fecha de archivado");

            if (TamanioArchivo.HasValue && TamanioArchivo.Value < 0)
                errores.Add("El tamaño del archivo no puede ser negativo");

            return errores;
        }

        #endregion
    }
}