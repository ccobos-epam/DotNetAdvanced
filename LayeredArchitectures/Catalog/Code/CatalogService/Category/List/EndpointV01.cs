using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;
using BusinessLayer.Product.List;
using CatalogService.Utils;

namespace CatalogService.Category.List;

public class EndpointV01 : FE.Endpoint<ListProductRequest, PaginatedResult<GetCategoryResponse>>
{

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public ICategoryService CategoryService { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public override void Configure()
    {
        Put(@"/categories");
        Version(1);
        AllowAnonymous();

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<ListProductRequest>("application/json");
            b.Produces<PaginatedResult<GetCategoryResponse>>(200, "application/json");
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

    public override async Task HandleAsync(ListProductRequest request, CancellationToken cancellationToken)
    {
        var response = await CategoryService.GetCategoryList();
        await Send.OkAsync(response, cancellationToken);
    }
}
