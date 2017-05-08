using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectCommentViewData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string Time { get; set; }

        public ProjectCommentViewData(ProjectComment comment)
        {
            Id = comment.Id;
            UserName = comment.User.FullName;
            Text = comment.CommentText;
            Time = comment.CommentingDate.ToString("HH:mm dd.MM.yyyy");
        }
    }
}