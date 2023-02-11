using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Events;

public sealed class GuestUpdatedDomainEventHandler : INotificationHandler<GuestUpdatedDomainEvent>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GuestUpdatedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IBbqRepository bbqRepository)
    {
        _unitOfWork = unitOfWork;
        _bbqRepository = bbqRepository;
    }

    public async Task Handle(GuestUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var bbq = await _bbqRepository.GetByIdAsync(notification.BbqId, cancellationToken);

        if (bbq is null)
        {
            throw new ArgumentNullException(nameof(Bbq), BbqErrors.BbqNotFound.Description);
        }

        if (bbq.Guests.Where(x => x.IsAttending.HasValue && x.IsAttending.Value).Count() >= Bbq.MINIMUM_GUEST_COUNT
            && bbq.Status.Equals(BbqStatus.PendingConfirmations))
        {
            bbq.ChangeStatusToConfirmed();
        }
        else if (bbq.Status.Equals(BbqStatus.Confirmed))
        {
            bbq.ChangeStatusToPendingConfirmations();
        }

        await _bbqRepository.UpdateAsync(bbq);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
