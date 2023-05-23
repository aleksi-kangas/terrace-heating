#include "mock_modbus_tcp_server.h"

#include <stdexcept>

#include "communicator/addresses.h"

using namespace communicator;

MockModbusTCPServer::MockModbusTCPServer(std::atomic<bool>& stop)
    : stop_{stop} {
  context_ = modbus_new_tcp(nullptr, MODBUS_TCP_DEFAULT_PORT);
  if (context_ == nullptr)
    throw std::runtime_error{"Failed to create modbus context"};
  constexpr int32_t kLastAddress = std::max({
      addresses::miscellaneous::kActiveCircuitCount,
      addresses::miscellaneous::kCompressorActive,
      addresses::miscellaneous::kSchedulingEnabled,
      addresses::tank_limits::kLast,
      addresses::temperatures::kLast,
      addresses::boosting_schedules::kCircuit3.LastHour(),
      addresses::boosting_schedules::kCircuit3.LastDelta(),
      addresses::boosting_schedules::kLowerTank.LastHour(),
      addresses::boosting_schedules::kLowerTank.LastDelta(),
  });
  mapping_ = modbus_mapping_new(0, 0, kLastAddress + 1, 0);
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

  mapping_->tab_registers[addresses::miscellaneous::kActiveCircuitCount] =
      static_cast<uint16_t>(kActiveCircuitCount);
  mapping_->tab_registers[addresses::miscellaneous::kCompressorActive] =
      static_cast<uint16_t>(kIsCompressorActive);
  mapping_->tab_registers[addresses::miscellaneous::kSchedulingEnabled] =
      static_cast<uint16_t>(kIsSchedulingEnabled);

  auto InitializeBoostingSchedule =
      [this](const addresses::boosting_schedules::BoostingScheduleAddresses&
                 addresses,
             const BoostingSchedule& schedule) {
        mapping_->tab_registers[addresses.monday.start_hour] =
            static_cast<uint16_t>(schedule.monday.start_hour);
        mapping_->tab_registers[addresses.monday.end_hour] =
            static_cast<uint16_t>(schedule.monday.end_hour);
        mapping_->tab_registers[addresses.monday.delta] =
            static_cast<uint16_t>(schedule.monday.temperature_delta);

        mapping_->tab_registers[addresses.tuesday.start_hour] =
            static_cast<uint16_t>(schedule.tuesday.start_hour);
        mapping_->tab_registers[addresses.tuesday.end_hour] =
            static_cast<uint16_t>(schedule.tuesday.end_hour);
        mapping_->tab_registers[addresses.tuesday.delta] =
            static_cast<uint16_t>(schedule.tuesday.temperature_delta);

        mapping_->tab_registers[addresses.wednesday.start_hour] =
            static_cast<uint16_t>(schedule.wednesday.start_hour);
        mapping_->tab_registers[addresses.wednesday.end_hour] =
            static_cast<uint16_t>(schedule.wednesday.end_hour);
        mapping_->tab_registers[addresses.wednesday.delta] =
            static_cast<uint16_t>(schedule.wednesday.temperature_delta);

        mapping_->tab_registers[addresses.thursday.start_hour] =
            static_cast<uint16_t>(schedule.thursday.start_hour);
        mapping_->tab_registers[addresses.thursday.end_hour] =
            static_cast<uint16_t>(schedule.thursday.end_hour);
        mapping_->tab_registers[addresses.thursday.delta] =
            static_cast<uint16_t>(schedule.thursday.temperature_delta);

        mapping_->tab_registers[addresses.friday.start_hour] =
            static_cast<uint16_t>(schedule.friday.start_hour);
        mapping_->tab_registers[addresses.friday.end_hour] =
            static_cast<uint16_t>(schedule.friday.end_hour);
        mapping_->tab_registers[addresses.friday.delta] =
            static_cast<uint16_t>(schedule.friday.temperature_delta);

        mapping_->tab_registers[addresses.saturday.start_hour] =
            static_cast<uint16_t>(schedule.saturday.start_hour);
        mapping_->tab_registers[addresses.saturday.end_hour] =
            static_cast<uint16_t>(schedule.saturday.end_hour);
        mapping_->tab_registers[addresses.saturday.delta] =
            static_cast<uint16_t>(schedule.saturday.temperature_delta);

        mapping_->tab_registers[addresses.sunday.start_hour] =
            static_cast<uint16_t>(schedule.sunday.start_hour);
        mapping_->tab_registers[addresses.sunday.end_hour] =
            static_cast<uint16_t>(schedule.sunday.end_hour);
        mapping_->tab_registers[addresses.sunday.delta] =
            static_cast<uint16_t>(schedule.sunday.temperature_delta);
      };
  InitializeBoostingSchedule(addresses::boosting_schedules::kCircuit3,
                             kCircuit3BoostingSchedule);
  InitializeBoostingSchedule(addresses::boosting_schedules::kLowerTank,
                             kLowerTankBoostingSchedule);

  auto InitializeTankLimits = [this](const TankLimits& limits) {
    mapping_->tab_registers[addresses::tank_limits::kLowerTankMinimum] =
        static_cast<uint16_t>(limits.lower_tank_minimum);
    mapping_->tab_registers[addresses::tank_limits::kLowerTankMinimumAdjusted] =
        static_cast<uint16_t>(limits.lower_tank_minimum_adjusted);
    mapping_->tab_registers[addresses::tank_limits::kLowerTankMaximum] =
        static_cast<uint16_t>(limits.lower_tank_maximum);
    mapping_->tab_registers[addresses::tank_limits::kLowerTankMaximumAdjusted] =
        static_cast<uint16_t>(limits.lower_tank_maximum_adjusted);
    mapping_->tab_registers[addresses::tank_limits::kUpperTankMinimum] =
        static_cast<uint16_t>(limits.upper_tank_minimum);
    mapping_->tab_registers[addresses::tank_limits::kUpperTankMinimumAdjusted] =
        static_cast<uint16_t>(limits.upper_tank_minimum_adjusted);
    mapping_->tab_registers[addresses::tank_limits::kUpperTankMaximum] =
        static_cast<uint16_t>(limits.upper_tank_maximum);
    mapping_->tab_registers[addresses::tank_limits::kUpperTankMaximumAdjusted] =
        static_cast<uint16_t>(limits.upper_tank_maximum_adjusted);
  };
  InitializeTankLimits(kTankLimits);

  auto TemperatureFloatToUInt16 = [](float temperature) {
    return static_cast<uint16_t>(temperature * 10);
  };

  mapping_->tab_registers[addresses::temperatures::kCircuit1] =
      TemperatureFloatToUInt16(kCircuit1Temperature);
  mapping_->tab_registers[addresses::temperatures::kCircuit2] =
      TemperatureFloatToUInt16(kCircuit2Temperature);
  mapping_->tab_registers[addresses::temperatures::kCircuit3] =
      TemperatureFloatToUInt16(kCircuit3Temperature);
  mapping_->tab_registers[addresses::temperatures::kGroundInput] =
      TemperatureFloatToUInt16(kGroundInputTemperature);
  mapping_->tab_registers[addresses::temperatures::kGroundOutput] =
      TemperatureFloatToUInt16(kGroundOutputTemperature);
  mapping_->tab_registers[addresses::temperatures::kHotGas] =
      TemperatureFloatToUInt16(kHotGasTemperature);
  mapping_->tab_registers[addresses::temperatures::kInside] =
      TemperatureFloatToUInt16(kInsideTemperature);
  mapping_->tab_registers[addresses::temperatures::kLowerTank] =
      TemperatureFloatToUInt16(kLowerTankTemperature);
  mapping_->tab_registers[addresses::temperatures::kOutside] =
      TemperatureFloatToUInt16(kOutsideTemperature);
  mapping_->tab_registers[addresses::temperatures::kUpperTank] =
      TemperatureFloatToUInt16(kUpperTankTemperature);
}
