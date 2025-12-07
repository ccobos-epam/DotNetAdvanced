using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Pagination;

namespace BusinessLayer.Product;

public static class ProductOrdering
{
    //ToDo: move validation logic to fluent validation
    public static readonly string[] PropertyNames = { "Id", "Name", "CategoryName", "Price", "Ammount" };
    public static readonly string[] OrderingValues = { "Asc", "Desc", "asc", "desc" };

    public static Func<IQueryable<ProductEntity>, IQueryable<ProductEntity>> CreateOrderingStrategy(SortingValues sortingValues)
    {
        if (sortingValues.SortingProperties is null || sortingValues.SortingProperties.Count == 0)
        {
            return source => source;
        }

        Func<IQueryable<ProductEntity>, IQueryable<ProductEntity>> result = (IQueryable<ProductEntity> source) =>
        {
            IOrderedQueryable<ProductEntity>? orderedQuery = null;

            foreach (var sortingElement in sortingValues.SortingProperties)
            {
                if (!ProductOrdering.PropertyNames.Contains(sortingElement.PropertyName))
                    continue;

                bool isAscending = sortingElement.Order.Equals("Asc", StringComparison.OrdinalIgnoreCase);

                if (orderedQuery == null)
                    orderedQuery = ApplyFirstSort(source, sortingElement.PropertyName, isAscending);
                else
                    orderedQuery = ApplyNextSort(orderedQuery, sortingElement.PropertyName, isAscending);
            }

            IQueryable<ProductEntity> result = orderedQuery ?? source;
            return result;
        };

        return result;
    }

    private static IOrderedQueryable<ProductEntity> ApplyFirstSort(IQueryable<ProductEntity> query, string propertyName, bool isAscending)
    {
        IOrderedQueryable<ProductEntity> result = (propertyName, isAscending) switch
        {
            ("Id", true) => query.OrderBy(x => x.Id),
            ("Id", false) => query.OrderByDescending(x => x.Id),
            ("Name", true) => query.OrderBy(x => x.Name),
            ("Name", false) => query.OrderByDescending(x => x.Name),
            ("CategoryName", true) => query.OrderBy(x => x.Category.Name),
            ("CategoryName", false) => query.OrderByDescending(x => x.Category.Name),
            ("Price", true) => query.OrderBy(x => x.Price),
            ("Price", false) => query.OrderByDescending(x => x.Price),
            ("Ammount", true) => query.OrderBy(x => x.Ammount),
            ("Ammount", false) => query.OrderByDescending(x => x.Ammount),
            _ => throw new InvalidOperationException($"Sorting not implemented for {propertyName}")
        };
        return result;
    }

    private static IOrderedQueryable<ProductEntity> ApplyNextSort(IOrderedQueryable<ProductEntity> query, string propertyName, bool isAscending)
    {
        IOrderedQueryable<ProductEntity> result = (propertyName, isAscending) switch
        {
            ("Id", true) => query.ThenBy(x => x.Id),
            ("Id", false) => query.ThenByDescending(x => x.Id),
            ("Name", true) => query.ThenBy(x => x.Name),
            ("Name", false) => query.ThenByDescending(x => x.Name),
            ("CategoryName", true) => query.ThenBy(x => x.Category.Name),
            ("CategoryName", false) => query.ThenByDescending(x => x.Category.Name),
            ("Price", true) => query.ThenBy(x => x.Price),
            ("Price", false) => query.ThenByDescending(x => x.Price),
            ("Ammount", true) => query.ThenBy(x => x.Ammount),
            ("Ammount", false) => query.ThenByDescending(x => x.Ammount),
            _ => throw new InvalidOperationException($"Sorting not implemented for {propertyName}")
        };
        return result;
    }
}
