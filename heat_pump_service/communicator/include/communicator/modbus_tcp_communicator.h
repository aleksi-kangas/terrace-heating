#pragma once

#include <cstdint>
#include <mutex>
#include <string>

#include <modbus.h>

#include "communicator/addresses.h"
#include "communicator/communicator.h"

namespace communicator {
class ModbusTCPCommunicator final : public ICommunicator {
 public:
  ModbusTCPCommunicator(const std::string& host, int32_t port);
  ~ModbusTCPCommunicator() override;

  [[nodiscard]] uint32_t ActiveCircuitCount() const override;
  [[nodiscard]] bool IsCompressorActive() const override;
  [[nodiscard]] bool IsSchedulingEnabled() const override;
  [[nodiscard]] BoostingSchedule ReadCircuit3BoostingSchedule() const override;
  [[nodiscard]] BoostingSchedule ReadLowerTankBoostingSchedule() const override;
  [[nodiscard]] Temperatures ReadTemperatures() const override;
  [[nodiscard]] TankLimits ReadTankLimits() const override;
  void WriteActiveCircuitCount(uint32_t count) override;

 private:
  modbus_t* context_{nullptr};
  mutable std::mutex mutex_{};

  [[nodiscard]] BoostingSchedule ReadBoostingSchedule(
      const addresses::boosting_schedules::BoostingScheduleAddresses& addresses)
      const;
};
}  // namespace communicator
