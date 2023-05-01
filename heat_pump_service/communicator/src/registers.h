#pragma once

#include <algorithm>
#include <cstdint>
#include <utility>

namespace communicator::registers {
// Temperatures (include 1 decimal place, shall be divided by 10)
constexpr int32_t kCircuit1Temperature = 5;
constexpr int32_t kCircuit2Temperature = 6;
constexpr int32_t kCircuit3Temperature = 117;
constexpr int32_t kGroundInputTemperature = 99;
constexpr int32_t kGroundOutputTemperature = 98;
constexpr int32_t kHotGasTemperature = 2;
constexpr int32_t kInsideTemperature = 74;
constexpr int32_t kLowerTankTemperature = 17;
constexpr int32_t kOutsideTemperature = 1;
constexpr int32_t kUpperTankTemperature = 18;

constexpr std::pair<int32_t, int32_t> kTemperatureRegisterRange{
    std::min({kCircuit1Temperature, kCircuit2Temperature, kCircuit3Temperature,
              kGroundInputTemperature, kGroundOutputTemperature,
              kHotGasTemperature, kInsideTemperature, kLowerTankTemperature,
              kOutsideTemperature, kUpperTankTemperature}),
    std::max({kCircuit1Temperature, kCircuit2Temperature, kCircuit3Temperature,
              kGroundInputTemperature, kGroundOutputTemperature,
              kHotGasTemperature, kInsideTemperature, kLowerTankTemperature,
              kOutsideTemperature, kUpperTankTemperature})};

// Tank limits
constexpr int32_t kLowerTankMinimum = 71;
constexpr int32_t kLowerTankMaximum = 72;
constexpr int32_t kUpperTankMinimum = 77;
constexpr int32_t kUpperTankMaximum = 78;
constexpr std::pair<int32_t, int32_t> kTankLimitRegisterRange{
    std::min({kLowerTankMinimum, kLowerTankMaximum, kUpperTankMinimum,
              kUpperTankMaximum}),
    std::max({kLowerTankMinimum, kLowerTankMaximum, kUpperTankMinimum,
              kUpperTankMaximum})};

}  // namespace communicator::registers
