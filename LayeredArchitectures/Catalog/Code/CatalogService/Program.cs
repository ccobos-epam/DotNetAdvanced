using BusinessLayer;
using DataAccess;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
        o.DocumentSettings = s =>
        {
            s.DocumentName = "Initial Release";
            s.Title = "Catalog API";
            s.Version = "v1";
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

Action<Config> FEConfig = options =>
{
    options.Versioning.Prefix = "v";
    options.Versioning.PrependToRoute = false;
    options.Versioning.DefaultVersion = 1;
};
app
    .UseFastEndpoints(FEConfig)
    .UseSwaggerGen();
app.Run();

public partial class Program { }