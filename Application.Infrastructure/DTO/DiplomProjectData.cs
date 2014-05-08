using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Infrastructure.DTO
{
    public class DiplomProjectData
    {
        public int? Id { get; set; }

        [Required]
        public string Theme { get; set; }

        public string Lecturer { get; set; }

        public int? LecturerId { get; set; }

        public IEnumerable<int> SelectedGroupsIds { get; set; }
    }
}
