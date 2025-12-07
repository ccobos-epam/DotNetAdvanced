using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Category.RR;

public record CreateCategoryRequest
{
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ParentCategoryId { get; set; }
}

public record CreateCategoryResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }

    public string? ParentCategoryName { get; set; }
    public Guid? ParentCategoryId { get; set; }
}