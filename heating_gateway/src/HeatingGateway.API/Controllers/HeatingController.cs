using HeatingGateway.Application.Services.Heating;
using HeatingGateway.Contracts.Heating;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace HeatingGateway.API.Controllers;

/*
 * HeatingController is a controller that handles heating requests at /heating.
 * These requests include information about the historical state of the heat pump,
 * the current state of the heat pump, and the ability to start and stop the heating.
 */
[Route("heating")]
public class HeatingController : ApiController {
  private readonly IMapper _mapper;
  private readonly IHeatingService _heatingService;

  public HeatingController(IMapper mapper, IHeatingService heatingService) {
    _mapper = mapper;
    _heatingService = heatingService;
  }

  /*
   * Returns a list of heat pump records between the given date and time range.
   * @param   request   The request containing the date and time range.
   * @return  A list of heat pump records between the given date and time range.
   */
  [HttpGet("history")]
  public async Task<IActionResult>
    GetHeatPumpRecords([FromQuery] GetHeatPumpRecordsRequest request) {
    var result =
      await _heatingService.GetHeatPumpRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      onValue: records => Ok(_mapper.Map<List<HeatPumpRecordResponse>>(records)),
      onError: Problem);
  }

  /*
   * Returns a list of compressor records between the given date and time range.
   * @param   request   The request containing the date and time range.
   * @return  A list of compressor records between the given date and time range.
   */
  [HttpGet("history/compressor")]
  public async Task<IActionResult> GetCompressorRecords(
    [FromQuery] GetCompressorRecordsRequest request) {
    var result =
      await _heatingService.GetHeatPumpRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      onValue: records => Ok(_mapper.Map<List<CompressorRecordResponse>>(records)),
      onError: Problem);
  }

  /*
   * Returns the current state of the heat pump.
   * @return  The current state of the heat pump.
   */
  [HttpGet("state")]
  public IActionResult GetState() {
    var result = _heatingService.GetHeatingState();
    return Ok(result);
  }

  /*
   * Starts the heating.
   * @param request The request containing the soft start flag.
   * @return  The new state of the heat pump.
   */
  [HttpPost("start")]
  public IActionResult Start([FromQuery] StartRequest request) {
    var result = _heatingService.Start(request.SoftStart);
    return result.Match(
      onValue: heatingState => Ok(heatingState),
      onError: Problem);
  }

  /*
   * Stops the heating.
   * @return  The new state of the heat pump.
   */
  [HttpPost("stop")]
  public IActionResult Stop() {
    var result = _heatingService.Stop();
    return result.Match(
      onValue: heatingState => Ok(heatingState),
      onError: Problem);
  }
}
