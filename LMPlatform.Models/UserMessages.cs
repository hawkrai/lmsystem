using System.ComponentModel.DataAnnotations;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class UserMessages : ModelBase
    {
        public UserMessages()
        {
        }

        public UserMessages(int recipientId, int authorId, int messageId)
        {
            RecipientId = recipientId;
            AuthorId = authorId;
            MessageId = messageId;
        }

        [Required]
        public int RecipientId { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int MessageId { get; set; }

        public User Recipient { get; set; }

        public User Author { get; set; }

        public Message Message { get; set; }
    }
}
