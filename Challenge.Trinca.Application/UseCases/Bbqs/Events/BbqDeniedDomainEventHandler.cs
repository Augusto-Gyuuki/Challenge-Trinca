using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Events;

public sealed class BbqDeniedDomainEventHandler : INotificationHandler<BbqDeniedDomainEvent>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BbqDeniedDomainEventHandler(IPeopleRepository peopleRepository, IUnitOfWork unitOfWork)
    {
        _peopleRepository = peopleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(BbqDeniedDomainEvent notification, CancellationToken cancellationToken)
    {
        var peopleList = await _peopleRepository.GetAllAsync(cancellationToken);

        peopleList.ForEach(async people =>
        {
            var invite = people.Invites
                .FirstOrDefault(invite => invite.BbqId.Equals(notification.BbqId));

            if (invite is null)
            {
                return;
            }

            people.DeclineInvite(invite);

            await _peopleRepository.UpdateAsync(people);
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
