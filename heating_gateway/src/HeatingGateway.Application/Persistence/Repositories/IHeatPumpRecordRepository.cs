using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories; 

public interface IHeatPumpRecordRepository : IRepositoryBase<HeatPumpRecord> {
  Task<IEnumerable<HeatPumpRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to, bool trackChanges = false);
  Task<HeatPumpRecord?> FindLatestBeforeAsync(DateTime beforeThis, bool? compressorActive = null);
}
