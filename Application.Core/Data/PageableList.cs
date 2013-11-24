using System.Collections.Generic;

namespace Application.Core.Data
{
    public class PageableList<TItem> : IPageableList<TItem>
    {
        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        public IList<TItem> Items { get; set; }

        public int TotalCount { get; set; }

        public IPageInfo PageInfo { get; set; }

        public PageableList()
        {
            PageInfo = new PageInfo();
            Items = new List<TItem>();
        }
    }
}
