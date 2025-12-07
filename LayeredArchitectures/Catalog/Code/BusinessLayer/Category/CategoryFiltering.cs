using Azure.Core.Serialization;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Pagination;
using System.Linq.Expressions;

namespace BusinessLayer.Category;

public static class CategoryFiltering
{
    //ToDo: move validation logic to fluent validation
    //ToDo: Add null exception handling
    public static readonly string[] PropertyNames = { "Id", "Name", "ParentCategoryName", "ParentCategoryId" };
    public static readonly string[] ComparisonValues = { "Equals", "eq", "Contains", "ct" };

    public static List< Expression<Func<CategoryEntity, bool>> > SelectFilteringConditions(FilteringValues filteringValues)
    {
        List<Expression<Func<CategoryEntity, bool>> > possibleFilters = [];

        if (filteringValues.Filters.Count == 0)
            return possibleFilters;

        foreach(var filteringElement in filteringValues.Filters)
        {
            if (!PropertyNames.Contains(filteringElement.PropertyName))
                throw new ArgumentException("The provided name is not valid for filtering", nameof(filteringElement.PropertyName));
            if (!PropertyNames.Contains(filteringElement.Comparison))
                throw new ArgumentException("The provided name is not valid for filtering", nameof(filteringElement.Comparison));

            Expression<Func<CategoryEntity, bool>> expression = (filteringElement.PropertyName, filteringElement.Comparison) switch
            {
                //Id Rules
                ("Id", "Equals" or "eq") => IdEquals(filteringElement.ValueToFilter),
                ("Id", "Contains" or "ct") => IdContains(filteringElement.ValueToFilter),

                //Name Rules
                ("Name", "Equals" or "eq") => NameEquals(filteringElement.ValueToFilter),
                ("Name", "Contains" or "ct") => NameContains(filteringElement.ValueToFilter),

                //ParentCategoryName Rules
                ("ParentCategoryName", "Equals" or "eq") => ParentCategoryIdEquals(filteringElement.ValueToFilter),
                ("ParentCategoryName", "Contains" or "ct") => ParentCategoryIdContains(filteringElement.ValueToFilter),

                //ParentCategoryId Rules
                ("ParentCategoryId", "Equals" or "eq") => ParentCategoryNameEquals(filteringElement.ValueToFilter),
                ("ParentCategoryId", "Contains" or "ct") => ParentCategoryNameContains(filteringElement.ValueToFilter),

                _ => throw new InvalidOperationException($"Logic not implemented for {filteringElement.PropertyName} with {filteringElement.Comparison}")
            };

            possibleFilters.Add(expression);
        }

        return possibleFilters;
    }

    private static Expression<Func<CategoryEntity, bool>> IdEquals(string value)
    { 
        /*
        bool succesfullConversion = Guid.TryParse(value, out Guid valueToCompare);
        if(!succesfullConversion)
        */
        Guid valueToCompare = Guid.Parse(value);
        Expression<Func<CategoryEntity, bool>> func = (CategoryEntity entity) => entity.Id.Equals(valueToCompare);
        return func;
    }

    private static Expression<Func<CategoryEntity, bool>> IdContains(string value)
    {
        Expression<Func<CategoryEntity, bool>> func = (CategoryEntity entity) => entity.Id.ToString().Contains(value);
        return func;
    }

    private static Expression<Func<CategoryEntity, bool>> NameEquals(string value)
    {
        Expression<Func<CategoryEntity, bool>> func = (CategoryEntity entity) => entity.Name.Equals(value);
        return func;
    }

    private static Expression<Func<CategoryEntity, bool>> NameContains(string value)
    {
        Expression<Func<CategoryEntity, bool>> func = (CategoryEntity entity) => entity.Name.Contains(value);
        return func;
    }

    private static Expression<Func<CategoryEntity, bool>> ParentCategoryIdEquals(string value)
    {
        if (!Guid.TryParse(value, out Guid valueToCompare))
            return entity => false;

        Expression<Func<CategoryEntity, bool>> func = (CategoryEntity entity) => entity.ParentCategoryId.HasValue  && entity.ParentCategoryId.Value == valueToCompare;
        return func;
    }

    private static Expression<Func<CategoryEntity, bool>> ParentCategoryIdContains(string value)
    {
        Expression<Func<CategoryEntity, bool>> func = (CategoryEntity entity) => entity.ParentCategoryId.HasValue  && entity.ParentCategoryId.Value.ToString().Contains(value);
        return func;
    }

    private static Expression<Func<CategoryEntity, bool>> ParentCategoryNameEquals(string value)
    {
        Expression<Func<CategoryEntity, bool>> func = (CategoryEntity entity) => entity.ParentCategoryId.HasValue  && entity.ParentCategory!.Name == value;
        return func;
    }

    private static Expression<Func<CategoryEntity, bool>> ParentCategoryNameContains(string value)
    {
        Expression<Func<CategoryEntity, bool>> func = (CategoryEntity entity) => entity.ParentCategoryId.HasValue  && entity.ParentCategory!.Name.Contains(value);
        return func;
    }

}
