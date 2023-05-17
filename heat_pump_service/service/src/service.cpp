#include "service/service.h"

#include <utility>

namespace service {
HeatPumpService::HeatPumpService(
    std::unique_ptr<communicator::ICommunicator> communicator)
    : communicator_{std::move(communicator)} {}

grpc::Status HeatPumpService::GetActiveCircuitCount(
    grpc::ServerContext* /* context */,
    const google::protobuf::Empty* /* request */,
    google::protobuf::UInt32Value* response) {
  try {
    response->set_value(communicator_->ActiveCircuitCount());
    return grpc::Status::OK;
  } catch (const std::exception& e) {
    return {grpc::StatusCode::INTERNAL, e.what()};
  }
}

grpc::Status HeatPumpService::GetCircuit3BoostingSchedule(
    grpc::ServerContext* /* context */,
    const google::protobuf::Empty* /* request */,
    heat_pump::BoostingSchedule* response) {
  try {
    const auto schedule = communicator_->ReadCircuit3BoostingSchedule();

    response->mutable_monday()->set_start_hour(schedule.monday.start_hour);
    response->mutable_monday()->set_end_hour(schedule.monday.end_hour);
    response->mutable_monday()->set_temperature_delta(
        schedule.monday.temperature_delta);

    response->mutable_tuesday()->set_start_hour(schedule.tuesday.start_hour);
    response->mutable_tuesday()->set_end_hour(schedule.tuesday.end_hour);
    response->mutable_tuesday()->set_temperature_delta(
        schedule.tuesday.temperature_delta);

    response->mutable_wednesday()->set_start_hour(
        schedule.wednesday.start_hour);
    response->mutable_wednesday()->set_end_hour(schedule.wednesday.end_hour);
    response->mutable_wednesday()->set_temperature_delta(
        schedule.wednesday.temperature_delta);

    response->mutable_thursday()->set_start_hour(schedule.thursday.start_hour);
    response->mutable_thursday()->set_end_hour(schedule.thursday.end_hour);
    response->mutable_thursday()->set_temperature_delta(
        schedule.thursday.temperature_delta);

    response->mutable_friday()->set_start_hour(schedule.friday.start_hour);
    response->mutable_friday()->set_end_hour(schedule.friday.end_hour);
    response->mutable_friday()->set_temperature_delta(
        schedule.friday.temperature_delta);

    response->mutable_saturday()->set_start_hour(schedule.saturday.start_hour);
    response->mutable_saturday()->set_end_hour(schedule.saturday.end_hour);
    response->mutable_saturday()->set_temperature_delta(
        schedule.saturday.temperature_delta);

    response->mutable_sunday()->set_start_hour(schedule.sunday.start_hour);
    response->mutable_sunday()->set_end_hour(schedule.sunday.end_hour);
    response->mutable_sunday()->set_temperature_delta(
        schedule.sunday.temperature_delta);

    return grpc::Status::OK;
  } catch (const std::exception& e) {
    return {grpc::StatusCode::INTERNAL, e.what()};
  }
}

grpc::Status HeatPumpService::GetTemperatures(
    grpc::ServerContext* /* context */,
    const google::protobuf::Empty* /* request */,
    heat_pump::Temperatures* response) {
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
    return {grpc::StatusCode::INTERNAL, e.what()};
  }
}

grpc::Status HeatPumpService::GetTankLimits(
    grpc::ServerContext* /* context */,
    const google::protobuf::Empty* /* request */,
    heat_pump::TankLimits* response) {
  try {
    const auto tank_limits = communicator_->ReadTankLimits();
    response->set_lower_tank_minimum(tank_limits.lower_tank_minimum);
    response->set_lower_tank_maximum(tank_limits.lower_tank_maximum);
    response->set_upper_tank_minimum(tank_limits.upper_tank_minimum);
    response->set_upper_tank_maximum(tank_limits.upper_tank_maximum);
    return grpc::Status::OK;
  } catch (const std::exception& e) {
    return {grpc::StatusCode::INTERNAL, e.what()};
  }
}

grpc::Status HeatPumpService::IsCompressorActive(
    grpc::ServerContext* /* context */,
    const google::protobuf::Empty* /* request */,
    google::protobuf::BoolValue* response) {
  try {
    response->set_value(communicator_->IsCompressorActive());
    return grpc::Status::OK;
  } catch (const std::exception& e) {
    return {grpc::StatusCode::INTERNAL, e.what()};
  }
}

grpc::Status HeatPumpService::IsSchedulingEnabled(
    grpc::ServerContext* /* context */,
    const google::protobuf::Empty* /* request */,
    google::protobuf::BoolValue* response) {
  try {
    response->set_value(communicator_->IsSchedulingEnabled());
    return grpc::Status::OK;
  } catch (const std::exception& e) {
    return {grpc::StatusCode::INTERNAL, e.what()};
  }
}

grpc::Status HeatPumpService::SetActiveCircuitCount(
    grpc::ServerContext* /* context */,
    const google::protobuf::UInt32Value* request,
    google::protobuf::Empty* /* response */) {
  try {
    if (request->value() > 3) {
      return {grpc::StatusCode::INVALID_ARGUMENT,
              "Active circuit count must be between 0 and 3"};
    }
    communicator_->WriteActiveCircuitCount(request->value());
    return grpc::Status::OK;
  } catch (const std::exception& e) {
    return {grpc::StatusCode::INTERNAL, e.what()};
  }
}

}  // namespace service
