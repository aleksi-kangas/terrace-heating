using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories;

public interface ICompressorRecordRepository : IRepositoryBase<CompressorRecord> {
  Task<IEnumerable<CompressorRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to,
    bool trackChanges = false);

  Task<CompressorRecord?> FindLatestBeforeAsync(DateTime beforeThis, bool? compressorActive = null);
}
