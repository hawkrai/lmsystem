using LMPlatform.Data.Repositories;
using LMPlatform.Models.KnowledgeTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;

namespace Application.Infrastructure.TestQuestionPassingManagement
{
    public class TestQuestionPassingService : ITestQuestionPassingService
    {
        public void SaveTestQuestionPassResults(TestQuestionPassResults item)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                if (item != null)
                {
                    repositoriesContainer.TestQuestionPassResultsRepository.Save(item);
                }
                repositoriesContainer.ApplyChanges();
            }
        }

        public List<AnswerOnTestQuestion> GetAll()
        {
	        using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
	        {
		        var data = repositoriesContainer.RepositoryFor<AnswerOnTestQuestion>().GetAll();

		        return data.ToList();
	        }
        }
	}
}
