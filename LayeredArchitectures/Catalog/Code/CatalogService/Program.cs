using Asp.Versioning;
using BusinessLayer.Category;
using BusinessLayer.Product.Service;
using CommandContracts.RabbitMQ;
using DataAccess;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;
using Scalar.AspNetCore;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => 
{
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    string? connectionString = configuration.GetConnectionString("PostgreSQL");
    options.UseSqlServer(connectionString!);
    //new AppDbContext(connectionString!);
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

//Minimal API Section:
CatalogService.Product.Update.V01.UpdateHandler.AddUpdateServices(builder.Services);
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = false;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.AddOpenApi("v-m01");


builder.Host.UseWolverine(options =>
{
    var rabbitMqSetting = builder.Configuration
        .GetSection(RabbitMqConfigValues.SectionName)
        .Get<RabbitMqConfigValues>() ?? new RabbitMqConfigValues();
    options.UseRabbitMq(mqOptions =>
    {
        mqOptions.HostName = rabbitMqSetting.HostName;
        mqOptions.Port = rabbitMqSetting.Port;
        mqOptions.UserName = rabbitMqSetting.UserName;
        mqOptions.Password = rabbitMqSetting.Password;
    }).AutoProvision();

    options.PublishMessage<CommandContracts.RabbitMQ.Product.Update.V01.UpdateCommand_V01>()
        .ToRabbitQueue(RabbitMqConfigValues.QueueNames.CartUpdateQueue);
});
//builder.Services
//    .AddFastEndpoints()
//    .SwaggerDocument(o =>
//    {
//        o.MaxEndpointVersion = 1;
//        o.MinEndpointVersion = 1;
//        o.FlattenSchema = false;
//        o.ShortSchemaNames = false;
//        o.DocumentSettings = s =>
//        {
//            s.DocumentName = "v1";
//            s.Title = "Catalog API";
//            s.Version = "v1";
//        };
//    });

builder.Services
    .AddFastEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.



//Minimal API Section:
app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions();
CatalogService.Product.Update.V01.UpdateHandler.RegisterUpdateRoutes(app);

app.MapOpenApi("minimal-api/docs/{documentName}.json");
app.MapScalarApiReference("scalar-docs", options =>
{
    options.AddDocument("v01", "Minimal API Testing V01", "minimal-api/docs/v-m01.json");
    options.WithTheme(ScalarTheme.Moon);
});

//FastEndpoints Section:
Action<Config> feConfig = options =>
{
    options.Versioning.Prefix = "v";
    options.Versioning.PrependToRoute = true;
    options.Versioning.DefaultVersion = 1;
};
Action<OpenApiDocumentMiddlewareSettings> feOpenApi = options =>
{
    options.Path = "/apiSpecs/specs.json";
    options.DocumentName = "v1";
};
Action<SwaggerUiSettings> feSwagger = options =>
{
    options.DocExpansion = "full";
    options.DocumentPath = "/apiSpecs/specs.json";
    options.Path = "/docs";
};
app
    .UseFastEndpoints(feConfig);
//.UseSwaggerGen(FEOpenApi, FESwagger);

await app.RunAsync();
public partial class Program { }