﻿using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Peoples.Common.Results;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Peoples.Commands.ConfirmInvite;

public sealed class ConfirmInviteCommandHandler : IRequestHandler<ConfirmInviteCommand, ErrorOr<InviteModelResult>>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmInviteCommandHandler(IPeopleRepository peopleRepository, IUnitOfWork unitOfWork)
    {
        _peopleRepository = peopleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<InviteModelResult>> Handle(ConfirmInviteCommand request, CancellationToken cancellationToken)
    {
        var people = await _peopleRepository.GetByIdAsync(
            Guid.Parse(request.PersonId),
            cancellationToken);

        if (people is null)
        {
            return PeopleErrors.PeopleNotFound;
        }

        var invite = people.Invites
            .FirstOrDefault(x => x.Id.Equals(Guid.Parse(request.InviteId)));

        if (invite is null)
        {
            return PeopleErrors.InviteNotFound;
        }

        if (!invite.Status.Equals(InviteStatus.Accepted))
        {
            people.AcceptInvite(invite, request.IsVeg);
        }

        await _peopleRepository.UpdateAsync(people);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return InviteModelResult.FromInvite(invite);
    }
}
