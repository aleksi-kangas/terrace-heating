﻿using Google.Protobuf.WellKnownTypes;
using HeatingService.Contracts.HeatPump;

namespace HeatingService.API.Services;

class HeatPumpService : IHeatPumpService {
  private readonly HeatPumpSvc.HeatPumpSvcClient _client;

  public HeatPumpService(HeatPumpSvc.HeatPumpSvcClient client) {
    _client = client;
  }

  public async Task<TemperaturesResponse> GetTemperaturesAsync() {
    var temperatures = await _client.GetTemperaturesAsync(new Empty());
    // TODO Use a proper mapper library
    return  new TemperaturesResponse(
      temperatures.Circuit1,
      temperatures.Circuit2,
      temperatures.Circuit3,
      temperatures.GroundInput,
      temperatures.GroundOutput,
      temperatures.HotGas,
      temperatures.Inside,
      temperatures.LowerTank,
      temperatures.Outside,
      temperatures.UpperTank);
  }

  public async Task<TankLimitsResponse> GetTankLimitsAsync() {
    var tankLimits = await _client.GetTankLimitsAsync(new Empty());
    // TODO Use a proper mapper library
    return new TankLimitsResponse(
      tankLimits.LowerTankMinimum,
      tankLimits.LowerTankMaximum,
      tankLimits.UpperTankMinimum,
      tankLimits.UpperTankMaximum);
  }
}