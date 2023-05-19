using HeatingService.Application.Services.Heating;
using HeatingService.Contracts.Heating;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace HeatingService.API.Controllers;

[Route($"heating")]
public class HeatingController : ApiController {
  private readonly IMapper _mapper;
  private readonly IHeatingService _heatingService;

  public HeatingController(IMapper mapper, IHeatingService heatingService) {
    _mapper = mapper;
    _heatingService = heatingService;
  }

  [HttpGet("records")]
  public async Task<IActionResult> GetHeatPumpRecords([FromQuery] GetHeatPumpRecordsRequest request) {
    var result = await _heatingService.GetHeatPumpRecordsDateTimeRangeAsync(request.From, request.To);
    return result.Match(
      onValue: records => Ok(_mapper.Map<List<HeatPumpRecordResponse>>(records)),
      onError: Problem);
  }
}
