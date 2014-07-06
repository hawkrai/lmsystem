using System.Collections.Generic;

namespace Application.Core.Data
{
    public class GetPagedListParams
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string SortExpression { get; set; }

        public Dictionary<string, string> Filters { get; set; } 
    }
}
