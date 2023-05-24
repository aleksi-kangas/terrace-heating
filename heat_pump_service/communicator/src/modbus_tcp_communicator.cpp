#include "communicator/modbus_tcp_communicator.h"
#include <modbus.h>

#include <array>
#include <cstdint>
#include <stdexcept>
#include <utility>
#include <vector>

#include "communicator/utils.h"

namespace communicator {

ModbusTCPCommunicator::ModbusTCPCommunicator(const std::string& host,
                                             int32_t port) {
  context_ = modbus_new_tcp(host.c_str(), port);
  if (context_ == nullptr) {
    throw std::runtime_error{"Failed to create modbus context"};
  }

  modbus_set_error_recovery(context_, MODBUS_ERROR_RECOVERY_LINK);

  constexpr int32_t kTimeoutSeconds = 2;
  modbus_set_response_timeout(context_, kTimeoutSeconds, 0);
  modbus_set_byte_timeout(context_, kTimeoutSeconds, 0);

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
      context_, addresses::miscellaneous::kActiveCircuitCount, 1,
      &active_circuit_count);
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
      context_, addresses::miscellaneous::kCompressorActive, 1,
      &is_compressor_active);
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
      context_, addresses::miscellaneous::kSchedulingEnabled, 1,
      &is_scheduling_enabled);
  if (rc != 1) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  return is_scheduling_enabled == 1;
}

BoostingSchedule ModbusTCPCommunicator::ReadCircuit3BoostingSchedule() const {
  return ReadBoostingSchedule(addresses::boosting_schedules::kCircuit3);
}

BoostingSchedule ModbusTCPCommunicator::ReadLowerTankBoostingSchedule() const {
  return ReadBoostingSchedule(addresses::boosting_schedules::kLowerTank);
}

Temperatures ModbusTCPCommunicator::ReadTemperatures() const {
  std::unique_lock<std::mutex> lock{mutex_};
  // Query values in bulk in order to limit the amount of round trips.
  std::array<uint16_t, addresses::temperatures::kQuantity> values{};
  const int32_t rc =
      modbus_read_registers(context_, addresses::temperatures::kFirst,
                            addresses::temperatures::kQuantity, values.data());
  if (rc != addresses::temperatures::kQuantity) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  lock.unlock();
  return utils::ParseTemperatures(values);
}

TankLimits ModbusTCPCommunicator::ReadTankLimits() const {
  std::unique_lock<std::mutex> lock{mutex_};
  // Query values in bulk in order to limit the amount of round trips.
  std::array<uint16_t, addresses::tank_limits::kQuantity> values{};
  const int32_t rc =
      modbus_read_registers(context_, addresses::tank_limits::kFirst,
                            addresses::tank_limits::kQuantity, values.data());
  if (rc != addresses::tank_limits::kQuantity) {
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
  const int32_t rc = modbus_write_register(
      context_, addresses::miscellaneous::kActiveCircuitCount, count);
  if (rc != 1) {
    throw std::runtime_error{"Failed to write registers: " +
                             std::string{modbus_strerror(errno)}};
  }
}

void ModbusTCPCommunicator::WriteCircuit3BoostingSchedule(
    const BoostingSchedule& schedule) {
  WriteBoostingSchedule(addresses::boosting_schedules::kCircuit3, schedule);
}

void ModbusTCPCommunicator::WriteLowerTankBoostingSchedule(
    const BoostingSchedule& schedule) {
  WriteBoostingSchedule(addresses::boosting_schedules::kLowerTank, schedule);
}

BoostingSchedule ModbusTCPCommunicator::ReadBoostingSchedule(
    const addresses::boosting_schedules::BoostingScheduleAddresses& addresses)
    const {
  std::unique_lock<std::mutex> lock{mutex_};
  // Query hour values in bulk in order to limit the amount of round trips.
  std::vector<uint16_t> hour_values(addresses.QuantityHour(), 0);
  int32_t rc =
      modbus_read_registers(context_, addresses.FirstHour(),
                            addresses.QuantityHour(), hour_values.data());
  if (rc != addresses.QuantityHour()) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  // Query delta values in bulk in order to limit the amount of round trips.
  std::vector<uint16_t> delta_values(addresses.QuantityDelta(), 0);
  rc = modbus_read_registers(context_, addresses.FirstDelta(),
                             addresses.QuantityDelta(), delta_values.data());
  if (rc != addresses.QuantityDelta()) {
    throw std::runtime_error{"Failed to read registers: " +
                             std::string{modbus_strerror(errno)}};
  }
  lock.unlock();
  return utils::ParseBoostingSchedule(addresses, hour_values, delta_values);
}

void ModbusTCPCommunicator::WriteBoostingSchedule(
    const addresses::boosting_schedules::BoostingScheduleAddresses& addresses,
    const BoostingSchedule& schedule) {
  // 1. Generate a sorted mapping of <address, value>
  const auto mappings =
      utils::GenerateSortedScheduleAddressValueMappings(addresses, schedule);
  // 2. Extract contiguous address ranges
  const auto contiguous_address_ranges =
      utils::ExtractContiguousAddressRanges(mappings);
  // 3. Write each contiguous address range in bulk to limit the amount of round trips.
  for (const auto& contiguous_address_range : contiguous_address_ranges) {
    const int32_t kFirst = contiguous_address_range.front().first;
    const auto kQuantity =
        static_cast<int32_t>(contiguous_address_range.size());

    std::vector<uint16_t> values{};
    values.reserve(contiguous_address_range.size());
    std::ranges::for_each(contiguous_address_range,
                          [&](const auto& address_value_pair) {
                            values.emplace_back(address_value_pair.second);
                          });
    const int32_t rc =
        modbus_write_registers(context_, kFirst, kQuantity, values.data());
    if (rc != kQuantity) {
      throw std::runtime_error{"Failed to write registers: " +
                               std::string{modbus_strerror(errno)}};
    }
  }
}

}  // namespace communicator
