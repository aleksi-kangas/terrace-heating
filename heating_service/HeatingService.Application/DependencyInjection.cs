using HeatingService.Application.Services.HeatPump;
using HeatPump;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeatingService.Application; 

public static class DependencyInjection {
  public static IServiceCollection AddApplication(this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services.AddGrpcClient<HeatPumpSvc.HeatPumpSvcClient>(o => {
      o.Address = new Uri("http://host.docker.internal:50051");
    });
    services.AddScoped<IHeatPumpService, HeatPumpService>();
    return services;
  }
}
