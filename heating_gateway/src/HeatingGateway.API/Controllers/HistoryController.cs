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
  private readonly IMapper _mapper;
  private readonly IHeatingHistoryService _heatingHistoryService;

  public HistoryController(IMapper mapper, IHeatingHistoryService heatingHistoryService) {
    _mapper = mapper;
    _heatingHistoryService = heatingHistoryService;
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
      await _heatingHistoryService.GetHeatPumpRecordsDateTimeRangeAsync(request.From, request.To);
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
      await _heatingHistoryService.GetHeatPumpRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      onValue: records => Ok(_mapper.Map<List<CompressorRecordResponse>>(records)),
      onError: Problem);
  }
}
