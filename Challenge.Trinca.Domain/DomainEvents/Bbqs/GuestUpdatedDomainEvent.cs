using Challenge.Trinca.Domain.Common.Models;

namespace Challenge.Trinca.Domain.DomainEvents.Bbqs;

public sealed record GuestUpdatedDomainEvent(Guid BbqId) : IDomainEvent;
