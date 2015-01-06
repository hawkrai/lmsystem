using System;
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

        public string Student { get; set; }

        public int? StudentId { get; set; }

        public string Group { get; set; }

        public DateTime? ApproveDate { get; set; }

        public string ApproveDateString
        {
            get
            {
                return ApproveDate.HasValue ? ApproveDate.Value.ToString("dd-MM-yyyy") : null;
            }
        }

        public int? LecturerId { get; set; }

        public IEnumerable<int> SelectedGroupsIds { get; set; }
    }
}
