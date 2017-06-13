using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
    using System.Linq;

    using Application.Core.UI.HtmlHelpers;

    public class ListSubjectViewModel : BaseNumberedGridItem
    {

        public List<LMPlatform.Models.Subject> Subjects { get; set; }

        public string Name { get; set; }

        public static ListSubjectViewModel FormSubjects(List<LMPlatform.Models.Subject> subjects, string name)
        {
            return new ListSubjectViewModel
            {
                Subjects = subjects,
                Name = name         
            };
        }
    }
}