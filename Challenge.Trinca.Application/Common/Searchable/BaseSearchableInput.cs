namespace Challenge.Trinca.Application.Common.Searchable;

public abstract record BaseSearchableInput
{
    public int Page { get; init; }

    public int PerPage { get; init; }
}