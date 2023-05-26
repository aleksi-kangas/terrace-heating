using HeatingGateway.API.Common.Mapping;

namespace HeatingGateway.API; 

public static class DependencyInjection {
  public static IServiceCollection AddAPI(this IServiceCollection services) {
    services.AddControllers();
    services.AddMappings();
    return services;
  }
}
