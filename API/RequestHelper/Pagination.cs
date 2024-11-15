namespace API.RequestHelper;

// Specify pagination properties we return to the client
public class Pagination<T> (int pageIndex, int pageSize, int totalCount, IReadOnlyList<T> data)  // () is new syntax for default constructor
{
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = totalCount; // Need to get this value BEFORE pagination, but AFTER filtering
    public IReadOnlyList<T> Data { get; set; } = data;

}