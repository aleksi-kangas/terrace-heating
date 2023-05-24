#pragma once

#include <cstdint>

namespace communicator {
struct WeekdayBoostingSchedule {
  uint32_t start_hour{0};        // [  0, 24]
  uint32_t end_hour{0};          // [  0, 24]
  int32_t temperature_delta{0};  // [-10, 10]

  auto operator<=>(const WeekdayBoostingSchedule&) const = default;

  [[nodiscard]] bool IsValid() const;
};

struct BoostingSchedule {
  WeekdayBoostingSchedule monday{};
  WeekdayBoostingSchedule tuesday{};
  WeekdayBoostingSchedule wednesday{};
  WeekdayBoostingSchedule thursday{};
  WeekdayBoostingSchedule friday{};
  WeekdayBoostingSchedule saturday{};
  WeekdayBoostingSchedule sunday{};

  auto operator<=>(const BoostingSchedule&) const = default;

  [[nodiscard]] bool IsValid() const;
};

struct Temperatures {
  float circuit1{0};
  float circuit2{0};
  float circuit3{0};
  float ground_input{0};
  float ground_output{0};
  float hot_gas{0};
  float inside{0};
  float lower_tank{0};
  float outside{0};
  float upper_tank{0};
};

struct TankLimits {
  int32_t lower_tank_minimum{0};
  int32_t lower_tank_minimum_adjusted{0};
  int32_t lower_tank_maximum{0};
  int32_t lower_tank_maximum_adjusted{0};
  int32_t upper_tank_minimum{0};
  int32_t upper_tank_minimum_adjusted{0};
  int32_t upper_tank_maximum{0};
  int32_t upper_tank_maximum_adjusted{0};

  auto operator<=>(const TankLimits&) const = default;
};

}  // namespace communicator
