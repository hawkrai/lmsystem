using System.ComponentModel;
using System.Web;
using Application.Core.UI.HtmlHelpers;
using LMPlatform.Models;

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

        public static TestResultItemListViewModel FromStudent(Student student, HtmlString marks)
        {
            return new TestResultItemListViewModel
            {
                StudentName = student.FullName,
                Marks = marks
            };
        }
    }
}