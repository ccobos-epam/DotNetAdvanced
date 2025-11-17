using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Pager;

public record SortingValues
{
    public List<string> SortingProperties { get; set; } = [];
}
