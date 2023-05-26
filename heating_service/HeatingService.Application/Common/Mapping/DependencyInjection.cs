using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace HeatingService.Application.Common.Mapping; 

public static class DependencyInjection {
  public static IServiceCollection AddMappings(this IServiceCollection services) {
    var config = TypeAdapterConfig.GlobalSettings;
    TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
    TypeAdapterConfig.GlobalSettings.RequireDestinationMemberSource = true;
    config.Scan(Assembly.GetExecutingAssembly());
    TypeAdapterConfig.GlobalSettings.Compile();
    services.AddSingleton(config);
    services.AddScoped<IMapper, ServiceMapper>();
    return services;
  }
}
