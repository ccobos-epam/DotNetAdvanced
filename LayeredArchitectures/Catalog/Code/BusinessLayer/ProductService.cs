using BusinessLayer.Product;
using DataAccess;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer;

public interface IProductService
{
    Task<CreateProductResponse> CreateProduct(CreateProductRequest request);
    Task<GetProductResponse> GetProduct(Guid productId);
    Task<IList<GetProductResponse>> GetProductList();
    Task<bool> DeleteProduct(Guid productId);
    Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request);
}

public class ProductService : IProductService
{
    private readonly IProductRepository repository;

    public ProductService(IProductRepository repository)
    {
        this.repository = repository;
    }

    public async Task<CreateProductResponse> CreateProduct(CreateProductRequest request)
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
        var response = new CreateProductResponse()
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

    public async Task<GetProductResponse> GetProduct(Guid productId)
    {
        var product = await repository.GetSingle(productId);
        GetProductResponse response = new()
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

    public async Task<IList<GetProductResponse>> GetProductList()
    {
        var productList = await repository.List();
        var responseList = productList.Select(x => new GetProductResponse()
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            Ammount = x.Ammount,
            Description = x.Description,
            ImageUrl = x.ImageUrl,
            CategoryName = x.Category.Name,
            CategoryId = x.Category.Id,
        }).ToList();
        return responseList;
    }

    public async Task<bool> DeleteProduct(Guid productId) { 
        bool result = await repository.DeleteSingle(productId);
        return result;
    }

    public async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request)
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
        UpdateProductResponse response = new UpdateProductResponse()
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
        return response;
    }
}
