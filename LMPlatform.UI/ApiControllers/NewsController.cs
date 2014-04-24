namespace LMPlatform.UI.ApiControllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc;

    using Application.Core;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;
    using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

    public class NewsController : ApiController
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
        public JsonResult GetNews(int subjectId)
        {
            try
            {
                var model = SubjectManagementService.GetSubject(subjectId).SubjectNewses.Select(e => new NewsDataViewModel(e)).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        Data = model,
                        Message = "Новости успешно загружены",
                        Error = false
                    }
                };
            }
            catch (Exception)
            {
                return new JsonResult()
                {
                    Data = new
                    {
                        Message = "Произошла ошибка при получении новостей",
                        Error = true
                    }
                };
            }
        }

        [System.Web.Http.HttpPost, ValidateInput(false)]
        public JsonResult Save(string subjectId, string newsId, string title, string body)
        {
            try
            {
                var model = new SubjectNews
                                {
                                    Body = body,
                                    EditDate = DateTime.Now,
                                    SubjectId = int.Parse(subjectId),
                                    Id = int.Parse(newsId),
                                    Title = title
                                };
                SubjectManagementService.SaveNews(model);
                return new JsonResult()
                {
                    Data = new
                    {
                        Message = "Новость успешно сохранена",
                        Error = false
                    }
                };
            }
            catch (Exception)
            {
                return new JsonResult()
                {
                    Data = new
                    {
                        Message = "Произошла ошибка при сохранении новости",
                        Error = true
                    }
                };
            }
        }
    }
}