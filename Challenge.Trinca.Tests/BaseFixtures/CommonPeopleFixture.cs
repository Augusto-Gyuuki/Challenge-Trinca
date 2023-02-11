using Bogus;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;

namespace Challenge.Trinca.Tests.Unit.BaseFixtures;

public sealed class CommonPeopleFixture
{
    private readonly static Faker faker = new("pt_BR");

    public static People GetPeople()
    {
        var peopleName = GetValidPeopleName();
        var peopleIsCoOwner = false;
        return People.Create(peopleName, peopleIsCoOwner);
    }

    public static People GetCoOwnerPeople()
    {
        var peopleName = GetValidPeopleName();
        var peopleIsCoOwner = true;
        return People.Create(peopleName, peopleIsCoOwner);
    }

    public static Invite GetInvite()
    {
        var bbqId = Guid.NewGuid();
        return Invite.Create(bbqId);
    }

    public static IEnumerable<People> GetRandomPeopleList()
    {
        yield return faker.PickRandom(GetCoOwnerPeople(), GetPeople());
    }

    public static string GetInvalidPeopleName()
    {
        return faker.Random.String((People.NAME_MAX_LENGTH + 1));
    }

    public static string GetValidPeopleName()
    {
        return faker.Person.FullName;
    }

    public static string GetNullOrWhiteSpacePeopleName()
    {
        var options = new List<string>()
        {
            string.Empty,
            null,
        };
        return faker.PickRandom(options);
    }

    public static bool GetRandomBool()
    {
        return faker.Random.Bool();
    }

    public static Guid GetPeopleId()
    {
        return Guid.NewGuid();
    }
}
