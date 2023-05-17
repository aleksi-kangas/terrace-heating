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

BoostingSchedule ParseBoostingSchedule(
    const std::array<uint16_t, 7 * 2>& hour_values,
    const std::array<uint16_t, 7>& delta_values) {
  constexpr auto kHourOffset = static_cast<uint32_t>(
      registers::kCircuit3BoostingSchedule.HourRegisterRange().first);
  constexpr auto kDeltaOffset = static_cast<uint32_t>(
      registers::kCircuit3BoostingSchedule.DeltaRegisterRange().first);
  // clang-format off
  return {
      .monday = {
          .start_hour = hour_values[registers::kCircuit3BoostingSchedule.monday.start_hour - kHourOffset],
          .end_hour = hour_values[registers::kCircuit3BoostingSchedule.monday.end_hour - kHourOffset],
          .temperature_delta = delta_values[registers::kCircuit3BoostingSchedule.monday.delta - kDeltaOffset],
      },
      .tuesday = {
          .start_hour = hour_values[registers::kCircuit3BoostingSchedule.tuesday.start_hour - kHourOffset],
          .end_hour = hour_values[registers::kCircuit3BoostingSchedule.tuesday.end_hour - kHourOffset],
          .temperature_delta = delta_values[registers::kCircuit3BoostingSchedule.tuesday.delta - kDeltaOffset],
      },
      .wednesday = {
          .start_hour = hour_values[registers::kCircuit3BoostingSchedule.wednesday.start_hour - kHourOffset],
          .end_hour = hour_values[registers::kCircuit3BoostingSchedule.wednesday.end_hour - kHourOffset],
          .temperature_delta = delta_values[registers::kCircuit3BoostingSchedule.wednesday.delta - kDeltaOffset],
      },
      .thursday = {
          .start_hour = hour_values[registers::kCircuit3BoostingSchedule.thursday.start_hour - kHourOffset],
          .end_hour = hour_values[registers::kCircuit3BoostingSchedule.thursday.end_hour - kHourOffset],
          .temperature_delta = delta_values[registers::kCircuit3BoostingSchedule.thursday.delta - kDeltaOffset],
      },
      .friday = {
          .start_hour = hour_values[registers::kCircuit3BoostingSchedule.friday.start_hour - kHourOffset],
          .end_hour = hour_values[registers::kCircuit3BoostingSchedule.friday.end_hour - kHourOffset],
          .temperature_delta = delta_values[registers::kCircuit3BoostingSchedule.friday.delta - kDeltaOffset],
      },
      .saturday = {
          .start_hour = hour_values[registers::kCircuit3BoostingSchedule.saturday.start_hour - kHourOffset],
          .end_hour = hour_values[registers::kCircuit3BoostingSchedule.saturday.end_hour - kHourOffset],
          .temperature_delta = delta_values[registers::kCircuit3BoostingSchedule.saturday.delta - kDeltaOffset],
      },
      .sunday = {
          .start_hour = hour_values[registers::kCircuit3BoostingSchedule.sunday.start_hour - kHourOffset],
          .end_hour = hour_values[registers::kCircuit3BoostingSchedule.sunday.end_hour - kHourOffset],
          .temperature_delta = delta_values[registers::kCircuit3BoostingSchedule.sunday.delta - kDeltaOffset],
      }
  };
  // clang-format on
}

}  // namespace communicator::utils
