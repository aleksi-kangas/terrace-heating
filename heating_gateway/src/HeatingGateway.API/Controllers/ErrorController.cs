using Microsoft.AspNetCore.Mvc;

namespace HeatingGateway.API.Controllers; 

/*
 * ErrorController is a controller that handles errors.
 */
public class ErrorController : ControllerBase {
  [Route("/error")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public IActionResult Error() => Problem();
}
