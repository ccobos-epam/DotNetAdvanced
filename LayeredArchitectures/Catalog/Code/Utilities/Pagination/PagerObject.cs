using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Pagination;

public record PagerObject
{
    public SortingValues SortingValues { get; set; } = new SortingValues();
    public PaginationValues PaginationValues { get; set; } = new PaginationValues();
    public FilteringValues FilteringValues { get; set; } = new FilteringValues();
}
