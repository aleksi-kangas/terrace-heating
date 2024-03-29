#pragma once

#include <array>
#include <cstdint>
#include <vector>

#include "communicator/addresses.h"
#include "communicator/communicator.h"

namespace communicator::utils {
[[nodiscard]] Temperatures ParseTemperatures(const std::array<uint16_t, addresses::temperatures::kQuantity>& values);

[[nodiscard]] TankLimits ParseTankLimits(const std::array<uint16_t, addresses::tank_limits::kQuantity>& values);

[[nodiscard]] BoostingSchedule ParseBoostingSchedule(
    const addresses::boosting_schedules::BoostingScheduleAddresses& addresses, const std::vector<uint16_t>& hour_values,
    const std::vector<uint16_t>& delta_values);

using ScheduleAddressValueMapping = std::pair<int32_t, uint16_t>;
using ScheduleAddressValueMappings = std::array<ScheduleAddressValueMapping, 3 * 7>;

[[nodiscard]] ScheduleAddressValueMappings GenerateSortedScheduleAddressValueMappings(
    const addresses::boosting_schedules::BoostingScheduleAddresses& addresses, const BoostingSchedule& schedule);

[[nodiscard]] std::vector<std::vector<ScheduleAddressValueMapping>> ExtractContiguousAddressRanges(
    const ScheduleAddressValueMappings& address_value_mappings);
}  // namespace communicator::utils
