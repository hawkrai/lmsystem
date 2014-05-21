namespace LMPlatform.UI.ViewModels.SubjectViewModels
{
    using System.Collections.Generic;

    using LMPlatform.Models;

    public class SubjectAttachmentsViewModel
    {
        public List<Attachment> Lectures { get; set; }

        public List<Attachment> Labs { get; set; }

        public List<Attachment> Practicals { get; set; }
    }
}