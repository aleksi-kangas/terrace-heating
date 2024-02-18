using System.Security.Cryptography.X509Certificates;
using HeatingGateway.Application.Common.Mapping;
using HeatingGateway.Application.Persistence;
using HeatingGateway.Application.Persistence.Repositories;
using HeatingGateway.Application.Services.Heating;
using HeatingGateway.Application.Services.HeatPump;
using HeatingGateway.Application.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeatingGateway.Application;

public static class DependencyInjection {
  public static IServiceCollection AddApplication(this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services
      .AddServices()
      .AddPersistence(configurationManager)
      .AddTasks()
      .AddMappings();
    return services;
  }

  private static IServiceCollection AddServices(this IServiceCollection services) {
    services
      .AddGrpcClient<HeatPumpProto.HeatPumpSvc.HeatPumpSvcClient>(o => {
        o.Address = new Uri("https://heat-pump-service:50051");
      })
      .ConfigurePrimaryHttpMessageHandler(() => {
        var heatingServiceCert = X509Certificate2.CreateFromPemFile(
          "/usr/src/app/certs/heating-gateway-cert.pem",
          "/usr/src/app/certs/heating-gateway-key.pem");
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(heatingServiceCert);
        // An ugly hack to accept an untrusted root CA used in the internal network
        // While this is 'dangerous', it does not matter that much as we are the client
        handler.ServerCertificateCustomValidationCallback =
          HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        return handler;
      });
    services.AddScoped<IHeatingHistoryService, HeatingHistoryService>();
    services.AddSingleton<IHeatingStateService, HeatingStateService>();
    services.AddScoped<IHeatPumpService, HeatPumpService>();
    return services;
  }

  private static IServiceCollection AddPersistence(this IServiceCollection services,
    ConfigurationManager configurationManager) {
    var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
    var database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");
    var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    services.AddDbContext<HeatingDbContext>(options =>
      options.UseNpgsql($"host={host};database={database};username={username};password={password}"));
    services.AddScoped<IHeatPumpRecordRepository, HeatPumpRecordRepository>();
    return services;
  }

  private static IServiceCollection AddTasks(this IServiceCollection services) {
    services.AddScheduler(builder => {
      builder.Services.AddScoped<HeatPumpService>();
      builder.Services.AddSingleton<HeatingStateService>();
      builder.Services.AddScoped<HeatPumpRecordRepository>();
      builder.AddJob<ComputeHeatingStateJob, ComputeHeatingStateJobOptions>();
      builder.AddJob<QueryHeatPumpRecordJob, QueryHeatPumpRecordJobOptions>();
      builder.AddJob<RemoveOldRecordsJob, RemoveOldRecordsJobOptions>();
    });
    return services;
  }
}
