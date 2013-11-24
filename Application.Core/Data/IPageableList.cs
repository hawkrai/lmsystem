using System.Collections.Generic;

namespace Application.Core.Data
{
    public interface IPageableList<TItem>
    {
        bool HasNext { get; set; }

        bool HasPrevious { get; set; }

        IList<TItem> Items { get; set; }

	    int TotalCount { get; set; }

        IPageInfo PageInfo { get; set; }
    }
}