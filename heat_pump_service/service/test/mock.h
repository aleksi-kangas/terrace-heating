#pragma once

#include <gmock/gmock.h>

#include "communicator/communicator.h"

class MockCommunicator : public communicator::ICommunicator {
 public:
  MOCK_METHOD(uint32_t, ActiveCircuitCount, (), (const, override));
  MOCK_METHOD(communicator::BoostingSchedule, ReadCircuit3BoostingSchedule, (),
              (const, override));
  MOCK_METHOD(communicator::BoostingSchedule, ReadLowerTankBoostingSchedule, (),
              (const, override));
  MOCK_METHOD(communicator::Temperatures, ReadTemperatures, (),
              (const, override));
  MOCK_METHOD(communicator::TankLimits, ReadTankLimits, (), (const, override));
  MOCK_METHOD(bool, IsCompressorActive, (), (const, override));
  MOCK_METHOD(bool, IsSchedulingEnabled, (), (const, override));
  MOCK_METHOD(void, WriteActiveCircuitCount, (uint32_t count), (override));
  MOCK_METHOD(void, WriteCircuit3BoostingSchedule, (const communicator::BoostingSchedule& schedule), (override));
  MOCK_METHOD(void, WriteLowerTankBoostingSchedule, (const communicator::BoostingSchedule& schedule), (override));
  MOCK_METHOD(void, WriteSchedulingEnabled, (bool scheduling_enabled), (override));
};
