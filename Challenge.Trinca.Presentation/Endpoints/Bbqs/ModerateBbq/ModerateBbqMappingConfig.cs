using Challenge.Trinca.Application.UseCases.Bbqs.Commands.ModerateBbq;
using Mapster;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.ModerateBbq;

public sealed class ModerateBbqMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(ModerateBbqRequest req, string bbqId), ModerateBbqCommand>()
                .Map(dest => dest.BbqId, src => src.bbqId)
                .Map(dest => dest.GonnaHappen, src => src.req.GonnaHappen)
                .Map(dest => dest.TrincaWillPay, src => src.req.TrincaWillPay);
    }
}
