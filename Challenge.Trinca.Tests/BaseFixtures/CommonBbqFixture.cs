using Bogus;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects;

namespace Challenge.Trinca.Tests.Unit.BaseFixtures;

public static class CommonBbqFixture
{
    private readonly static Faker faker = new("pt_BR");

    private readonly static int ONE_LENGTH_STRING = 1;

    public static Bbq GetBbq()
    {
        var bbqReason = GetValidReason();
        var bbqDate = GetValidDate();
        var bbqIsTrincaPaying = GetIsTrincaPaying();

        return Bbq.Create(bbqReason, bbqDate, bbqIsTrincaPaying);
    }

    public static Guest GetGuest()
    {
        var guestId = Guid.NewGuid();
        var guestIsVegetarian = faker.Random.Bool();

        return Guest.Create(guestId, guestIsVegetarian);
    }

    public static string GetValidReason()
    {
        return faker.Random.String(ONE_LENGTH_STRING, Bbq.REASON_MAX_LENGTH);
    }

    public static DateTime GetValidDate()
    {
        return faker.Date.Future();
    }

    public static string GetInvalidMaxLengthReason()
    {
        return faker.Random.String(Bbq.REASON_MAX_LENGTH + 1);
    }

    public static string GetNullOrWhiteSpaceReason()
    {
        var options = new List<string>()
        {
            string.Empty,
            null,
        };
        return faker.PickRandom(options);
    }

    public static DateTime GetInvalidDate()
    {
        return faker.Date.Past();
    }

    public static bool GetIsTrincaPaying()
    {
        return faker.Random.Bool();
    }
}
