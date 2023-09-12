using ErrorOr;
using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Services.Heating; 

public interface IHeatingStateService {
  Task<HeatingState> GetHeatingStateAsync();
  
  Task<ErrorOr<HeatingState>> ComputeHeatingStateAsync();
}
