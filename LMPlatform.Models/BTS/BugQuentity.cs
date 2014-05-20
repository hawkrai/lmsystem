using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;

namespace LMPlatform.Models.BTS
{
    public class BugQuentity : ModelBase
    {
        public string Name { get; set; }

        public int Quentity { get; set; }
    }
}
