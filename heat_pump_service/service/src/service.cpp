#include "service/service.h"

#include <utility>

namespace service {
HeatPumpService::HeatPumpService(
    std::unique_ptr<communicator::ICommunicator> communicator)
    : communicator_{std::move(communicator)} {}

grpc::Status HeatPumpService::GetTemperatures(
    grpc::ServerContext* context, const google::protobuf::Empty* request,
    Temperatures* response) {
  try {
    const auto temperatures = communicator_->ReadTemperatures();
    response->set_circuit1(temperatures.circuit1);
    response->set_circuit2(temperatures.circuit2);
    response->set_circuit3(temperatures.circuit3);
    response->set_ground_input(temperatures.ground_input);
    response->set_ground_output(temperatures.ground_output);
    response->set_hot_gas(temperatures.hot_gas);
    response->set_inside(temperatures.inside);
    response->set_lower_tank(temperatures.lower_tank);
    response->set_outside(temperatures.outside);
    response->set_upper_tank(temperatures.upper_tank);
    return grpc::Status::OK;
  } catch (const std::exception& e) {
    return grpc::Status(grpc::StatusCode::INTERNAL, e.what());
  }
}

grpc::Status HeatPumpService::GetTankLimits(
    grpc::ServerContext* context, const google::protobuf::Empty* request,
    TankLimits* response) {
  try {
    const auto tank_limits = communicator_->ReadTankLimits();
    response->set_lower_tank_minimum(tank_limits.lower_tank_minimum);
    response->set_lower_tank_maximum(tank_limits.lower_tank_maximum);
    response->set_upper_tank_minimum(tank_limits.upper_tank_minimum);
    response->set_upper_tank_maximum(tank_limits.upper_tank_maximum);
    return grpc::Status::OK;
  } catch (const std::exception& e) {
    return grpc::Status(grpc::StatusCode::INTERNAL, e.what());
  }
}

}  // namespace service
