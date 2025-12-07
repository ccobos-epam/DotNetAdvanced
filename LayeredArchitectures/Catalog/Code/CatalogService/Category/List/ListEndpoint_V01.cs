using FE = FastEndpoints;
using Utilities.Pagination;
using BusinessLayer.Category.RR;
using BusinessLayer.Category;

namespace CatalogService.Category.List;

public class ListEndpoint_V01 : FE.Endpoint<ListCategoryRequest, ListCategoryResponse>
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
            b.Accepts<ListCategoryRequest>("application/json");
            b.Produces<ListCategoryResponse>(200, "application/json");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Retrieves multiples categories given search and order criteria";
            s.Description = "Queries the database given filters so that we can fecth the data the client needs";
            s.Responses[200] = "a lists of categories";
            object[] response200 = { 
                new PaginatedResult<GetCategoryResponse> { 
                    PageNumber = 1 , 
                    PageSize = 1, 
                    RetrievedItems = 1, 
                    TotalAvailableItems = 2, 
                    ResultItems = [
                        new GetCategoryResponse {
                            Id = Guid.CreateVersion7(),
                            Name = "Category Name",
                            ImageUrl = @"https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fes%2Ffotos%2Famor&psig=AOvVaw0CUb7IMmNazRRlJNHUbZd2&ust=1763487208730000&source=images&cd=vfe&opi=89978449&ved=0CBUQjRxqFwoTCNifhPXb-ZADFQAAAAAdAAAAABAE",
                            ParentCategoryId = Guid.CreateVersion7(),
                            ParentCategoryName = "Parent Category Name"
                        }
                    ] 
                }  
            };
            s.ResponseExamples[200] =  response200;
        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task HandleAsync(ListCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await CategoryService.GetCategoryList(request.PagerValues!);
        var result = new ListCategoryResponse { Results = response };
        await Send.OkAsync(result, cancellationToken);
    }
}
