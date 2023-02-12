﻿using Challenge.Trinca.Application.UseCases.Peoples.Commands.ConfirmInvite;
using Challenge.Trinca.Application.UseCases.Peoples.Commands.DeclineInvite;
using Challenge.Trinca.Presentation.Endpoints.Peoples.Common.Requests;
using Mapster;

namespace Challenge.Trinca.Presentation.Endpoints.Peoples.Common.MappingConfig;

public sealed class InviteMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(string peopleId, string inviteId, InviteRequest request), ConfirmInviteCommand>()
            .Map(dest => dest.IsVeg, src => src.request.IsVeg)
            .Map(dest => dest.PeopleId, src => src.peopleId)
            .Map(dest => dest.InviteId, src => src.inviteId);

        config.NewConfig<(string peopleId, string inviteId, InviteRequest request), DeclineInviteCommand>()
            .Map(dest => dest.IsVeg, src => src.request.IsVeg)
            .Map(dest => dest.PeopleId, src => src.peopleId)
            .Map(dest => dest.InviteId, src => src.inviteId);
    }
}
