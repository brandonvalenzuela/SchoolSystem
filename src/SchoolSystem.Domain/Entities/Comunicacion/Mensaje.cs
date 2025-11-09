using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Comunicacion
{
    /// <summary>
    /// Mensajes directos entre usuarios del sistema (chat)
    /// </summary>
    [Table("Mensajes")]
    public class Mensaje : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Usuario emisor del mensaje
        /// </summary>
        [Required]
        public int EmisorId { get; set; }

        /// <summary>
        /// Usuario receptor del mensaje
        /// </summary>
        [Required]
        public int ReceptorId { get; set; }

        /// <summary>
        /// Alumno relacionado (contexto del mensaje)
        /// </summary>
        public int? AlumnoRelacionadoId { get; set; }

        /// <summary>
        /// Asunto del mensaje
        /// </summary>
        [StringLength(200)]
        public string Asunto { get; set; }

        /// <summary>
        /// Contenido del mensaje
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Contenido { get; set; }

        #endregion

        #region Fechas y Estado

        /// <summary>
        /// Fecha de envío del mensaje
        /// </summary>
        [Required]
        public DateTime FechaEnvio { get; set; }

        /// <summary>
        /// Indica si el mensaje fue leído
        /// </summary>
        [Required]
        public bool Leido { get; set; }

        /// <summary>
        /// Fecha en que se leyó el mensaje
        /// </summary>
        public DateTime? FechaLectura { get; set; }

        #endregion

        #region Archivos Adjuntos

        /// <summary>
        /// URL del archivo adjunto (opcional)
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

        #region Control de Mensajes

        /// <summary>
        /// Mensaje padre (si es respuesta)
        /// </summary>
        public int? MensajePadreId { get; set; }

        /// <summary>
        /// Indica si el mensaje fue eliminado por el emisor
        /// </summary>
        public bool EliminadoPorEmisor { get; set; }

        /// <summary>
        /// Indica si el mensaje fue eliminado por el receptor
        /// </summary>
        public bool EliminadoPorReceptor { get; set; }

        /// <summary>
        /// Indica si es un mensaje importante/marcado
        /// </summary>
        public bool Importante { get; set; }

        /// <summary>
        /// Indica si el mensaje está archivado
        /// </summary>
        public bool Archivado { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Usuario emisor
        /// </summary>
        [ForeignKey("EmisorId")]
        public virtual Usuario Emisor { get; set; }

        /// <summary>
        /// Usuario receptor
        /// </summary>
        [ForeignKey("ReceptorId")]
        public virtual Usuario Receptor { get; set; }

        /// <summary>
        /// Alumno relacionado
        /// </summary>
        [ForeignKey("AlumnoRelacionadoId")]
        public virtual Alumno? AlumnoRelacionado { get; set; }

        /// <summary>
        /// Mensaje padre (si es respuesta)
        /// </summary>
        [ForeignKey("MensajePadreId")]
        public virtual Mensaje MensajePadre { get; set; }

        /// <summary>
        /// Respuestas a este mensaje
        /// </summary>
        [InverseProperty("MensajePadre")]
        public virtual ICollection<Mensaje> Respuestas { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public Mensaje()
        {
            FechaEnvio = DateTime.Now;
            Leido = false;
            EliminadoPorEmisor = false;
            EliminadoPorReceptor = false;
            Importante = false;
            Archivado = false;
            Respuestas = new HashSet<Mensaje>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si el mensaje está pendiente de leer
        /// </summary>
        public bool PendienteLectura => !Leido;

        /// <summary>
        /// Indica si es una respuesta a otro mensaje
        /// </summary>
        public bool EsRespuesta => MensajePadreId.HasValue;

        /// <summary>
        /// Indica si tiene archivo adjunto
        /// </summary>
        public bool TieneArchivo => !string.IsNullOrWhiteSpace(ArchivoAdjuntoUrl);

        /// <summary>
        /// Indica si el mensaje está activo (no eliminado por ninguno)
        /// </summary>
        public bool EstaActivo => !EliminadoPorEmisor && !EliminadoPorReceptor;

        /// <summary>
        /// Tiempo transcurrido desde el envío
        /// </summary>
        public TimeSpan TiempoDesdeEnvio => DateTime.Now - FechaEnvio;

        /// <summary>
        /// Minutos desde el envío
        /// </summary>
        public int MinutosDesdeEnvio => (int)TiempoDesdeEnvio.TotalMinutes;

        /// <summary>
        /// Horas desde el envío
        /// </summary>
        public int HorasDesdeEnvio => (int)TiempoDesdeEnvio.TotalHours;

        /// <summary>
        /// Días desde el envío
        /// </summary>
        public int DiasDesdeEnvio => (int)TiempoDesdeEnvio.TotalDays;

        /// <summary>
        /// Total de respuestas
        /// </summary>
        public int TotalRespuestas => Respuestas?.Count ?? 0;

        /// <summary>
        /// Descripción del tiempo transcurrido
        /// </summary>
        public string TiempoTranscurridoTexto
        {
            get
            {
                var minutos = MinutosDesdeEnvio;
                if (minutos < 1) return "Hace un momento";
                if (minutos < 60) return $"Hace {minutos} minuto{(minutos > 1 ? "s" : "")}";

                var horas = HorasDesdeEnvio;
                if (horas < 24) return $"Hace {horas} hora{(horas > 1 ? "s" : "")}";

                var dias = DiasDesdeEnvio;
                if (dias < 7) return $"Hace {dias} día{(dias > 1 ? "s" : "")}";

                var semanas = dias / 7;
                if (semanas < 4) return $"Hace {semanas} semana{(semanas > 1 ? "s" : "")}";

                var meses = dias / 30;
                return $"Hace {meses} mes{(meses > 1 ? "es" : "")}";
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Marca el mensaje como leído
        /// </summary>
        public void MarcarComoLeido()
        {
            if (!Leido)
            {
                Leido = true;
                FechaLectura = DateTime.Now;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Marca el mensaje como no leído
        /// </summary>
        public void MarcarComoNoLeido()
        {
            if (Leido)
            {
                Leido = false;
                FechaLectura = null;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Elimina el mensaje para un usuario específico
        /// </summary>
        /// <param name="usuarioId">ID del usuario que elimina</param>
        public void EliminarPara(int usuarioId)
        {
            if (usuarioId == EmisorId)
            {
                EliminadoPorEmisor = true;
            }
            else if (usuarioId == ReceptorId)
            {
                EliminadoPorReceptor = true;
            }
            else
            {
                throw new InvalidOperationException("El usuario no es emisor ni receptor de este mensaje");
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Restaura el mensaje para un usuario específico
        /// </summary>
        /// <param name="usuarioId">ID del usuario que restaura</param>
        public void RestaurarPara(int usuarioId)
        {
            if (usuarioId == EmisorId)
            {
                EliminadoPorEmisor = false;
            }
            else if (usuarioId == ReceptorId)
            {
                EliminadoPorReceptor = false;
            }
            else
            {
                throw new InvalidOperationException("El usuario no es emisor ni receptor de este mensaje");
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca o desmarca el mensaje como importante
        /// </summary>
        public void AlternarImportante()
        {
            Importante = !Importante;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca el mensaje como importante
        /// </summary>
        public void MarcarComoImportante()
        {
            if (!Importante)
            {
                Importante = true;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Desmarca el mensaje como importante
        /// </summary>
        public void DesmarcarComoImportante()
        {
            if (Importante)
            {
                Importante = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Archiva el mensaje
        /// </summary>
        public void Archivar()
        {
            if (!Archivado)
            {
                Archivado = true;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Desarchivar el mensaje
        /// </summary>
        public void Desarchivar()
        {
            if (Archivado)
            {
                Archivado = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Adjunta un archivo al mensaje
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
        /// Crea una respuesta a este mensaje
        /// </summary>
        /// <param name="contenido">Contenido de la respuesta</param>
        /// <param name="emisorId">ID del usuario que responde</param>
        /// <returns>Nuevo mensaje de respuesta</returns>
        public Mensaje CrearRespuesta(string contenido, int emisorId)
        {
            if (string.IsNullOrWhiteSpace(contenido))
                throw new ArgumentException("El contenido de la respuesta no puede estar vacío");

            var respuesta = new Mensaje
            {
                EscuelaId = this.EscuelaId,
                EmisorId = emisorId,
                ReceptorId = emisorId == this.EmisorId ? this.ReceptorId : this.EmisorId,
                AlumnoRelacionadoId = this.AlumnoRelacionadoId,
                Asunto = this.Asunto?.StartsWith("Re:") == true ? this.Asunto : $"Re: {this.Asunto}",
                Contenido = contenido,
                MensajePadreId = this.MensajePadreId
            };

            return respuesta;
        }

        /// <summary>
        /// Verifica si un usuario puede ver este mensaje
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <returns>True si puede ver el mensaje</returns>
        public bool PuedeVerMensaje(int usuarioId)
        {
            if (usuarioId == EmisorId && !EliminadoPorEmisor) return true;
            if (usuarioId == ReceptorId && !EliminadoPorReceptor) return true;
            return false;
        }

        /// <summary>
        /// Valida que el mensaje sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Contenido))
                errores.Add("El contenido del mensaje es requerido");

            if (EmisorId == ReceptorId)
                errores.Add("El emisor y el receptor no pueden ser el mismo usuario");

            if (EmisorId <= 0)
                errores.Add("El emisor es requerido");

            if (ReceptorId <= 0)
                errores.Add("El receptor es requerido");

            if (Asunto?.Length > 200)
                errores.Add("El asunto no puede exceder 200 caracteres");

            return errores;
        }

        #endregion
    }
}