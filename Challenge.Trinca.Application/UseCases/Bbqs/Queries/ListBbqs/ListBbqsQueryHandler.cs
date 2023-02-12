using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Searchable;
using ErrorOr;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Queries.ListBbqs;

public sealed class ListBbqsQueryHandler : IRequestHandler<ListBbqsQuery, ErrorOr<ListBbqsQueryResult>>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly ILogger _logger;

    public ListBbqsQueryHandler(IBbqRepository bbqRepository, ILogger logger)
    {
        _bbqRepository = bbqRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<ListBbqsQueryResult>> Handle(ListBbqsQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize list bbqs {ListBbqsQuery}", request);

        var bbqsSearchInput = new BbqsSearchInput(
            request.Page,
            request.PerPage);

        var searchableOutput = await _bbqRepository.SearchAsync(bbqsSearchInput, cancellationToken);
        _logger.Information("Bbqs count found: {BbqsFoundTotal}", searchableOutput.Total);

        return new ListBbqsQueryResult(
            searchableOutput.CurrentPage,
            searchableOutput.PerPage,
            searchableOutput.Items?.Select(BbqModelResult.FromBbq).ToList(),
            searchableOutput.Total);
    }
}
