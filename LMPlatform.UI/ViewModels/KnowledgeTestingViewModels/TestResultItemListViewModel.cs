using System.ComponentModel;
using System.Linq;
using System.Web;
using Application.Core.UI.HtmlHelpers;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.UI.ViewModels.KnowledgeTestingViewModels
{
    public class TestResultItemListViewModel : BaseNumberedGridItem
    {
        [DisplayName("Студент")]
        public string StudentName
        {
            get;
            set;
        }

        [DisplayName("Оценки")]
        public HtmlString Marks
        {
            get;
            set;
        }

        public TestPassResult[] TestPassResults
        {
            get;
            set;
        }

        public static TestResultItemListViewModel FromStudent(Student student, HtmlString marks)
        {
            return new TestResultItemListViewModel
            {
                StudentName = student.FullName,
                Marks = marks,
                TestPassResults = student.User.TestPassResults.ToArray()
            };
        }

        public static TestResultItemListViewModel FromStudent(Student student)
        {
            return new TestResultItemListViewModel
            {
                StudentName = student.FullName,
                TestPassResults = student.User.TestPassResults.ToArray()
            };
        }
    }
}