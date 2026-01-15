namespace SchoolSystem.Web.Models
{
    public class ChartDto
    {
        public List<string> Labels { get; set; } = new();
        public List<ChartSeriesDto> Series { get; set; } = new();
    }
}
