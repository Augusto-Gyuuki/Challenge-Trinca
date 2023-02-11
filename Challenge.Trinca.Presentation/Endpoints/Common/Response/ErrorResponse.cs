namespace Challenge.Trinca.Presentation.Endpoints.Common.Response;

public sealed class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Error { get; set; }
}
