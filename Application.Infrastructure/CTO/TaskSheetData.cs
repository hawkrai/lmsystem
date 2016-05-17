using System;

namespace Application.Infrastructure.CTO
{
    public class TaskSheetData
    {
        public int CourseProjectId { get; set; }

        public string InputData { get; set; }

        public string Faculty { get; set; }

        public string HeadCathedra { get; set; }

        public string RpzContent { get; set; }

        public string DrawMaterials { get; set; }

        public string Univer { get; set; }

        public DateTime? DateEnd { get; set; }

        public string DateEndString
        {
            get
            {
                return DateEnd.HasValue ? DateEnd.Value.ToString("dd-MM-yyyy") : null;
            }
        }


        public DateTime? DateStart { get; set; }

        public string DateStartString
        {
            get
            {
                return DateStart.HasValue ? DateStart.Value.ToString("dd-MM-yyyy") : null;
            }
        }

        public string Consultants { get; set; }
    }
}
