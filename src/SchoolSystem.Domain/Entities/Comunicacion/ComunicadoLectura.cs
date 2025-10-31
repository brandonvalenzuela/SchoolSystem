using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Comunicacion
{
    /// <summary>
    /// Registro de lectura y confirmación de comunicados
    /// </summary>
    [Table("ComunicadoLecturas")]
    public class ComunicadoLectura : BaseEntity
    {
        /// <summary>
        /// Comunicado relacionado
        /// </summary>
        [Required]
        public int ComunicadoId { get; set; }

        /// <summary>
        /// Usuario que leyó el comunicado
        /// </summary>
        [Required]
        public int UsuarioId { get; set; }

        /// <summary>
        /// Fecha de lectura
        /// </summary>
        [Required]
        public DateTime FechaLectura { get; set; }

        /// <summary>
        /// Indica si confirmó la lectura
        /// </summary>
        [Required]
        public bool Confirmado { get; set; }

        /// <summary>
        /// Fecha de confirmación
        /// </summary>
        public DateTime? FechaConfirmacion { get; set; }

        /// <summary>
        /// Comentario del usuario (si está habilitado)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Comentario { get; set; }

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Comunicado relacionado
        /// </summary>
        [ForeignKey("ComunicadoId")]
        public virtual Comunicado Comunicado { get; set; }

        /// <summary>
        /// Usuario que leyó
        /// </summary>
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        #endregion

        #region Constructor

        public ComunicadoLectura()
        {
            FechaLectura = DateTime.Now;
            Confirmado = false;
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Confirma la lectura del comunicado
        /// </summary>
        public void Confirmar()
        {
            if (!Confirmado)
            {
                Confirmado = true;
                FechaConfirmacion = DateTime.Now;
            }
        }

        /// <summary>
        /// Agrega un comentario
        /// </summary>
        public void AgregarComentario(string comentario)
        {
            Comentario = comentario;
        }

        #endregion
    }
}