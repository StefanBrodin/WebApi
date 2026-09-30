namespace Models.DTO;

public class ResponsePageDto<T>
{
#if DEBUG
    // Only used in debug mode to show the connection string
    public string ConnectionString { get; init; }
#endif

    public List<T> PageItems { get; init; } = new();
    public int DbItemsCount { get; init; }

    public int PageNr { get; init; }
    public int PageSize { get; init; }
    public int PageCount => PageSize > 0 ? (int)Math.Ceiling((double)DbItemsCount / PageSize) : 0;
}

public class ResponseItemDto<T>
{
#if DEBUG
    // Only used in debug mode to show the connection string
    public string ConnectionString { get; init; }
#endif

    public T Item { get; init; }
}