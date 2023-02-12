using Challenge.Trinca.Application.UseCases.Peoples.Commands.DeclineInvite;
using Challenge.Trinca.Tests.Unit.BaseFixtures;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Peoples.Commands.DeclineInvite;

public static class DeclineInviteCommandFixture
{
    public static DeclineInviteCommand GetValidDeclineInviteCommand()
    {
        return new DeclineInviteCommand
        {
            PeopleId = CommonPeopleFixture.GetPeopleId().ToString(),
            InviteId = CommonPeopleFixture.GetPeopleId().ToString(),
            IsVeg = CommonPeopleFixture.GetRandomBool(),
        };
    }
}
