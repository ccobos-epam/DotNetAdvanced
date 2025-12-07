using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Pagination;

public record SortingValues
{
    public List<SortingElement> SortingProperties { get; set; } = [];
}

public record SortingElement
{
    public string PropertyName { get; set; } = string.Empty;
    public string Order { get; set; } = string.Empty;
}