using System;

namespace Application.Infrastructure.DTO
{
    public class PercentageGraphData
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public double Percentage { get; set; }

        public DateTime Date { get; set; }
    }
}
