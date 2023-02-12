using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using ErrorOr;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Queries.GetBbq;

public sealed class GetBbqQueryHandler : IRequestHandler<GetBbqQuery, ErrorOr<GetBbqQueryResult>>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IPeopleRepository _peopleRepository;
    private readonly ILogger _logger;

    public GetBbqQueryHandler(IBbqRepository bbqRepository, IPeopleRepository peopleRepository, ILogger logger)
    {
        _bbqRepository = bbqRepository;
        _peopleRepository = peopleRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<GetBbqQueryResult>> Handle(GetBbqQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize get bbq {GetBbqQuery}", request);

        var people = await _peopleRepository.GetByIdAsync(Guid.Parse(request.PeopleId), cancellationToken);

        if (people is null)
        {
            _logger.Error("People not found with ID: {PeopleId}", request.PeopleId);
            return PeopleErrors.PeopleNotFound;
        }
        _logger.Information("People found with ID: {PeopleId} and Name: {PeopleName}", request.PeopleId, people.Name);

        if (!people.IsCoOwner)
        {
            _logger.Error("People Not Authorized with ID: {PeopleId} and Name: {PeopleName}", request.PeopleId, people.Name);
            return PeopleErrors.NotAuthorized;
        }

        var bbq = await _bbqRepository.GetByIdAsync(Guid.Parse(request.BbqId), cancellationToken);

        if (bbq is null)
        {
            _logger.Error("Bbq was not found with ID: {BbqId}", request.BbqId);
            return BbqErrors.BbqNotFound;
        }
        _logger.Information("Bbq found with ID: {BbqId}", request.BbqId);

        return GetBbqQueryResult.FromBbq(bbq);
    }
}
