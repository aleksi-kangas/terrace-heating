using ErrorOr;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;
using static HeatingGateway.Application.Common.Errors.Errors.DateTimeRange;

namespace HeatingGateway.Application.Services.Heating;

public class HeatingHistoryService : IHeatingHistoryService {
  private readonly ICompressorRecordRepository _compressorRecordRepository;
  private readonly ITankLimitRecordRepository _tankLimitRecordRepository;
  private readonly ITemperatureRecordRepository _temperatureRecordRepository;

  public HeatingHistoryService(ICompressorRecordRepository compressorRecordRepository, ITankLimitRecordRepository tankLimitRecordRepository, ITemperatureRecordRepository temperatureRecordRepository) {
    _compressorRecordRepository = compressorRecordRepository;
    _tankLimitRecordRepository = tankLimitRecordRepository;
    _temperatureRecordRepository = temperatureRecordRepository;
  }

  public async Task<ErrorOr<List<CompressorRecord>>> GetCompressorRecordsDateTimeRangeAsync(
    DateTime from,
    DateTime to) {
    if (from.Kind != DateTimeKind.Utc || to.Kind != DateTimeKind.Utc)
      return DatetimeNotUTC;
    if (to < from)
      return FromIsAfterTo;
    try {
      var records = await _compressorRecordRepository.FindByDateTimeRangeAsync(from, to);
      return ErrorOrFactory.From(records);
    } catch (Exception e) {
      return Error.Failure(description: e.Message);
    }
  }
  
  public async Task<ErrorOr<List<TankLimitRecord>>> GetTankLimitRecordsDateTimeRangeAsync(
    DateTime from,
    DateTime to) {
    if (from.Kind != DateTimeKind.Utc || to.Kind != DateTimeKind.Utc)
      return DatetimeNotUTC;
    if (to < from)
      return FromIsAfterTo;
    try {
      var records = await _tankLimitRecordRepository.FindByDateTimeRangeAsync(from, to);
      return ErrorOrFactory.From(records);
    } catch (Exception e) {
      return Error.Failure(description: e.Message);
    }
  }
  
  public async Task<ErrorOr<List<TemperatureRecord>>> GetTemperatureRecordsDateTimeRangeAsync(
    DateTime from,
    DateTime to) {
    if (from.Kind != DateTimeKind.Utc || to.Kind != DateTimeKind.Utc)
      return DatetimeNotUTC;
    if (to < from)
      return FromIsAfterTo;
    try {
      var records = await _temperatureRecordRepository.FindByDateTimeRangeAsync(from, to);
      return ErrorOrFactory.From(records);
    } catch (Exception e) {
      return Error.Failure(description: e.Message);
    }
  }
}
