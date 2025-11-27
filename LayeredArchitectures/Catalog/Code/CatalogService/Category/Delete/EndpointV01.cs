using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;

namespace CatalogService.Category.Delete;

public class EndpointV01 : FE.Endpoint<FE.EmptyRequest, FE.EmptyResponse>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Delete("/categories/{categoryId}");
        Version(1);

        AllowAnonymous();

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<FE.EmptyRequest>("text/plain");
            b.Produces<FE.EmptyResponse>(204, "text/plain");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Deletes a single category";
            s.Description = "Delete a single cantegory given the Id of the category";
            s.Responses[204] = "Confirmation of the object deleted";
            s.Responses[404] = "The identifier doesn't found any valid category";
            s.Params["categoryId"] = "The Id of the category that we wish to delete";
        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken cancellationToken)
    {
        Guid categoryId = Route<Guid>("categoryId");
        var response = await CategoryService.DeleteCategory(categoryId);
        if (response)
            await Send.NoContentAsync(cancellationToken);
        else 
            await Send.NotFoundAsync(cancellationToken);
    }
}
