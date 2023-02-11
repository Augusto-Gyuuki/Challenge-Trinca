using Challenge.Trinca.Application.UseCases.Bbqs.Queries.ListBbqs;
using Mapster;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.ListBbqs;

public sealed class ListBbqsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        const int DEFAULT_PAGE = 1;
        const int DEFAULT_PER_PAGE = 20;

        config.NewConfig<ListBbqsRequest, ListBbqsQuery>()
                .Map(dest => dest.Page, src => src.Page != default
                    ? src.Page
                    : DEFAULT_PAGE)
                .Map(dest => dest.PerPage, src => src.PerPage != default
                    ? src.PerPage
                    : DEFAULT_PER_PAGE);
    }
}
