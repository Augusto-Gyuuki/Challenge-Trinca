using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Events;

public sealed class GuestUpdatedDomainEventHandler : INotificationHandler<GuestUpdatedDomainEvent>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public GuestUpdatedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IBbqRepository bbqRepository,
        ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _bbqRepository = bbqRepository;
        _logger = logger;
    }

    public async Task Handle(GuestUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize guest updated event {GuestUpdatedDomainEvent}", notification);

        var bbq = await _bbqRepository.GetByIdAsync(notification.BbqId, cancellationToken);

        if (bbq is null)
        {
            _logger.Error("Bbq was not found with ID: {BbqId}", notification.BbqId);
            throw new ArgumentNullException(nameof(Bbq), BbqErrors.BbqNotFound.Description);
        }

        if (bbq.Guests.Where(x => x.IsAttending.HasValue && x.IsAttending.Value).Count() >= Bbq.MINIMUM_GUEST_COUNT
            && bbq.Status.Equals(BbqStatus.PendingConfirmations))
        {
            bbq.ChangeStatusToConfirmed();
            _logger.Information("Bbq status update to Confirmed with ID: {BbqId}", notification.BbqId);
        }
        else if (bbq.Status.Equals(BbqStatus.Confirmed))
        {
            bbq.ChangeStatusToPendingConfirmations();
            _logger.Information("Bbq status update to pending confirmations with ID: {BbqId}", notification.BbqId);
        }

        await _bbqRepository.UpdateAsync(bbq);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.Information("Bbq with ID: {BbqId} updated on database", notification.BbqId);
    }
}
