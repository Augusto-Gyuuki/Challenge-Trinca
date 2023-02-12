using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.DomainEvents.Peoples;
using Challenge.Trinca.Domain.Repositories;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Peoples.Events;

public sealed class InviteDeclinedDomainEventHandler : INotificationHandler<InviteDeclinedDomainEvent>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public InviteDeclinedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IBbqRepository bbqRepository,
        ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _bbqRepository = bbqRepository;
        _logger = logger;
    }

    public async Task Handle(InviteDeclinedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize invite declined event {InviteDeclinedDomainEvent}", notification);

        var bbq = await _bbqRepository.GetByIdAsync(notification.BbqId, cancellationToken);

        if (bbq is null)
        {
            _logger.Error("Bbq was not found with ID: {BbqId}", notification.BbqId);
            throw new ArgumentNullException(nameof(Bbq), BbqErrors.BbqNotFound.Description);
        }

        var guest = bbq.Guests.FirstOrDefault(x => x.PeopleId.Equals(notification.PeopleId));

        if (guest is null)
        {
            _logger.Error("Guest was not found with ID: {PeopleId}", notification.PeopleId);
            throw new ArgumentNullException(nameof(Guest), BbqErrors.GuestNotFound.Description);
        }

        if (!guest.IsAttending.HasValue || guest.IsAttending.Value)
        {
            bbq.DeclineGuest(guest);
        }

        await _bbqRepository.UpdateAsync(bbq);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.Information("Bbq with ID: {BbqId} updated on database", notification.BbqId);
    }
}
