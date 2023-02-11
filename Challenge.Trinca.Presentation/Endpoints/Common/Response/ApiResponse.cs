namespace Challenge.Trinca.Presentation.Endpoints.Common.Response;

public class ApiResponse<TData>
{
    public TData Data { get; private set; }

    public ApiResponse(TData data)
        => Data = data;
}
