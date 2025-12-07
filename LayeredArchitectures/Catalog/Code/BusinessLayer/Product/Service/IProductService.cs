using BusinessLayer.Product.RR.Create;
using BusinessLayer.Product.RR.Update;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Pagination;
using RR = BusinessLayer.Product.RR;

namespace BusinessLayer.Product.Service;

public interface IProductService
{
    Task<RR.Create.ProductResponse_V01> CreateProduct(RR.Create.ProductRequest_V01 request);
    Task<RR.Get.ProductResponse_V01> GetProduct(Guid productId);
    Task<PaginatedResult<RR.List.ProductResponse_V01>> GetProductList(PagerObject pagerObject);
    Task<bool> DeleteProduct(Guid productId);
    Task<RR.Update.ProductResponse_V01> UpdateProduct(RR.Update.ProductRequest_V01 request);
}
