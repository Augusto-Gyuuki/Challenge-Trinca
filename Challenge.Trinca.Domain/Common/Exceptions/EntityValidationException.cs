namespace Challenge.Trinca.Domain.Common.Exceptions;

public sealed class EntityValidationException : Exception
{
    public EntityValidationException(
        string? message)
        : base(message)
    {

    }
}
