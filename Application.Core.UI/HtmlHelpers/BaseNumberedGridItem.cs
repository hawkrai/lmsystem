using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Core.UI.HtmlHelpers
{
    public class BaseNumberedGridItem
    {
        [DisplayAttribute(Order = 1)]
        [DisplayName("№")]
        public virtual int Number
        {
            get;
            set;
        }
    }
}