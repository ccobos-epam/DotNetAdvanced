using CartWebApi.Common;
using CartWebApi.Features.Cart.CreateCart.V01;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("MartenConfig.json", false, true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddEndpointsServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.AddEndpointsDefinitions();

app.Run();