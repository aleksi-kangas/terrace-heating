using ErrorOr;
using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Services.Heating; 

public interface IHeatingStateService {
  HeatingState GetHeatingState();
  ErrorOr<HeatingState> ComputeHeatingState();
  ErrorOr<HeatingState> Start(bool softStart);
  ErrorOr<HeatingState> Stop();
}
