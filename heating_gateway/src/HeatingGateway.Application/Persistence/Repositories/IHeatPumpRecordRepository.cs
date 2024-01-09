using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories; 

public interface IHeatPumpRecordRepository : IRepositoryBase<HeatPumpRecord> {
  Task<List<HeatPumpRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to);
  Task<HeatPumpRecord?> FindLatestBeforeAsync(DateTime beforeThis, bool? compressorActive = null, bool? hasUsage = null);
}
