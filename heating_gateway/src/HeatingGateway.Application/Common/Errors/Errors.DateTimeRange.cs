using ErrorOr;

namespace HeatingGateway.Application.Common.Errors;

public static partial class Errors {
  public static class DateTimeRange {
    public static Error DatetimeNotUTC = Error.Validation(
        code: "DateTimeRange.DatetimeNotUTC",
        description: "Datetime must be UTC"
    );
    public static Error FromIsAfterTo = Error.Validation(
        code: "DateTimeRange.FromIsAfterTo",
        description: "From can not be after to"
    );
  }
}
