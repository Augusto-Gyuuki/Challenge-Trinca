namespace Challenge.Trinca.Application.Common.Queries;

public abstract record BasePaginatedListQuery
{
    public const int DEFAULT_PAGE = 1;
    public const int DEFAULT_PER_PAGE = 20;

    public int Page { get; init; } = DEFAULT_PAGE;

    public int PerPage { get; init; } = DEFAULT_PER_PAGE;
}
