using System;
using System.Collections.Generic;

namespace Application.Infrastructure.DTO
{
    public class PercentageGraphData
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }

        public double Percentage { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<int> SelectedGroupsIds { get; set; }
    }
}
