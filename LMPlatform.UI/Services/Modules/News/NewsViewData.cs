namespace LMPlatform.UI.Services.Modules.News
{
    using System.Runtime.Serialization;

    using LMPlatform.Models;

    [DataContract]
    public class NewsViewData
    {
        public NewsViewData()
        {
        }

        public NewsViewData(SubjectNews news)
        {
            Body = news.Body;
            NewsId = news.Id;
            Title = news.Title;
            SubjectId = news.SubjectId;
            DateCreate = news.EditDate.ToShortDateString();
	        Disabled = news.Disabled;
        }

        [DataMember]
        public int NewsId { get; set; }

        [DataMember]
        public int SubjectId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

		[DataMember]
		public bool Disabled { get; set; }
    }
}