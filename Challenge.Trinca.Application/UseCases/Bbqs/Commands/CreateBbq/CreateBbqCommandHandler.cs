using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.Repositories;
using ErrorOr;
using MediatR;
using Serilog;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Commands.CreateBbq;

public sealed class CreateBbqCommandHandler : IRequestHandler<CreateBbqCommand, ErrorOr<BbqModelResult>>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public CreateBbqCommandHandler(IBbqRepository bbqRepository, IUnitOfWork unitOfWork, ILogger logger)
    {
        _bbqRepository = bbqRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<BbqModelResult>> Handle(CreateBbqCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Initialize create bbq {CreateBbqCommand}", request);

        var bbq = Bbq.Create(
            request.Reason,
            request.Date,
            request.IsTrincaPaying);
        _logger.Information("Bbq created {BbqId}", bbq.Id);

        await _bbqRepository.AddAsync(bbq, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.Information("Bbq with ID: {BbqId} added to database", bbq.Id);

        return BbqModelResult.FromBbq(bbq);
    }
}
