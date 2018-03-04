using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.CTO
{
    public class SubjectData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public bool IsNeededCopyToBts { get; set; }

        public IEnumerable<Group> Groups { get; set; }

    }
}
