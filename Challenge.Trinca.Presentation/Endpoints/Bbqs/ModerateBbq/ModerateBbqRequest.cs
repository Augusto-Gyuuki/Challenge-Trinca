namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.ModerateBbq;

public sealed record ModerateBbqRequest
{
    public bool GonnaHappen { get; init; }
    public bool TrincaWillPay { get; init; }
}
