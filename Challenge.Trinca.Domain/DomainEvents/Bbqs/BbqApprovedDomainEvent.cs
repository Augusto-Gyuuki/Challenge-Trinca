using Challenge.Trinca.Domain.Common.Models;

namespace Challenge.Trinca.Domain.DomainEvents.Bbqs;

public sealed record BbqApprovedDomainEvent(Guid BbqId) : IDomainEvent;