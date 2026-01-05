namespace SchoolSystem.Web.Models
{
    public class AlergiaDto
    {
        public int Id { get; set; }
        public string NombreAlergeno { get; set; }
        public string Sintomas { get; set; }
        public string TratamientoRecomendado { get; set; }
        public bool EsGrave { get; set; }
    }
}
