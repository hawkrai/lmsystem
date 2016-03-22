using LMPlatform.Models;
using System;
using System.Collections.Generic;

namespace Application.Infrastructure.CTO
{
    public class PercentageGraphData
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }

        public double Percentage { get; set; }

        public DateTime Date { get; set; }

        public int SubjectId { get; set; }

        public IEnumerable<int> SelectedGroupsIds { get; set; }
    }
}
