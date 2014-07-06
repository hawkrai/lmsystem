namespace LMPlatform.Models.DP
{
    public class DiplomPercentagesGraphToGroup
    {
        public int DiplomPercentagesGraphToGroupId { get; set; }

        public int DiplomPercentagesGraphId { get; set; }

        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public virtual DiplomPercentagesGraph DiplomPercentagesGraph { get; set; }
    }
}
