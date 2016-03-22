namespace LMPlatform.Models.CP
{
    public class CoursePercentagesGraphToGroup
    {
        public int CoursePercentagesGraphToGroupId { get; set; }

        public int CoursePercentagesGraphId { get; set; }

        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public virtual CoursePercentagesGraph CoursePercentagesGraph { get; set; }
    }
}
