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

        public string StudentShortName
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
                StudentShortName = GetShortStudentName(student),
                Marks = marks,
                TestPassResults = student.User.TestPassResults.ToArray()
            };
        }

        public static TestResultItemListViewModel FromStudent(Student student)
        {
            return new TestResultItemListViewModel
            {
                StudentName = student.FullName,
                StudentShortName = GetShortStudentName(student),
                TestPassResults = student.User.TestPassResults.ToArray()
            };
        }

        private static string GetShortStudentName(Student student)
        {
            return student.LastName + ' ' + student.FirstName[0] + '.';
        }
    }
}