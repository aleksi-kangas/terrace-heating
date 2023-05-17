#include "mock_modbus_tcp_server.h"

#include <stdexcept>

#include "communicator/registers.h"

using namespace communicator;

MockModbusTCPServer::MockModbusTCPServer(std::atomic<bool>& stop)
    : stop_{stop} {
  context_ = modbus_new_tcp(nullptr, MODBUS_TCP_DEFAULT_PORT);
  if (context_ == nullptr)
    throw std::runtime_error{"Failed to create modbus context"};
  constexpr int32_t kRegisterCount = std::max(
      {registers::kTemperatureRegisterRange.second,
       registers::kTankLimitRegisterRange.second,
       registers::kCircuit3BoostingSchedule.HourRegisterRange().second,
       registers::kCircuit3BoostingSchedule.DeltaRegisterRange().second,
       registers::kActiveCircuitCount, registers::kCompressorActive,
       registers::kSchedulingEnabled});
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
      if (rc == -1) {
        if (errno == ECONNRESET) {
          modbus_tcp_accept(context_, &socket_);
          continue;
        }
        throw std::runtime_error{"Failed to receive modbus TCP query: " +
                                 std::string{modbus_strerror(errno)}};
      }
      std::lock_guard<std::mutex> lock{mutex_};
      modbus_reply(context_, query, rc, mapping_);
    }
  });
}

void MockModbusTCPServer::InitializeMapping() {
  std::lock_guard<std::mutex> lock{mutex_};

  mapping_->tab_registers[registers::kActiveCircuitCount] = kActiveCircuitCount;
  mapping_->tab_registers[registers::kCompressorActive] = kIsCompressorActive;
  mapping_->tab_registers[registers::kSchedulingEnabled] = kIsSchedulingEnabled;

  // clang-format off
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.monday.start_hour] =
      kCircuit3BoostingSchedule.monday.start_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.monday.end_hour] =
      kCircuit3BoostingSchedule.monday.end_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.monday.delta] =
      kCircuit3BoostingSchedule.monday.temperature_delta;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.tuesday.start_hour] =
      kCircuit3BoostingSchedule.tuesday.start_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.tuesday.end_hour] =
      kCircuit3BoostingSchedule.tuesday.end_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.tuesday.delta] =
      kCircuit3BoostingSchedule.tuesday.temperature_delta;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.wednesday.start_hour] =
      kCircuit3BoostingSchedule.wednesday.start_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.wednesday.end_hour] =
      kCircuit3BoostingSchedule.wednesday.end_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.wednesday.delta] =
      kCircuit3BoostingSchedule.wednesday.temperature_delta;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.thursday.start_hour] =
      kCircuit3BoostingSchedule.thursday.start_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.thursday.end_hour] =
      kCircuit3BoostingSchedule.thursday.end_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.thursday.delta] =
      kCircuit3BoostingSchedule.thursday.temperature_delta;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.friday.start_hour] =
      kCircuit3BoostingSchedule.friday.start_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.friday.end_hour] =
      kCircuit3BoostingSchedule.friday.end_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.friday.delta] =
      kCircuit3BoostingSchedule.friday.temperature_delta;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.saturday.start_hour] =
      kCircuit3BoostingSchedule.saturday.start_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.saturday.end_hour] =
      kCircuit3BoostingSchedule.saturday.end_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.saturday.delta] =
      kCircuit3BoostingSchedule.saturday.temperature_delta;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.sunday.start_hour] =
      kCircuit3BoostingSchedule.sunday.start_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.sunday.end_hour] =
      kCircuit3BoostingSchedule.sunday.end_hour;
  mapping_ -> tab_registers[registers::kCircuit3BoostingSchedule.sunday.delta] =
      kCircuit3BoostingSchedule.sunday.temperature_delta;
  // clang-format on

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
