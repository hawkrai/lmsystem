using System.Collections.Generic;
using System.Runtime.Serialization;
using Application.Core;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.SubjectManagement;

namespace LMPlatform.UI.Services.Modules.Lectures
{
    using Models;

    [DataContract]
    public class LecturesViewData
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService => filesManagementService.Value;

        public ISubjectManagementService SubjectManagementService => subjectManagementService.Value;

        public LecturesViewData(Lectures lectures)
        {
            Theme = lectures.Theme;
            LecturesId = lectures.Id;
            Duration = lectures.Duration;
            SubjectId = lectures.SubjectId;
            Order = lectures.Order;
            PathFile = lectures.Attachments;
            Attachments = FilesManagementService.GetAttachments(lectures.Attachments);
        }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public int LecturesId { get; set; }

        [DataMember]
        public int SubjectId { get; set; }

        [DataMember]
        public string Theme { get; set; }

        [DataMember]
        public int Duration { get; set; }

        [DataMember]
        public string PathFile { get; set; }

        [DataMember]
        public IList<Attachment> Attachments { get; set; } 
    }
}
