using Challenge.Trinca.Application.UseCases.Bbqs.Queries.GetBbq;
using Mapster;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.GetBbq;

public sealed class GetBbqMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(string peopleId, string bbqId), GetBbqQuery>()
            .Map(dest => dest.PeopleId, src => src.peopleId)
            .Map(dest => dest.BbqId, src => src.bbqId);
    }
}
