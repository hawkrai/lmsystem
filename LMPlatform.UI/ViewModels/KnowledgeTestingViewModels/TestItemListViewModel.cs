using System.ComponentModel;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.UI.ViewModels.KnowledgeTestingViewModels
{
    public class TestItemListViewModel
    {
        [DisplayName("Номер")]
        public int Id
        {
            get;
            set;
        }

        [DisplayName("Название")]
        public string Title
        {
            get;
            set;
        }

        public static TestItemListViewModel FromTest(Test test)
        {
            return new TestItemListViewModel
            {
                Id = test.Id,
                Title = test.Title
            };
        }
    }
}