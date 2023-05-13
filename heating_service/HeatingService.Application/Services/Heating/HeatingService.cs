using ErrorOr;
using HeatingService.Application.Persistence;
using HeatingService.Domain.HeatPump;

namespace HeatingService.Application.Services.Heating;

public class HeatingService : IHeatingService {
  private readonly IHeatPumpRecordRepository _heatPumpRecordRepository;

  public HeatingService(IHeatPumpRecordRepository heatPumpRecordRepository) {
    _heatPumpRecordRepository = heatPumpRecordRepository;
  }

  public async Task<ErrorOr<IEnumerable<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(
    DateTime from,
    DateTime to) {
    if (from.Kind != DateTimeKind.Utc)
      return Error.Validation(description: "From must be UTC");
    if (to.Kind != DateTimeKind.Utc)
      return Error.Validation(description: "To must be UTC");
    if (to < from)
      return Error.Validation(description: "To must be greater than from");

    try {
      var records = await _heatPumpRecordRepository.FindByDateTimeRangeAsync(from, to);
      return ErrorOr.ErrorOr.From(records);
    }
    catch (Exception e) {
      return Error.Failure(description: e.Message);
    }
  }
}
