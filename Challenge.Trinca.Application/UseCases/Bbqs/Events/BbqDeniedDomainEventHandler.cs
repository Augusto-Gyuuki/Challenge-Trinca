using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Events;

public sealed class BbqDeniedDomainEventHandler : INotificationHandler<BbqDeniedDomainEvent>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public BbqDeniedDomainEventHandler(IPeopleRepository peopleRepository, IUnitOfWork unitOfWork, ILogger logger)
    {
        _peopleRepository = peopleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(BbqDeniedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize bbq denied {BbqDeniedDomainEvent}", notification);

        var peopleList = await _peopleRepository.GetAllAsync(cancellationToken);

        peopleList.ForEach(async people =>
        {
            var invite = people.Invites
                .FirstOrDefault(invite => invite.BbqId.Equals(notification.BbqId));

            if (invite is null)
            {
                _logger.Error("Invite for bbq with ID: {BbqId} not found", notification.BbqId);
                return;
            }

            people.DeclineInvite(invite);
            _logger.Information("Decline invite with ID: {InviteId} for the People with ID: {PeopleId} and Name: {PeopleName}", invite.Id, people.Id, people.Name);

            await _peopleRepository.UpdateAsync(people);
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
