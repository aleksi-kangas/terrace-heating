using HeatingService.Application.Persistence;
using HeatingService.Application.Persistence.Repositories;
using HeatingService.Application.Services.HeatPump;
using HeatingService.Infrastructure.Persistence;
using HeatingService.Infrastructure.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HeatPumpRecordRepository = HeatingService.Infrastructure.Persistence.Repositories.HeatPumpRecordRepository;

namespace HeatingService.Infrastructure; 

public static class DependencyInjection {
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services.AddDbContext<HeatingDbContext>(options =>
      options.UseNpgsql(configurationManager.GetConnectionString("HeatingContext")));
    services.AddScoped<IHeatPumpRecordRepository, HeatPumpRecordRepository>();
    services.AddScheduler(builder => {
      builder.Services.AddScoped<HeatPumpService>();
      builder.Services.AddScoped<HeatPumpRecordRepository>();
      builder.AddJob<QueryHeatPumpRecordJob, QueryHeatPumpRecordJobOptions>();
    });
    return services;
  }
}
