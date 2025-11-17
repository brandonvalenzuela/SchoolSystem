using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Grados
{
    /// <summary>
    /// DTO para actualizar un grado existente.
    /// </summary>
    public class UpdateGradoDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nivel educativo es obligatorio.")]
        public int NivelEducativoId { get; set; }

        [Required(ErrorMessage = "El nombre del grado es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El orden es obligatorio.")]
        [Range(0, 100, ErrorMessage = "El orden debe ser un número positivo.")]
        public int Orden { get; set; }

        public bool Activo { get; set; }

        [Range(3, 18, ErrorMessage = "La edad recomendada debe ser realista.")]
        public int? EdadRecomendada { get; set; }

        [Range(1, 100)]
        public int? CapacidadMaximaPorGrupo { get; set; }

        [Range(1, 100)]
        public int? HorasSemanales { get; set; }

        public string Requisitos { get; set; }
    }
}
