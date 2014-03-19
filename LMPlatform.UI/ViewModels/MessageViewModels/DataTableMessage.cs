using System.ComponentModel;

namespace LMPlatform.UI.ViewModels.MessageViewModels
{
    public class DataTableMessage
    {
        public DataTableMessage(string message)
        {
            Message = message;
        }

        [DisplayName("Сообщения")]
        public string Message { get; set; }
    }
}