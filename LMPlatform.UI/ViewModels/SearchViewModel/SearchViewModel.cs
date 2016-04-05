using System.Collections.Generic;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.SearchViewModel
{
    public class SearchViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Lecturer> Lecturers { get; set; }
    }
}