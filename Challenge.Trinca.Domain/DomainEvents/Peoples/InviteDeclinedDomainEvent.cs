using Challenge.Trinca.Domain.Common.Models;

namespace Challenge.Trinca.Domain.DomainEvents.Peoples;

public sealed record InviteDeclinedDomainEvent(
    Guid PeopleId,
    Guid BbqId) : IDomainEvent;
