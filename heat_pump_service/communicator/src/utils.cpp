#include "communicator/utils.h"

#include <cassert>
#include <ranges>
#include <utility>

namespace {
float TemperatureUInt16ToFloat(uint16_t value) {
  // Temperature are represented with one decimal place,
  // i.e. 123 means 12.3 degrees.
  return static_cast<float>(value) / 10.0f;
}
}  // namespace

namespace communicator::utils {
Temperatures ParseTemperatures(
    const std::array<uint16_t, addresses::temperatures::kQuantity>& values) {
  using namespace addresses::temperatures;
  return {
      .circuit1 = TemperatureUInt16ToFloat(values[kCircuit1 - kFirst]),
      .circuit2 = TemperatureUInt16ToFloat(values[kCircuit2 - kFirst]),
      .circuit3 = TemperatureUInt16ToFloat(values[kCircuit3 - kFirst]),
      .ground_input = TemperatureUInt16ToFloat(values[kGroundInput - kFirst]),
      .ground_output = TemperatureUInt16ToFloat(values[kGroundOutput - kFirst]),
      .hot_gas = TemperatureUInt16ToFloat(values[kHotGas - kFirst]),
      .inside = TemperatureUInt16ToFloat(values[kInside - kFirst]),
      .lower_tank = TemperatureUInt16ToFloat(values[kLowerTank - kFirst]),
      .outside = TemperatureUInt16ToFloat(values[kOutside - kFirst]),
      .upper_tank = TemperatureUInt16ToFloat(values[kUpperTank - kFirst]),
  };
}

TankLimits ParseTankLimits(
    const std::array<uint16_t, addresses::tank_limits::kQuantity>& values) {
  using namespace addresses::tank_limits;
  return {
      // clang-format off
      .lower_tank_minimum = static_cast<int16_t>(values[kLowerTankMinimum - kFirst]),
      .lower_tank_minimum_adjusted = static_cast<int16_t>(values[kLowerTankMinimumAdjusted - kFirst]),
      .lower_tank_maximum = static_cast<int16_t>(values[kLowerTankMaximum - kFirst]),
      .lower_tank_maximum_adjusted = static_cast<int16_t>(values[kLowerTankMaximumAdjusted - kFirst]),
      .upper_tank_minimum = static_cast<int16_t>(values[kUpperTankMinimum - kFirst]),
      .upper_tank_minimum_adjusted = static_cast<int16_t>(values[kUpperTankMinimumAdjusted - kFirst]),
      .upper_tank_maximum = static_cast<int16_t>(values[kUpperTankMaximum - kFirst]),
      .upper_tank_maximum_adjusted = static_cast<int16_t>(values[kUpperTankMaximumAdjusted - kFirst]),
      // clang-format off
  };
}

BoostingSchedule ParseBoostingSchedule(
    const addresses::boosting_schedules::BoostingScheduleAddresses& addresses,
    const std::vector<uint16_t>& hour_values,
    const std::vector<uint16_t>& delta_values) {
  return {
      // clang-format off
      .monday = {
          .start_hour = hour_values[addresses.monday.start_hour - addresses.FirstHour()],
          .end_hour = hour_values[addresses.monday.end_hour - addresses.FirstHour()],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.monday.delta - addresses.FirstDelta()]),
      },
      .tuesday = {
          .start_hour = hour_values[addresses.tuesday.start_hour - addresses.FirstHour()],
          .end_hour = hour_values[addresses.tuesday.end_hour - addresses.FirstHour()],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.tuesday.delta - addresses.FirstDelta()]),
      },
      .wednesday = {
          .start_hour = hour_values[addresses.wednesday.start_hour - addresses.FirstHour()],
          .end_hour = hour_values[addresses.wednesday.end_hour - addresses.FirstHour()],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.wednesday.delta - addresses.FirstDelta()]),
      },
      .thursday = {
          .start_hour = hour_values[addresses.thursday.start_hour - addresses.FirstHour()],
          .end_hour = hour_values[addresses.thursday.end_hour - addresses.FirstHour()],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.thursday.delta - addresses.FirstDelta()]),
      },
      .friday = {
          .start_hour = hour_values[addresses.friday.start_hour - addresses.FirstHour()],
          .end_hour = hour_values[addresses.friday.end_hour - addresses.FirstHour()],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.friday.delta - addresses.FirstDelta()]),
      },
      .saturday = {
          .start_hour = hour_values[addresses.saturday.start_hour - addresses.FirstHour()],
          .end_hour = hour_values[addresses.saturday.end_hour - addresses.FirstHour()],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.saturday.delta - addresses.FirstDelta()]),
      },
      .sunday = {
          .start_hour = hour_values[addresses.sunday.start_hour - addresses.FirstHour()],
          .end_hour = hour_values[addresses.sunday.end_hour - addresses.FirstHour()],
          .temperature_delta = static_cast<int16_t>(delta_values[addresses.sunday.delta - addresses.FirstDelta()]),
      }
      // clang-format on
  };
}

