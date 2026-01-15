using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Dashboard
{
    public class ChartSeriesDto
    {
        public string Name { get; set; }
        public double[] Data { get; set; }
    }
}
