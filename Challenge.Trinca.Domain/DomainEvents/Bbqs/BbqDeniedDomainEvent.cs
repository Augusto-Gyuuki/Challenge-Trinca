using Challenge.Trinca.Domain.Common.Models;

namespace Challenge.Trinca.Domain.DomainEvents.Bbqs;

public sealed record BbqDeniedDomainEvent(Guid BbqId) : IDomainEvent;