using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services.News
{
    using System.Web.Http;
    using System.Web.Mvc;

    using Application.Core;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;
    using LMPlatform.UI.Services.Modules;
    using LMPlatform.UI.Services.Modules.News;
    using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

    public class NewsService : INewsService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public NewsResult GetNews(string subjectId)
        {
            try
            {
                var model = SubjectManagementService.GetSubject(int.Parse(subjectId)).SubjectNewses.OrderByDescending(e => e.EditDate).Select(e => new NewsViewData(e)).ToList();

                return new NewsResult
                           {
                               News = model,
                               Message = "Новости успешно загружены",
                               Code = "200"
                           };
            }
            catch (Exception)
            {
                return new NewsResult()
                           {
                               Message = "Произошла ошибка при получении новостей",
                               Code = "500"
                           };
            }
        }

        public ResultViewData Save(string subjectId, string id, string title, string body, bool disabled, bool isOldDate)
        {
            try
            {
                var newsIds = string.IsNullOrEmpty(id) ? 0 : int.Parse(id);
                var date = DateTime.Now;

	            if ((newsIds != 0 && isOldDate) || (newsIds != 0 && disabled))
	            {
		            date = SubjectManagementService.GetNews(newsIds, int.Parse(subjectId)).EditDate;
	            }
				else if ((newsIds != 0 && !disabled))
	            {
		            if (SubjectManagementService.GetNews(newsIds, int.Parse(subjectId)).Disabled)
		            {
			            date = DateTime.Now;
		            }
	            }

                var model = new SubjectNews
                                {
                                    Id = newsIds,
                                    SubjectId = int.Parse(subjectId),
                                    Body = body,
                                    EditDate = date,
                                    Title = title,
									Disabled = disabled
                                };
                SubjectManagementService.SaveNews(model);
                return new ResultViewData()
                {
                    Message = "Новость успешно сохранена",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при сохранении новости",
                    Code = "500"
                };
            }
        }

        public ResultViewData Delete(string id, string subjectId)
        {
            try
            {
                var model = new SubjectNews
                {
                    Id = string.IsNullOrEmpty(id) ? 0 : int.Parse(id),
                    SubjectId = int.Parse(subjectId),
                };
                SubjectManagementService.DeleteNews(model);
                return new ResultViewData()
                {
                    Message = "Новость успешно удалена",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при удалении новости",
                    Code = "500"
                };
            }
        }
    }
}
