using Microsoft.AspNetCore.Mvc;

namespace HeatingService.API.Controllers; 

public class ErrorController : ControllerBase {
  [Route("/error")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public IActionResult Error() => Problem();
}
