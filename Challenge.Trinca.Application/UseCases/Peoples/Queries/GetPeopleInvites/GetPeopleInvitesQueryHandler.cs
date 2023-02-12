using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Peoples.Common.Results;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;
using ErrorOr;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Peoples.Queries.GetPeopleInvites;

public sealed class GetPeopleInvitesQueryHandler : IRequestHandler<GetPeopleInvitesQuery, ErrorOr<PeopleModelResult>>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IBbqRepository _bbqRepository;
    private readonly ILogger _logger;

    public GetPeopleInvitesQueryHandler(IPeopleRepository peopleRepository, IBbqRepository bbqRepository, ILogger logger)
    {
        _peopleRepository = peopleRepository;
        _bbqRepository = bbqRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<PeopleModelResult>> Handle(GetPeopleInvitesQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize get people invites {GetPeopleInvitesQuery}", request);

        var people = await _peopleRepository.GetByIdAsync(
            Guid.Parse(request.PeopleId),
            cancellationToken);

        if (people is null)
        {
            _logger.Error("People not found with ID: {PeopleId}", request.PeopleId);
            return PeopleErrors.PeopleNotFound;
        }

        _logger.Information("People found with ID: {PeopleId} and Name: {PeopleName}", request.PeopleId, people.Name);
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
