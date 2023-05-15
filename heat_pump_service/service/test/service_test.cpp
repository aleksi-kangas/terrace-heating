#include "service/service.h"

#include <memory>
#include <utility>

#include <gtest/gtest.h>

#include "mock.h"

using namespace ::testing;
using namespace service;

class HeatPumpServiceTest : public Test {
 protected:
  HeatPumpServiceTest() {
    auto mockCommunicator = std::make_unique<MockCommunicator>();
    mockCommunicator_ = mockCommunicator.get();
    service_ = std::make_unique<HeatPumpService>(std::move(mockCommunicator));
  }

  MockCommunicator* mockCommunicator_;
  std::unique_ptr<HeatPumpService> service_{nullptr};
};

TEST_F(HeatPumpServiceTest, GetActiveCircuitCount) {
  EXPECT_CALL(*mockCommunicator_, ActiveCircuitCount())
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
  EXPECT_CALL(*mockCommunicator_, ActiveCircuitCount())
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

TEST_F(HeatPumpServiceTest, GetTemperatures) {
  EXPECT_CALL(*mockCommunicator_, ReadTemperatures())
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
  EXPECT_CALL(*mockCommunicator_, ReadTemperatures())
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
  EXPECT_CALL(*mockCommunicator_, ReadTankLimits())
      .Times(1)
      .WillOnce(Return(communicator::TankLimits{1, 2, 3, 4}));

  google::protobuf::Empty request{};
  heat_pump::TankLimits response{};
  grpc::ServerContext context{};
  const auto status = service_->GetTankLimits(&context, &request, &response);
  EXPECT_TRUE(status.ok());
  EXPECT_EQ(response.lower_tank_minimum(), 1);
  EXPECT_EQ(response.lower_tank_maximum(), 2);
  EXPECT_EQ(response.upper_tank_minimum(), 3);
  EXPECT_EQ(response.upper_tank_maximum(), 4);
}

TEST_F(HeatPumpServiceTest, GetTankLimitsFailure) {
  EXPECT_CALL(*mockCommunicator_, ReadTankLimits())
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
  EXPECT_CALL(*mockCommunicator_, IsCompressorActive())
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
  EXPECT_CALL(*mockCommunicator_, IsCompressorActive())
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
  EXPECT_CALL(*mockCommunicator_, IsSchedulingEnabled())
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
  EXPECT_CALL(*mockCommunicator_, IsSchedulingEnabled())
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
