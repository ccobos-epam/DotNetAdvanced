using BusinessLayer.Category;
using BusinessLayer.Product.Service;
using DataAccess;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => 
{
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    string? connectionString = configuration.GetConnectionString("SqlServer");
    options.UseSqlServer(connectionString!);
    //new AppDbContext(connectionString!);
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

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
            s.Title = "Catalog API";
            s.Version = "v1";
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

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