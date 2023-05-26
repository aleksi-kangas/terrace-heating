using System.Security.Cryptography.X509Certificates;
using HeatingService.Application.Common.Mapping;
using HeatingService.Application.Persistence;
using HeatingService.Application.Persistence.Repositories;
using HeatingService.Application.Services.Heating;
using HeatingService.Application.Services.HeatPumpService;
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
      .AddTasks(configurationManager)
      .AddMappings();
    return services;
  }

  private static IServiceCollection AddServices(this IServiceCollection services) {
    services
      .AddGrpcClient<HeatPumpSvc.HeatPumpSvcClient>(o => {
        o.Address = new Uri("https://host.docker.internal:50051");
      })
      .ConfigurePrimaryHttpMessageHandler(() => {
        var heatingServiceCert = X509Certificate2.CreateFromPemFile(
          "/usr/src/app/certs/heating-service-cert.pem",
          "/usr/src/app/certs/heating-service-key.pem");
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(heatingServiceCert);
        // An ugly hack to accept an untrusted root CA used in the internal network
        // While this is 'dangerous', it does not matter that much as we are the client
        handler.ServerCertificateCustomValidationCallback =
          HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        return handler;
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
