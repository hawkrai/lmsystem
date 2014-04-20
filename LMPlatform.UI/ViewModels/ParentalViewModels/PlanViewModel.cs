using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMPlatform.UI.ViewModels.ParentalViewModels
{
    using LMPlatform.Models;

    public class PlanViewModel : ParentalViewModel
    {
        public PlanViewModel(Group group, int subjectId)
            : base(group)
        {
            PlanSubject = Subjects.First(s => s.Id == subjectId);
        }

        public Subject PlanSubject { get; set; }
    }
}