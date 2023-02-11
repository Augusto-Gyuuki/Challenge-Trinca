using ErrorOr;

namespace Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;

public static class BbqErrors
{
    public static Error ReasonMaxLength => Error.Validation("Bbq.ReasonMaxLength", "Reason property is to big");

    public static Error ReasonNullOrWhiteSpace => Error.Validation("Bbq.ReasonNull", "Reason property can't be null or white space");

    public static Error DateInvalid => Error.Validation("Bbq.DateInvalid", "Date can't be earlier than the current date");

    public static Error BbqNotFound => Error.Validation("Bbq.BbqNotFound", "Bbq was not found");

    public static Error GuestNotFound => Error.Validation("Bbq.GuestNotFound", "Guest was not found");

}
