using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;
using FastEndpoints;

namespace CatalogService.Category.Create;

public class EndpointV01 : FE.Endpoint<CreateCategoryRequest, CreateCategoryResponse>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Post("/categories");
        Version(1);
        
        AllowAnonymous();

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<CreateCategoryRequest>("application/json+custom");
            b.Produces<CreateCategoryResponse>(201, "application/json+custom");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Create a single category";
            s.Description = "Create a new category and return the mapping values to the frontend";
            s.RequestExamples
                .Add( new FE.RequestExample (
                    new CreateCategoryRequest 
                    { 
                        Name = "New Category Name", 
                        ParentCategoryId = Guid.NewGuid(),
                        ImageUrl = @"https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fes%2Ffotos%2Famor&psig=AOvVaw0CUb7IMmNazRRlJNHUbZd2&ust=1763487208730000&source=images&cd=vfe&opi=89978449&ved=0CBUQjRxqFwoTCNifhPXb-ZADFQAAAAAdAAAAABAE"
                    }, 
                    "Simple Example", 
                    "New category creation", 
                    "Send the values ")
                );
            s.ResponseExamples[201] = new CreateCategoryResponse
            { 
                Id = Guid.NewGuid(), 
                Name = "Category Name",
                ImageUrl = @"https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fes%2Ffotos%2Famor&psig=AOvVaw0CUb7IMmNazRRlJNHUbZd2&ust=1763487208730000&source=images&cd=vfe&opi=89978449&ved=0CBUQjRxqFwoTCNifhPXb-ZADFQAAAAAdAAAAABAE",
                ParentCategoryId = Guid.NewGuid(), 
                ParentCategoryName = "Parent Category" 
            };
            s.Responses[200] = "The newly created object";
        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task HandleAsync(CreateCategoryRequest request,  CancellationToken cancellationToken)
    {
        var response = await CategoryService.CreateCategory(request);
        await Send.CreatedAtAsync<GetEndpoint>(response.Id,response, FE.Http.GET, null, false, default);
    }
}
