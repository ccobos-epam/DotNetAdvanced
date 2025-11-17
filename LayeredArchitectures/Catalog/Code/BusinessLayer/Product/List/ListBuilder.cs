using BusinessLayer.Pager;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessLayer.Product.List;

public static class ListBuilder
{
    public static IList<ProductEntity> CreateResultList(IList<ProductEntity> unfilteredResults, PagerObject pager)
    {
        IEnumerable<ProductEntity> query = unfilteredResults;

        if (pager.FilteringValues.Filters.Count > 0)
            query = FilterProducts.Filter(unfilteredResults, pager.FilteringValues);

        return [];
    }
}

internal static class FilterProducts
{
    internal static IEnumerable<ProductEntity> Filter(IEnumerable<ProductEntity> unfilteredResults, FilteringValues filteringConditions)
    {
        /*
        Func<ProductEntity, bool> IdPredicate = 
        foreach (var filterCondition in filteringConditions.Filters)
        {
            
            Func<ProductEntity, bool>? stepPredicate =
            filterCondition.PropertyName switch
            {
                "Id" => (ProductEntity element) => element.Id == new Guid(filterCondition.ValueToFilter),
                "Name" => (ProductEntity element) => element.Name.Contains(filterCondition.ValueToFilter),
                "Description" => (ProductEntity element) =>
                {
                    if
                }
                ,
                _ => null
            };
            if(stepPredicate is not null)
                unfilteredResults.Where(stepPredicate);
        }
        */
        return unfilteredResults;
    }
}

internal static class SortProducts
{

}

internal static class PaginateProducts
{

}

public static class MapProducts
{
    public static GetProductResponse MapToResponseModel(ProductEntity x)
    {
        return new GetProductResponse
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            Ammount = x.Ammount,
            Description = x.Description,
            ImageUrl = x.ImageUrl,
            CategoryName = x.Category.Name,
            CategoryId = x.Category.Id,
        };
    }
}