using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Dashboard
{
    public class ChartDto
    {
        public List<string> Labels { get; set; } = new();
        public List<ChartSeriesDto> Series { get; set; } = new();
    }
}
