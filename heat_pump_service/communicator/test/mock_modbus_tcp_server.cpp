#include "mock_modbus_tcp_server.h"

#include <stdexcept>

#include "communicator/registers.h"

using namespace communicator;

MockModbusTCPServer::MockModbusTCPServer(std::atomic<bool>& stop)
    : stop_{stop} {
  context_ = modbus_new_tcp(nullptr, MODBUS_TCP_DEFAULT_PORT);
  if (context_ == nullptr)
    throw std::runtime_error{"Failed to create modbus context"};
  constexpr int32_t kRegisterCount = std::max({
      registers::kActiveCircuitCount,
      registers::kCompressorActive,
      registers::kSchedulingEnabled,
      registers::kTemperatureRegisterRange.second,
      registers::kTankLimitRegisterRange.second,
      registers::kCircuit3BoostingScheduleHours.Range().second,
      registers::kCircuit3BoostingScheduleDeltas.Range().second,
      registers::kLowerTankBoostingScheduleHours.Range().second,
      registers::kLowerTankBoostingScheduleDeltas.Range().second,
  });
  mapping_ = modbus_mapping_new(0, 0, kRegisterCount + 1, 0);
  if (mapping_ == nullptr)
    throw std::runtime_error{"Failed to create modbus mapping"};
  InitializeMapping();
  socket_ = modbus_tcp_listen(context_, 1);
  if (socket_ == -1)
    throw std::runtime_error{"Failed to listen for modbus TCP connections"};
}

MockModbusTCPServer::~MockModbusTCPServer() {
  if (socket_ != -1)
    close(socket_);
  if (mapping_)
    modbus_mapping_free(mapping_);
  if (context_) {
    modbus_close(context_);
    modbus_free(context_);
  }
}

std::thread MockModbusTCPServer::Run() {
  return std::thread([this]() {
    modbus_tcp_accept(context_, &socket_);
    while (!stop_) {
      uint8_t query[MODBUS_TCP_MAX_ADU_LENGTH];
      const int32_t rc = modbus_receive(context_, query);
      if (rc > 0) {
        std::lock_guard<std::mutex> lock{mutex_};
        modbus_reply(context_, query, rc, mapping_);
      } else if (rc == -1) {
        if (errno == ECONNRESET) {
          modbus_tcp_accept(context_, &socket_);
          continue;
        }
      }
    }
  });
}

