using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;

namespace CatalogService.Category.Update;

public class EndpointV01 : FE.Endpoint<UpdateCategoryRequest, UpdateCategoryResponse>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Patch("/category/{categoryId}");
        Version(1);
        AllowAnonymous();
        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<UpdateCategoryRequest>("application/json+custom");
            b.Produces<UpdateCategoryResponse>(200, "application/json+custom");
        };
        Description(rhb, true);
        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Update a single category";
            s.Description = "Receives a categoryId and a request object to update an already created category";
            s.RequestExamples
                .Add(new(new { Name = "Name to add", ParentCategoryId = Guid.NewGuid() }, "Basic Example", "Change in name and category", "Send new name to change"));
            s.ResponseExamples[200] = new UpdateCategoryResponse { Id = Guid.NewGuid(), Name = "Name to add", ParentCategoryId = Guid.NewGuid(), ParentCategoryName = "NewCategory" };
            s.Responses[200] = "The modified object";
        };
        Summary(es);
    }

    public override async Task HandleAsync(UpdateCategoryRequest request,  CancellationToken cancellationToken)
    {
        Guid categoryId = Route<Guid>("categoryId");
        UpdateCategoryRequest newRequest = request with { CategoryId = categoryId };
        var response = await CategoryService.UpdateCategory(newRequest);
        await Send.OkAsync(response);
    }
}
