using LMPlatform.Data.Repositories;
using LMPlatform.Models.KnowledgeTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
