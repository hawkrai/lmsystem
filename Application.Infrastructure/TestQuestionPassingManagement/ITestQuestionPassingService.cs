using LMPlatform.Models.KnowledgeTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.TestQuestionPassingManagement
{
    public interface ITestQuestionPassingService
    {
	    List<AnswerOnTestQuestion> GetAll();

		void SaveTestQuestionPassResults(TestQuestionPassResults item);
    }
}