void MockModbusTCPServer::InitializeMapping() {
  std::lock_guard<std::mutex> lock{mutex_};

  mapping_->tab_registers[registers::kActiveCircuitCount] =
      static_cast<uint16_t>(kActiveCircuitCount);
  mapping_->tab_registers[registers::kCompressorActive] =
      static_cast<uint16_t>(kIsCompressorActive);
  mapping_->tab_registers[registers::kSchedulingEnabled] =
      static_cast<uint16_t>(kIsSchedulingEnabled);

  auto InitializeBoostingSchedule =
      [this](const registers::BoostingScheduleHourAddresses& hour_addresses,
             const registers::BoostingScheduleDeltaAddresses& delta_addresses,
             const BoostingSchedule& schedule) {
        mapping_->tab_registers[hour_addresses.monday.start_hour] =
            static_cast<uint16_t>(schedule.monday.start_hour);
        mapping_->tab_registers[hour_addresses.monday.end_hour] =
            static_cast<uint16_t>(schedule.monday.end_hour);
        mapping_->tab_registers[delta_addresses.monday.delta] =
            static_cast<uint16_t>(schedule.monday.temperature_delta);

        mapping_->tab_registers[hour_addresses.tuesday.start_hour] =
            static_cast<uint16_t>(schedule.tuesday.start_hour);
        mapping_->tab_registers[hour_addresses.tuesday.end_hour] =
            static_cast<uint16_t>(schedule.tuesday.end_hour);
        mapping_->tab_registers[delta_addresses.tuesday.delta] =
            static_cast<uint16_t>(schedule.tuesday.temperature_delta);

        mapping_->tab_registers[hour_addresses.wednesday.start_hour] =
            static_cast<uint16_t>(schedule.wednesday.start_hour);
        mapping_->tab_registers[hour_addresses.wednesday.end_hour] =
            static_cast<uint16_t>(schedule.wednesday.end_hour);
        mapping_->tab_registers[delta_addresses.wednesday.delta] =
            static_cast<uint16_t>(schedule.wednesday.temperature_delta);

        mapping_->tab_registers[hour_addresses.thursday.start_hour] =
            static_cast<uint16_t>(schedule.thursday.start_hour);
        mapping_->tab_registers[hour_addresses.thursday.end_hour] =
            static_cast<uint16_t>(schedule.thursday.end_hour);
        mapping_->tab_registers[delta_addresses.thursday.delta] =
            static_cast<uint16_t>(schedule.thursday.temperature_delta);

        mapping_->tab_registers[hour_addresses.friday.start_hour] =
            static_cast<uint16_t>(schedule.friday.start_hour);
        mapping_->tab_registers[hour_addresses.friday.end_hour] =
            static_cast<uint16_t>(schedule.friday.end_hour);
        mapping_->tab_registers[delta_addresses.friday.delta] =
            static_cast<uint16_t>(schedule.friday.temperature_delta);

        mapping_->tab_registers[hour_addresses.saturday.start_hour] =
            static_cast<uint16_t>(schedule.saturday.start_hour);
        mapping_->tab_registers[hour_addresses.saturday.end_hour] =
            static_cast<uint16_t>(schedule.saturday.end_hour);
        mapping_->tab_registers[delta_addresses.saturday.delta] =
            static_cast<uint16_t>(schedule.saturday.temperature_delta);

        mapping_->tab_registers[hour_addresses.sunday.start_hour] =
            static_cast<uint16_t>(schedule.sunday.start_hour);
        mapping_->tab_registers[hour_addresses.sunday.end_hour] =
            static_cast<uint16_t>(schedule.sunday.end_hour);
        mapping_->tab_registers[delta_addresses.sunday.delta] =
            static_cast<uint16_t>(schedule.sunday.temperature_delta);

      };
  InitializeBoostingSchedule(registers::kCircuit3BoostingScheduleHours,
                             registers::kCircuit3BoostingScheduleDeltas,
                             kCircuit3BoostingSchedule);
  InitializeBoostingSchedule(registers::kLowerTankBoostingScheduleHours,
                             registers::kLowerTankBoostingScheduleDeltas,
                             kLowerTankBoostingSchedule);

  mapping_->tab_registers[registers::kLowerTankMinimum] = kLowerTankMinimum;
  mapping_->tab_registers[registers::kLowerTankMaximum] = kLowerTankMaximum;
  mapping_->tab_registers[registers::kUpperTankMinimum] = kUpperTankMinimum;
  mapping_->tab_registers[registers::kUpperTankMaximum] = kUpperTankMaximum;

  auto TemperatureFloatToUInt16 = [](float temperature) {
    return static_cast<uint16_t>(temperature * 10);
  };

  mapping_->tab_registers[registers::kCircuit1Temperature] =
      TemperatureFloatToUInt16(kCircuit1Temperature);
  mapping_->tab_registers[registers::kCircuit2Temperature] =
      TemperatureFloatToUInt16(kCircuit2Temperature);
  mapping_->tab_registers[registers::kCircuit3Temperature] =
      TemperatureFloatToUInt16(kCircuit3Temperature);
  mapping_->tab_registers[registers::kGroundInputTemperature] =
      TemperatureFloatToUInt16(kGroundInputTemperature);
  mapping_->tab_registers[registers::kGroundOutputTemperature] =
      TemperatureFloatToUInt16(kGroundOutputTemperature);
  mapping_->tab_registers[registers::kHotGasTemperature] =
      TemperatureFloatToUInt16(kHotGasTemperature);
  mapping_->tab_registers[registers::kInsideTemperature] =
      TemperatureFloatToUInt16(kInsideTemperature);
  mapping_->tab_registers[registers::kLowerTankTemperature] =
      TemperatureFloatToUInt16(kLowerTankTemperature);
  mapping_->tab_registers[registers::kOutsideTemperature] =
      TemperatureFloatToUInt16(kOutsideTemperature);
  mapping_->tab_registers[registers::kUpperTankTemperature] =
      TemperatureFloatToUInt16(kUpperTankTemperature);
}
