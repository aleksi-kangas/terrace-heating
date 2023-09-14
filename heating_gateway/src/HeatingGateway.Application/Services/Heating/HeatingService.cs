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

  public HeatingState GetHeatingState() {
    return _stateService.GetHeatingState();
  }

  public ErrorOr<HeatingState> ComputeHeatingState() {
    return _stateService.ComputeHeatingState();
  }

  public ErrorOr<HeatingState> Start(bool softStart) {
    return _stateService.Start(softStart);
  }

  public ErrorOr<HeatingState> Stop() {
    return _stateService.Stop();
  }
}
