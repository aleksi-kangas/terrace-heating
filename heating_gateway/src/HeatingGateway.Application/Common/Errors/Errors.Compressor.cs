using ErrorOr;

namespace HeatingGateway.Application.Common.Errors;

public static partial class Errors {
  public static class Compressor {
    public static Error UnableToComputeUsage = Error.Failure(
      code: "Compressor.UnableToComputeUsage",
      description: "Unable to compute compressor usage due to the lack of information");
  }
}

