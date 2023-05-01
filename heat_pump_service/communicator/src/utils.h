#pragma once

#include <array>
#include <cstdint>

#include "communicator/communicator.h"

#include "registers.h"

namespace communicator::utils {
[[nodiscard]] Temperatures ParseTemperatures(
    const std::array<uint16_t, 117>& values);

[[nodiscard]] TankLimits ParseTankLimits(
    const std::array<uint16_t, 8>& values);
}  // namespace communicator::utils
