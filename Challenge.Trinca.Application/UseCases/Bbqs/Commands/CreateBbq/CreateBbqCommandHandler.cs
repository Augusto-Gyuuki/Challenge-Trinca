using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Commands.CreateBbq;

public sealed class CreateBbqCommandHandler : IRequestHandler<CreateBbqCommand, ErrorOr<BbqModelResult>>
{
    private readonly IBbqRepository _bbqRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBbqCommandHandler(IBbqRepository bbqRepository, IUnitOfWork unitOfWork)
    {
        _bbqRepository = bbqRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<BbqModelResult>> Handle(CreateBbqCommand request, CancellationToken cancellationToken)
    {
        var bbq = Bbq.Create(
            request.Reason,
            request.Date,
            request.IsTrincaPaying);

        await _bbqRepository.AddAsync(bbq, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BbqModelResult.FromBbq(bbq);
    }
}
