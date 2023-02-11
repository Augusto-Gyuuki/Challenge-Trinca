using Challenge.Trinca.Application.Common.Searchable;
using Challenge.Trinca.Application.UseCases.Bbqs.Queries.ListBbqs;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Tests.Unit.BaseFixtures;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Queries.ListBbqs;

public static class ListBbqsQueryFixture
{
    public static ListBbqsQuery GetListBbqsQuery(int page = 1, int perPage = 10)
    {
        return new ListBbqsQuery
        {
            Page = page,
            PerPage = perPage
        };
    }

    public static SearchableOutput<Bbq> GetSearchableOutput(int page = 1, int perPage = 10)
    {
        var bbqList = Enumerable.Range(1, perPage)
            .Select(x => CommonBbqFixture.GetBbq())
            .ToList();

        return new SearchableOutput<Bbq>()
        {
            CurrentPage = page,
            PerPage = perPage,
            Total = bbqList.Count,
            Items = bbqList
        };
    }
}
