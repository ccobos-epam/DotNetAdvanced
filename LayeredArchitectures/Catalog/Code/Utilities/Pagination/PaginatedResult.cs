namespace Utilities.Pagination;

public record PaginatedResult<T> where T : class
{
    public IList<T> ResultItems { get; set; } = [];

    public int TotalAvailableItems { get; set; } = 0;
    public int RetrievedItems { get; set; } = 0; 
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
