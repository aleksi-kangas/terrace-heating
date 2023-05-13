using ErrorOr;
using HeatingService.Domain.HeatPump;

namespace HeatingService.Application.Services.Heating; 

public interface IHeatingService {
  Task<ErrorOr<IEnumerable<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(DateTime from, DateTime to);
}
