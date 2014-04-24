namespace LMPlatform.UI.ApiControllers
{
    using System;
    using System.Linq;
    using System.Web.Helpers;
    using System.Web.Http;
    using System.Web.Mvc;

    using Application.Core;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

    public class LabsController : ApiController
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        [System.Web.Http.HttpGet]
        public JsonResult GetLabs(int subjectId)
        {
            try
            {
                var model = SubjectManagementService.GetSubject(subjectId).Labs.Select(e => new LabsDataViewModel(e));
                return new JsonResult()
                {
                    Data = new
                    {
                        Data = model,
                        Message = "Произошла ошибка при получении лабораторных работ",
                        Error = true
                    }
                };
            }
            catch (Exception)
            {
                return new JsonResult()
                           {
                               Data = new
                                          {
                                              Message = "Произошла ошибка при получении лабораторныйх работ",
                                              Error = true
                                          }
                           };
            }
        }
    }
}