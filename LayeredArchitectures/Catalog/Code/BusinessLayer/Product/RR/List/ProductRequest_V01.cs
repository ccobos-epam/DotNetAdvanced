using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Pagination;

namespace BusinessLayer.Product.RR.List;

public record ProductRequest_V01
{
    public PagerObject? PagerValues { get; set; } = default;
}
