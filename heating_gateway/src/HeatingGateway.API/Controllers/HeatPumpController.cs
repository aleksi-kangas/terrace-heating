using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Services.HeatPump;
using HeatingGateway.Contracts.HeatPump;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace HeatingGateway.API.Controllers;

/*
 * HeatPumpController is a controller that handles requests related to the heat pump.
 * These requests include information about the heat pump's current state, the ability
 * to change the heat pump's state, and the ability to change the heat pump's settings.
 */
[Route("heat-pump")]
public class HeatPumpController : ApiController {
  private readonly IMapper _mapper;
  private readonly IHeatPumpService _heatPumpService;

  public HeatPumpController(IMapper mapper, IHeatPumpService heatPumpService) {
    _mapper = mapper;
    _heatPumpService = heatPumpService;
  }

  /*
   * Returns the current number of active circuits.
   * @return  The current number of active circuits.
   */
  [HttpGet("active-circuits")]
  public async Task<IActionResult> GetActiveCircuitCount() {
    var result = await _heatPumpService.GetActiveCircuitCountAsync();
    return result.Match(
      onValue: activeCircuitCount => Ok(activeCircuitCount),
      onError: Problem);
  }

  /*
   * Returns true if the compressor is active, false otherwise.
   * @return  True if the compressor is active, false otherwise.
   */
  [HttpGet("compressor-active")]
  public async Task<IActionResult> IsCompressorActive() {
    var result = await _heatPumpService.IsCompressorActiveAsync();
    return result.Match(
      onValue: isCompressorActive => Ok(isCompressorActive),
      onError: Problem);
  }

  /*
   * Returns true if the scheduled boosting is enabled (distinct from it being currently active).
   * @return  True if the scheduled boosting is enabled, false otherwise.
   */ 
  [HttpGet("scheduling-enabled")]
  public async Task<IActionResult> IsSchedulingEnabled() {
    var result = await _heatPumpService.IsSchedulingEnabledAsync();
    return result.Match(
      onValue: isSchedulingEnabled => Ok(isSchedulingEnabled),
      onError: Problem);
  }

  /*
   * Returns the boosting schedule of heat distribution circuit 3.
   * @return  The boosting schedule of heat distribution circuit 3.
   */
  [HttpGet("schedules/circuit3")]
  public async Task<IActionResult> GetCircuit3BoostingSchedule() {
    var result = await _heatPumpService.GetCircuit3BoostingScheduleAsync();
    return result.Match(
      onValue: circuit3BoostingSchedule =>
        Ok(_mapper.Map<BoostingScheduleResponse>(circuit3BoostingSchedule)),
      onError: Problem);
  }

  /*
   * Updates the boosting schedule of the heat distribution circuit 3.
   * @param   request   The request containing the new boosting schedule.
   * @return  No content if the boosting schedule was successfully updated, a problem
   *          if the boosting schedule was not successfully updated.
   */
  [HttpPut("schedules/circuit3")]
  public async Task<IActionResult> PutCircuit3BoostingSchedule(
    [FromBody] PutBoostingScheduleRequest request) {
    var result =
      await _heatPumpService.SetCircuit3BoostingScheduleAsync(
        _mapper.Map<BoostingSchedule>(request));
    return result.Match(
      onValue: _ => NoContent(),
      onError: Problem);
  }

  /*
   * Returns the boosting schedule of the lower tank.
   * @return  The boosting schedule of the lower tank.
   */
  [HttpGet("schedules/lower-tank")]
  public async Task<IActionResult> GetLowerTankBoostingSchedule() {
    var result = await _heatPumpService.GetLowerTankBoostingScheduleAsync();
    return result.Match(
      onValue: lowerTankBoostingSchedule =>
        Ok(_mapper.Map<BoostingScheduleResponse>(lowerTankBoostingSchedule)),
      onError: Problem);
  }
  
  /*
   * Updates the boosting schedule of the lower tank.
   * @param   request   The request containing the new boosting schedule.
   * @return  No content if the boosting schedule was successfully updated, a problem
   *          if the boosting schedule was not successfully updated.
   */
  [HttpPut("schedules/lower-tank")]
  public async Task<IActionResult> PutLowerTankBoostingSchedule(
    [FromBody] PutBoostingScheduleRequest request) {
    var result =
      await _heatPumpService.SetLowerTankBoostingScheduleAsync(
        _mapper.Map<BoostingSchedule>(request));
    return result.Match(
      onValue: _ => NoContent(),
      onError: Problem);
  }

  /*
   * Returns the current temperatures of the heat pump.
   * @return  The current temperatures of the heat pump.
   */
  [HttpGet("temperatures")]
  public async Task<IActionResult> GetTemperatures() {
    var result = await _heatPumpService.GetTemperaturesAsync();
    return result.Match(
      onValue: temperatures => Ok(_mapper.Map<TemperaturesResponse>(temperatures)),
      onError: Problem);
  }

  /*
   * Returns the current tank limits of the heat pump, which guide it in its operation.
   * @return  The current tank limits of the heat pump.
   */
  [HttpGet("tank-limits")]
  public async Task<IActionResult> GetTankLimits() {
    var result = await _heatPumpService.GetTankLimitsAsync();
    return result.Match(
      onValue: tankLimits => Ok(_mapper.Map<TankLimitsResponse>(tankLimits)),
      onError: Problem);
  }
}
