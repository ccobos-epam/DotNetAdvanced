using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Utilities.Pagination;

namespace BusinessLayer.Product;

public static class ProductFiltering
{
    //ToDo: move validation logic to fluent validation
    //ToDo: Add null exception handling
    public static readonly string[] PropertyNames = { "Id", "Name", "CategoryName", "Price", "Ammount" };
    public static readonly string[] ComparisonValues = { "eq", "ct", "gte", "lte", "gt", "lt"};

    public static List< Expression<Func<ProductEntity, bool>> > SelectFilteringConditions(FilteringValues filteringValues)
    {
        List<Expression<Func<ProductEntity, bool>>> possibleFilters = [];

        if (filteringValues.Filters.Count == 0)
            return possibleFilters;

        foreach(var filteringElement in filteringValues.Filters)
        {
            if (!PropertyNames.Contains(filteringElement.PropertyName))
                throw new ArgumentException("The provided name is not valid for filtering", nameof(filteringElement.PropertyName));
            if (!PropertyNames.Contains(filteringElement.Comparison))
                throw new ArgumentException("The provided name is not valid for filtering", nameof(filteringElement.Comparison));

            Expression<Func<ProductEntity, bool>> expression = (filteringElement.PropertyName, filteringElement.Comparison) switch
            {
                //Property: Id
                ("Id", "eq") => IdEquals(filteringElement.ValueToFilter),
                ("Id", "ct") => IdContains(filteringElement.ValueToFilter),

                //Property: Name
                ("Name", "eq") => NameEquals(filteringElement.ValueToFilter),
                ("Name", "ct") => NameContains(filteringElement.ValueToFilter),

                //Property: CategoryName
                ("CategoryName", "eq") => CategoryNameEquals(filteringElement.ValueToFilter),
                ("CategoryName", "ct") => CategoryNameContains(filteringElement.ValueToFilter),

                //Property: Price
                ("Price", "eq") => PriceEquals(filteringElement.ValueToFilter),
                ("Price", "gt") => PriceGreaterThan(filteringElement.ValueToFilter),
                ("Price", "gte") => PriceGreaterThanEquals(filteringElement.ValueToFilter),
                ("Price", "lt") => PriceLessThan(filteringElement.ValueToFilter),
                ("Price", "lte") => PriceLessThanEquals(filteringElement.ValueToFilter),

                //Property: Ammount
                ("Ammount", "eq") => AmmountEquals(filteringElement.ValueToFilter),
                ("Ammount", "gt") => AmmountGreaterThan(filteringElement.ValueToFilter),
                ("Ammount", "gte") => AmmountGreaterThanEquals(filteringElement.ValueToFilter),
                ("Ammount", "lt") => AmmountLessThan(filteringElement.ValueToFilter),
                ("Ammount", "lte") => AmmountLessThanEquals(filteringElement.ValueToFilter),

                _ => throw new InvalidOperationException($"Logic not implemented for {filteringElement.PropertyName} with {filteringElement.Comparison}")
            };

            possibleFilters.Add(expression);
        }

        return possibleFilters;
    }

    private static Expression<Func<ProductEntity, bool>> IdEquals(string value) => entity => entity.Id.Equals(Guid.Parse(value));
    private static Expression<Func<ProductEntity, bool>> IdContains(string value) => entity => entity.Id.ToString().Contains(value);

    private static Expression<Func<ProductEntity, bool>> NameEquals(string value) => entity => entity.Name.Equals(value, StringComparison.OrdinalIgnoreCase);
    private static Expression<Func<ProductEntity, bool>> NameContains(string value) => entity => entity.Name.Contains(value);

    private static Expression<Func<ProductEntity, bool>> CategoryNameEquals(string value) => entity => entity.Category.Name.Equals(value, StringComparison.OrdinalIgnoreCase);
    private static Expression<Func<ProductEntity, bool>> CategoryNameContains(string value) => entity => entity.Category.Name.Contains(value);

    private static Expression<Func<ProductEntity, bool>> PriceEquals(string value) => entity => entity.Price == decimal.Parse(value);
    private static Expression<Func<ProductEntity, bool>> PriceGreaterThan(string value) => entity => entity.Price > decimal.Parse(value);
    private static Expression<Func<ProductEntity, bool>> PriceGreaterThanEquals(string value) => entity => entity.Price >= decimal.Parse(value);
    private static Expression<Func<ProductEntity, bool>> PriceLessThan(string value) => entity => entity.Price < decimal.Parse(value);
    private static Expression<Func<ProductEntity, bool>> PriceLessThanEquals(string value) => entity => entity.Price < decimal.Parse(value);

    private static Expression<Func<ProductEntity, bool>> AmmountEquals(string value) => entity => entity.Ammount == decimal.Parse(value);
    private static Expression<Func<ProductEntity, bool>> AmmountGreaterThan(string value) => entity => entity.Ammount > decimal.Parse(value);
    private static Expression<Func<ProductEntity, bool>> AmmountGreaterThanEquals(string value) => entity => entity.Ammount >= decimal.Parse(value);
    private static Expression<Func<ProductEntity, bool>> AmmountLessThan(string value) => entity => entity.Ammount < decimal.Parse(value);
    private static Expression<Func<ProductEntity, bool>> AmmountLessThanEquals(string value) => entity => entity.Ammount < decimal.Parse(value);
}
