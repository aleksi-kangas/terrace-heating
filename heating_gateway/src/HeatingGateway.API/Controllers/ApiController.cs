using System.Diagnostics;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HeatingGateway.API.Controllers;

/*
 * ApiController is a base class for all controllers in the API project.
 */
[ApiController]
public class ApiController : ControllerBase {
  protected IActionResult Problem(List<Error> errors) {
    if (errors.Count is 0) {
      Debug.Assert(false); // Should never happen
      return Problem();
    }

    return errors.All(error => error.Type == ErrorType.Validation)
      ? ValidationProblem(errors)
      : Problem(errors[0]);
  }

  private IActionResult Problem(Error error) {
    var statusCode = error.Type switch {
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      _ => StatusCodes.Status500InternalServerError
    };
    return Problem(statusCode: statusCode, title: error.Description);
  }

  private IActionResult ValidationProblem(List<Error> errors) {
    var modelStateDictionary = new ModelStateDictionary();
    foreach (var error in errors) {
      modelStateDictionary.AddModelError(error.Code, error.Description);
    }

    return ValidationProblem(modelStateDictionary);
  }
}
