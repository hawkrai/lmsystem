using System.Collections.Generic;

namespace Application.Infrastructure.DTO
{
    public class DiplomProjectData
    {
        public int? Id { get; set; }

        public string Theme { get; set; }

        public string Lecturer { get; set; }

        public int? LecturerId { get; set; }

        public IEnumerable<int> SelectedGroupsIds { get; set; }
    }
}
