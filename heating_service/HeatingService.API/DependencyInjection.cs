using HeatingService.API.Common.Mapping;

namespace HeatingService.API; 

public static class DependencyInjection {
  public static IServiceCollection AddAPI(this IServiceCollection services) {
    services.AddControllers();
    services.AddMappings();
    return services;
  }
}
