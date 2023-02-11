using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Queries.GetBbq;

public sealed class GetBbqQueryHandler : IRequestHandler<GetBbqQuery, ErrorOr<GetBbqQueryResult>>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IPeopleRepository _peopleRepository;

    public GetBbqQueryHandler(IBbqRepository bbqRepository, IPeopleRepository peopleRepository)
    {
        _bbqRepository = bbqRepository;
        _peopleRepository = peopleRepository;
    }

    public async Task<ErrorOr<GetBbqQueryResult>> Handle(GetBbqQuery request, CancellationToken cancellationToken)
    {
        var people = await _peopleRepository.GetByIdAsync(Guid.Parse(request.PersonId), cancellationToken);

        if (people is null)
        {
            return PeopleErrors.PeopleNotFound;
        }

        if (!people.IsCoOwner)
        {
            return PeopleErrors.NotAuthorized;
        }

        var bbq = await _bbqRepository.GetByIdAsync(Guid.Parse(request.BbqId), cancellationToken);

        if (bbq is null)
        {
            return BbqErrors.BbqNotFound;
        }

        return GetBbqQueryResult.FromBbq(bbq);
    }
}
