using ErrorOr;
using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Services; 

public interface IHeatingService {
  Task<ErrorOr<IEnumerable<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(DateTime from, DateTime to);
}