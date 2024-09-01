#pragma once

#include <cstdint>
#include <memory>
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

  /*
   * Read the number of active heat distribution circuits.
   * @return  The number of active heat distribution circuits.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] uint32_t ActiveCircuitCount() const override;

  /*
   * Read the compressor status.
   * @return  True if the compressor is active, false otherwise.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] bool IsCompressorActive() const override;

  /*
   * Read the scheduling status.
   * @return  True if scheduling is enabled, false otherwise.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] bool IsSchedulingEnabled() const override;

  /*
   * Read the boosting schedule of heat distribution circuit 3.
   * @return  The boosting schedule of heat distribution circuit 3.
   * @throw   std::runtime_error if the read operation fails.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] BoostingSchedule ReadCircuit3BoostingSchedule() const override;

  /*
   * Read the boosting schedule of the lower tank.
   * @return  The boosting schedule of the lower tank.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] BoostingSchedule ReadLowerTankBoostingSchedule() const override;

  /*
   * Read the temperatures of various parts of the heat pump.
   * @return  The temperatures of various parts of the heat pump.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] Temperatures ReadTemperatures() const override;

  /*
   * Read the tank temperature limits which guide the heat pump in its operation.
   * @return  The tank temperature limits which guide the heat pump in its operation.
   * @throw   std::runtime_error if the read operation fails.
   */
  [[nodiscard]] TankLimits ReadTankLimits() const override;

  /*
   * Write the number of active heat distribution circuits.
   * @param[in]  count The number of active heat distribution circuits.
   * @throw      std::invalid_argument if count is not in [0, 3].
   *             std::runtime_error if the write operation fails.
   */
  void WriteActiveCircuitCount(uint32_t count) override;

  /*
   * Write the boosting schedule of the heat distribution circuit 3.
   * @param[in]  schedule  The boosting schedule of the heat distribution circuit 3.
   * @throw      std::runtime_error if the write operation fails.
   */
  void WriteCircuit3BoostingSchedule(const BoostingSchedule& schedule) override;

  /*
   * Write the boosting schedule of the lower tank.
   * @param[in]  schedule  The boosting schedule of the lower tank.
   * @throw      std::runtime_error if the write operation fails.
   */
  void WriteLowerTankBoostingSchedule(const BoostingSchedule& schedule) override;

  /*
   * Write the scheduling status.
   * @param[in]  scheduling_enabled  True if scheduling should be enabled, false otherwise.
   * @throw      std::runtime_error if the write operation fails.
   */
  void WriteSchedulingEnabled(bool scheduling_enabled) override;

  class Factory : public IFactory {
    public:
    Factory(std::string host, int32_t port);

    [[nodiscard]] std::unique_ptr<ICommunicator> Instance() override;

    private:
    std::string host_;
    int32_t port_;
  };

 private:
  modbus_t* context_{nullptr};
  mutable std::mutex mutex_{};

  [[nodiscard]] BoostingSchedule ReadBoostingSchedule(
      const addresses::boosting_schedules::BoostingScheduleAddresses& addresses) const;

  void WriteBoostingSchedule(const addresses::boosting_schedules::BoostingScheduleAddresses& addresses,
                             const BoostingSchedule& schedule);
};
}  // namespace communicator
