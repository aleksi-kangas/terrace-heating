using HeatingService.API.Common.Mapping;
using HeatingService.API.Services.HeatPump;

namespace HeatingService.API; 

public static class DependencyInjection {
  public static IServiceCollection AddAPI(this IServiceCollection services) {
    services.AddControllers();
    services.AddCors(options => {
      options.AddPolicy("AllowSpecificOrigin", corsPolicyBuilder => {
        corsPolicyBuilder
          .WithOrigins("http://localhost:8000")
          .AllowAnyHeader()
          .AllowAnyMethod();
      });
    });
    services.AddMappings();
    return services;
  }

  public static IServiceCollection AddApplication(this IServiceCollection services) {
    services.AddGrpcClient<HeatPumpSvc.HeatPumpSvcClient>(o => {
      o.Address = new Uri("http://host.docker.internal:50051");
    });
    services.AddScoped<IHeatPumpService, HeatPumpService>();
    return services;
  }
}
