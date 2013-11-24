namespace Application.Core.Data
{
    public interface ISortCriteria
    {
        string Name
        {
            get; set;
        }

        SortDirection SortDirection
        {
            get; set;
        }
    }
}