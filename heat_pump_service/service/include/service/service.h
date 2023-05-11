#pragma once

#include <memory>

#include <grpcpp/grpcpp.h>

#include "communicator/communicator.h"
#include "heat_pump_service.grpc.pb.h"
#include "heat_pump_service.pb.h"

namespace service {
class HeatPumpService final : public HeatPumpSvc::Service {
 public:
  explicit HeatPumpService(
      std::unique_ptr<communicator::ICommunicator> communicator);
  ~HeatPumpService() override = default;

  grpc::Status GetActiveCircuitCount(
      grpc::ServerContext* context, const google::protobuf::Empty* request,
      google::protobuf::UInt32Value* response) override;

  grpc::Status GetTemperatures(grpc::ServerContext* context,
                               const google::protobuf::Empty* request,
                               Temperatures* response) override;

  grpc::Status GetTankLimits(grpc::ServerContext* context,
                             const google::protobuf::Empty* request,
                             TankLimits* response) override;

 private:
  std::unique_ptr<communicator::ICommunicator> communicator_{nullptr};
};
}  // namespace service
