using ErrorOr;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HeatPump;
using MapsterMapper;
using BoostingSchedule = HeatingService.Application.Domain.BoostingSchedule;
using TankLimits = HeatingService.Application.Domain.TankLimits;
using Temperatures = HeatingService.Application.Domain.Temperatures;

namespace HeatingService.Application.Services.HeatPump;

public class HeatPumpService : IHeatPumpService {
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

  public async Task<ErrorOr<BoostingSchedule>> GetCircuit3BoostingScheduleAsync() {
    try {
      var boostingSchedule = await _client.GetCircuit3BoostingScheduleAsync(new Empty());
      return _mapper.Map<BoostingSchedule>(boostingSchedule);
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<Temperatures>> GetTemperaturesAsync() {
    try {
      var temperatures = await _client.GetTemperaturesAsync(new Empty());
      return _mapper.Map<Temperatures>(temperatures);
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<TankLimits>> GetTankLimitsAsync() {
    try {
      var tankLimits = await _client.GetTankLimitsAsync(new Empty());
      return _mapper.Map<TankLimits>(tankLimits);
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