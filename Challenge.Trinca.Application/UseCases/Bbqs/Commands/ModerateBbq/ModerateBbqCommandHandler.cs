using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Commands.ModerateBbq;

public sealed class ModerateBbqCommandHandler : IRequestHandler<ModerateBbqCommand, ErrorOr<BbqModelResult>>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ModerateBbqCommandHandler(IBbqRepository bbqRepository, IUnitOfWork unitOfWork)
    {
        _bbqRepository = bbqRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<BbqModelResult>> Handle(ModerateBbqCommand request, CancellationToken cancellationToken)
    {
        var bbq = await _bbqRepository.GetByIdAsync(
            Guid.Parse(request.BbqId),
            cancellationToken);

        if (bbq is null)
        {
            return BbqErrors.BbqNotFound;
        }

        if (request.TrincaWillPay)
        {
            bbq.TrincaWillPay();
        }
        else
        {
            bbq.TrincaWillNotPay();
        }

        if (request.GonnaHappen)
        {
            bbq.ChangeStatusToPendingConfirmations();
        }
        else
        {
            bbq.DenyBbq();
        }

        await _bbqRepository.UpdateAsync(bbq);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BbqModelResult.FromBbq(bbq);
    }
}
