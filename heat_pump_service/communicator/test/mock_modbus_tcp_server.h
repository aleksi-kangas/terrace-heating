#pragma once

#include <atomic>
#include <cstdint>
#include <mutex>
#include <string>
#include <thread>

#include <modbus.h>

constexpr uint32_t kActiveCircuitCount = 3;
constexpr bool kIsCompressorActive = true;
constexpr bool kIsSchedulingEnabled = true;

constexpr uint32_t kLowerTankMinimum = 10;
constexpr uint32_t kLowerTankMaximum = 20;
constexpr uint32_t kUpperTankMinimum = 30;
constexpr uint32_t kUpperTankMaximum = 40;

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
