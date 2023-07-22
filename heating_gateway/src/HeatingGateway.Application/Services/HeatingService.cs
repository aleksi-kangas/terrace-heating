using ErrorOr;
using HeatingGateway.Application.Common.Errors;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;

namespace HeatingGateway.Application.Services;

public class HeatingService : IHeatingService {
  private readonly ICompressorRecordRepository _compressorRecordRepository;
  private readonly IHeatPumpRecordRepository _heatPumpRecordRepository;

  public HeatingService(ICompressorRecordRepository compressorRecordRepository,
    IHeatPumpRecordRepository heatPumpRecordRepository) {
    _compressorRecordRepository = compressorRecordRepository;
    _heatPumpRecordRepository = heatPumpRecordRepository;
  }

  public async Task<ErrorOr<IEnumerable<CompressorRecord>>> GetCompressorRecordsDateTimeRangeAsync(
    DateTime from,
    DateTime to) {
    if (from.Kind != DateTimeKind.Utc || to.Kind != DateTimeKind.Utc)
      return Errors.DateTimeRange.DatetimeNotUTC;
    if (to < from)
      return Errors.DateTimeRange.FromIsAfterTo;
    try {
      var records = await _compressorRecordRepository.FindByDateTimeRangeAsync(from, to);
      return ErrorOrFactory.From(records);
    } catch (Exception e) {
      return Error.Failure(description: e.Message);
    }
  }

  public async Task<ErrorOr<IEnumerable<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(
    DateTime from,
    DateTime to) {
    if (from.Kind != DateTimeKind.Utc || to.Kind != DateTimeKind.Utc)
      return Errors.DateTimeRange.DatetimeNotUTC;
    if (to < from)
      return Errors.DateTimeRange.FromIsAfterTo;
    try {
      var records = await _heatPumpRecordRepository.FindByDateTimeRangeAsync(from, to);
      return ErrorOrFactory.From(records);
    } catch (Exception e) {
      return Error.Failure(description: e.Message);
    }
  }
}
