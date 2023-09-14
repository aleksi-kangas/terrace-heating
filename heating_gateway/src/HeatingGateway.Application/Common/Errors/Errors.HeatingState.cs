using ErrorOr;

namespace HeatingGateway.Application.Common.Errors;

public static partial class Errors {
  public static class HeatingState {
    public static Error AlreadyStarted = Error.Conflict(
      code: "HeatingState.AlreadyStarted",
      description: "Heating system has already been started.");
    public static Error AlreadyStopped = Error.Conflict(
      code: "HeatingState.AlreadyStopped",
      description: "Heating system has already been stopped.");
  }
}
