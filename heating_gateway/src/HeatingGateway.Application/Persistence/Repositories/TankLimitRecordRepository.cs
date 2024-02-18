using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories;

public class TankLimitRecordRepository : RepositoryBase<TankLimitRecord>, ITankLimitRecordRepository {
  public TankLimitRecordRepository(HeatingDbContext context) : base(context) { }

  public Task<List<TankLimitRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to) {
    return Task.FromResult(Context.TankLimitRecords
      .Where(r => from <= r.Time && r.Time <= to)
      .OrderBy(r => r.Time).ToList());
  }

  public async Task DeleteOlderThanAsync(DateTime olderThanThis) {
    var records = Context.TemperatureRecords.Where(r => r.Time < olderThanThis);
    foreach (var record in records) {
      Context.TemperatureRecords.Remove(record);
    }
    await Context.SaveChangesAsync();
  }
}
