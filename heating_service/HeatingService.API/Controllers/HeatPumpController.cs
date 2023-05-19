using HeatingService.Application.Services.HeatPump;
using HeatingService.Contracts.HeatPump;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace HeatingService.API.Controllers;

[Route("heat-pump")]
public class HeatPumpController : ApiController {
  private readonly IMapper _mapper;
  private readonly IHeatPumpService _heatPumpService;

  public HeatPumpController(IMapper mapper, IHeatPumpService heatPumpService) {
    _mapper = mapper;
    _heatPumpService = heatPumpService;
  }

  [HttpGet("active-circuits")]
  public async Task<IActionResult> GetActiveCircuitCount() {
    var result = await _heatPumpService.GetActiveCircuitCountAsync();
    return result.Match(
      onValue: activeCircuitCount => Ok(activeCircuitCount),
      onError: Problem);
  }

  [HttpGet("compressor-active")]
  public async Task<IActionResult> IsCompressorActive() {
    var result = await _heatPumpService.IsCompressorActiveAsync();
    return result.Match(
      onValue: isCompressorActive => Ok(isCompressorActive),
      onError: Problem);
  }

  [HttpGet("scheduling-enabled")]
  public async Task<IActionResult> IsSchedulingEnabled() {
    var result = await _heatPumpService.IsSchedulingEnabledAsync();
    return result.Match(
      onValue: isSchedulingEnabled => Ok(isSchedulingEnabled),
      onError: Problem);
  }

  [HttpGet("schedules/circuit3")]
  public async Task<IActionResult> GetCircuit3BoostingSchedule() {
    var result = await _heatPumpService.GetCircuit3BoostingScheduleAsync();
    return result.Match(
      onValue: circuit3BoostingSchedule =>
        Ok(_mapper.Map<BoostingScheduleResponse>(circuit3BoostingSchedule)),
      onError: Problem);
  }

  [HttpGet("schedules/lower-tank")]
  public async Task<IActionResult> GetLowerTankBoostingSchedule() {
    var result = await _heatPumpService.GetLowerTankBoostingScheduleAsync();
    return result.Match(
      onValue: lowerTankBoostingSchedule =>
        Ok(_mapper.Map<BoostingScheduleResponse>(lowerTankBoostingSchedule)),
      onError: Problem);
  }

  [HttpGet("temperatures")]
  public async Task<IActionResult> GetTemperatures() {
    var result = await _heatPumpService.GetTemperaturesAsync();
    return result.Match(
      onValue: temperatures => Ok(_mapper.Map<TemperaturesResponse>(temperatures)),
      onError: Problem);
  }

  [HttpGet("tank-limits")]
  public async Task<IActionResult> GetTankLimits() {
    var result = await _heatPumpService.GetTankLimitsAsync();
    return result.Match(
      onValue: tankLimits => Ok(_mapper.Map<TankLimitsResponse>(tankLimits)),
      onError: Problem);
  }
}
