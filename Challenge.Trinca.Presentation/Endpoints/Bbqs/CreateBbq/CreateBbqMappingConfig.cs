using Challenge.Trinca.Application.UseCases.Bbqs.Commands.CreateBbq;
using Mapster;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.CreateBbq;

public sealed class CreateBbqMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateBbqRequest, CreateBbqCommand>()
            .Map(dest => dest.Reason, src => src.Reason)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.IsTrincaPaying, src => src.IsTrincaPaying);
    }
}
