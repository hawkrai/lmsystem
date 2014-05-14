namespace LMPlatform.UI.Services.Modules.Labs
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Application.Core;
    using Application.Infrastructure.FilesManagement;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;

    [DataContract]
    public class LabsViewData
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get
            {
                return filesManagementService.Value;
            }
        }

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public LabsViewData()
        {
        }

        public LabsViewData(Labs labs)
        {
            Theme = labs.Theme;
            LabId = labs.Id;
            Duration = labs.Duration;
            SubjectId = labs.SubjectId;
            Order = labs.Order;
            PathFile = labs.Attachments;
            ShortName = labs.ShortName;
            Attachments = FilesManagementService.GetAttachments(labs.Attachments);
        }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public int LabId { get; set; }

        [DataMember]
        public int SubjectId { get; set; }

        [DataMember]
        public string Theme { get; set; }

        [DataMember]
        public int Duration { get; set; }

        [DataMember]
        public string PathFile { get; set; }

        [DataMember]
        public IList<Attachment> Attachments
        {
            get;
            set;
        }

        [DataMember]
        public List<ScheduleProtectionLab> ScheduleProtectionLabsRecomend { get; set; }
    }
}