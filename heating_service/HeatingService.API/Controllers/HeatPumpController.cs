using HeatingService.API.Services.HeatPump;
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
      onError: _ => Problem());
  }

  [HttpGet("compressor-active")]
  public async Task<IActionResult> IsCompressorActive() {
    var result = await _heatPumpService.IsCompressorActiveAsync();
    return result.Match(
      onValue: isCompressorActive => Ok(isCompressorActive),
      onError: _ => Problem());
  }

  [HttpGet("scheduling-enabled")]
  public async Task<IActionResult> IsSchedulingEnabled() {
    var result = await _heatPumpService.IsSchedulingEnabledAsync();
    return result.Match(
      onValue: isSchedulingEnabled => Ok(isSchedulingEnabled),
      onError: _ => Problem());
  }

  [HttpGet("temperatures")]
  public async Task<IActionResult> GetTemperatures() {
    var result = await _heatPumpService.GetTemperaturesAsync();
    return result.Match(
      onValue: temperatures => Ok(_mapper.Map<TemperaturesResponse>(temperatures)),
      onError: _ => Problem());
  }

  [HttpGet("tank-limits")]
  public async Task<IActionResult> GetTankLimits() {
    var result = await _heatPumpService.GetTankLimitsAsync();
    return result.Match(
      onValue: tankLimits => Ok(_mapper.Map<TankLimitsResponse>(tankLimits)),
      onError: _ => Problem());
  }
}