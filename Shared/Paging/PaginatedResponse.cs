namespace Shared.Paging;

public class PaginatedResponse<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public List<T>? Items { get; set; }
}
