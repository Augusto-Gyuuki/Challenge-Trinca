using Challenge.Trinca.Application.Common.Searchable;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Common.Searchable;

public sealed record BbqsSearchInput : BaseSearchableInput
{
    public BbqsSearchInput(
        int page,
        int perPage)
    {
        Page = page;
        PerPage = perPage;
    }
}
