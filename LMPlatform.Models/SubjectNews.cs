using System.Web.UI.WebControls;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class SubjectNews : ModelBase
    {
        public string Title
        {
            get;
            set;
        }

        public string Body
        {
            get; 
            set;
        }

        public int SubjectId
        {
            get;
            set;
        }

        public Subject Subject
        {
            get;
            set;
        }
    }
}