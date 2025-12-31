using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateMateriaDto
    {
        public int EscuelaId { get; set; } = 1;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        public string Clave { get; set; }

        public int Area { get; set; } = 1; // 1 = Ciencias Exactas (por defecto)

        public int Tipo { get; set; } = 1; // 1 = Teórica (por defecto)

        // --- PROPIEDADES QUE FALTABAN ---

        public string Color { get; set; } = "#FFFFFF"; // Valor por defecto para el input color

        public string Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public bool RequiereMateriales { get; set; }

        public string MaterialesRequeridos { get; set; }

        public bool RequiereInstalacionesEspeciales { get; set; }

        public string InstalacionesRequeridas { get; set; } // Por si acaso la usas
    }
}
