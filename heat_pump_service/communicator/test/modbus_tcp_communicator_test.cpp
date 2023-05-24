#include "communicator/modbus_tcp_communicator.h"

#include <thread>

#include <gtest/gtest.h>

#include "communicator/addresses.h"

#include "mock_modbus_tcp_server.h"

using namespace ::testing;
using namespace communicator;

class ModbusTCPCommunicatorTest : public Test {
 protected:
  static void SetUpTestSuite() {
    mock_server = new MockModbusTCPServer{stop};
    mock_server_thread = mock_server->Run();
    mock_server_thread.detach();
  }

  static void TearDownTestSuite() {
    stop = true;
    delete mock_server;
    mock_server = nullptr;
  }

  void SetUp() override { mock_server->InitializeMapping(); }

 public:
  static std::atomic<bool> stop;
  static MockModbusTCPServer* mock_server;
  static std::thread mock_server_thread;
};

std::atomic<bool> ModbusTCPCommunicatorTest::stop = false;
MockModbusTCPServer* ModbusTCPCommunicatorTest::mock_server = nullptr;
std::thread ModbusTCPCommunicatorTest::mock_server_thread;

TEST_F(ModbusTCPCommunicatorTest, ActiveCircuitCount) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  EXPECT_EQ(communicator.ActiveCircuitCount(), kActiveCircuitCount);
}

TEST_F(ModbusTCPCommunicatorTest, IsCompressorActive) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  EXPECT_EQ(communicator.IsCompressorActive(), kIsCompressorActive);
}

TEST_F(ModbusTCPCommunicatorTest, IsSchedulingEnabled) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  EXPECT_EQ(communicator.IsSchedulingEnabled(), kIsSchedulingEnabled);
}

TEST_F(ModbusTCPCommunicatorTest, ReadCircuit3BoostingSchedule) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  const auto schedule = communicator.ReadCircuit3BoostingSchedule();
  EXPECT_EQ(schedule, kCircuit3BoostingSchedule);
}

TEST_F(ModbusTCPCommunicatorTest, ReadLowerTankBoostingSchedule) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  const auto schedule = communicator.ReadLowerTankBoostingSchedule();
  EXPECT_EQ(schedule, kLowerTankBoostingSchedule);
}

TEST_F(ModbusTCPCommunicatorTest, ReadTankLimitsAdjusted) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  const auto tank_limits = communicator.ReadTankLimits();
  EXPECT_EQ(tank_limits, kTankLimits);
}

TEST_F(ModbusTCPCommunicatorTest, ReadTemperatures) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  const auto temperatures = communicator.ReadTemperatures();
  EXPECT_EQ(temperatures.circuit1, kCircuit1Temperature);
  EXPECT_EQ(temperatures.circuit2, kCircuit2Temperature);
  EXPECT_EQ(temperatures.circuit3, kCircuit3Temperature);
  EXPECT_EQ(temperatures.ground_input, kGroundInputTemperature);
  EXPECT_EQ(temperatures.ground_output, kGroundOutputTemperature);
  EXPECT_EQ(temperatures.hot_gas, kHotGasTemperature);
  EXPECT_EQ(temperatures.inside, kInsideTemperature);
  EXPECT_EQ(temperatures.lower_tank, kLowerTankTemperature);
  EXPECT_EQ(temperatures.outside, kOutsideTemperature);
  EXPECT_EQ(temperatures.upper_tank, kUpperTankTemperature);
}

TEST_F(ModbusTCPCommunicatorTest, WriteActiveCircuitCount) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  EXPECT_EQ(communicator.ActiveCircuitCount(), kActiveCircuitCount);
  const uint32_t new_active_circuit_count = kActiveCircuitCount - 1;
  communicator.WriteActiveCircuitCount(new_active_circuit_count);
  EXPECT_EQ(communicator.ActiveCircuitCount(), new_active_circuit_count);
}

TEST_F(ModbusTCPCommunicatorTest, WriteCircuit3BoostingSchedule) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  EXPECT_EQ(communicator.ReadCircuit3BoostingSchedule(), kCircuit3BoostingSchedule);
  constexpr communicator::BoostingSchedule new_schedule{
      .monday = {0, 7, -8},
      .tuesday = {1, 8, -9},
      .wednesday = {2, 9, -10},
      .thursday = {3, 10, -7},
      .friday = {4, 11, -6},
      .saturday = {5, 12, -5},
      .sunday = {6, 13, -4}};
  communicator.WriteCircuit3BoostingSchedule(new_schedule);
  EXPECT_EQ(communicator.ReadCircuit3BoostingSchedule(), new_schedule);
}

TEST_F(ModbusTCPCommunicatorTest, WriteLowerTankBoostingSchedule) {
  ASSERT_NE(ModbusTCPCommunicatorTest::mock_server, nullptr);
  ModbusTCPCommunicator communicator{MockModbusTCPServer::Host(),
                                     MockModbusTCPServer::Port()};
  EXPECT_EQ(communicator.ReadLowerTankBoostingSchedule(), kLowerTankBoostingSchedule);
  constexpr communicator::BoostingSchedule new_schedule{
      .monday = {1, 8, -1},
      .tuesday = {2, 9, -2},
      .wednesday = {3, 10, -3},
      .thursday = {4, 11, -4},
      .friday = {5, 12, -5},
      .saturday = {6, 13, -6},
      .sunday = {7, 14, -7}};
  communicator.WriteLowerTankBoostingSchedule(new_schedule);
  EXPECT_EQ(communicator.ReadLowerTankBoostingSchedule(), new_schedule);
}
