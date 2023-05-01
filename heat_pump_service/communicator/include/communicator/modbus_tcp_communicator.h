#pragma once

#include <cstdint>
#include <string>

#include <modbus.h>

#include "communicator/communicator.h"

namespace communicator {
class ModbusTCPCommunicator final : public ICommunicator {
 public:
  ModbusTCPCommunicator(const std::string& host, int32_t port);
  ~ModbusTCPCommunicator() override;

  [[nodiscard]] Temperatures ReadTemperatures() const override;
  [[nodiscard]] TankLimits ReadTankLimits() const override;

 private:
  modbus_t* context_{nullptr};
};
}  // namespace communicator
