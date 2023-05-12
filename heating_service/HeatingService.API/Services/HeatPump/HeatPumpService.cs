using ErrorOr;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HeatPump;
using MapsterMapper;

namespace HeatingService.API.Services.HeatPump;

class HeatPumpService : IHeatPumpService {
  private readonly IMapper _mapper;
  private readonly HeatPumpSvc.HeatPumpSvcClient _client;

  public HeatPumpService(IMapper mapper, HeatPumpSvc.HeatPumpSvcClient client) {
    _mapper = mapper;
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
      return _mapper.Map<HeatingService.API.Domain.Temperatures>(temperatures);
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<HeatingService.API.Domain.TankLimits>> GetTankLimitsAsync() {
    try {
      var tankLimits = await _client.GetTankLimitsAsync(new Empty());
      return _mapper.Map<HeatingService.API.Domain.TankLimits>(tankLimits);
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