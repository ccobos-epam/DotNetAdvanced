using System;
using System.Collections.Generic;
using System.Text;
using Testcontainers.MsSql;
using DbUp;
using BusinessLayer.Category;
using Microsoft.Data.SqlClient;

namespace IntegrationTest.Endpoint.Categories.GetList;

public class Test
{
    private MsSqlContainer _dbContainer;
    private string _connectionString;
    private CustomWebApplicationFactory _webApplicationFactory;
    private HttpClient _testClient;

    [Before(HookType.Test)]
    public async Task InitializeTestEnv()
    {
        _dbContainer = new Testcontainers.MsSql.MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            
            .WithPassword("YourStrong!Passw0rd")
            .Build();

        await _dbContainer.StartAsync();
        _connectionString = _dbContainer.GetConnectionString();

        await CreateCustomDatabaseAsync(_connectionString, "test");

        var newConnectionString = new SqlConnectionStringBuilder(_connectionString)
        {
            InitialCatalog = "test"
        };
        _connectionString = newConnectionString.ConnectionString;

        await ExecuteMigrationScripts();

        _webApplicationFactory = new CustomWebApplicationFactory(_connectionString);
        _testClient = _webApplicationFactory.CreateClient();
    }

    [After(HookType.Test)]
    public async Task DisposeTestEnv()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        _webApplicationFactory?.Dispose();
    }

    protected async Task ExecuteMigrationScripts()
    {
        EnsureDatabase.For.SqlDatabase(_connectionString);

        string migrationsPath = Path.Combine("BaseDataScript");
        var upgraderMigrations = DeployChanges.To
            .SqlDatabase(_connectionString)
            .WithScriptsFromFileSystem(migrationsPath)
            .Build();

        var upgraderMigrationsResult = upgraderMigrations.PerformUpgrade();
        if(!upgraderMigrationsResult.Successful)
            throw new Exception($"DbUp failed: {upgraderMigrationsResult.Error}");

        string[] seedFolders = ["Endpoint", "Categories", "GetList"];
        string seedPath = Path.Combine(seedFolders);
        var upgraderSeedData = DeployChanges.To
            .SqlDatabase(_connectionString)
            .WithScriptsFromFileSystem(seedPath)
            .Build();

        var upgraderSeedDataResult = upgraderSeedData.PerformUpgrade();
        if (!upgraderSeedDataResult.Successful)
            throw new Exception($"DbUp failed: {upgraderSeedDataResult.Error}");
    }

    private static async Task CreateCustomDatabaseAsync(string masterConnectionString, string databaseName)
    {
        // Note: The connection must use 'master' to create another database
        using var connection = new SqlConnection(masterConnectionString);
        using var command = connection.CreateCommand();

        command.CommandText = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE {databaseName};";

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    [Test]
    public async Task GetListOfElements()
    {
        //Arrange

        //Act
        var optional = await _testClient.GetAsync("categories");
        var stringResult = optional.Content;
        var result = System.Text.Json.JsonSerializer.Deserialize<List<GetCategoryResponse>>(stringResult.ReadAsStream());

        //Assert
        await Assert.That(result!.Count).IsEqualTo(2);
    }
}
