#include "service/service.h"

#include <memory>
#include <utility>

#include <gtest/gtest.h>

#include "mock.h"

using namespace ::testing;
using namespace service;

constexpr communicator::BoostingSchedule kCircuit3BoostingSchedule{
    .monday = {1, 18, -1},
    .tuesday = {2, 19, -2},
    .wednesday = {3, 20, -3},
    .thursday = {4, 21, -4},
    .friday = {5, 22, -5},
    .saturday = {6, 23, -6},
    .sunday = {7, 24, -7}};

constexpr communicator::BoostingSchedule kLowerTankBoostingSchedule{
    .monday = {8, 15, 1},
    .tuesday = {9, 16, 2},
    .wednesday = {10, 17, 3},
    .thursday = {11, 18, 4},
    .friday = {12, 19, 5},
    .saturday = {13, 20, 6},
    .sunday = {14, 21, 7}};

class HeatPumpServiceTest : public Test {
 protected:
  HeatPumpServiceTest() {
    service_ = std::make_unique<HeatPumpService>(std::make_unique<MockCommunicator::Factory>());
  }
  std::unique_ptr<HeatPumpService> service_{nullptr};

  MockCommunicator* GetMockCommunicator() {
    return dynamic_cast<MockCommunicator*>(service_->GetCommunicator());
  }
};

TEST_F(HeatPumpServiceTest, GetActiveCircuitCount) {
  EXPECT_CALL(*GetMockCommunicator(), ActiveCircuitCount())
      .Times(1)
      .WillOnce(Return(3));
  google::protobuf::Empty request{};
  google::protobuf::UInt32Value response{};
  grpc::ServerContext context{};
  const auto status =
      service_->GetActiveCircuitCount(&context, &request, &response);
  EXPECT_TRUE(status.ok());
  EXPECT_EQ(response.value(), 3);
}

