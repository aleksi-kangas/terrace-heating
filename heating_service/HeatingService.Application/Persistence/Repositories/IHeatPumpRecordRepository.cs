using HeatPumpRecord = HeatingService.Application.Domain.HeatPumpRecord;

namespace HeatingService.Application.Persistence.Repositories; 

public interface IHeatPumpRecordRepository : IRepositoryBase<HeatPumpRecord> {
  Task<IEnumerable<HeatPumpRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to, bool trackChanges = false);
}
