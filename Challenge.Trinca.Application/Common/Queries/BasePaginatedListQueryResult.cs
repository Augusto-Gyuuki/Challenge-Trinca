namespace Challenge.Trinca.Application.Common.Queries;

public record BasePaginatedListQueryResult<T> where T : class
{
    public int CurrentPage { get; set; }

    public int PerPage { get; set; }

    public IReadOnlyList<T>? Items { get; set; }

    public int Total { get; set; }
}
