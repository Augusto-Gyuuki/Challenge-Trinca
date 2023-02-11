using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Peoples.Common.Results;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Peoples.Queries.GetPeopleInvites;

public sealed class GetPeopleInvitesQueryHandler : IRequestHandler<GetPeopleInvitesQuery, ErrorOr<PeopleModelResult>>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IBbqRepository _bbqRepository;

    public GetPeopleInvitesQueryHandler(IPeopleRepository peopleRepository, IBbqRepository bbqRepository)
    {
        _peopleRepository = peopleRepository;
        _bbqRepository = bbqRepository;
    }

    public async Task<ErrorOr<PeopleModelResult>> Handle(GetPeopleInvitesQuery request, CancellationToken cancellationToken)
    {
        var people = await _peopleRepository.GetByIdAsync(
            Guid.Parse(request.PersonId),
            cancellationToken);

        if (people is null)
        {
            return PeopleErrors.PeopleNotFound;
        }

        var invites = new List<Invite>();

        people.Invites.ToList().ForEach(async invite =>
        {
            var bbq = await _bbqRepository.GetByIdAsync(invite.BbqId, cancellationToken);

            if (bbq?.Date > DateTime.UtcNow)
            {
                invites.Add(invite);
            }
        });

        return PeopleModelResult.FromPeople(people, invites);
    }
}
