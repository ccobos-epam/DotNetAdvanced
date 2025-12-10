using CartService.DataStorage;
using CartService.Entities;
using CartService.UseCases;
using FastEndpoints;
using FastEndpoints.Swagger;
using LiteDB;
using NSwag.AspNetCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddScoped<LiteDatabase>(sp =>
{
    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
    string connectionString = configuration.GetConnectionString("LiteDb")!;
    return new LiteDatabase(connectionString);
});
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o => 
    {
        o.MaxEndpointVersion = 1;
        o.MinEndpointVersion = 1;
        o.FlattenSchema = false;
        o.ShortSchemaNames = false;
        o.DocumentSettings = s =>
        {
            s.DocumentName = "v1";
            s.Title = "Cart API V1";
            s.Version = "v1";
        };
    })
    .SwaggerDocument(o =>
    {
        o.MaxEndpointVersion = 2;
        o.MinEndpointVersion = 2;
        o.FlattenSchema = false;
        o.ShortSchemaNames = false;
        o.DocumentSettings = s =>
        {
            s.DocumentName = "v2";
            s.Title = "Cart API V2";
            s.Version = "v2";
        };
    })
    ;

builder.Services
    .RegisterAddItemServices()
    .RegisterCreateCartServices()
    .RegisterGetListServices()
    .RegisterRemoveItemServices();

builder.Services.AddScoped<CartService.UseCases.GetCart.V01.IBusinessLogic, CartService.UseCases.GetCart.V01.BusinessLogic>();
builder.Services.AddScoped<CartService.UseCases.GetCart.V01.IRepository, CartService.UseCases.GetCart.V01.Repository>();
builder.Services.AddScoped<CartService.UseCases.GetCart.V02.IBusinessLogic, CartService.UseCases.GetCart.V02.BusinessLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

Action<Config> FEConfig = options =>
{
    options.Versioning.Prefix = "v";
    options.Versioning.PrependToRoute = true;
    options.Versioning.DefaultVersion = 1;
};
Action<OpenApiDocumentMiddlewareSettings> FEOpenApi = options =>
{
    options.Path = "/apiSpecs/specs.json";
    options.DocumentName = "v1";
};
Action<SwaggerUiSettings> FESwagger = options =>
{
    options.DocExpansion = "full";
    options.DocumentPath = "/apiSpecs/specs.json";
    options.Path = "/docs";
};

app
    .UseFastEndpoints(FEConfig)
    .UseSwaggerGen(FEOpenApi, FESwagger);
app.Run();

public partial class Program { }