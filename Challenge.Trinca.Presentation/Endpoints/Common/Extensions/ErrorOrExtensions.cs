using Challenge.Trinca.Application.Common.Queries;
using Challenge.Trinca.Presentation.Endpoints.Common.Response;
using ErrorOr;

namespace Challenge.Trinca.Presentation.Endpoints.Common.Extensions;

public static class ErrorOrExtensions
{
    public static ApiResponseList<ApiResponse<T>> ToApiResponseList<T>(this BasePaginatedListQueryResult<T> result)
        where T : class
    {
        var items = result?.Items.Select(x => x.ToApiResponse()).ToList();

        return new ApiResponseList<ApiResponse<T>>(
            result.CurrentPage,
            result.PerPage,
            result.Total,
            items);
    }

    public static ApiResponse<T> ToApiResponse<T>(this T result)
    {
        return new ApiResponse<T>(result);
    }

    public static ErrorResponse ToErrorResponse(this List<Error> erros)
    {
        return new ErrorResponse()
        {
            Error = string.Join(Environment.NewLine, erros.Select(x => x.Description)),
            StatusCode = 400,
        };
    }
}
