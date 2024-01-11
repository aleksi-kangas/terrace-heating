#pragma once

#include <atomic>
#include <cstdint>
#include <mutex>
#include <string>
#include <thread>

#include <modbus.h>

#include "communicator/communicator.h"

constexpr uint32_t kActiveCircuitCount = 3;
constexpr bool kIsCompressorActive = true;
constexpr bool kIsSchedulingEnabled = true;

constexpr float kCircuit1Temperature = 1.1f;
constexpr float kCircuit2Temperature = 2.2f;
constexpr float kCircuit3Temperature = 3.3f;
constexpr float kGroundInputTemperature = 4.4f;
constexpr float kGroundOutputTemperature = 5.5f;
constexpr float kHotGasTemperature = 6.6f;
constexpr float kInsideTemperature = 7.7f;
constexpr float kLowerTankTemperature = 8.8f;
constexpr float kOutsideTemperature = 9.9f;
constexpr float kUpperTankTemperature = 10.10f;

constexpr communicator::BoostingSchedule kCircuit3BoostingSchedule{.monday = {1, 18, -1},
                                                                   .tuesday = {2, 19, -2},
                                                                   .wednesday = {3, 20, -3},
                                                                   .thursday = {4, 21, -4},
                                                                   .friday = {5, 22, -5},
                                                                   .saturday = {6, 23, -6},
                                                                   .sunday = {7, 24, -7}};

constexpr communicator::BoostingSchedule kLowerTankBoostingSchedule{.monday = {8, 15, 1},
                                                                    .tuesday = {9, 16, 2},
                                                                    .wednesday = {10, 17, 3},
                                                                    .thursday = {11, 18, 4},
                                                                    .friday = {12, 19, 5},
                                                                    .saturday = {13, 20, 6},
                                                                    .sunday = {14, 21, 7}};

constexpr communicator::TankLimits kTankLimits{.lower_tank_minimum = 10,
                                               .lower_tank_minimum_adjusted = 11,
                                               .lower_tank_maximum = 20,
                                               .lower_tank_maximum_adjusted = 21,
                                               .upper_tank_minimum = 30,
                                               .upper_tank_minimum_adjusted = 31,
                                               .upper_tank_maximum = 40,
                                               .upper_tank_maximum_adjusted = 41};

class MockModbusTCPServer {
 public:
  explicit MockModbusTCPServer(std::atomic<bool>& stop);

  ~MockModbusTCPServer();

  [[nodiscard]] std::thread Run();

  [[nodiscard]] static std::string Host() { return "127.0.0.1"; }

  [[nodiscard]] static int32_t Port() { return 502; }

  void InitializeMapping();

 private:
  std::mutex mutex_{};
  std::atomic<bool>& stop_;
  modbus_t* context_{nullptr};
  modbus_mapping_t* mapping_{nullptr};
  int socket_{-1};
};
