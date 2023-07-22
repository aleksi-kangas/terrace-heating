using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories;

public class CompressorRecordRepository : RepositoryBase<CompressorRecord>,
  ICompressorRecordRepository {
  public CompressorRecordRepository(HeatingDbContext context) : base(context) { }

  public async Task<IEnumerable<CompressorRecord>> FindByDateTimeRangeAsync(DateTime from,
    DateTime to, bool trackChanges) {
    return await FindByConditionAsync(r => from <= r.Time && r.Time <= to && 0 <= r.Usage,
      trackChanges);
  }

  public Task<CompressorRecord?> FindLatestBeforeAsync(DateTime beforeThis,
    bool? compressorActive = null) {
    return compressorActive is null
      ? Task.FromResult(Context.CompressorRecords
        .Where(r => r.Time < beforeThis)
        .OrderByDescending(r => r.Time).Cast<CompressorRecord?>().FirstOrDefault())
      : Task.FromResult(Context.CompressorRecords
        .Where(r => r.Time < beforeThis && r.Active == compressorActive)
        .OrderByDescending(r => r.Time).Cast<CompressorRecord?>().FirstOrDefault());
  }
}
