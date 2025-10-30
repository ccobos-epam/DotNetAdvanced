using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Product;

public record CreateProductRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Ammount { get; set; }
    public Guid CategoryId { get; set; }

}

public record CreateProductResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Ammount { get; set; }

    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
}
