using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories;

public interface ITemperatureRecordRepository : IRepositoryBase<TemperatureRecord> {
  /*
   * Find all temperature records between from and to, both inclusive.
   * @param   from  The start of the range.
   * @param   to    The end of the range.
   * @return        A list of records.
   */
  Task<List<TemperatureRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to);
  
  Task DeleteOlderThanAsync(DateTime olderThanThis);
}
