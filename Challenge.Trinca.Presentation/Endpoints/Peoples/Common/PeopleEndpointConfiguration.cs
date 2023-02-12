namespace Challenge.Trinca.Presentation.Endpoints.Peoples.Common;

public sealed class PeopleEndpointConfiguration
{
    private const string RoutePrefix = "person";
    private const string InvitePrefix = "invites";
    private const string AcceptPrefix = "accept";
    private const string DeclinePrefix = "decline";

    public const string InviteIdParam = "inviteId";
    public const string PeopleIdHeaderName = "personId";

    public const string GetPeopleInvites = $"{RoutePrefix}/{InvitePrefix}";
    public const string ConfirmInvite = $"{RoutePrefix}/{InvitePrefix}/{{{InviteIdParam}}}/{AcceptPrefix}";
    public const string DeclineInvite = $"{RoutePrefix}/{InvitePrefix}/{{{InviteIdParam}}}/{DeclinePrefix}";
}
