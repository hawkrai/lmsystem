using Application.Core.Data;
using LMPlatform.Models.KnowledgeTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface ITestQuestionPassResultsRepository : IRepositoryBase<TestQuestionPassResults>
    {
        void Create(TestQuestionPassResults item);
    }
}
