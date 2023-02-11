using Challenge.Trinca.Domain.Common.Models;

namespace Challenge.Trinca.Domain.DomainEvents.Bbqs;

public sealed record BbqCreatedDomainEvent(Guid BbqId) : IDomainEvent;