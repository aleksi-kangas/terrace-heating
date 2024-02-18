using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories;

public interface ITankLimitRecordRepository : IRepositoryBase<TankLimitRecord> {
  /*
   * Find all tank limit records between from and to, both inclusive.
   * @param   from  The start of the range.
   * @param   to    The end of the range.
   * @return        A list of records.
   */
  Task<List<TankLimitRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to);
  
  Task DeleteOlderThanAsync(DateTime olderThanThis);
}
