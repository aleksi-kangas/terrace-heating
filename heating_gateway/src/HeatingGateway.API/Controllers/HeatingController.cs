using HeatingGateway.Application.Services.Heating;
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

  [HttpGet("history")]
  public async Task<IActionResult>
    GetHeatPumpRecords([FromQuery] GetHeatPumpRecordsRequest request) {
    var result =
      await _heatingService.GetHeatPumpRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      onValue: records => Ok(_mapper.Map<List<HeatPumpRecordResponse>>(records)),
      onError: Problem);
  }

  [HttpGet("history/compressor")]
  public async Task<IActionResult> GetCompressorRecords(
    [FromQuery] GetCompressorRecordsRequest request) {
    var result =
      await _heatingService.GetHeatPumpRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      onValue: records => Ok(_mapper.Map<List<CompressorRecordResponse>>(records)),
      onError: Problem);
  }

  [HttpGet("state")]
  public async Task<IActionResult> GetState() {
    var result = await _heatingService.GetHeatingStateAsync();
    return Ok(result);
  }
}
