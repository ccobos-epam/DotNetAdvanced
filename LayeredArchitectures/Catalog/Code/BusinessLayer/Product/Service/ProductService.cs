using DataAccess;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using RR = BusinessLayer.Product.RR;

using FilteredProduct = System.Func<DomainEntities.ProductEntity, bool>;
using System.ComponentModel;
using Utilities.Pagination;
using BusinessLayer.Product.RR.Get;
using BusinessLayer.Product.RR.Update;
using BusinessLayer.Product.RR.Create;
namespace BusinessLayer.Product.Service;



public class ProductService : IProductService
{
    private readonly IProductRepository repository;

    public ProductService(IProductRepository repository)
    {
        this.repository = repository;
    }

    public async Task<RR.Create.ProductResponse_V01> CreateProduct(RR.Create.ProductRequest_V01 request)
    {
        ProductEntity product = new ProductEntity()
        {
            Id = Guid.CreateVersion7(),
            Ammount = request.Ammount,
            CategoryId = request.CategoryId,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Name = request.Name,
            Price = request.Price,
        };
        await repository.AddSingle(product);
        var createdProduct = await repository.GetSingle(product.Id);
        var response = new RR.Create.ProductResponse_V01()
        {
            Name = createdProduct!.Name,
            Price = createdProduct.Price,
            Ammount = createdProduct.Ammount,
            CategoryId= createdProduct.CategoryId,
            Description = createdProduct.Description,
            ImageUrl = createdProduct.ImageUrl,
            CategoryName = createdProduct.Category.Name,
            Id = createdProduct.Id,
        };
        return response;
    }

    public async Task<RR.Get.ProductResponse_V01> GetProduct(Guid productId)
    {
        var product = await repository.GetSingle(productId);
        RR.Get.ProductResponse_V01 response = new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Ammount= product.Ammount,
            CategoryName = product.Category.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            CategoryId = product.Category.Id,
        };
        return response;
    }

    public async Task<PaginatedResult<RR.List.ProductResponse_V01>> GetProductList(PagerObject pagerObject)
    {
        var filteringConditions = ProductFiltering.SelectFilteringConditions(pagerObject.FilteringValues);
        var orderingconditions = ProductOrdering.CreateOrderingStrategy(pagerObject.SortingValues);
        var paginationConditions = GeneralPagination.CreatePagination<ProductEntity>(pagerObject.PaginationValues);

        PaginatedResult<ProductEntity> productList = await repository.List(filteringConditions, orderingconditions, paginationConditions);

        PaginatedResult<RR.List.ProductResponse_V01> result = new()
        {
            PageNumber = pagerObject.PaginationValues.PageIndex,
            PageSize = pagerObject.PaginationValues.RecordsPerPage,
            RetrievedItems = productList.RetrievedItems,
            TotalAvailableItems = productList.TotalAvailableItems,
            ResultItems = [.. productList.ResultItems.Select(x => new RR.List.ProductResponse_V01
            {
                Ammount = x.Ammount,
                CategoryId = x.Category.Id,
                CategoryName = x.Category.Name,
                Description = x.Description,
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                Price = x.Price,
            })]
        };
        return result;
    }

    public async Task<bool> DeleteProduct(Guid productId) { 
        bool result = await repository.DeleteSingle(productId);
        return result;
    }

    public async Task<RR.Update.ProductResponse_V01> UpdateProduct(RR.Update.ProductRequest_V01 request)
    {
        var productToModify = await repository.GetSingle(request.Id);
        productToModify.Ammount = request.Ammount;
        productToModify.Description = request.Description;
        productToModify.ImageUrl = request.ImageUrl;
        productToModify.Name = request.Name;
        productToModify.Price = request.Price;
        productToModify.CategoryId = request.CategoryId;
        await repository.UpdateSingle(productToModify);
        var modifiedProduct = await repository.GetSingle(request.Id);
        RR.Update.ProductResponse_V01 response = new RR.Update.ProductResponse_V01()
        {
            Id = modifiedProduct.Id,
            Ammount = modifiedProduct.Ammount,
            Description = modifiedProduct.Description,
            Name = modifiedProduct.Name,
            Price = modifiedProduct.Price,
            ImageUrl = modifiedProduct.ImageUrl,
            CategoryId = modifiedProduct.CategoryId,
            CategoryName = modifiedProduct.Category.Name
        };

        var command = new RR.Update.UpdateCommand_V01()
        {
            ProductName = response.Name,
            ProductPrice = response.Price
        };

        return response;
    }

    public static List<Expression<FilteredProduct>> GenerateFilteringConditions(List<FilteringElement> filters)
    {
        var result = new List<Expression<FilteredProduct>>();

        ParameterExpression parameter = Expression.Parameter(typeof(FilteringElement), "product");

        foreach (var filter in filters)
        {
            MemberExpression property = Expression.Property(parameter, filter.PropertyName);
            var (constant, propertyType) = GetTypedValue(property, filter.ValueToFilter);
        }

        return result;
    }

    public static (ConstantExpression, Type) GetTypedValue(MemberExpression property, string value)
    {
        Type propertyType = property.Type;
        Type underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        object? typedValue;
        if (value == null || value.Equals("null", StringComparison.OrdinalIgnoreCase))
            typedValue = null;
        else
        {
            TypeConverter converter = TypeDescriptor.GetConverter(underlyingType);
            typedValue = converter.ConvertFromInvariantString(value);
        }

        ConstantExpression constant = Expression.Constant(typedValue, propertyType);
        return (constant, propertyType);
    }

    public static Expression BuildComparison(MemberExpression property, string operation, ConstantExpression constant)
    {
        return operation.ToLowerInvariant() switch
        {
            "equals" or "==" or "eq" => Expression.Equal(property, constant),
            "notequals" or "!=" => Expression.NotEqual(property, constant),
            "greaterthan" or ">" => Expression.GreaterThan(property, constant),
            "greaterthanorequal" or ">=" => Expression.GreaterThanOrEqual(property, constant),
            "lessthan" or "<" => Expression.LessThan(property, constant),
            "lessthanorequal" or "<=" => Expression.LessThanOrEqual(property, constant),

            // --- String Operations ---

            "contains" => BuildStringMethodCall(property, "Contains", constant),
            "startswith" => BuildStringMethodCall(property, "StartsWith", constant),
            "endswith" => BuildStringMethodCall(property, "EndsWith", constant),

            _ => throw new NotSupportedException($"Operation '{operation}' is not supported.")
        };
    }

    private static Expression BuildStringMethodCall(MemberExpression property, string methodName, ConstantExpression constant)
    {
        if (property.Type != typeof(string))
        {
            throw new InvalidOperationException($"{methodName} is only supported for string properties.");
        }

        var method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
        if (method == null)
        {
            throw new MissingMethodException($"Could not find method '{methodName}' on type 'string'.");
        }

        // Creates the call: property.Contains(value)
        return Expression.Call(property, method, constant);
    }
}
