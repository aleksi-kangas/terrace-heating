#pragma once

#include <memory>

#include <grpcpp/grpcpp.h>

#include "communicator/communicator.h"
#include "heat_pump_service.grpc.pb.h"
#include "heat_pump_service.pb.h"

namespace service {
class HeatPumpService final : public heat_pump::HeatPumpSvc::Service {
 public:
  explicit HeatPumpService(
      std::unique_ptr<communicator::ICommunicator> communicator);
  ~HeatPumpService() override = default;

  grpc::Status GetActiveCircuitCount(
      grpc::ServerContext* context, const google::protobuf::Empty* request,
      google::protobuf::UInt32Value* response) override;

  grpc::Status GetCircuit3BoostingSchedule(
      grpc::ServerContext* context, const google::protobuf::Empty* request,
      heat_pump::BoostingSchedule* response) override;

  grpc::Status GetLowerTankBoostingSchedule(
      grpc::ServerContext* context, const google::protobuf::Empty* request,
      heat_pump::BoostingSchedule* response) override;

  grpc::Status GetTemperatures(grpc::ServerContext* context,
                               const google::protobuf::Empty* request,
                               heat_pump::Temperatures* response) override;

  grpc::Status GetTankLimits(grpc::ServerContext* context,
                             const google::protobuf::Empty* request,
                             heat_pump::TankLimits* response) override;

  grpc::Status IsCompressorActive(
      grpc::ServerContext* context, const google::protobuf::Empty* request,
      google::protobuf::BoolValue* response) override;

  grpc::Status IsSchedulingEnabled(
      grpc::ServerContext* context, const google::protobuf::Empty* request,
      google::protobuf::BoolValue* response) override;

  grpc::Status SetActiveCircuitCount(
      grpc::ServerContext* context,
      const google::protobuf::UInt32Value* request,
      google::protobuf::Empty* response) override;

  grpc::Status SetCircuit3BoostingSchedule(
      grpc::ServerContext* context, const heat_pump::BoostingSchedule* request,
      google::protobuf::Empty* response) override;

  grpc::Status SetLowerTankBoostingSchedule(
      grpc::ServerContext* context, const heat_pump::BoostingSchedule* request,
      google::protobuf::Empty* response) override;

 private:
  std::unique_ptr<communicator::ICommunicator> communicator_{nullptr};
};
}  // namespace service
