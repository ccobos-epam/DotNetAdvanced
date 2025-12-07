using Utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Category.RR;

public record ListCategoryRequest
{
    public PagerObject? PagerValues { get; set; } = default;
}

public record ListCategoryResponse
{
    public required PaginatedResult<GetCategoryResponse> Results { get; set; }
}



