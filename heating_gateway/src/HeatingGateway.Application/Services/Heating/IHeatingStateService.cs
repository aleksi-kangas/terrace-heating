using ErrorOr;
using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Services.Heating; 

public interface IHeatingStateService {
  /*
   * Get the current heating state.
   */
  HeatingState GetHeatingState();
  
  /*
   * Compute the current heating state.
   */
  ErrorOr<HeatingState> ComputeHeatingState();
  
  /*
   * Start or stop the heating.
   * @param   softStart  Whether to start the heating with a soft start.
   */
  ErrorOr<HeatingState> Start(bool softStart);
  
  /*
   * Stop the heating.
   */
  ErrorOr<HeatingState> Stop();
}
