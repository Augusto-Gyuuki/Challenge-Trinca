using Challenge.Trinca.Application.Common.Searchable;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.ListBbqs;

public sealed record ListBbqsRequest
{
    public const int DEFAULT_PAGE = 1;
    public const int DEFAULT_PER_PAGE = 20;
    public const SearchOrder DEFAULT_ORDER = SearchOrder.Ascending;

    public int Page { get; init; }

    public int PerPage { get; init; } = DEFAULT_PER_PAGE;

    public string Search { get; init; } = string.Empty;

    public string Sort { get; init; } = string.Empty;

    public SearchOrder Dir { get; init; } = DEFAULT_ORDER;
}
