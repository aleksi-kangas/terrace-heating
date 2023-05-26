﻿using ErrorOr;
using BoostingSchedule = HeatingService.Application.Domain.BoostingSchedule;
using TankLimits = HeatingService.Application.Domain.TankLimits;
using Temperatures = HeatingService.Application.Domain.Temperatures;

namespace HeatingService.Application.Services.HeatPumpService;

public interface IHeatPumpService {
  Task<ErrorOr<UInt32>> GetActiveCircuitCountAsync();
  Task<ErrorOr<BoostingSchedule>> GetCircuit3BoostingScheduleAsync();
  Task<ErrorOr<BoostingSchedule>> GetLowerTankBoostingScheduleAsync();
  Task<ErrorOr<Temperatures>> GetTemperaturesAsync();
  Task<ErrorOr<TankLimits>> GetTankLimitsAsync();
  Task<ErrorOr<Boolean>> IsCompressorActiveAsync();
  Task<ErrorOr<Boolean>> IsSchedulingEnabledAsync();
  Task<ErrorOr<Success>> SetCircuit3BoostingScheduleAsync(BoostingSchedule boostingSchedule);
  Task<ErrorOr<Success>> SetLowerTankBoostingScheduleAsync(BoostingSchedule boostingSchedule);
}