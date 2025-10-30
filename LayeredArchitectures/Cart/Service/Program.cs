using CartService.DataStorage;
using CartService.Entities;
using CartService.UseCases;
using FastEndpoints;
using FastEndpoints.Swagger;
using LiteDB;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddSingleton<LiteDatabase>(_ =>
{
    var filename = builder.Configuration.GetConnectionString(InfrastructureData.connectionName) ?? "data.db";
    return new LiteDatabase(filename);
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