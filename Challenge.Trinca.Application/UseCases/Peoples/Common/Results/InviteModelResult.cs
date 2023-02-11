using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;

namespace Challenge.Trinca.Application.UseCases.Peoples.Common.Results;

public sealed record InviteModelResult
{
    public Guid Id { get; private set; }

    public Guid BbqId { get; private set; }

    public string Status { get; private set; }

    public DateTime Date { get; private set; }

    private InviteModelResult(Guid id, Guid bbqId, string status, DateTime date)
    {
        Id = id;
        BbqId = bbqId;
        Status = status;
        Date = date;
    }

    public static InviteModelResult FromInvite(Invite invite)
    {
        return new(
            invite.Id,
            invite.BbqId,
            invite.Status.Name,
            invite.Date);
    }
}
