namespace SchoolSystem.Web.Models
{
    public class MateriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public string Area { get; set; }
        public string Tipo { get; set; }
        public bool Activo { get; set; }
    }
}
