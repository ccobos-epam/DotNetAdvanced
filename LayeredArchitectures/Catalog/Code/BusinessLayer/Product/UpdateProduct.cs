using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Product;

public record UpdateProductRequest : CreateProductRequest
{
    public Guid Id { get; set; }
}

public record UpdateProductResponse : CreateProductResponse { }
