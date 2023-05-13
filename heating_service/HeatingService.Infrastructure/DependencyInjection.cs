using HeatingService.Application.Persistence;
using HeatingService.Infrastructure.Persistence;
using HeatingService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeatingService.Infrastructure; 

public static class DependencyInjection {
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services.AddDbContext<HeatingDbContext>(options =>
      options.UseNpgsql(configurationManager.GetConnectionString("HeatingContext")));
    services.AddScoped<IHeatPumpRecordRepository, HeatPumpRecordRepository>();
    return services;
  }
}
