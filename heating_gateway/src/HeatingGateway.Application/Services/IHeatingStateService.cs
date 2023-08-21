using ErrorOr;
using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Services; 

public interface IHeatingStateService {
  HeatingState GetHeatingState();
  
  Task<ErrorOr<HeatingState>> ComputeHeatingStateAsync();
}
