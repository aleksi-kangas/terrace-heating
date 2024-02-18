using ErrorOr;
using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Services.Heating;

public interface IHeatingHistoryService {
  /*
   * Get compressor records for a given date range.
   * @param   from  The start of the range.
   * @param   to    The end of the range.
   * @return        A list of compressor records.
   */
  Task<ErrorOr<List<CompressorRecord>>> GetCompressorRecordsDateTimeRangeAsync(DateTime from, DateTime to);
  
  /*
   * Get tank limit records for a given date range.
   * @param   from  The start of the range.
   * @param   to    The end of the range.
   * @return        A list of tank limit records.
   */
  Task<ErrorOr<List<TankLimitRecord>>> GetTankLimitRecordsDateTimeRangeAsync(DateTime from, DateTime to);
  
  /*
   * Get temperature records for a given date range.
   * @param   from  The start of the range.
   * @param   to    The end of the range.
   * @return        A list of temperature records.
   */
  Task<ErrorOr<List<TemperatureRecord>>> GetTemperatureRecordsDateTimeRangeAsync(DateTime from, DateTime to);
}
