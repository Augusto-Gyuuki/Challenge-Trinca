using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Peoples.Common.Results;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.Repositories;
using ErrorOr;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Peoples.Commands.DeclineInvite;

public sealed record DeclineInviteCommandHandler : IRequestHandler<DeclineInviteCommand, ErrorOr<InviteModelResult>>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public DeclineInviteCommandHandler(IPeopleRepository peopleRepository, IUnitOfWork unitOfWork, ILogger logger)
    {
        _peopleRepository = peopleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<InviteModelResult>> Handle(DeclineInviteCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize decline invite {DeclineInviteCommand}", request);

        var people = await _peopleRepository.GetByIdAsync(
            Guid.Parse(request.PeopleId),
            cancellationToken);

        if (people is null)
        {
            _logger.Error("People not found with ID: {PeopleId}", request.PeopleId);
            return PeopleErrors.PeopleNotFound;
        }
        _logger.Information("People found with ID: {PeopleId} and Name: {PeopleName}", request.PeopleId, people.Name);

        var invite = people.Invites
            .FirstOrDefault(x => x.Id.Equals(Guid.Parse(request.InviteId)));

        if (invite is null)
        {
            _logger.Error("Invite not found with ID: {InviteId}", request.InviteId);
            return PeopleErrors.InviteNotFound;
        }
        _logger.Information("Invite found with ID: {InviteId}", request.InviteId);

        if (!invite.Status.Equals(InviteStatus.Declined))
        {
            people.DeclineInvite(invite);
        }

        await _peopleRepository.UpdateAsync(people);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.Information("People with ID: {PeopleId} updated on database", request.PeopleId);

        return InviteModelResult.FromInvite(invite);
    }
}
