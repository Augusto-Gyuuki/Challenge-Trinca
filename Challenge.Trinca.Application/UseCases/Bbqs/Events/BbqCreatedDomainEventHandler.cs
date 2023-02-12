using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Events;

public sealed class BbqCreatedDomainEventHandler : INotificationHandler<BbqCreatedDomainEvent>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public BbqCreatedDomainEventHandler(IPeopleRepository peopleRepository, IUnitOfWork unitOfWork, ILogger logger)
    {
        _peopleRepository = peopleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(BbqCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize bbq created event {BbqCreatedDomainEvent}", notification);

        var coOwnerPeopleList = await _peopleRepository.GetAllCoOwnersAsync(cancellationToken);

        coOwnerPeopleList.ForEach(async people =>
        {
            var invite = Invite.Create(notification.BbqId);
            people.Invite(invite);
            await _peopleRepository.UpdateAsync(people);
            _logger.Information("Invite send with ID: {InviteId} for the People with ID: {PeopleId} and Name: {PeopleName}", invite.Id, people.Id, people.Name);
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
