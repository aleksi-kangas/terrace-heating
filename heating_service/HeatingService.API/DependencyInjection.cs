using CronScheduler.Extensions.Scheduler;
using HeatingService.API.Common.Mapping;
using HeatingService.API.Services.HeatPump;
using HeatingService.API.Tasks;
using HeatingService.Infrastructure.Persistence;
using HeatPump;
using Microsoft.EntityFrameworkCore;

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
    return services;
  }

  public static IServiceCollection AddApplication(this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services.AddGrpcClient<HeatPumpSvc.HeatPumpSvcClient>(o => {
      o.Address = new Uri("http://host.docker.internal:50051");
    });
    services.AddScoped<IHeatPumpService, HeatPumpService>();
    services.AddDbContext<HeatingDbContext>(options =>
      options.UseNpgsql(configurationManager.GetConnectionString("HeatingContext")));
    services.AddMappings();
    services.AddScheduler(builder => {
        builder.Services.AddScoped<HeatPumpService>();
        builder.Services.AddDbContext<HeatingDbContext>(options =>
          options.UseNpgsql(configurationManager.GetConnectionString("HeatingContext")));
        builder.AddJob<QueryHeatPumpRecordJob, QueryHeatPumpRecordJobOptions>();
    });
    return services;
  }
}
