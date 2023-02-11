using Challenge.Trinca.Domain.Common.Models;

namespace Challenge.Trinca.Domain.DomainEvents.Peoples;

public sealed record InviteAcceptedDomainEvent(
    bool IsVeg,
    Guid PeopleId,
    Guid BbqId) : IDomainEvent;