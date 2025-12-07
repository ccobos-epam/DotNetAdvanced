using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Product.RR.List;

public record ProductResponse_V01
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Ammount { get; set; }
    public string CategoryName { get; set; } = null!;
    public Guid CategoryId { get; set; }
}
