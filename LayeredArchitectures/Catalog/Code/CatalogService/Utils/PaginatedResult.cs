namespace CatalogService.Utils;

public record PaginatedResult<T> where T : class
{
    public IList<T> ResultItems { get; set; } = [];

    public int TotalAvailableItems { get; set; } = 0;
    public int RetrievedItem { get; set; } = 0; 
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 0;
}
