using Microsoft.AspNetCore.Mvc.Testing;
using CatalogService;
using System;
using System.Collections.Generic;
using System.Text;
using Testcontainers.MsSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace IntegrationTest.Endpoint;

public class CustomWebApplicationFactory : WebApplicationFactory<CatalogService.Category.CreateEnpoint>
{
    private readonly MsSqlContainer? _dbContainer;
    private readonly string? _connectionString;

    public CustomWebApplicationFactory(MsSqlContainer sqlContainer)
    {
        _dbContainer = sqlContainer;
    }

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        //builder.UseConfiguration()
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var testConfigurationValues = new Dictionary<string, string?>()
            {
                ["ConnectionStrings:SqlServer"] = _connectionString
            };
            configBuilder.AddInMemoryCollection(testConfigurationValues);

        });

        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        /*
        builder.ConfigureServices(services =>
        {
            var dbContextServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
            if (dbContextServiceDescriptor is not null)
                services.Remove(dbContextServiceDescriptor);

            services.AddScoped<AppDbContext>(_ => new AppDbContext(_dbContainer.GetConnectionString()));
        });
        */
        
        return base.CreateHost(builder);
    }
}
