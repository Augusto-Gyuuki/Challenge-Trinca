using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Events;

public sealed class BbqApprovedDomainEventHandler : INotificationHandler<BbqApprovedDomainEvent>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BbqApprovedDomainEventHandler(
        IPeopleRepository peopleRepository,
        IUnitOfWork unitOfWork,
        IBbqRepository bbqRepository)
    {
        _peopleRepository = peopleRepository;
        _unitOfWork = unitOfWork;
        _bbqRepository = bbqRepository;
    }

    public async Task Handle(BbqApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var bbq = await _bbqRepository.GetByIdAsync(notification.BbqId, cancellationToken);

        if (bbq is null)
        {
            throw new ArgumentNullException(nameof(Bbq), BbqErrors.BbqNotFound.Description);
        }

        var workersPeopleList = await _peopleRepository.GetAllAsync(cancellationToken);

        workersPeopleList
            .Where(x => !bbq.Guests.Any(guest => guest.PeopleId.Equals(x.Id)))
            .ToList()
            .ForEach(async people =>
            {
                if (!people.IsCoOwner)
                {
                    var invite = Invite.Create(notification.BbqId);
                    await _peopleRepository.UpdateAsync(people);
                    people.Invite(invite);
                }

                var guest = Guest.Create(people.Id);
                bbq.AddGuest(guest);
            });

        await _bbqRepository.UpdateAsync(bbq);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
