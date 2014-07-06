using System.Collections.Generic;

namespace Application.Core.Data
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }

        public int Total { get; set; }
    }
}
