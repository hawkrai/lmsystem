using System;
using System.ComponentModel;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.MessageViewModels
{
    public class DisplayMessageViewModel
    {
        [DisplayName("")]
        public int AttachmentsCount { get; set; }

        [DisplayName("")]
        public string AuthorName { get; set; }

        [DisplayName("")]
        public string Subject { get; set; }

        [DisplayName("")]
        public string PreviewText
        {
            get
            {
                return !string.IsNullOrEmpty(Text) ? Text.Substring(0, Math.Min(Text.Length, 100)) : Text;
            }
        }

        [DisplayName("")]
        public string Date
        {
            get
            {
                if (DateTime.Date == DateTime.Now.Date)
                {
                    return DateTime.ToString("t");
                }
                else
                {
                    return DateTime.ToString("d");
                }
            }
        }

        [DisplayName("")]
        public string HtmlLinks { get; set; }

        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public bool IsReaded { get; set; }

        public int MessageId { get; set; }

        public int AuthorId { get; set; }

        public static DisplayMessageViewModel FormMessageToDisplay(UserMessages userMessages, string htmlLinks)
        {
            return new DisplayMessageViewModel
                {
                    AuthorName = userMessages.Author.FullName,
                    DateTime = userMessages.Date,
                    Text = userMessages.Message.Text,
                    MessageId = userMessages.MessageId,
                    AuthorId = userMessages.AuthorId,
                    IsReaded = userMessages.IsReaded,
                    Subject = userMessages.Message.Subject,
                    AttachmentsCount = userMessages.Message.Attachments.Count,
                    HtmlLinks = htmlLinks, 
                };
        }
    }
}