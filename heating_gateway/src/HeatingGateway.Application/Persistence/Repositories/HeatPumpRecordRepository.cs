using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories;

public class HeatPumpRecordRepository : RepositoryBase<HeatPumpRecord>, IHeatPumpRecordRepository {
  public HeatPumpRecordRepository(HeatingDbContext context) : base(context) { }

  public Task<List<HeatPumpRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to) {
    return Task.FromResult(Context.HeatPumpRecords
      .Where(r => from <= r.Time && r.Time <= to)
      .OrderByDescending(r => r.Time).ToList());
  }

  public Task<HeatPumpRecord?>
    FindLatestBeforeAsync(DateTime beforeThis, bool? compressorActive = null, bool? hasUsage = null) {
    if (compressorActive is not null && hasUsage is not null) {
      return Task.FromResult(Context.HeatPumpRecords
        .Where(r => r.Time < beforeThis && r.Compressor.Active == compressorActive && r.Compressor.Usage.HasValue)
        .OrderByDescending(r => r.Time).Cast<HeatPumpRecord?>().FirstOrDefault());
    }
    if (hasUsage is not null) {
      return Task.FromResult(Context.HeatPumpRecords
        .Where(r => r.Time < beforeThis && r.Compressor.Usage.HasValue)
        .OrderByDescending(r => r.Time).Cast<HeatPumpRecord?>().FirstOrDefault());
    }
    if (compressorActive is not null) {
      return Task.FromResult(Context.HeatPumpRecords
        .Where(r => r.Time < beforeThis && r.Compressor.Active == compressorActive)
        .OrderByDescending(r => r.Time).Cast<HeatPumpRecord?>().FirstOrDefault());
    }
    
    return Task.FromResult(Context.HeatPumpRecords
      .Where(r => r.Time < beforeThis)
      .OrderByDescending(r => r.Time).Cast<HeatPumpRecord?>().FirstOrDefault());
  }
}
