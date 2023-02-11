namespace Challenge.Trinca.Presentation.Endpoints.Common.Response;

public class ApiResponseList<TItemData>
{
    public ApiResponseListMeta Meta { get; private set; }

    public IReadOnlyList<TItemData> Data { get; private set; }

    public ApiResponseList(
        int currentPage,
        int perPage,
        int total,
        IReadOnlyList<TItemData> data
    )
    {
        Meta = new ApiResponseListMeta(currentPage, perPage, total);
        Data = data;
    }
}
