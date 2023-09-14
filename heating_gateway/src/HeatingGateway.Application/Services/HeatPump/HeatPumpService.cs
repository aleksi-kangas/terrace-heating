using ErrorOr;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HeatingGateway.Application.Common.Errors;
using HeatingGateway.Application.Domain;
using MapsterMapper;

namespace HeatingGateway.Application.Services.HeatPump;

public class HeatPumpService : IHeatPumpService, IDisposable {
  private readonly IMapper _mapper;
  private readonly HeatPumpProto.HeatPumpSvc.HeatPumpSvcClient _client;

  public HeatPumpService(IMapper mapper, HeatPumpProto.HeatPumpSvc.HeatPumpSvcClient client) {
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

  public async Task<ErrorOr<BoostingSchedule>> GetLowerTankBoostingScheduleAsync() {
    try {
      var boostingSchedule = await _client.GetLowerTankBoostingScheduleAsync(new Empty());
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

  public async Task<ErrorOr<Success>> SetActiveCircuitCountAsync(UInt32 activeCircuitCount) {
    try {
      if (activeCircuitCount >= 4)
        return Errors.HeatDistributionCircuit.InvalidActiveCircuitCount;
      var request = new UInt32Value { Value = activeCircuitCount };
      await _client.SetActiveCircuitCountAsync(request);
      return Result.Success;
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<Success>> SetCircuit3BoostingScheduleAsync(
    BoostingSchedule boostingSchedule) {
    try {
      var validationResult = ValidateBoostingSchedule(boostingSchedule);
      if (validationResult.IsError)
        return validationResult;
      var request = _mapper.Map<HeatPumpProto.BoostingSchedule>(boostingSchedule);
      await _client.SetCircuit3BoostingScheduleAsync(request);
      return Result.Success;
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<Success>> SetLowerTankBoostingScheduleAsync(
    BoostingSchedule boostingSchedule) {
    try {
      var validationResult = ValidateBoostingSchedule(boostingSchedule);
      if (validationResult.IsError)
        return validationResult;
      var request = _mapper.Map<HeatPumpProto.BoostingSchedule>(boostingSchedule);
      await _client.SetLowerTankBoostingScheduleAsync(request);
      return Result.Success;
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  public async Task<ErrorOr<Success>> SetSchedulingEnabledAsync(bool enabled) {
    try {
      var request = new BoolValue { Value = enabled };
      await _client.SetSchedulingEnabledAsync(request);
      return Result.Success;
    } catch (RpcException e) {
      return Error.Failure(e.Message);
    }
  }

  private static ErrorOr<Success> ValidateBoostingSchedule(BoostingSchedule schedule) {
    if (schedule.Monday.StartHour > 24 || schedule.Monday.EndHour > 24 ||
        schedule.Tuesday.StartHour > 24 || schedule.Tuesday.EndHour > 24 ||
        schedule.Wednesday.StartHour > 24 || schedule.Wednesday.EndHour > 24 ||
        schedule.Thursday.StartHour > 24 || schedule.Thursday.EndHour > 24 ||
        schedule.Friday.StartHour > 24 || schedule.Friday.EndHour > 24 ||
        schedule.Saturday.StartHour > 24 || schedule.Saturday.EndHour > 24 ||
        schedule.Sunday.StartHour > 24 || schedule.Sunday.EndHour > 24)
      return Errors.BoostingSchedule.OutOfRangeHour;
    if (schedule.Monday.TemperatureDelta < -10 || 10 < schedule.Monday.TemperatureDelta ||
        schedule.Tuesday.TemperatureDelta < -10 || 10 < schedule.Tuesday.TemperatureDelta ||
        schedule.Wednesday.TemperatureDelta < -10 || 10 < schedule.Wednesday.TemperatureDelta ||
        schedule.Thursday.TemperatureDelta < -10 || 10 < schedule.Thursday.TemperatureDelta ||
        schedule.Friday.TemperatureDelta < -10 || 10 < schedule.Friday.TemperatureDelta ||
        schedule.Saturday.TemperatureDelta < -10 || 10 < schedule.Saturday.TemperatureDelta ||
        schedule.Sunday.TemperatureDelta < -10 || 10 < schedule.Sunday.TemperatureDelta)
      return Errors.BoostingSchedule.OutOfRangeTemperatureDelta;
    return Result.Success;
  }

  public void Dispose() {
    // Nothing to dispose
  }
}
