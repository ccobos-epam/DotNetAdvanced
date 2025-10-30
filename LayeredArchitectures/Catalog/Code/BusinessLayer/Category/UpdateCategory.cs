using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Category;

public record UpdateCategoryRequest : CreateCategoryRequest 
{
    public Guid CategoryId { get; set; }
}

public record UpdateCategoryResponse : CreateCategoryResponse { }