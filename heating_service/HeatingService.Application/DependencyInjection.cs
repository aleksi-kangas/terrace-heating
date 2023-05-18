using HeatingService.Application.Persistence;
using HeatingService.Application.Persistence.Repositories;
using HeatingService.Application.Services.Heating;
using HeatingService.Application.Services.HeatPump;
using HeatingService.Application.Tasks;
using HeatPump;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeatingService.Application;

public static class DependencyInjection {
  public static IServiceCollection AddApplication(this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services
      .AddServices()
      .AddPersistence(configurationManager)
      .AddTasks(configurationManager);
    return services;
  }

  private static IServiceCollection AddServices(this IServiceCollection services) {
    services.AddGrpcClient<HeatPumpSvc.HeatPumpSvcClient>(o => {
      o.Address = new Uri("http://host.docker.internal:50051");
    });
    services.AddScoped<IHeatingService, Services.Heating.HeatingService>();
    services.AddScoped<IHeatPumpService, HeatPumpService>();
    return services;
  }

  private static IServiceCollection AddPersistence(this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services.AddDbContext<HeatingDbContext>(options =>
      options.UseNpgsql(configurationManager.GetConnectionString("HeatingContext")));
    services.AddScoped<IHeatPumpRecordRepository, HeatPumpRecordRepository>();
    return services;
  }

  private static IServiceCollection AddTasks(this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services.AddScheduler(builder => {
      builder.Services.AddScoped<HeatPumpService>();
      builder.Services.AddScoped<HeatPumpRecordRepository>();
      builder.AddJob<QueryHeatPumpRecordJob, QueryHeatPumpRecordJobOptions>();
    });
    return services;
  }
}