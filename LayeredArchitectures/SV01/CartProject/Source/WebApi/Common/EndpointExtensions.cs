

namespace CartWebApi.Common;

public static class EndpointExtensions
{
  public static void AddEndpointsServices(this WebApplicationBuilder builder)
  {
    var definitions = new List<Type>();

    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
      var types = assembly.GetTypes().Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t))
        .Where(t => t is { IsInterface: false, IsAbstract: false });
      definitions.AddRange(types);
    }

    foreach (var def in definitions)
    {
      def.GetMethod(nameof(IEndpointDefinition.DefineServices))?.Invoke(null, [builder.Services, builder.Configuration]);
    }
    
    builder.Services.AddSingleton(definitions);
  }

  public static void AddEndpointsDefinitions(this WebApplication app)
  {
    var definitions = app.Services.GetRequiredService<List<Type>>();
    foreach (var def in definitions)
    {
      def.GetMethod(nameof(IEndpointDefinition.DefineEndpoints))?.Invoke(null, [app]);
    }
  }
}