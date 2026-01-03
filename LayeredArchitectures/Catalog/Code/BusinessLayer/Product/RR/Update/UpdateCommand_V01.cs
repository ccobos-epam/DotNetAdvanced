using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Product.RR.Update;

public record UpdateCommand_V01
{
    public string ProductName { get; set; } = null!;
    public decimal ProductPrice { get; set; }
}
