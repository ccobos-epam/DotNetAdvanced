using System;
using System.Collections.Generic;
using System.Text;

namespace DomainEntities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Ammount { 
        get; 
        set {
            if (value < 0)
                throw new ArgumentException("The value must be positive", nameof(Ammount));
            else
                field = value;
        }
    }


    public CategoryEntity Category { get; set; } = null!;
    public Guid CategoryId { get; set; }
}
