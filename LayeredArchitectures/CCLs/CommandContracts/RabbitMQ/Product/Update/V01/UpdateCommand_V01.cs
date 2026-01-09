using System;
using System.Collections.Generic;
using System.Text;

namespace CommandContracts.RabbitMQ.Product.Update.V01;

public record UpdateCommand_V01
{
    public string ProductName { get; set; } = null!;
    public decimal ProductPrice { get; set; }
}
