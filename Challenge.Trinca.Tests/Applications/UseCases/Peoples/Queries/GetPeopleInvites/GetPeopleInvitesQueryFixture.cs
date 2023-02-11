using Challenge.Trinca.Application.UseCases.Peoples.Queries.GetPeopleInvites;
using Challenge.Trinca.Tests.Unit.BaseFixtures;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Peoples.Queries.GetPeopleInvites;

public static class GetPeopleInvitesQueryFixture
{
    public static GetPeopleInvitesQuery GetValidGetPeopleInvitesQuery()
    {
        return new GetPeopleInvitesQuery
        {
            PersonId = CommonPeopleFixture.GetPeopleId().ToString()
        };
    }
}
