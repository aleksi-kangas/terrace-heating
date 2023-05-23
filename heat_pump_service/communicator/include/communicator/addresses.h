#pragma once

#include <algorithm>
#include <cstdint>
#include <iterator>
#include <utility>
#include <vector>

namespace communicator::addresses {
namespace miscellaneous {
constexpr int32_t kActiveCircuitCount = 5100;
constexpr int32_t kCompressorActive = 5158;
constexpr int32_t kSchedulingEnabled = 134;
}  // namespace miscellaneous

namespace tank_limits {
constexpr int32_t kLowerTankMinimum{71};
constexpr int32_t kLowerTankMinimumAdjusted{75};
constexpr int32_t kLowerTankMaximum{72};
constexpr int32_t kLowerTankMaximumAdjusted{76};
constexpr int32_t kUpperTankMinimum{77};
constexpr int32_t kUpperTankMinimumAdjusted{79};
constexpr int32_t kUpperTankMaximum{78};
constexpr int32_t kUpperTankMaximumAdjusted{80};

constexpr int32_t kFirst{71};
constexpr int32_t kLast{80};
constexpr int32_t kQuantity{10};
}  // namespace tank_limits

namespace temperatures {
constexpr int32_t kCircuit1{5};
constexpr int32_t kCircuit2{6};
constexpr int32_t kCircuit3{117};
constexpr int32_t kGroundInput{99};
constexpr int32_t kGroundOutput{98};
constexpr int32_t kHotGas{2};
constexpr int32_t kInside{74};
constexpr int32_t kLowerTank{17};
constexpr int32_t kOutside{1};
constexpr int32_t kUpperTank{18};

constexpr int32_t kFirst{1};
constexpr int32_t kLast{117};
constexpr int32_t kQuantity{117};
}  // namespace temperatures

namespace boosting_schedules {
struct BoostingScheduleAddresses {
  struct WeekdayAddresses {
    int32_t start_hour;
    int32_t end_hour;
    int32_t delta;
  };

  constexpr BoostingScheduleAddresses(WeekdayAddresses monday,
                                      WeekdayAddresses tuesday,
                                      WeekdayAddresses wednesday,
                                      WeekdayAddresses thursday,
                                      WeekdayAddresses friday,
                                      WeekdayAddresses saturday,
                                      WeekdayAddresses sunday)
      : monday{monday},
        tuesday{tuesday},
        wednesday{wednesday},
        thursday{thursday},
        friday{friday},
        saturday{saturday},
        sunday{sunday} {}

  WeekdayAddresses monday;
  WeekdayAddresses tuesday;
  WeekdayAddresses wednesday;
  WeekdayAddresses thursday;
  WeekdayAddresses friday;
  WeekdayAddresses saturday;
  WeekdayAddresses sunday;

  [[nodiscard]] constexpr int32_t FirstHour() const {
    return std::min({monday.start_hour, monday.end_hour, tuesday.start_hour,
                     tuesday.end_hour, wednesday.start_hour, wednesday.end_hour,
                     thursday.start_hour, thursday.end_hour, friday.start_hour,
                     friday.end_hour, saturday.start_hour, saturday.end_hour,
                     sunday.start_hour, sunday.end_hour});
  }

  [[nodiscard]] constexpr int32_t LastHour() const {
    return std::max({monday.start_hour, monday.end_hour, tuesday.start_hour,
                     tuesday.end_hour, wednesday.start_hour, wednesday.end_hour,
                     thursday.start_hour, thursday.end_hour, friday.start_hour,
                     friday.end_hour, saturday.start_hour, saturday.end_hour,
                     sunday.start_hour, sunday.end_hour});
  }

  [[nodiscard]] constexpr int32_t QuantityHour() const {
    return LastHour() - FirstHour() + 1;
  }

  [[nodiscard]] constexpr int32_t FirstDelta() const {
    return std::min({monday.delta, tuesday.delta, wednesday.delta,
                     thursday.delta, friday.delta, saturday.delta,
                     sunday.delta});
  }

  [[nodiscard]] constexpr int32_t LastDelta() const {
    return std::max({monday.delta, tuesday.delta, wednesday.delta,
                     thursday.delta, friday.delta, saturday.delta,
                     sunday.delta});
  }

  [[nodiscard]] constexpr int32_t QuantityDelta() const {
    return LastDelta() - FirstDelta() + 1;
  }
};

constexpr BoostingScheduleAddresses kCircuit3{
    {5214, 5213, 107}, {5211, 5212, 106}, {5220, 5221, 110}, {5222, 5215, 111},
    {5223, 5224, 112}, {5216, 5217, 108}, {5218, 5219, 109}};
constexpr BoostingScheduleAddresses kLowerTank{
    {5014, 5021, 36}, {5015, 5022, 37}, {5016, 5023, 38}, {5017, 5024, 39},
    {5018, 5025, 41}, {5019, 5026, 42}, {5020, 5027, 43}};

}  // namespace boosting_schedules

}  // namespace communicator::addresses
