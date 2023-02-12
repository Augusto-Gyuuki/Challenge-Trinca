using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.Repositories;
using ErrorOr;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Commands.ModerateBbq;

public sealed class ModerateBbqCommandHandler : IRequestHandler<ModerateBbqCommand, ErrorOr<BbqModelResult>>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public ModerateBbqCommandHandler(IBbqRepository bbqRepository, IUnitOfWork unitOfWork, ILogger logger)
    {
        _bbqRepository = bbqRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<BbqModelResult>> Handle(ModerateBbqCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize moderate bbq {ModerateBbqCommand}", request);

        var bbq = await _bbqRepository.GetByIdAsync(
            Guid.Parse(request.BbqId),
            cancellationToken);

        if (bbq is null)
        {
            _logger.Error("Bbq was not found with ID: {BbqId}", request.BbqId);
            return BbqErrors.BbqNotFound;
        }
        _logger.Information("Bbq found with ID: {BbqId}", request.BbqId);

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
            _logger.Information("Update the status of the bbq with ID: {BbqId} to PendingConfirmation", request.BbqId);
            bbq.ChangeStatusToPendingConfirmations();
        }
        else
        {
            _logger.Information("Update the status of the bbq with ID: {BbqId} to ItsNotGonnaHappen", request.BbqId);
            bbq.DenyBbq();
        }

        await _bbqRepository.UpdateAsync(bbq);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.Information("Bbq with ID: {BbqId} updated on database", request.BbqId);

        return BbqModelResult.FromBbq(bbq);
    }
}
