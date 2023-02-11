using Challenge.Trinca.Application.UseCases.Peoples.Commands.ConfirmInvite;
using Challenge.Trinca.Tests.Unit.BaseFixtures;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Peoples.Commands.ConfirmInvite;

public static class ConfirmInviteCommandFixture
{
    public static ConfirmInviteCommand GetValidConfirmInviteCommand()
    {
        return new ConfirmInviteCommand
        {
            PersonId = CommonPeopleFixture.GetPeopleId().ToString(),
            InviteId = CommonPeopleFixture.GetPeopleId().ToString(),
            IsVeg = CommonPeopleFixture.GetRandomBool(),
        };
    }
}
