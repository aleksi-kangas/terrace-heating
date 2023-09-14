using ErrorOr;

namespace HeatingGateway.Application.Common.Errors;

public static partial class Errors {
  public static class HeatDistributionCircuit {
    public static Error InvalidActiveCircuitCount = Error.Validation(
      code: "HeatDistributionCircuit.InvalidActiveCircuitCount",
      description: "Active circuit count must be [0, 3]");
  }
}
