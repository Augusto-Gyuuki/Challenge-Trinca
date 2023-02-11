using Challenge.Trinca.Application.Common.Queries;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Queries.ListBbqs;

public sealed record ListBbqsQueryResult : BasePaginatedListQueryResult<BbqModelResult>
{
    public ListBbqsQueryResult(
        int currentPage,
        int perPage,
        IReadOnlyList<BbqModelResult>? items,
        int total)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Items = items;
        Total = total;
    }
}
