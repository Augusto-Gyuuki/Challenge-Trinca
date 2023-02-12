using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Events;

public sealed class BbqApprovedDomainEventHandler : INotificationHandler<BbqApprovedDomainEvent>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public BbqApprovedDomainEventHandler(
        IPeopleRepository peopleRepository,
        IUnitOfWork unitOfWork,
        IBbqRepository bbqRepository,
        ILogger logger)
    {
        _peopleRepository = peopleRepository;
        _unitOfWork = unitOfWork;
        _bbqRepository = bbqRepository;
        _logger = logger;
        _logger.ForContext<BbqApprovedDomainEvent>();
    }

    public async Task Handle(BbqApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize bbq approved event {BbqApprovedDomainEvent}", notification);

        var bbq = await _bbqRepository.GetByIdAsync(notification.BbqId, cancellationToken);

        if (bbq is null)
        {
            _logger.Error("Bbq was not found with ID: {BbqId}", notification.BbqId);
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
                    people.Invite(invite);
                    await _peopleRepository.UpdateAsync(people);
                    _logger.Information("Invite send with ID: {InviteId} for the People with ID: {PeopleId} and Name: {PeopleName}", invite.Id, people.Id, people.Name);
                }

                var guest = Guest.Create(people.Id);
                bbq.AddGuest(guest);
                _logger.Information("Guest with ID: {PeopleId}, added to Bbq with ID: {BbqId}", people.Id, bbq.Id);
            });

        await _bbqRepository.UpdateAsync(bbq);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.Information("Bbq with ID: {BbqId} updated on database", notification.BbqId);
    }
}
