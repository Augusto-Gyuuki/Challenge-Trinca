using Challenge.Trinca.Application.UseCases.Bbqs.Commands.CreateBbq;
using Challenge.Trinca.Tests.Unit.BaseFixtures;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Commands.CreateBbq;

public static class CreateBbqCommandFixture
{
    public static CreateBbqCommand GetCreateBbqCommand()
    {
        var bbqReason = CommonBbqFixture.GetValidReason();
        var bbqDate = CommonBbqFixture.GetValidDate();
        var bbqIsTrincaPaying = CommonBbqFixture.GetIsTrincaPaying();

        return new CreateBbqCommand
        {
            Reason = bbqReason,
            Date = bbqDate,
            IsTrincaPaying = bbqIsTrincaPaying,
        };
    }

}
