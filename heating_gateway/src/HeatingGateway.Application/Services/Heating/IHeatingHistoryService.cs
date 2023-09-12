using ErrorOr;
using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Services.Heating;

public interface IHeatingHistoryService {
  Task<ErrorOr<List<Compressor>>> GetCompressorRecordsDateTimeRangeAsync(DateTime from,
    DateTime to);

  Task<ErrorOr<List<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(DateTime from,
    DateTime to);
}
