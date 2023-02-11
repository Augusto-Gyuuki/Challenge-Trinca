using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Searchable;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Queries.ListBbqs;

public sealed class ListBbqsQueryHandler : IRequestHandler<ListBbqsQuery, ErrorOr<ListBbqsQueryResult>>
{
    private readonly IBbqRepository _bbqRepository;

    public ListBbqsQueryHandler(IBbqRepository bbqRepository)
    {
        _bbqRepository = bbqRepository;
    }

    public async Task<ErrorOr<ListBbqsQueryResult>> Handle(ListBbqsQuery request, CancellationToken cancellationToken)
    {
        var bbqsSearchInput = new BbqsSearchInput(
            request.Page,
            request.PerPage);

        var searchableOutput = await _bbqRepository.SearchAsync(bbqsSearchInput, cancellationToken);

        return new ListBbqsQueryResult(
            searchableOutput.CurrentPage,
            searchableOutput.PerPage,
            searchableOutput.Items?.Select(BbqModelResult.FromBbq).ToList(),
            searchableOutput.Total);
    }
}
