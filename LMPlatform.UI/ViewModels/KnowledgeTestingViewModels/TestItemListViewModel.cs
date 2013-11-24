using System.ComponentModel;

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
    }
}