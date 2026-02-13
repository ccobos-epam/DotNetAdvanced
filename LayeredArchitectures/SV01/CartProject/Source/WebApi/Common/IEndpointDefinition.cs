namespace CartWebApi.Common;

public interface IEndpointDefinition
{
  void DefineEndpoints(IEndpointRouteBuilder endpoints);
  void DefineServices(IServiceCollection services, IConfiguration configuration);
}