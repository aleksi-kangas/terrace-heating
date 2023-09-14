#pragma once

#include <cstdint>

#include "communicator/types.h"

namespace communicator {
class ICommunicator {
 public:
  virtual ~ICommunicator() = default;

  [[nodiscard]] virtual uint32_t ActiveCircuitCount() const = 0;
  [[nodiscard]] virtual bool IsCompressorActive() const = 0;
  [[nodiscard]] virtual bool IsSchedulingEnabled() const = 0;
  [[nodiscard]] virtual BoostingSchedule ReadCircuit3BoostingSchedule()
      const = 0;
  [[nodiscard]] virtual BoostingSchedule ReadLowerTankBoostingSchedule()
      const = 0;
  [[nodiscard]] virtual Temperatures ReadTemperatures() const = 0;
  [[nodiscard]] virtual TankLimits ReadTankLimits() const = 0;
  virtual void WriteActiveCircuitCount(uint32_t count) = 0;
  virtual void WriteCircuit3BoostingSchedule(
      const BoostingSchedule& schedule) = 0;
  virtual void WriteLowerTankBoostingSchedule(
      const BoostingSchedule& schedule) = 0;
  virtual void WriteSchedulingEnabled(bool scheduling_enabled) = 0;
};
}  // namespace communicator
