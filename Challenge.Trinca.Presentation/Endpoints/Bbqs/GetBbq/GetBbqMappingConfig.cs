using Challenge.Trinca.Application.UseCases.Bbqs.Queries.GetBbq;
using Mapster;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.GetBbq;

public sealed class GetBbqMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(string personId, string bbqId), GetBbqQuery>()
            .Map(dest => dest.PersonId, src => src.personId)
            .Map(dest => dest.BbqId, src => src.bbqId);
    }
}
