using ErrorOr;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;

namespace HeatingGateway.Application.Services;

public class HeatingService : IHeatingService {
  private readonly IHeatPumpRecordRepository _heatPumpRecordRepository;

  public HeatingService(IHeatPumpRecordRepository heatPumpRecordRepository) {
    _heatPumpRecordRepository = heatPumpRecordRepository;
  }

  public async Task<ErrorOr<IEnumerable<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(
    DateTime from,
    DateTime to) {
    if (from.Kind != DateTimeKind.Utc)
      return Error.Validation(description: "'From' must be a valid UTC timestamp, e.g. 1970-01-01T00:00:00Z");
    if (to.Kind != DateTimeKind.Utc)
      return Error.Validation(description: "'To' must be a valid UTC timestamp, e.g. 1970-01-01T00:00:00Z");
    if (to < from)
      return Error.Validation(description: "'To' must be greater than 'From'");
    try {
      var records = await _heatPumpRecordRepository.FindByDateTimeRangeAsync(from, to);
      return ErrorOr.ErrorOr.From(records);
    }
    catch (Exception e) {
      return Error.Failure(description: e.Message);
    }
  }
}
