using Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Commands.ModerateBbq;

public sealed class ModerateBbqCommand : IRequest<ErrorOr<BbqModelResult>>
{
    public bool GonnaHappen { get; init; }

    public bool TrincaWillPay { get; init; }

    public string BbqId { get; set; } = string.Empty;
}
