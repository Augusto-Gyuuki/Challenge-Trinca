using System.ComponentModel;

namespace Challenge.Trinca.Application.Common.Searchable;

public enum SearchOrder
{
    [Description("asc")]
    Ascending,

    [Description("des")]
    Descending,
}
