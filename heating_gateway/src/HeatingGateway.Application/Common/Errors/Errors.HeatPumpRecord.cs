using ErrorOr;

namespace HeatingGateway.Application.Common.Errors;

public static partial class Errors {
  public static class HeatPumpRecord {
    public static Error DatetimeNotUTC = Error.Validation(
        code: "HeatPumpRecord.DatetimeNotUTC",
        description: "Datetime must be UTC"
    );
    public static Error FromIsAfterTo = Error.Validation(
        code: "HeatPumpRecord.FromIsAfterTo",
        description: "From can not be after to"
    );
  }
}
