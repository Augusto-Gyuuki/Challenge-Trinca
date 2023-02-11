﻿using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.DomainEvents.Peoples;
using Challenge.Trinca.Domain.Repositories;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Peoples.Events;

public sealed class InviteAcceptedDomainEventHandler : INotificationHandler<InviteAcceptedDomainEvent>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InviteAcceptedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IBbqRepository bbqRepository)
    {
        _unitOfWork = unitOfWork;
        _bbqRepository = bbqRepository;
    }

    public async Task Handle(InviteAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        var bbq = await _bbqRepository.GetByIdAsync(notification.BbqId, cancellationToken);

        if (bbq is null)
        {
            throw new ArgumentNullException(nameof(Bbq), BbqErrors.BbqNotFound.Description);
        }

        var guest = bbq.Guests.FirstOrDefault(x => x.PeopleId.Equals(notification.PeopleId));

        if (guest is null)
        {
            throw new ArgumentNullException(nameof(Guest), BbqErrors.GuestNotFound.Description);
        }

        if (notification.IsVeg != guest.IsVegetarian)
        {
            guest.SetIsVegetarian(notification.IsVeg);
        }

        if (!guest.IsAttending.HasValue || !guest.IsAttending.Value)
        {
            bbq.ConfirmGuest(guest);
        }

        await _bbqRepository.UpdateAsync(bbq);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
