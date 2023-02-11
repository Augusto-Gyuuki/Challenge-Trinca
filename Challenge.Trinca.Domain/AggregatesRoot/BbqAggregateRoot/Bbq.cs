using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.Common.Exceptions;
using Challenge.Trinca.Domain.Common.Models;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;

namespace Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;

public sealed class Bbq : AggregateRoot
{
    public const int REASON_MAX_LENGTH = 10_000;
    public const int MINIMUM_GUEST_COUNT = 7;

    private readonly List<Guest> _guests = new();

    public Guid Id { get; private set; }

    public string Reason { get; private set; }

    public BbqStatus Status { get; private set; }

    public DateTime Date { get; private set; }

    public bool IsTrincaPaying { get; private set; }

    public IReadOnlyList<Guest> Guests => _guests.AsReadOnly();

    private Bbq() { }

    private Bbq(
        Guid bbqId,
        string reason,
        BbqStatus status,
        DateTime date,
        bool isTrincaPaying)
    {
        Id = bbqId;
        Reason = reason;
        Status = status;
        Date = date;
        IsTrincaPaying = isTrincaPaying;
        UpdatedDateTime = DateTime.UtcNow;
        CreatedDateTime = DateTime.UtcNow;
    }

    public static Bbq Create(
        string reason,
        DateTime date,
        bool isTrincaPaying = false)
    {
        var bbq = new Bbq(
            Guid.NewGuid(),
            reason,
            BbqStatus.New,
            date,
            isTrincaPaying);

        bbq.Validate();

        bbq.RaiseDomainEvent(new BbqCreatedDomainEvent(bbq.Id));

        return bbq;
    }

    public void AddGuest(Guest guest)
    {
        _guests.Add(guest);

        Validate();
    }

    public void ConfirmGuest(Guest guest)
    {
        guest.WillAttend();

        RaiseDomainEvent(new GuestUpdatedDomainEvent(Id));
    }

    public void DeclineGuest(Guest guest)
    {
        guest.WillNotAttend();

        RaiseDomainEvent(new GuestUpdatedDomainEvent(Id));
    }

    public void Update(
        string reason,
        DateTime? date = null)
    {
        Date = date ?? Date;
        Reason = reason;

        Validate();
    }

    public void TrincaWillPay()
    {
        IsTrincaPaying = true;

        Validate();
    }

    public void TrincaWillNotPay()
    {
        IsTrincaPaying = false;

        Validate();
    }

    public void ChangeStatusToPendingConfirmations()
    {
        Status = BbqStatus.PendingConfirmations;

        Validate();

        RaiseDomainEvent(new BbqApprovedDomainEvent(Id));
    }

    public void ChangeStatusToConfirmed()
    {
        Status = BbqStatus.Confirmed;

        Validate();
    }

    public void DenyBbq()
    {
        Status = BbqStatus.ItsNotGonnaHappen;

        Validate();

        RaiseDomainEvent(new BbqDeniedDomainEvent(Id));
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Reason))
        {
            throw new EntityValidationException(BbqErrors.ReasonNullOrWhiteSpace.Description);
        }

        if (Reason.Length > REASON_MAX_LENGTH)
        {
            throw new EntityValidationException(BbqErrors.ReasonMaxLength.Description);
        }

        if (Date.ToUniversalTime() < DateTime.UtcNow)
        {
            throw new EntityValidationException(BbqErrors.DateInvalid.Description);
        }

        UpdatedDateTime = DateTime.UtcNow;
    }
}
