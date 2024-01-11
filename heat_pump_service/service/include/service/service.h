#pragma once

#include <memory>

#include <grpcpp/grpcpp.h>

#include "communicator/communicator.h"
#include "heat_pump_service.grpc.pb.h"
#include "heat_pump_service.pb.h"

namespace service {
class HeatPumpService final : public heat_pump::HeatPumpSvc::Service {
 public:
  explicit HeatPumpService(std::unique_ptr<communicator::ICommunicator> communicator);
  ~HeatPumpService() override = default;

  /*
   * Returns the current number of active heat distribution circuits.
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   Not used.
   * @param[out]  response  The current number of active heat distribution circuits.
   * @return      The gRPC status:
   *                - OK on success
   *                - INTERNAL on failure.
   */
  grpc::Status GetActiveCircuitCount(grpc::ServerContext* context, const google::protobuf::Empty* request,
                                     google::protobuf::UInt32Value* response) override;

  /*
   * Returns the current boosting schedule of the heat distribution circuit 3.
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   Not used.
   * @param[out]  response  The current boosting schedule of the heat distribution circuit 3.
   * @return      The gRPC status:
   *                - OK on success
   *                - INTERNAL on failure.
   */
  grpc::Status GetCircuit3BoostingSchedule(grpc::ServerContext* context, const google::protobuf::Empty* request,
                                           heat_pump::BoostingSchedule* response) override;

  /* Returns the current boosting schedule of the lower tank.
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   Not used.
   * @param[out]  response  The current boosting schedule of the lower tank.
   * @return      The gRPC status:
   *                - OK on success
   *                - INTERNAL on failure.
   */
  grpc::Status GetLowerTankBoostingSchedule(grpc::ServerContext* context, const google::protobuf::Empty* request,
                                            heat_pump::BoostingSchedule* response) override;

  /*
   * Returns the current temperatures of various parts of the heat pump.
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   Not used.
   * @param[out]  response  The current temperatures of various parts of the heat pump.
   * @return      The gRPC status:
   *                - OK on success
   *                - INTERNAL on failure.
   */
  grpc::Status GetTemperatures(grpc::ServerContext* context, const google::protobuf::Empty* request,
                               heat_pump::Temperatures* response) override;

  /*
   * Returns the current tank temperature limits which guide the heat pump in its operation.
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   Not used.
   * @param[out]  response  The current tank temperature limits which guide the heat pump in its operation.
   * @return      The gRPC status:
   *                - OK on success
   *                - INTERNAL on failure.
   */
  grpc::Status GetTankLimits(grpc::ServerContext* context, const google::protobuf::Empty* request,
                             heat_pump::TankLimits* response) override;

  /*
   * Returns whether the compressor is currently active.
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   Not used.
   * @param[out]  response  Whether the compressor is currently active.
   * @return      The gRPC status:
   *                - OK on success
   *                - INTERNAL on failure.
   */
  grpc::Status IsCompressorActive(grpc::ServerContext* context, const google::protobuf::Empty* request,
                                  google::protobuf::BoolValue* response) override;

  /*
   * Returns whether the scheduled boosting is enabled (distinct from it being currently active).
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   Not used.
   * @param[out]  response  Whether the scheduled boosting is enabled (distinct from it being currently active).
   * @return      The gRPC status:
   *                - OK on success
   *                - INTERNAL on failure.
   */
  grpc::Status IsSchedulingEnabled(grpc::ServerContext* context, const google::protobuf::Empty* request,
                                   google::protobuf::BoolValue* response) override;

  /*
   * Sets the current number of active heat distribution circuits.
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   The current number of active heat distribution circuits.
   * @param[out]  response  None, refer to the status.
   * @return      The gRPC status:
   *                - OK on success
   *                - INVALID_ARGUMENT on out-of-range count, must be in [0, 3].
   *                - INTERNAL on failure.
   */
  grpc::Status SetActiveCircuitCount(grpc::ServerContext* context, const google::protobuf::UInt32Value* request,
                                     google::protobuf::Empty* response) override;

  /*
   * Sets the boosting schedule of the heat distribution circuit 3.
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   The new boosting schedule of the heat distribution circuit 3.
   * @param[out]  response  None, refer to the status.
   * @return      The gRPC status:
   *                - OK on success
   *                - INVALID_ARGUMENT on malformed boosting schedule.
   *                - INTERNAL on failure.
   */
  grpc::Status SetCircuit3BoostingSchedule(grpc::ServerContext* context, const heat_pump::BoostingSchedule* request,
                                           google::protobuf::Empty* response) override;

  /*
   * Sets the boosting schedule of the lower tank.
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   The new boosting schedule of the lower tank.
   * @param[out]  response  None, refer to the status.
   * @return      The gRPC status:
   *                - OK on success
   *                - INVALID_ARGUMENT on malformed boosting schedule.
   *                - INTERNAL on failure.
   */
  grpc::Status SetLowerTankBoostingSchedule(grpc::ServerContext* context, const heat_pump::BoostingSchedule* request,
                                            google::protobuf::Empty* response) override;

  /*
   * Disables/Enables the scheduled boosting (distinct from it being currently active).
   * @param[in]   context   The internal gRPC context.
   * @param[in]   request   Whether the scheduled boosting should be enabled.
   * @param[out]  response  None, refer to the status.
   * @return      The gRPC status:
   *                - OK on success
   *                - INTERNAL on failure.
   */
  grpc::Status SetSchedulingEnabled(grpc::ServerContext* context, const google::protobuf::BoolValue* request,
                                    google::protobuf::Empty* response) override;

 private:
  std::unique_ptr<communicator::ICommunicator> communicator_{nullptr};
};
}  // namespace service
