using HeatingService.API.Common.Mapping;

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
}
