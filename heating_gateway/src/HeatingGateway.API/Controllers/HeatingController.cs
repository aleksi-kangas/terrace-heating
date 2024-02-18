using HeatingGateway.Application.Services.Heating;
using HeatingGateway.Contracts.Heating;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace HeatingGateway.API.Controllers;

/*
 * HeatingController is a controller that handles heating requests at /heating.
 * These requests include information about the current state of the heat pump, and the ability to start and stop the heating.
 */
[Route("heating")]
public class HeatingController : ApiController {
  private readonly IMapper _mapper;
  private readonly IHeatingStateService _heatingStateService;

  public HeatingController(IMapper mapper, IHeatingStateService heatingStateService) {
    _mapper = mapper;
    _heatingStateService = heatingStateService;
  }

  /*
   * Returns the current state of the heat pump.
   * @return  The current state of the heat pump.
   */
  [HttpGet("state")]
  public IActionResult GetState() {
    var result = _heatingStateService.GetHeatingState();
    return Ok(result);
  }

  /*
   * Starts the heating.
   * @param request The request containing the soft start flag.
   * @return  The new state of the heat pump.
   */
  [HttpPost("start")]
  public IActionResult Start([FromQuery] StartRequest request) {
    var result = _heatingStateService.Start(request.SoftStart);
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
    var result = _heatingStateService.Stop();
    return result.Match(
      onValue: heatingState => Ok(heatingState),
      onError: Problem);
  }
}
