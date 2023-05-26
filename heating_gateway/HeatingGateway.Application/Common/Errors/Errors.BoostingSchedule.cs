using ErrorOr;

namespace HeatingGateway.Application.Common.Errors;

public static partial class Errors {
  public static class BoostingSchedule {
    public static Error OutOfRangeHour = Error.Validation(
      code: "BoostingSchedule.OutOfRangeHour",
      description: "Hour must be in range [0, 24]");

    public static Error OutOfRangeTemperatureDelta = Error.Validation(
      code: "BoostingSchedule.OutOfRangeTemperatureDelta",
      description: "Temperature delta must be in range [-10, 10]");
  }
}
