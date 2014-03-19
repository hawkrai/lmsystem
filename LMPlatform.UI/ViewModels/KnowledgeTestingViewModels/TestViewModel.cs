using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.UI.ViewModels.KnowledgeTestingViewModels
{
    public class TestViewModel
    {
        public int Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int SubjectId
        {
            get;
            set;
        }

        public int TimeForCompleting
        {
            get;
            set;
        }

        public bool SetTimeForAllTest
        {
            get;
            set;
        }

        public int CountOfQuestions
        {
            get;
            set;
        }

        public bool IsNecessary
        {
            get;
            set;
        }

        public bool ForSelfStudy
        {
            get;
            set;
        }

        public Test ToTest()
        {
            return new Test
            {
                Id = Id,
                Title = Title,
                Description = Description,
                TimeForCompleting = TimeForCompleting,
                SetTimeForAllTest = SetTimeForAllTest,
                SubjectId = SubjectId,
                CountOfQuestions = CountOfQuestions,
                ForSelfStudy = ForSelfStudy,
                IsNecessary = IsNecessary
            };
        }

        public static TestViewModel FromTest(Test test)
        {
            return new TestViewModel
            {
                Id = test.Id,
                Title = test.Title,
                Description = test.Description,
                TimeForCompleting = test.TimeForCompleting,
                SetTimeForAllTest = test.SetTimeForAllTest,
                ForSelfStudy = test.ForSelfStudy,
                CountOfQuestions = test.CountOfQuestions,
                IsNecessary = test.IsNecessary
            };
        }
    }
}