#include "communicator/utils.h"

#include "communicator/registers.h"

namespace {
float TemperatureUInt16ToFloat(uint16_t value) {
  // Temperature are represented with one decimal place,
  // i.e. 123 means 12.3 degrees.
  return static_cast<float>(value) / 10.0f;
}
}  // namespace

namespace communicator::utils {
Temperatures ParseTemperatures(const std::array<uint16_t, 117>& values) {
  constexpr int32_t kOffset = registers::kTemperatureRegisterRange.first;
  return Temperatures{
      .circuit1 = TemperatureUInt16ToFloat(
          values[registers::kCircuit1Temperature - kOffset]),
      .circuit2 = TemperatureUInt16ToFloat(
          values[registers::kCircuit2Temperature - kOffset]),
      .circuit3 = TemperatureUInt16ToFloat(
          values[registers::kCircuit3Temperature - kOffset]),
      .ground_input = TemperatureUInt16ToFloat(
          values[registers::kGroundInputTemperature - kOffset]),
      .ground_output = TemperatureUInt16ToFloat(
          values[registers::kGroundOutputTemperature - kOffset]),
      .hot_gas = TemperatureUInt16ToFloat(
          values[registers::kHotGasTemperature - kOffset]),
      .inside = TemperatureUInt16ToFloat(
          values[registers::kInsideTemperature - kOffset]),
      .lower_tank = TemperatureUInt16ToFloat(
          values[registers::kLowerTankTemperature - kOffset]),
      .outside = TemperatureUInt16ToFloat(
          values[registers::kOutsideTemperature - kOffset]),
      .upper_tank = TemperatureUInt16ToFloat(
          values[registers::kUpperTankTemperature - kOffset]),
  };
}

TankLimits ParseTankLimits(const std::array<uint16_t, 8>& values) {
  constexpr auto kOffset =
      static_cast<uint32_t>(registers::kTankLimitRegisterRange.first);
  return TankLimits{
      .lower_tank_minimum =
          static_cast<uint32_t>(values[registers::kLowerTankMinimum - kOffset]),
      .lower_tank_maximum =
          static_cast<uint32_t>(values[registers::kLowerTankMaximum - kOffset]),
      .upper_tank_minimum =
          static_cast<uint32_t>(values[registers::kUpperTankMinimum - kOffset]),
      .upper_tank_maximum =
          static_cast<uint32_t>(values[registers::kUpperTankMaximum - kOffset]),
  };
}

}  // namespace communicator::utils
