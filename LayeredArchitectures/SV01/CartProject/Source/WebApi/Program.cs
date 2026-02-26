using CartWebApi.Common;
using CartWebApi.Features.Cart.CreateCart.V01;
using FluentValidation;
using SharedClasses.OptionsPattern;
using SharedClasses.OptionsPattern.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

//Added configuration files
builder.Configuration.AddJsonFile("PostgreSQLConfig.json", false, true);
builder.Services.AddScoped<IValidator<PostgreSqlInstanceOptions>, PostgreSqlInstanceValidator>();
builder.Services.AddScoped<IValidator<PostgreSqlUserOptions>, PostgreSqlUserValidator>();
builder.Services.AddOptions<PostgreSqlInstanceOptions>()
    .BindConfiguration(PostgreSqlInstanceOptions.BasePath)
    .ValidateFluentValidation()
    .ValidateOnStart();
builder.Services.AddOptions<PostgreSqlUserOptions>()
    .BindConfiguration(PostgreSqlUserOptions.DocumentsPath)
    .ValidateFluentValidation()
    .ValidateOnStart();


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