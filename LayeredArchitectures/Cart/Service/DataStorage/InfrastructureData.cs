using CartService.Entities;
using LiteDB;

namespace CartService.DataStorage;

public static class InfrastructureData
{
    public const string connectionName = "LiteDb";
    public const string collectionName = "customerCarts";
}
