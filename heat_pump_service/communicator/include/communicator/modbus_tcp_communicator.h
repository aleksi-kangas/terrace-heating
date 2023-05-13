#pragma once

#include <mutex>
#include <string>

#include <modbus.h>

#include "communicator/communicator.h"

namespace communicator {
class ModbusTCPCommunicator final : public ICommunicator {
 public:
  ModbusTCPCommunicator(const std::string& host, int32_t port);
  ~ModbusTCPCommunicator() override;

  [[nodiscard]] uint32_t ActiveCircuitCount() const override;
  [[nodiscard]] bool IsCompressorActive() const override;
  [[nodiscard]] bool IsSchedulingEnabled() const override;
  [[nodiscard]] Temperatures ReadTemperatures() const override;
  [[nodiscard]] TankLimits ReadTankLimits() const override;
  void WriteActiveCircuitCount(uint32_t count) override;

 private:
  modbus_t* context_{nullptr};
  mutable std::mutex mutex_{};
};
}  // namespace communicator
