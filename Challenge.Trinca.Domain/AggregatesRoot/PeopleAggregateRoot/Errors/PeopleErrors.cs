using ErrorOr;

namespace Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;

public sealed class PeopleErrors
{
    public static Error NameMaxLength => Error.Validation("People.NameMaxLength", "Name property is to big");

    public static Error NameNullOrWhiteSpace => Error.Validation("People.NameNullOrWhiteSpace", "Name property can't be null or white space");

    public static Error PeopleNotFound => Error.Validation("People.PeopleNotFound", "People was not found");

    public static Error InviteNotFound => Error.Validation("People.InviteNotFound", "This person do not have a invite with this ID");

    public static Error NotAuthorized => Error.Validation("People.NotAuthorized", "This person do not have permission to access this data");
}
