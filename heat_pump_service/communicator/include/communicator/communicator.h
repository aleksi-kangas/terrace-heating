#pragma once

#include <cstdint>

#include "communicator/types.h"

namespace communicator {
class ICommunicator {
 public:
  virtual ~ICommunicator() = default;

  /*
   * Read the number of active heat distribution circuits.
   * @return  The number of active heat distribution circuits.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] virtual uint32_t ActiveCircuitCount() const = 0;

  /*
   * Read the compressor status.
   * @return  True if the compressor is active, false otherwise.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] virtual bool IsCompressorActive() const = 0;

  /*
   * Read the scheduling status.
   * @return  True if scheduling is enabled, false otherwise.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] virtual bool IsSchedulingEnabled() const = 0;

  /*
   * Read the boosting schedule of heat distribution circuit 3.
   * @return  The boosting schedule of heat distribution circuit 3.
   * @throw   std::runtime_error if the read operation fails.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] virtual BoostingSchedule ReadCircuit3BoostingSchedule() const = 0;

  /*
   * Read the boosting schedule of the lower tank.
   * @return  The boosting schedule of the lower tank.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] virtual BoostingSchedule ReadLowerTankBoostingSchedule() const = 0;

  /*
   * Read the temperatures of various parts of the heat pump.
   * @return  The temperatures of various parts of the heat pump.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] virtual Temperatures ReadTemperatures() const = 0;

  /*
   * Read the tank temperature limits which guide the heat pump in its operation.
   * @return  The tank temperature limits which guide the heat pump in its operation.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] virtual TankLimits ReadTankLimits() const = 0;

  /*
   * Write the number of active heat distribution circuits.
   * @param[in]  count The number of active heat distribution circuits.
   * @throw      std::invalid_argument if count is not in [0, 3].
   *             std::runtime_error if the write operation fails.
   */
  virtual void WriteActiveCircuitCount(uint32_t count) = 0;

  /*
   * Write the boosting schedule of the heat distribution circuit 3.
   * @param[in]  schedule  The boosting schedule of the heat distribution circuit 3.
   * @throw      std::runtime_error if the write operation fails.
   */
  virtual void WriteCircuit3BoostingSchedule(const BoostingSchedule& schedule) = 0;

  /*
   * Write the boosting schedule of the lower tank.
   * @param[in]  schedule  The boosting schedule of the lower tank.
   * @throw      std::runtime_error if the write operation fails.
   */
  virtual void WriteLowerTankBoostingSchedule(const BoostingSchedule& schedule) = 0;

  /*
   * Write the scheduling status.
   * @param[in]  scheduling_enabled  True if scheduling should be enabled, false otherwise.
   * @throw      std::runtime_error if the write operation fails.
   */
  virtual void WriteSchedulingEnabled(bool scheduling_enabled) = 0;
};
}  // namespace communicator