ScheduleAddressValueMappings GenerateSortedScheduleAddressValueMappings(
    const addresses::boosting_schedules::BoostingScheduleAddresses& addresses,
    const BoostingSchedule& schedule) {
  ScheduleAddressValueMappings mappings{{
      // clang-format off
      {addresses.monday.start_hour, static_cast<uint16_t>(schedule.monday.start_hour)},
      {addresses.monday.end_hour, static_cast<uint16_t>(schedule.monday.end_hour)},
      {addresses.monday.delta, static_cast<uint16_t>(schedule.monday.temperature_delta)},

      {addresses.tuesday.start_hour, static_cast<uint16_t>(schedule.tuesday.start_hour)},
      {addresses.tuesday.end_hour, static_cast<uint16_t>(schedule.tuesday.end_hour)},
      {addresses.tuesday.delta, static_cast<uint16_t>(schedule.tuesday.temperature_delta)},

      {addresses.wednesday.start_hour, static_cast<uint16_t>(schedule.wednesday.start_hour)},
      {addresses.wednesday.end_hour, static_cast<uint16_t>(schedule.wednesday.end_hour)},
      {addresses.wednesday.delta, static_cast<uint16_t>(schedule.wednesday.temperature_delta)},

      {addresses.thursday.start_hour, static_cast<uint16_t>(schedule.thursday.start_hour)},
      {addresses.thursday.end_hour, static_cast<uint16_t>(schedule.thursday.end_hour)},
      {addresses.thursday.delta, static_cast<uint16_t>(schedule.thursday.temperature_delta)},

      {addresses.friday.start_hour, static_cast<uint16_t>(schedule.friday.start_hour)},
      {addresses.friday.end_hour, static_cast<uint16_t>(schedule.friday.end_hour)},
      {addresses.friday.delta, static_cast<uint16_t>(schedule.friday.temperature_delta)},

      {addresses.saturday.start_hour, static_cast<uint16_t>(schedule.saturday.start_hour)},
      {addresses.saturday.end_hour, static_cast<uint16_t>(schedule.saturday.end_hour)},
      {addresses.saturday.delta, static_cast<uint16_t>(schedule.saturday.temperature_delta)},

      {addresses.sunday.start_hour, static_cast<uint16_t>(schedule.sunday.start_hour)},
      {addresses.sunday.end_hour, static_cast<uint16_t>(schedule.sunday.end_hour)},
      {addresses.sunday.delta, static_cast<uint16_t>(schedule.sunday.temperature_delta)},
      // clang-format on
  }};
  std::ranges::sort(mappings);
  return mappings;
}

std::vector<std::vector<ScheduleAddressValueMapping>>
ExtractContiguousAddressRanges(
    const ScheduleAddressValueMappings& address_value_mappings) {
  assert(std::ranges::is_sorted(address_value_mappings));
  std::vector<std::vector<ScheduleAddressValueMapping>> result{};
  // Iterate over the sorted addresses and extract contiguous address ranges
  auto b = address_value_mappings.begin();
  while (b != address_value_mappings.end()) {
    auto e = b + 1;
    while (e != address_value_mappings.end() &&
           e->first - (e - 1)->first == 1) {
      ++e;
    }
    result.insert(result.end(), {b, e});
    b = e;
  }
  return result;
};

}  // namespace communicator::utils
