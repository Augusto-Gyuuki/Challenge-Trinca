using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Queries.GetBbq;

public sealed class GetBbqQueryResult
{
    public Guid Id { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public DateTime Date { get; private set; }

    public bool IsTrincaPaying { get; private set; }

    public string Status { get; private set; }

    public float VegetableAmountInKilograms { get; private set; }

    public float MeatAmountInKilograms { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    private GetBbqQueryResult(
        Guid id,
        string reason,
        DateTime date,
        bool isTrincaPaying,
        string status,
        DateTime updatedDateTime,
        DateTime createdDateTime,
        float vegetableAmountInKilograms,
        float meatAmountInKilograms)
    {
        Id = id;
        Reason = reason;
        Date = date;
        IsTrincaPaying = isTrincaPaying;
        Status = status;
        UpdatedDateTime = updatedDateTime;
        CreatedDateTime = createdDateTime;
        VegetableAmountInKilograms = vegetableAmountInKilograms;
        MeatAmountInKilograms = meatAmountInKilograms;
    }

    public static GetBbqQueryResult FromBbq(Bbq bbq)
    {
        var vegetarianGuests = bbq.Guests
            .Where(x => x.IsAttending.HasValue && x.IsAttending.Value && x.IsVegetarian.HasValue && x.IsVegetarian.Value)
            .ToList();

        var nonVegetarianGuests = bbq.Guests
            .Where(x => x.IsAttending.HasValue && x.IsAttending.Value && x.IsVegetarian.HasValue && !x.IsVegetarian.Value)
            .ToList();

        var vegetableAmount = 0f;
        var meatAmount = 0f;

        vegetarianGuests.ForEach(x =>
        {
            vegetableAmount += 600;
        });

        nonVegetarianGuests.ForEach(x =>
        {
            vegetableAmount += 300;
            meatAmount += 300;
        });

        return new(
            bbq.Id,
            bbq.Reason,
            bbq.Date,
            bbq.IsTrincaPaying,
            bbq.Status.Name,
            bbq.UpdatedDateTime,
            bbq.CreatedDateTime,
            vegetableAmount / 1000,
            meatAmount / 1000);
    }
}
