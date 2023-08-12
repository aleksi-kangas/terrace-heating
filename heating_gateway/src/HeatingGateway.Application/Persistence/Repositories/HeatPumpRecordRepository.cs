using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories;

public class HeatPumpRecordRepository : RepositoryBase<HeatPumpRecord>, IHeatPumpRecordRepository {
  public HeatPumpRecordRepository(HeatingDbContext context) : base(context) { }

  public async Task<IEnumerable<HeatPumpRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to,
    bool trackChanges) {
    return await FindByConditionAsync(r => from <= r.Time && r.Time <= to, trackChanges);
  }

  public Task<HeatPumpRecord?>
    FindLatestBeforeAsync(DateTime beforeThis, bool? compressorActive = null) {
    return compressorActive is null
      ? Task.FromResult(Context.HeatPumpRecords
        .Where(r => r.Time < beforeThis)
        .OrderByDescending(r => r.Time).Cast<HeatPumpRecord?>().FirstOrDefault())
      : Task.FromResult(Context.HeatPumpRecords
        .Where(r => r.Time < beforeThis && r.Compressor.Active == compressorActive)
        .OrderByDescending(r => r.Time).Cast<HeatPumpRecord?>().FirstOrDefault());
  }
}
