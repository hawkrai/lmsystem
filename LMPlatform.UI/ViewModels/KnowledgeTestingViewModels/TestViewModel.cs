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

        public bool ForEUMK
        {
            get;
            set;
        }

        public bool BeforeEUMK
        {
            get;
            set;
        }

        public int? TestNumber
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
                ForSelfStudy = ForSelfStudy || ForEUMK || BeforeEUMK,
                ForEUMK = ForEUMK,
                BeforeEUMK = BeforeEUMK,
                IsNecessary = IsNecessary,
                TestNumber = TestNumber
            };
        }

        public static TestViewModel FromTest(Test test)
        {
            return new TestViewModel
            {
                Id = test.Id,
                SubjectId = test.SubjectId,
                Title = test.Title,
                Description = test.Description,
                TimeForCompleting = test.TimeForCompleting,
                SetTimeForAllTest = test.SetTimeForAllTest,
                ForSelfStudy = test.ForSelfStudy,
                ForEUMK = test.ForEUMK,
                BeforeEUMK = test.BeforeEUMK,
                CountOfQuestions = test.CountOfQuestions,
                IsNecessary = test.IsNecessary,
                TestNumber = test.TestNumber
            };
        }
    }
}