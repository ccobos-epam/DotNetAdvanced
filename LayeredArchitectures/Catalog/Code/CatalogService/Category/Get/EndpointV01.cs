using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;

namespace CatalogService.Category.Get;

public class EndpointV01 : FE.Endpoint<FE.EmptyRequest, GetCategoryResponse>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Get("/categories/{categoryId}");
        Version(1);
        AllowAnonymous();

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<FE.EmptyRequest>("text/plain");
            b.Produces<GetCategoryResponse>(200, "application/json");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Retrieves a single category";
            s.Description = "Queries a single cantegory given the Id of the category";
            s.Responses[200] = "Object retrieved from the database";
            s.ResponseExamples[200] = new GetCategoryResponse
            {
                Id = Guid.CreateVersion7(),
                Name = "Category Name",
                ImageUrl = @"https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fes%2Ffotos%2Famor&psig=AOvVaw0CUb7IMmNazRRlJNHUbZd2&ust=1763487208730000&source=images&cd=vfe&opi=89978449&ved=0CBUQjRxqFwoTCNifhPXb-ZADFQAAAAAdAAAAABAE",
                ParentCategoryId = Guid.CreateVersion7(),
                ParentCategoryName = "Parent Category Name"
            };
            s.Params["categoryId"] = "The Id of the category that we wish to query";
        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken cancellationToken)
    {
        Guid categoryId = Route<Guid>("categoryId");
        var response = await CategoryService.GetCategory(categoryId);
        await Send.OkAsync(response, cancellationToken);
    }

    /*
     * ToDo: Update endpoint to support the case when an Id doesn't found a specific category
     */
}
