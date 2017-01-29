using Application.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMPlatform.Models
{
    public class WatchingTime: ModelBase
    {
        public WatchingTime()
        {

        }

        //public WatchingTime(int userId, Concept concept, int time)
        public WatchingTime(int userId, int conceptId, int time)
        {
            UserId = userId;
            ConceptId = conceptId;
            //Concept = concept;
            Time = time;
        }

        public int UserId{ get; set; }
        
        //public virtual Concept Concept { get; set; }

        public int ConceptId { get; set; }

        public int Time { get; set; }
    }
}
