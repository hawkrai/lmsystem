using System;

namespace Application.Core.Data
{
    public class SortCriteria : ISortCriteria
    {
        public string Name { get; set; }

        public SortDirection SortDirection { get; set; }

        public SortCriteria(string name, SortDirection sortDirection)
        {
            Name = name;
            SortDirection = sortDirection;
        }

        public SortCriteria(string name, string sortDirection)
        {
            Name = name;
            SortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirection);
        }

        public SortCriteria()
        {
        }
    }
}
