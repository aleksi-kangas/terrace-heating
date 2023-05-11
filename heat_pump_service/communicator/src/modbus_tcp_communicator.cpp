#include "communicator/modbus_tcp_communicator.h"

#include <array>
#include <cstdint>
#include <stdexcept>

#include "registers.h"
#include "utils.h"

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

}  // namespace communicator
