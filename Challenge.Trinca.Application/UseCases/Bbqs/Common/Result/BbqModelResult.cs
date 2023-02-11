using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;

public sealed class BbqModelResult
{
    public Guid Id { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public DateTime Date { get; private set; }

    public bool IsTrincaPaying { get; private set; }

    public string Status { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    private BbqModelResult(
        Guid id,
        string reason,
        DateTime date,
        bool isTrincaPaying,
        string status,
        DateTime updatedDateTime,
        DateTime createdDateTime)
    {
        Id = id;
        Reason = reason;
        Date = date;
        IsTrincaPaying = isTrincaPaying;
        Status = status;
        UpdatedDateTime = updatedDateTime;
        CreatedDateTime = createdDateTime;
    }

    public static BbqModelResult FromBbq(Bbq bbq)
    {
        return new(
            bbq.Id,
            bbq.Reason,
            bbq.Date,
            bbq.IsTrincaPaying,
            bbq.Status.Name,
            bbq.UpdatedDateTime,
            bbq.CreatedDateTime);
    }
}
