using ErrorOr;
using HeatingGateway.Application.Common.Errors;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;
using static HeatingGateway.Application.Common.Errors.Errors.DateTimeRange;

namespace HeatingGateway.Application.Services;

public class HeatingService : IHeatingService {
  private readonly IHeatPumpRecordRepository _heatPumpRecordRepository;

  public HeatingService(IHeatPumpRecordRepository heatPumpRecordRepository) {
    _heatPumpRecordRepository = heatPumpRecordRepository;
  }

  public async Task<ErrorOr<List<Compressor>>> GetCompressorRecordsDateTimeRangeAsync(
    DateTime from,
    DateTime to) {
    var heatPumpRecords = await GetHeatPumpRecordsDateTimeRangeAsync(from, to);
    if (heatPumpRecords.IsError) {
      if (heatPumpRecords.FirstError == DatetimeNotUTC)
        return DatetimeNotUTC;
      if (heatPumpRecords.FirstError == FromIsAfterTo)
        return FromIsAfterTo;
      return Error.Failure(description: heatPumpRecords.FirstError.Description);
    }

    var compressorRecords = heatPumpRecords.Value
      .Select(r => r.Compressor).Where(c => c.Usage.HasValue);
    return ErrorOrFactory.From(compressorRecords.ToList());
  }

  public async Task<ErrorOr<List<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(
    DateTime from,
    DateTime to) {
    if (from.Kind != DateTimeKind.Utc || to.Kind != DateTimeKind.Utc)
      return DatetimeNotUTC;
    if (to < from)
      return FromIsAfterTo;
    try {
      var records = await _heatPumpRecordRepository.FindByDateTimeRangeAsync(from, to);
      return ErrorOrFactory.From(records.ToList());
    } catch (Exception e) {
      return Error.Failure(description: e.Message);
    }
  }
}
