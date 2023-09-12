using ErrorOr;
using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Services.Heating;

public class HeatingService : IHeatingService {
  private readonly IHeatingHistoryService _historyService;
  private readonly IHeatingStateService _stateService;

  public HeatingService(IHeatingHistoryService historyService, IHeatingStateService stateService) {
    _historyService = historyService;
    _stateService = stateService;
  }

  public Task<ErrorOr<List<Compressor>>> GetCompressorRecordsDateTimeRangeAsync(DateTime from,
    DateTime to) {
    return _historyService.GetCompressorRecordsDateTimeRangeAsync(from, to);
  }

  public Task<ErrorOr<List<HeatPumpRecord>>> GetHeatPumpRecordsDateTimeRangeAsync(DateTime from,
    DateTime to) {
    return _historyService.GetHeatPumpRecordsDateTimeRangeAsync(from, to);
  }

  public Task<HeatingState> GetHeatingStateAsync() {
    return _stateService.GetHeatingStateAsync();
  }

  public Task<ErrorOr<HeatingState>> ComputeHeatingStateAsync() {
    return _stateService.ComputeHeatingStateAsync();
  }
}
