using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories; 

public interface IHeatPumpRecordRepository : IRepositoryBase<HeatPumpRecord> {
  /*
   * Find all records between from and to, both inclusive.
   * @param   from  The start of the range.
   * @param   to    The end of the range.
   * @return        A list of records.
   */
  Task<List<HeatPumpRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to);
  
  /*
   * Find the latest record before beforeThis.
   * @param   beforeThis        The time to find the latest record before.
   * @param   compressorActive  If the compressor should be active or not.
   * @param   hasUsage          If the compressor should have usage or not.
   * @return                    The latest record before beforeThis satisfying the given conditions.
   */
  Task<HeatPumpRecord?> FindLatestBeforeAsync(DateTime beforeThis, bool? compressorActive = null, bool? hasUsage = null);
}
