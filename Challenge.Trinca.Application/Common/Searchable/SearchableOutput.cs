namespace Challenge.Trinca.Application.Common.Searchable;

public class SearchableOutput<T>
{
    public int CurrentPage { get; set; }

    public int PerPage { get; set; }

    public IReadOnlyList<T>? Items { get; set; }

    public int Total { get; set; }
}
