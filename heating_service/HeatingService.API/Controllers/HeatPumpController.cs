using HeatingService.API.Services;
using HeatingService.Contracts.HeatPump;
using Microsoft.AspNetCore.Mvc;

namespace HeatingService.API.Controllers;

[Route("heat-pump")]
public class HeatPumpController : ApiController {
  private readonly IHeatPumpService _heatPumpService;

  public HeatPumpController(IHeatPumpService heatPumpService) {
    _heatPumpService = heatPumpService;
  }

  [HttpGet("active-circuits")]
  public async Task<UInt32> GetActiveCircuitCount() {
    return await _heatPumpService.GetActiveCircuitCountAsync();
  }

  [HttpGet("temperatures")]
  public async Task<TemperaturesResponse> GetTemperatures() {
    return await _heatPumpService.GetTemperaturesAsync();
  }

  [HttpGet("tank-limits")]
  public async Task<TankLimitsResponse> GetTankLimits() {
    return await _heatPumpService.GetTankLimitsAsync();
  }
}