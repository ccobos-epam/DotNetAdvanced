using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;

namespace CatalogService.Category;

public class CreateEnpoint : FE.Endpoint<CreateCategoryRequest, CreateCategoryResponse>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Post("/category");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateCategoryRequest request,  CancellationToken cancellationToken)
    {
        var response = await CategoryService.CreateCategory(request);
        await Send.CreatedAtAsync<GetEndpoint>(response.Id,response, FE.Http.GET, null, false, default);
    }
}
