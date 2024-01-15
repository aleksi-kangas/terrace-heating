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
  Task<ErrorOr<List<Compressor>>> GetCompressorRecordsDateTimeRangeAsync(DateTime from,
    DateTime to);

  /*
   * Get heat pump records for a given date range.
   * @param   from  The start of the range.
   * @param   to    The end of the range.
   * @return        A list of heat pump records.
   */
  Task<ErrorOr<List<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(DateTime from,
    DateTime to);
}
