#pragma once

#include <array>
#include <cstdint>
#include <vector>

#include "communicator/communicator.h"

#include "communicator/registers.h"

namespace communicator::utils {
[[nodiscard]] Temperatures ParseTemperatures(
    const std::array<uint16_t, 117>& values);

[[nodiscard]] TankLimits ParseTankLimits(
    const registers::TankLimitAddresses& addresses,
    const std::vector<uint16_t>& values);

[[nodiscard]] BoostingSchedule ParseBoostingSchedule(
    const registers::BoostingScheduleHourAddresses& hour_addresses,
    const std::vector<uint16_t>& hour_values,
    const registers::BoostingScheduleDeltaAddresses& delta_addresses,
    const std::vector<uint16_t>& delta_values);

}  // namespace communicator::utils
