using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Pager;

public record FilteringValues 
{
    public List<FilteringElement> Filters { get; set; } = [];
}

public record FilteringElement
{
    public string PropertyName { get; set; } = string.Empty;
    public string Comparison {  get; set; } = string.Empty;
    public string ValueToFilter { get; set; } = string.Empty;
}