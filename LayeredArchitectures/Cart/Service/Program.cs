using CartService.DataStorage;
using CartService.Entities;
using CartService.UseCases;
using FastEndpoints;
using FastEndpoints.Swagger;
using LiteDB;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddScoped<LiteDatabase>(sp =>
{
    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
    string connectionString = configuration.GetConnectionString("LiteDb")!;
    return new LiteDatabase(connectionString);
});
builder.Services.AddFastEndpoints().SwaggerDocument();
builder.Services
    .RegisterAddItemServices()
    .RegisterCreateCartServices()
    .RegisterGetListServices()
    .RegisterRemoveItemServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();
app.UseFastEndpoints().UseSwaggerGen();
app.Run();

public partial class Program { }