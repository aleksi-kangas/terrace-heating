using ErrorOr;
using HeatPumpRecord = HeatingService.Application.Domain.HeatPumpRecord;

namespace HeatingService.Application.Services.Heating; 

public interface IHeatingService {
  Task<ErrorOr<IEnumerable<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(DateTime from, DateTime to);
}
