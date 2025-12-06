using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Pagination;

namespace BusinessLayer.Category;

public static class CategoryOrdering
{
    //ToDo: move validation logic to fluent validation
    public static readonly string[] PropertyNames = { "Id", "Name", "ParentCategoryName", "ParentCategoryId" };
    public static readonly string[] OrderingValues = { "Asc", "Desc", "asc", "desc" };

    public static Func< IQueryable<CategoryEntity>, IQueryable<CategoryEntity> > CreateOrderingStrategy(SortingValues sortingValues)
    {
        // 1. If no sorting, return an "Identity" function (do nothing)
        if (sortingValues.SortingProperties is null || sortingValues.SortingProperties.Count == 0)
        {
            return source => source;
        }

        // 2. Return a closure that captures the logic
        Func<IQueryable<CategoryEntity>, IQueryable<CategoryEntity>> result = (IQueryable<CategoryEntity> source) =>
        {
            IOrderedQueryable<CategoryEntity>? orderedQuery = null;

            foreach (var sortingElement in sortingValues.SortingProperties) 
            {
                if (!CategoryFiltering.PropertyNames.Contains(sortingElement.PropertyName)) 
                    continue;

                bool isAscending = sortingElement.Order.Equals("Asc", StringComparison.OrdinalIgnoreCase);

                if (orderedQuery == null)
                    orderedQuery = ApplyFirstSort(source, sortingElement.PropertyName, isAscending);
                else
                    orderedQuery = ApplyNextSort(orderedQuery, sortingElement.PropertyName, isAscending);
            }

            IQueryable<CategoryEntity> result = orderedQuery ?? source;
            return result;
        };

        return result;
    }

    private static IOrderedQueryable<CategoryEntity> ApplyFirstSort(IQueryable<CategoryEntity> query, string propertyName, bool isAscending)
    {
        IOrderedQueryable<CategoryEntity> result = (propertyName, isAscending) switch
        {
            ("Id", true) => query.OrderBy(x => x.Id),
            ("Id", false) => query.OrderByDescending(x => x.Id),
            ("Name", true) => query.OrderBy(x => x.Name),
            ("Name", false) => query.OrderByDescending(x => x.Name),
            ("ParentCategoryId", true) => query.OrderBy(x => x.ParentCategoryId),
            ("ParentCategoryId", false) => query.OrderByDescending(x => x.ParentCategoryId),
            ("ParentCategoryName", true) => query.OrderBy(x => x.ParentCategory.Name),
            ("ParentCategoryName", false) => query.OrderByDescending(x => x.ParentCategory.Name),
            _ => throw new InvalidOperationException($"Sorting not implemented for {propertyName}")
        };
        return result;
    }

    private static IOrderedQueryable<CategoryEntity> ApplyNextSort(IOrderedQueryable<CategoryEntity> query, string propertyName, bool isAscending)
    {
        IOrderedQueryable<CategoryEntity> result = (propertyName, isAscending) switch
        {
            ("Id", true) => query.ThenBy(x => x.Id),
            ("Id", false) => query.ThenByDescending(x => x.Id),
            ("Name", true) => query.ThenBy(x => x.Name),
            ("Name", false) => query.ThenByDescending(x => x.Name),
            ("ParentCategoryId", true) => query.ThenBy(x => x.ParentCategoryId),
            ("ParentCategoryId", false) => query.ThenByDescending(x => x.ParentCategoryId),
            ("ParentCategoryName", true) => query.ThenBy(x => x.ParentCategory.Name),
            ("ParentCategoryName", false) => query.ThenByDescending(x => x.ParentCategory.Name),
            _ => throw new InvalidOperationException($"Sorting not implemented for {propertyName}")
        };
        return result;
    }
}
