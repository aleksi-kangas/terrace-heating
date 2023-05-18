#pragma once

#include <cstdint>
#include <mutex>
#include <string>

#include <modbus.h>

#include "communicator/communicator.h"
#include "communicator/registers.h"

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
      const registers::BoostingScheduleHourAddresses& hour_addresses,
      const registers::BoostingScheduleDeltaAddresses& delta_addresses) const;
};
}  // namespace communicator
