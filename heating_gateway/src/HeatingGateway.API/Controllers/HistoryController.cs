using HeatingGateway.Application.Services.Heating;
using HeatingGateway.Contracts.History;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace HeatingGateway.API.Controllers;

/*
 * HistoryController is a controller that handles requests for historical data at /history.
 * These requests include information about the historical state of the heat pump, e.g. temperatures, tank limits and compressor information.
 */
[Route("history")]
public class HistoryController : ApiController {
  private readonly IHeatingHistoryService _heatingHistoryService;
  private readonly IMapper _mapper;

  public HistoryController(IMapper mapper, IHeatingHistoryService heatingHistoryService) {
    _mapper = mapper;
    _heatingHistoryService = heatingHistoryService;
  }

  /*
   * Returns a list of compressor records between the given date and time range.
   * @param   request   The request containing the date and time range.
   * @return            A list of compressor records between the given date and time range.
   */
  [HttpGet("compressor")]
  public async Task<IActionResult> GetCompressorRecords(
    [FromQuery] DateTimeRangeRequest request) {
    var result =
      await _heatingHistoryService.GetCompressorRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      records => Ok(_mapper.Map<List<CompressorRecordResponse>>(records)),
      Problem);
  }

  /*
   * Returns a list of tank limit records between the given date and time range.
   * @param   request   The request containing the date and time range.
   * @return            A list of tank limit records between the given date and time range.
   */
  [HttpGet("tank-limits")]
  public async Task<IActionResult> GetTankLimitRecords([FromQuery] DateTimeRangeRequest request) {
    var result = await _heatingHistoryService.GetTankLimitRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      records => Ok(_mapper.Map<List<TankLimitRecordResponse>>(records)),
      Problem);
  }

  /*
   * Returns a list of temperature records between the given date and time range.
   * @param   request   The request containing the date and time range.
   * @return            A list of temperature records between the given date and time range.
   */
  [HttpGet("temperatures")]
  public async Task<IActionResult> GetTemperatureRecords([FromQuery] DateTimeRangeRequest request) {
    var result = await _heatingHistoryService.GetTemperatureRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      records => Ok(_mapper.Map<List<TemperatureRecordResponse>>(records)),
      Problem);
  }
}
