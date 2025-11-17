using BusinessLayer.Pager;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Product.List;

public record ListProductRequest
{
    public PagerObject PagerValues { get; set; }
}



