namespace LMPlatform.Models.DP
{
    public class DiplomProjectGroup
    {
        public int DiplomProjectGroupId { get; set; }

        public int DiplomProjectId { get; set; }

        public int GroupId { get; set; }

        public virtual DiplomProject DiplomProject { get; set; }
        
        public virtual Group Group { get; set; }
    }
}