TEST_F(HeatPumpServiceTest, GetActiveCircuitCountFailure) {
  EXPECT_CALL(*GetMockCommunicator(), ActiveCircuitCount())
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  google::protobuf::Empty request{};
  google::protobuf::UInt32Value response{};
  grpc::ServerContext context{};
  const auto status =
      service_->GetActiveCircuitCount(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}

TEST_F(HeatPumpServiceTest, GetCircuit3BoostingSchedule) {
  EXPECT_CALL(*GetMockCommunicator(), ReadCircuit3BoostingSchedule())
      .Times(1)
      .WillOnce(Return(kCircuit3BoostingSchedule));

  google::protobuf::Empty request{};
  heat_pump::BoostingSchedule response{};
  grpc::ServerContext context{};
  const auto status =
      service_->GetCircuit3BoostingSchedule(&context, &request, &response);
  EXPECT_TRUE(status.ok());
  // clang-format off
  EXPECT_EQ(response.monday().start_hour(), kCircuit3BoostingSchedule.monday.start_hour);
  EXPECT_EQ(response.monday().end_hour(), kCircuit3BoostingSchedule.monday.end_hour);
  EXPECT_EQ(response.monday().temperature_delta(), kCircuit3BoostingSchedule.monday.temperature_delta);

  EXPECT_EQ(response.tuesday().start_hour(), kCircuit3BoostingSchedule.tuesday.start_hour);
  EXPECT_EQ(response.tuesday().end_hour(), kCircuit3BoostingSchedule.tuesday.end_hour);
  EXPECT_EQ(response.tuesday().temperature_delta(), kCircuit3BoostingSchedule.tuesday.temperature_delta);

  EXPECT_EQ(response.wednesday().start_hour(), kCircuit3BoostingSchedule.wednesday.start_hour);
  EXPECT_EQ(response.wednesday().end_hour(), kCircuit3BoostingSchedule.wednesday.end_hour);
  EXPECT_EQ(response.wednesday().temperature_delta(), kCircuit3BoostingSchedule.wednesday.temperature_delta);

  EXPECT_EQ(response.thursday().start_hour(), kCircuit3BoostingSchedule.thursday.start_hour);
  EXPECT_EQ(response.thursday().end_hour(), kCircuit3BoostingSchedule.thursday.end_hour);
  EXPECT_EQ(response.thursday().temperature_delta(), kCircuit3BoostingSchedule.thursday.temperature_delta);

  EXPECT_EQ(response.friday().start_hour(), kCircuit3BoostingSchedule.friday.start_hour);
  EXPECT_EQ(response.friday().end_hour(), kCircuit3BoostingSchedule.friday.end_hour);
  EXPECT_EQ(response.friday().temperature_delta(), kCircuit3BoostingSchedule.friday.temperature_delta);

  EXPECT_EQ(response.saturday().start_hour(), kCircuit3BoostingSchedule.saturday.start_hour);
  EXPECT_EQ(response.saturday().end_hour(), kCircuit3BoostingSchedule.saturday.end_hour);
  EXPECT_EQ(response.saturday().temperature_delta(), kCircuit3BoostingSchedule.saturday.temperature_delta);

  EXPECT_EQ(response.sunday().start_hour(), kCircuit3BoostingSchedule.sunday.start_hour);
  EXPECT_EQ(response.sunday().end_hour(), kCircuit3BoostingSchedule.sunday.end_hour);
  EXPECT_EQ(response.sunday().temperature_delta(), kCircuit3BoostingSchedule.sunday.temperature_delta);
  // clang-format on
}

TEST_F(HeatPumpServiceTest, GetCircuit3BoostingScheduleFailure) {
  EXPECT_CALL(*GetMockCommunicator(), ReadCircuit3BoostingSchedule())
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  google::protobuf::Empty request{};
  heat_pump::BoostingSchedule response{};
  grpc::ServerContext context{};
  const auto status =
      service_->GetCircuit3BoostingSchedule(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}

TEST_F(HeatPumpServiceTest, GetLowerTankBoostingSchedule) {
  EXPECT_CALL(*GetMockCommunicator(), ReadLowerTankBoostingSchedule())
      .Times(1)
      .WillOnce(Return(kLowerTankBoostingSchedule));

  google::protobuf::Empty request{};
  heat_pump::BoostingSchedule response{};
  grpc::ServerContext context{};
  const auto status =
      service_->GetLowerTankBoostingSchedule(&context, &request, &response);
  EXPECT_TRUE(status.ok());
  // clang-format off
  EXPECT_EQ(response.monday().start_hour(), kLowerTankBoostingSchedule.monday.start_hour);
  EXPECT_EQ(response.monday().end_hour(), kLowerTankBoostingSchedule.monday.end_hour);
  EXPECT_EQ(response.monday().temperature_delta(), kLowerTankBoostingSchedule.monday.temperature_delta);

  EXPECT_EQ(response.tuesday().start_hour(), kLowerTankBoostingSchedule.tuesday.start_hour);
  EXPECT_EQ(response.tuesday().end_hour(), kLowerTankBoostingSchedule.tuesday.end_hour);
  EXPECT_EQ(response.tuesday().temperature_delta(), kLowerTankBoostingSchedule.tuesday.temperature_delta);

  EXPECT_EQ(response.wednesday().start_hour(), kLowerTankBoostingSchedule.wednesday.start_hour);
  EXPECT_EQ(response.wednesday().end_hour(), kLowerTankBoostingSchedule.wednesday.end_hour);
  EXPECT_EQ(response.wednesday().temperature_delta(), kLowerTankBoostingSchedule.wednesday.temperature_delta);

  EXPECT_EQ(response.thursday().start_hour(), kLowerTankBoostingSchedule.thursday.start_hour);
  EXPECT_EQ(response.thursday().end_hour(), kLowerTankBoostingSchedule.thursday.end_hour);
  EXPECT_EQ(response.thursday().temperature_delta(), kLowerTankBoostingSchedule.thursday.temperature_delta);

  EXPECT_EQ(response.friday().start_hour(), kLowerTankBoostingSchedule.friday.start_hour);
  EXPECT_EQ(response.friday().end_hour(), kLowerTankBoostingSchedule.friday.end_hour);
  EXPECT_EQ(response.friday().temperature_delta(), kLowerTankBoostingSchedule.friday.temperature_delta);

  EXPECT_EQ(response.saturday().start_hour(), kLowerTankBoostingSchedule.saturday.start_hour);
  EXPECT_EQ(response.saturday().end_hour(), kLowerTankBoostingSchedule.saturday.end_hour);
  EXPECT_EQ(response.saturday().temperature_delta(), kLowerTankBoostingSchedule.saturday.temperature_delta);

  EXPECT_EQ(response.sunday().start_hour(), kLowerTankBoostingSchedule.sunday.start_hour);
  EXPECT_EQ(response.sunday().end_hour(), kLowerTankBoostingSchedule.sunday.end_hour);
  EXPECT_EQ(response.sunday().temperature_delta(), kLowerTankBoostingSchedule.sunday.temperature_delta);
  // clang-format on
}

TEST_F(HeatPumpServiceTest, GetLowerTankBoostingScheduleFailure) {
  EXPECT_CALL(*GetMockCommunicator(), ReadLowerTankBoostingSchedule())
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  google::protobuf::Empty request{};
  heat_pump::BoostingSchedule response{};
  grpc::ServerContext context{};
  const auto status =
      service_->GetLowerTankBoostingSchedule(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}

TEST_F(HeatPumpServiceTest, GetTemperatures) {
  EXPECT_CALL(*GetMockCommunicator(), ReadTemperatures())
      .Times(1)
      .WillOnce(
          Return(communicator::Temperatures{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}));

  google::protobuf::Empty request{};
  heat_pump::Temperatures response{};
  grpc::ServerContext context{};
  const auto status = service_->GetTemperatures(&context, &request, &response);
  EXPECT_TRUE(status.ok());
  EXPECT_EQ(response.circuit1(), 1);
  EXPECT_EQ(response.circuit2(), 2);
  EXPECT_EQ(response.circuit3(), 3);
  EXPECT_EQ(response.ground_input(), 4);
  EXPECT_EQ(response.ground_output(), 5);
  EXPECT_EQ(response.hot_gas(), 6);
  EXPECT_EQ(response.inside(), 7);
  EXPECT_EQ(response.lower_tank(), 8);
  EXPECT_EQ(response.outside(), 9);
  EXPECT_EQ(response.upper_tank(), 10);
}

TEST_F(HeatPumpServiceTest, GetTemperaturesFailure) {
  EXPECT_CALL(*GetMockCommunicator(), ReadTemperatures())
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  google::protobuf::Empty request{};
  heat_pump::Temperatures response{};
  grpc::ServerContext context{};
  const auto status = service_->GetTemperatures(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}

TEST_F(HeatPumpServiceTest, GetTankLimits) {
  EXPECT_CALL(*GetMockCommunicator(), ReadTankLimits())
      .Times(1)
      .WillOnce(Return(communicator::TankLimits{1, 2, 3, 4, 5, 6, 7, 8}));

  google::protobuf::Empty request{};
  heat_pump::TankLimits response{};
  grpc::ServerContext context{};
  const auto status = service_->GetTankLimits(&context, &request, &response);
  EXPECT_TRUE(status.ok());
  EXPECT_EQ(response.lower_tank_minimum(), 1);
  EXPECT_EQ(response.lower_tank_minimum_adjusted(), 2);
  EXPECT_EQ(response.lower_tank_maximum(), 3);
  EXPECT_EQ(response.lower_tank_maximum_adjusted(), 4);
  EXPECT_EQ(response.upper_tank_minimum(), 5);
  EXPECT_EQ(response.upper_tank_minimum_adjusted(), 6);
  EXPECT_EQ(response.upper_tank_maximum(), 7);
  EXPECT_EQ(response.upper_tank_maximum_adjusted(), 8);
}

TEST_F(HeatPumpServiceTest, GetTankLimitsFailure) {
  EXPECT_CALL(*GetMockCommunicator(), ReadTankLimits())
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  google::protobuf::Empty request{};
  heat_pump::TankLimits response{};
  grpc::ServerContext context{};
  const auto status = service_->GetTankLimits(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}

TEST_F(HeatPumpServiceTest, IsCompressorActive) {
  EXPECT_CALL(*GetMockCommunicator(), IsCompressorActive())
      .Times(1)
      .WillOnce(Return(true));

  google::protobuf::Empty request{};
  google::protobuf::BoolValue response{};
  grpc::ServerContext context{};
  const auto status =
      service_->IsCompressorActive(&context, &request, &response);
  EXPECT_TRUE(status.ok());
  EXPECT_TRUE(response.value());
}

TEST_F(HeatPumpServiceTest, IsCompressorActiveFailure) {
  EXPECT_CALL(*GetMockCommunicator(), IsCompressorActive())
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  google::protobuf::Empty request{};
  google::protobuf::BoolValue response{};
  grpc::ServerContext context{};
  const auto status =
      service_->IsCompressorActive(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}

TEST_F(HeatPumpServiceTest, IsSchedulingEnabled) {
  EXPECT_CALL(*GetMockCommunicator(), IsSchedulingEnabled())
      .Times(1)
      .WillOnce(Return(true));

  google::protobuf::Empty request{};
  google::protobuf::BoolValue response{};
  grpc::ServerContext context{};
  const auto status =
      service_->IsSchedulingEnabled(&context, &request, &response);
  EXPECT_TRUE(status.ok());
  EXPECT_TRUE(response.value());
}

TEST_F(HeatPumpServiceTest, IsSchedulingEnabledFailure) {
  EXPECT_CALL(*GetMockCommunicator(), IsSchedulingEnabled())
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  google::protobuf::Empty request{};
  google::protobuf::BoolValue response{};
  grpc::ServerContext context{};
  const auto status =
      service_->IsSchedulingEnabled(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}

TEST_F(HeatPumpServiceTest, SetActiveCircuitCount) {
  EXPECT_CALL(*GetMockCommunicator(), WriteActiveCircuitCount(3))
      .Times(1);

  google::protobuf::UInt32Value request{};
  request.set_value(3);
  google::protobuf::Empty response{};
  grpc::ServerContext context{};
  const auto status =
      service_->SetActiveCircuitCount(&context, &request, &response);
  EXPECT_TRUE(status.ok());
}

TEST_F(HeatPumpServiceTest, SetActiveCircuitCountFailure) {
  EXPECT_CALL(*GetMockCommunicator(), WriteActiveCircuitCount(_))
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  google::protobuf::UInt32Value request{};
  request.set_value(3);
  google::protobuf::Empty response{};
  grpc::ServerContext context{};
  const auto status =
      service_->SetActiveCircuitCount(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}

TEST_F(HeatPumpServiceTest, SetActiveCircuitCountInvalidArgument) {
  EXPECT_CALL(*GetMockCommunicator(), WriteActiveCircuitCount(_))
      .Times(0);

  google::protobuf::UInt32Value request{};
  request.set_value(4);
  google::protobuf::Empty response{};
  grpc::ServerContext context{};
  const auto status =
      service_->SetActiveCircuitCount(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INVALID_ARGUMENT);
}

TEST_F(HeatPumpServiceTest, SetCircuit3BoostingSchedule) {
  EXPECT_CALL(*GetMockCommunicator(), WriteCircuit3BoostingSchedule(_))
      .Times(1);

  heat_pump::BoostingSchedule request{};
  request.mutable_monday()->set_start_hour(1);
  request.mutable_monday()->set_end_hour(18);
  request.mutable_monday()->set_temperature_delta(-1);
  request.mutable_tuesday()->set_start_hour(2);
  request.mutable_tuesday()->set_end_hour(19);
  request.mutable_tuesday()->set_temperature_delta(-2);
  request.mutable_wednesday()->set_start_hour(3);
  request.mutable_wednesday()->set_end_hour(20);
  request.mutable_wednesday()->set_temperature_delta(-3);
  request.mutable_thursday()->set_start_hour(4);
  request.mutable_thursday()->set_end_hour(21);
  request.mutable_thursday()->set_temperature_delta(-4);
  request.mutable_friday()->set_start_hour(5);
  request.mutable_friday()->set_end_hour(22);
  request.mutable_friday()->set_temperature_delta(-5);
  request.mutable_saturday()->set_start_hour(6);
  request.mutable_saturday()->set_end_hour(23);
  request.mutable_saturday()->set_temperature_delta(-6);
  request.mutable_sunday()->set_start_hour(7);
  request.mutable_sunday()->set_end_hour(24);
  request.mutable_sunday()->set_temperature_delta(-7);

  google::protobuf::Empty response{};
  grpc::ServerContext context{};
  const auto status =
      service_->SetCircuit3BoostingSchedule(&context, &request, &response);
  EXPECT_TRUE(status.ok());
}

TEST_F(HeatPumpServiceTest, SetCircuit3BoostingScheduleFailure) {
  EXPECT_CALL(*GetMockCommunicator(), WriteCircuit3BoostingSchedule(_))
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  heat_pump::BoostingSchedule request{};
  google::protobuf::Empty response{};
  grpc::ServerContext context{};
  const auto status =
      service_->SetCircuit3BoostingSchedule(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}

TEST_F(HeatPumpServiceTest, SetCircuit3BoostingScheduleArgument) {
  EXPECT_CALL(*GetMockCommunicator(), WriteCircuit3BoostingSchedule(_)).Times(0);

  heat_pump::BoostingSchedule request{};
  request.mutable_monday()->set_start_hour(25);
  request.mutable_monday()->set_temperature_delta(-11);

  google::protobuf::Empty response{};
  grpc::ServerContext context{};
  const auto status =
      service_->SetCircuit3BoostingSchedule(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INVALID_ARGUMENT);
}

TEST_F(HeatPumpServiceTest, SetSchedulingEnabled) {
  EXPECT_CALL(*GetMockCommunicator(), WriteSchedulingEnabled(true)).Times(1);

  google::protobuf::BoolValue request{};
  request.set_value(true);
  google::protobuf::Empty response{};
  grpc::ServerContext context{};
  const auto status =
      service_->SetSchedulingEnabled(&context, &request, &response);
  EXPECT_TRUE(status.ok());
}

TEST_F(HeatPumpServiceTest, SetSchedulingEnabledFailure) {
  EXPECT_CALL(*GetMockCommunicator(), WriteSchedulingEnabled(true))
      .Times(1)
      .WillOnce(Throw(std::runtime_error{"test"}));

  google::protobuf::BoolValue request{};
  request.set_value(true);
  google::protobuf::Empty response{};
  grpc::ServerContext context{};
  const auto status =
      service_->SetSchedulingEnabled(&context, &request, &response);
  EXPECT_FALSE(status.ok());
  EXPECT_EQ(status.error_code(), grpc::StatusCode::INTERNAL);
}
