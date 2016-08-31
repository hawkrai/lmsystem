using Application.Core.Data;

namespace LMPlatform.Models
{
    public class SubjectLecturer : ModelBase
    {
        public int LecturerId
        {
            get;
            set;
        }

        public int SubjectId
        {
            get;
            set;
        }

		public int? Owner
		{
			get;
			set;
		}

        public Lecturer Lecturer
        {
            get;
            set;
        }

        public Subject Subject
        {
            get;
            set;
        }     
    }
}