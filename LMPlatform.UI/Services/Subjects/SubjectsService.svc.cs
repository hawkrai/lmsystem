using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.UI.Services.Modules.BTS;
using WebMatrix.WebData;
using System.Web.Http;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Parental;

namespace LMPlatform.UI.Services.Subjects
{
    [Authorize(Roles = "lector")]
    public class SubjectsService : ISubjectsService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public SubjectResult Update(SubjectViewData subject)
        {
            var loadedSubject = SubjectManagementService.GetSubject(subject.Id);
            loadedSubject.IsNeededCopyToBts = subject.IsNeededCopyToBts;
            SubjectManagementService.SaveSubject(loadedSubject);
            return new SubjectResult
            {
                Subject = new SubjectViewData(loadedSubject)
            };
        }
    }
}
