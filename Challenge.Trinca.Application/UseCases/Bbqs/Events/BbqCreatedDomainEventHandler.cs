using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Events;

public sealed class BbqCreatedDomainEventHandler : INotificationHandler<BbqCreatedDomainEvent>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BbqCreatedDomainEventHandler(IPeopleRepository peopleRepository, IUnitOfWork unitOfWork)
    {
        _peopleRepository = peopleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(BbqCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var coOwnerPeopleList = await _peopleRepository.GetAllCoOwnersAsync(cancellationToken);

        coOwnerPeopleList.ForEach(async people =>
        {
            var invite = Invite.Create(notification.BbqId);
            people.Invite(invite);

            await _peopleRepository.UpdateAsync(people);
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
