using Application.Core.Data;

namespace LMPlatform.Models
{
    public class StudentPracticalMark : ModelBase
    {
        public int PracticalId { get; set; }

        public int StudentId { get; set; }

        public string Mark { get; set; }

        public Practical Practical { get; set; }

        public Student Student { get; set; }     
    }
}