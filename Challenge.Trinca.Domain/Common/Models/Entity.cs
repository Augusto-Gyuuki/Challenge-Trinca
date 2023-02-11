using System.Diagnostics.CodeAnalysis;

namespace Challenge.Trinca.Domain.Common.Models;

[ExcludeFromCodeCoverage]
public abstract class Entity : IEquatable<Entity>
{
    public DateTime CreatedDateTime { get; protected set; }

    public DateTime UpdatedDateTime { get; protected set; }

    public bool Equals(Entity? obj)
    {
        return Equals((object?)obj);
    }

    public static bool operator ==(Entity left, Entity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !Equals(left, right);
    }
}
