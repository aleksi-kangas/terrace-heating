using ErrorOr;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace HeatingService.API.Services.HeatPump;

class HeatPumpService : IHeatPumpService {
  private readonly HeatPumpSvc.HeatPumpSvcClient _client;

  public HeatPumpService(HeatPumpSvc.HeatPumpSvcClient client) {
    _client = client;
  }

  public async Task<ErrorOr<UInt32>> GetActiveCircuitCountAsync() {
    try {
      var count = await _client.GetActiveCircuitCountAsync(new Empty());
      return count.Value;
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<HeatingService.API.Domain.Temperatures>> GetTemperaturesAsync() {
    try {
      var temperatures = await _client.GetTemperaturesAsync(new Empty());
      return new HeatingService.API.Domain.Temperatures(
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
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<HeatingService.API.Domain.TankLimits>> GetTankLimitsAsync() {
    try {
      var tankLimits = await _client.GetTankLimitsAsync(new Empty());
      return new HeatingService.API.Domain.TankLimits(
        tankLimits.LowerTankMinimum,
        tankLimits.LowerTankMaximum,
        tankLimits.UpperTankMinimum,
        tankLimits.UpperTankMaximum);
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<Boolean>> IsCompressorActiveAsync() {
    try {
      var isCompressorActive = await _client.IsCompressorActiveAsync(new Empty());
      return isCompressorActive.Value;

    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<Boolean>> IsSchedulingEnabledAsync() {
    try {
      var isSchedulingEnabled = await _client.IsSchedulingEnabledAsync(new Empty());
      return isSchedulingEnabled.Value;
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }
}