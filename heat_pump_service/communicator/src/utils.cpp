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
    const registers::BoostingScheduleAddresses& addresses,
    const std::vector<uint16_t>& hour_values,
    const std::vector<uint16_t>& delta_values) {
  const auto kHourOffset = addresses.HourRegisterRange().first;
  const auto kDeltaOffset = addresses.DeltaRegisterRange().first;
  // clang-format off
  return {
      .monday = {
          .start_hour = hour_values[addresses.monday.start_hour - kHourOffset],
          .end_hour = hour_values[addresses.monday.end_hour - kHourOffset],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.monday.delta - kDeltaOffset]),
      },
      .tuesday = {
          .start_hour = hour_values[addresses.tuesday.start_hour - kHourOffset],
          .end_hour = hour_values[addresses.tuesday.end_hour - kHourOffset],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.tuesday.delta - kDeltaOffset]),
      },
      .wednesday = {
          .start_hour = hour_values[addresses.wednesday.start_hour - kHourOffset],
          .end_hour = hour_values[addresses.wednesday.end_hour - kHourOffset],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.wednesday.delta - kDeltaOffset]),
      },
      .thursday = {
          .start_hour = hour_values[addresses.thursday.start_hour - kHourOffset],
          .end_hour = hour_values[addresses.thursday.end_hour - kHourOffset],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.thursday.delta - kDeltaOffset]),
      },
      .friday = {
          .start_hour = hour_values[addresses.friday.start_hour - kHourOffset],
          .end_hour = hour_values[addresses.friday.end_hour - kHourOffset],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.friday.delta - kDeltaOffset]),
      },
      .saturday = {
          .start_hour = hour_values[addresses.saturday.start_hour - kHourOffset],
          .end_hour = hour_values[addresses.saturday.end_hour - kHourOffset],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.saturday.delta - kDeltaOffset]),
      },
      .sunday = {
          .start_hour = hour_values[addresses.sunday.start_hour - kHourOffset],
          .end_hour = hour_values[addresses.sunday.end_hour - kHourOffset],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.sunday.delta - kDeltaOffset]),
      }
  };
  // clang-format on
}

}  // namespace communicator::utils
