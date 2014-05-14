namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Application.Core;
    using Application.Infrastructure.FilesManagement;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;

    public class ScheduleProtectionLabsDataViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IFilesManagementService> _filesManagementService = new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get
            {
                return _filesManagementService.Value;
            }
        }

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public ScheduleProtectionLabsDataViewModel()
        {
        }

        public ScheduleProtectionLabsDataViewModel(Labs lab, int subGroupId)
        {
            SuGroupId = subGroupId;
            LabId = lab.Id;
            Order = lab.Order;
            Name = lab.Theme;
        }

        public ScheduleProtectionLabsDataViewModel(int id, int subGroupId)
        {
        }

        public string Name { get; set; }

        public int Order { get; set; }

        public List<ScheduleProtectionLabs> ScheduleProtection { get; set; }

        public int SuGroupId { get; set; }

        public int LabId { get; set; }
    }
}