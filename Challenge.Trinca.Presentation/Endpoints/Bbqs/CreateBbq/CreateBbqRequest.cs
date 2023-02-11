namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.CreateBbq;

public sealed record CreateBbqRequest
{
    public string Reason { get; init; } = string.Empty;

    public DateTime Date { get; init; }

    public bool IsTrincaPaying { get; init; }
}
