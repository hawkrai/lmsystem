using System;

namespace LMPlatform.Models.DP
{
    public class DiplomProjectTaskSheetTemplate
    {
        public int Id { get; set; }

        public int LecturerId { get; set; }

        public string Name { get; set; }

        public string InputData { get; set; }

        public string RpzContent { get; set; }

        public string DrawMaterials { get; set; }

        public string Consultants { get; set; }

        public string Faculty { get; set; }

        public string Univer { get; set; }

        public string HeadCathedra { get; set; }

        public DateTime? DateEnd { get; set; }

        public DateTime? DateStart { get; set; }
    }
}
