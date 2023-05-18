#pragma once

#include <array>
#include <cstdint>

#include "communicator/communicator.h"

#include "communicator/registers.h"

namespace communicator::utils {
[[nodiscard]] Temperatures ParseTemperatures(
    const std::array<uint16_t, 117>& values);

[[nodiscard]] TankLimits ParseTankLimits(const std::array<uint16_t, 8>& values);

[[nodiscard]] BoostingSchedule ParseBoostingSchedule(
    const registers::BoostingScheduleAddresses& addresses,
    const std::vector<uint16_t>& hour_values,
    const std::vector<uint16_t>& delta_values);

}  // namespace communicator::utils
