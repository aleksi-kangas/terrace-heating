using HeatingService.Domain.HeatPump;

namespace HeatingService.Application.Persistence; 

public interface IHeatPumpRecordRepository : IRepositoryBase<HeatPumpRecord> {
  Task<IEnumerable<HeatPumpRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to, bool trackChanges = false);
}
