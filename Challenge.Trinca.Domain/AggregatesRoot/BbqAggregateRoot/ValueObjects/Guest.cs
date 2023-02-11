using Challenge.Trinca.Domain.Common.Models;

namespace Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects;

public sealed class Guest : ValueObject
{
    public Guid PeopleId { get; private set; }

    public bool? IsVegetarian { get; private set; }

    public bool? IsAttending { get; private set; }

    private Guest() { }

    public Guest(
        Guid peopleId,
        bool isVegetarian)
    {
        PeopleId = peopleId;
        IsVegetarian = isVegetarian;
    }

    public static Guest Create(
        Guid peopleId,
        bool isVegetarian = false)
    {
        return new(
            peopleId,
            isVegetarian);
    }

    public void WillAttend()
    {
        IsAttending = true;
    }

    public void WillNotAttend()
    {
        IsAttending = false;
    }

    public void SetIsVegetarian(bool isVegetarian)
    {
        IsVegetarian = isVegetarian;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
