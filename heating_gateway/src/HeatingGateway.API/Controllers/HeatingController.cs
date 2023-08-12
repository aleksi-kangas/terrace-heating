using HeatingGateway.Application.Services;
using HeatingGateway.Contracts.Heating;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace HeatingGateway.API.Controllers;

[Route("heating")]
public class HeatingController : ApiController {
  private readonly IMapper _mapper;
  private readonly IHeatingService _heatingService;

  public HeatingController(IMapper mapper, IHeatingService heatingService) {
    _mapper = mapper;
    _heatingService = heatingService;
  }

  [HttpGet("records")]
  public async Task<IActionResult>
    GetHeatPumpRecords([FromQuery] GetHeatPumpRecordsRequest request) {
    var result =
      await _heatingService.GetHeatPumpRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      onValue: records => Ok(_mapper.Map<List<HeatPumpRecordResponse>>(records)),
      onError: Problem);
  }

  [HttpGet("records/compressor")]
  public async Task<IActionResult> GetCompressorRecords(
    [FromQuery] GetCompressorRecordsRequest request) {
    var result =
      await _heatingService.GetHeatPumpRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      onValue: records => Ok(_mapper.Map<List<CompressorRecordResponse>>(records)),
      onError: Problem);
  }
}
