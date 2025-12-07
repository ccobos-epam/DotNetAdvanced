using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Pagination;

public record PaginationValues
{
    public int PageIndex { get; set; }
    public int RecordsPerPage { get; set; }
}
