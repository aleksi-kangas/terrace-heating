#include "communicator/modbus_tcp_communicator.h"

#include <array>
#include <cstdint>
#include <stdexcept>

#include "communicator/utils.h"

namespace communicator {

ModbusTCPCommunicator::ModbusTCPCommunicator(const std::string& host,
                                             int32_t port) {
  context_ = modbus_new_tcp(host.c_str(), port);
  if (context_ == nullptr) {
    throw std::runtime_error{"Failed to create modbus context"};
  }

  modbus_set_error_recovery(context_, MODBUS_ERROR_RECOVERY_LINK);

  if (modbus_connect(context_) == -1) {
    throw std::runtime_error{"Failed to connect to modbus server: " +
                             std::string{modbus_strerror(errno)}};
  }
}

ModbusTCPCommunicator::~ModbusTCPCommunicator() {
  std::lock_guard<std::mutex> lock{mutex_};
  modbus_close(context_);
  modbus_free(context_);
}

uint32_t ModbusTCPCommunicator::ActiveCircuitCount() const {
  std::lock_guard<std::mutex> lock{mutex_};
  uint16_t active_circuit_count{0};
  const int32_t rc = modbus_read_registers(
      context_, registers::kActiveCircuitCount, 1, &active_circuit_count);
  if (rc != 1) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  return active_circuit_count;
}

bool ModbusTCPCommunicator::IsCompressorActive() const {
  std::lock_guard<std::mutex> lock{mutex_};
  uint16_t is_compressor_active{0};
  const int32_t rc = modbus_read_registers(
      context_, registers::kCompressorActive, 1, &is_compressor_active);
  if (rc != 1) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  return is_compressor_active == 1;
}

bool ModbusTCPCommunicator::IsSchedulingEnabled() const {
  std::lock_guard<std::mutex> lock{mutex_};
  uint16_t is_scheduling_enabled{0};
  const int32_t rc = modbus_read_registers(
      context_, registers::kSchedulingEnabled, 1, &is_scheduling_enabled);
  if (rc != 1) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  return is_scheduling_enabled == 1;
}

BoostingSchedule ModbusTCPCommunicator::ReadCircuit3BoostingSchedule() const {
  return ReadBoostingSchedule(registers::kCircuit3BoostingSchedule);
}

BoostingSchedule ModbusTCPCommunicator::ReadLowerTankBoostingSchedule() const {
  return ReadBoostingSchedule(registers::kLowerTankBoostingSchedule);
}

Temperatures ModbusTCPCommunicator::ReadTemperatures() const {
  std::unique_lock<std::mutex> lock{mutex_};
  // Query values in bulk in order to limit the amount of round trips.
  constexpr int kStartAddress = registers::kTemperatureRegisterRange.first;
  constexpr int32_t kQuantity = registers::kTemperatureRegisterRange.second -
                                registers::kTemperatureRegisterRange.first + 1;
  std::array<uint16_t, kQuantity> values{};
  const int32_t rc =
      modbus_read_registers(context_, kStartAddress, kQuantity, values.data());
  if (rc != kQuantity) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  lock.unlock();
  return utils::ParseTemperatures(values);
}

TankLimits ModbusTCPCommunicator::ReadTankLimits() const {
  std::unique_lock<std::mutex> lock{mutex_};
  // Query values in bulk in order to limit the amount of round trips.
  constexpr int kStartAddress = registers::kTankLimitRegisterRange.first;
  constexpr int32_t kQuantity = registers::kTankLimitRegisterRange.second -
                                registers::kTankLimitRegisterRange.first + 1;
  std::array<uint16_t, kQuantity> values{};
  const int32_t rc =
      modbus_read_registers(context_, kStartAddress, kQuantity, values.data());
  if (rc != kQuantity) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  lock.unlock();
  return utils::ParseTankLimits(values);
}

void ModbusTCPCommunicator::WriteActiveCircuitCount(uint32_t count) {
  if (count > 3)
    throw std::invalid_argument{"Invalid active circuit count: " +
                                std::to_string(count)};

  std::lock_guard<std::mutex> lock{mutex_};
  const int32_t rc =
      modbus_write_register(context_, registers::kActiveCircuitCount, count);
  if (rc != 1) {
    throw std::runtime_error{"Failed to write registers: " +
                             std::string{modbus_strerror(errno)}};
  }
}

BoostingSchedule ModbusTCPCommunicator::ReadBoostingSchedule(
    const registers::BoostingScheduleAddresses& addresses) const {
  const int32_t kHourStartAddress = addresses.HourRegisterRange().first;
  const int32_t kHourQuantity = addresses.HourRegisterRange().second -
                                addresses.HourRegisterRange().first + 1;
  const int32_t kDeltaStartAddress = addresses.DeltaRegisterRange().first;
  const int32_t kDeltaQuantity = addresses.DeltaRegisterRange().second -
                                 addresses.DeltaRegisterRange().first + 1;

  std::unique_lock<std::mutex> lock{mutex_};
  // Query hour values in bulk in order to limit the amount of round trips.
  std::vector<uint16_t> hour_values(kHourQuantity, 0);
  int32_t rc = modbus_read_registers(context_, kHourStartAddress, kHourQuantity,
                                     hour_values.data());
  if (rc != kHourQuantity) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  // Query temperature delta values in bulk in order to limit the amount of round trips.
  std::vector<uint16_t> temperature_delta_values(kDeltaQuantity, 0);
  rc = modbus_read_registers(context_, kDeltaStartAddress, kDeltaQuantity,
                             temperature_delta_values.data());
  if (rc != kDeltaQuantity) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  lock.unlock();
  return utils::ParseBoostingSchedule(addresses, hour_values,
                                      temperature_delta_values);
}

}  // namespace communicator
