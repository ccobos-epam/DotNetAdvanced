namespace CartService.UseCases;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterAddItemServices(this IServiceCollection sc)
    {
        sc.AddScoped<AddItem.IBusinessLogic, AddItem.BusinessLogic>();
        sc.AddScoped<AddItem.IDataAccess, AddItem.DataAccess>();
        return sc;
    }

    public static IServiceCollection RegisterCreateCartServices(this IServiceCollection sc)
    {
        sc.AddScoped<CreateCart.IBusinessLogic, CreateCart.BusinessLogic>();
        sc.AddScoped<CreateCart.IDataAccess, CreateCart.DataAccess>();
        return sc;
    }

    public static IServiceCollection RegisterGetListServices(this IServiceCollection sc)
    {
        sc.AddScoped<GetList.IBusinessLogic, GetList.BusinessLogic>();
        sc.AddScoped<GetList.IDataAccess, GetList.DataAccess>();
        return sc;
    }

    public static IServiceCollection RegisterRemoveItemServices(this IServiceCollection sc)
    {
        sc.AddScoped<RemoveItem.IBusinessLogic, RemoveItem.BusinessLogic>();
        sc.AddScoped<RemoveItem.IDataAccess, RemoveItem.DataAccess>();
        return sc;
    }
}
