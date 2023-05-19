using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace HeatingService.API.Common.Errors;

public class HeatingServiceProblemDetailsFactory : ProblemDetailsFactory {
  private readonly ApiBehaviorOptions _options;

  public HeatingServiceProblemDetailsFactory(IOptions<ApiBehaviorOptions> options) {
    _options = options.Value ?? throw new ArgumentNullException(nameof(options));
  }

  public override ProblemDetails CreateProblemDetails(
    HttpContext httpContext,
    int? statusCode = null,
    string? title = null,
    string? type = null,
    string? detail = null,
    string? instance = null) {
    var problemDetails = new ProblemDetails() {
      Status = statusCode ?? 500,
      Title = title,
      Type = type,
      Detail = detail,
      Instance = instance,
    };
    ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode ?? 500);
    return problemDetails;
  }

  public override ValidationProblemDetails CreateValidationProblemDetails(
    HttpContext httpContext,
    ModelStateDictionary modelStateDictionary,
    int? statusCode = null,
    string? title = null,
    string? type = null,
    string? detail = null,
    string? instance = null) {
    if (modelStateDictionary == null) {
      throw new ArgumentNullException(nameof(modelStateDictionary));
    }

    var problemDetails = new ValidationProblemDetails(modelStateDictionary) {
      Status = statusCode ?? 400, Type = type, Detail = detail, Instance = instance
    };
    ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode ?? 400);
    return problemDetails;
  }

  private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode) {
    problemDetails.Status ??= statusCode;
    if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData)) {
      problemDetails.Title ??= clientErrorData.Title;
      problemDetails.Type ??= clientErrorData.Link;
    }

    var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
    problemDetails.Extensions["traceId"] = traceId;
  }
}
