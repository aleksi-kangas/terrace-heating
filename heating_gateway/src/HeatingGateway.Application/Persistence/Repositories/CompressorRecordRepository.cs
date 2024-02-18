using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories;

public class CompressorRecordRepository : RepositoryBase<CompressorRecord>, ICompressorRecordRepository {
  public CompressorRecordRepository(HeatingDbContext context) : base(context) { }

  public Task<List<CompressorRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to) {
    return Task.FromResult(Context.CompressorRecords
      .Where(r => from <= r.Time && r.Time <= to)
      .OrderBy(r => r.Time).ToList());
  }
  
  public Task<CompressorRecord?>
    FindLatestBeforeAsync(DateTime beforeThis, bool? compressorActive = null, bool? hasUsage = null) {
    if (compressorActive is not null && hasUsage is not null) {
      return Task.FromResult(Context.CompressorRecords
        .Where(r => r.Time < beforeThis && r.Active == compressorActive && r.Usage.HasValue)
        .OrderByDescending(r => r.Time).Cast<CompressorRecord?>().FirstOrDefault());
    }
    if (hasUsage is not null) {
      return Task.FromResult(Context.CompressorRecords
        .Where(r => r.Time < beforeThis && r.Usage.HasValue)
        .OrderByDescending(r => r.Time).Cast<CompressorRecord?>().FirstOrDefault());
    }
    if (compressorActive is not null) {
      return Task.FromResult(Context.CompressorRecords
        .Where(r => r.Time < beforeThis && r.Active == compressorActive)
        .OrderByDescending(r => r.Time).Cast<CompressorRecord?>().FirstOrDefault());
    }
    
    return Task.FromResult(Context.CompressorRecords
      .Where(r => r.Time < beforeThis)
      .OrderByDescending(r => r.Time).Cast<CompressorRecord?>().FirstOrDefault());
  }

  public async Task DeleteOlderThanAsync(DateTime olderThanThis) {
    var records = Context.TemperatureRecords.Where(r => r.Time < olderThanThis);
    foreach (var record in records) {
      Context.TemperatureRecords.Remove(record);
    }
    await Context.SaveChangesAsync();
  }
}
