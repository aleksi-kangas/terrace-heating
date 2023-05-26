using Microsoft.AspNetCore.Mvc;

namespace HeatingGateway.API.Controllers; 

public class ErrorController : ControllerBase {
  [Route("/error")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public IActionResult Error() => Problem();
}
