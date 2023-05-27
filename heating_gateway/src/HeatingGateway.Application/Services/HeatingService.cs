using ErrorOr;
using HeatingGateway.Application.Common.Errors;
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
    if (from.Kind != DateTimeKind.Utc || to.Kind != DateTimeKind.Utc)
      return Errors.HeatPumpRecord.DatetimeNotUTC;
    if (to < from)
      return Errors.HeatPumpRecord.FromIsAfterTo;
    try {
      var records = await _heatPumpRecordRepository.FindByDateTimeRangeAsync(from, to);
      return ErrorOrFactory.From(records);
    }
    catch (Exception e) {
      return Error.Failure(description: e.Message);
    }
  }
}
