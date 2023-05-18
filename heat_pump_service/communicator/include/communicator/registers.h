#pragma once

#include <algorithm>
#include <cstdint>
#include <utility>

namespace communicator::registers {
struct IAddressRange {
  ~IAddressRange() = default;

  [[nodiscard]] virtual constexpr std::pair<int32_t, int32_t> Range() const = 0;

  [[nodiscard]] virtual constexpr int32_t RangeSpan() const {
    return Range().second - Range().first + 1;
  }
};

// Temperatures (include 1 decimal place, shall be divided by 10)
constexpr int32_t kCircuit1Temperature = 5;
constexpr int32_t kCircuit2Temperature = 6;
constexpr int32_t kCircuit3Temperature = 117;
constexpr int32_t kGroundInputTemperature = 99;
constexpr int32_t kGroundOutputTemperature = 98;
constexpr int32_t kHotGasTemperature = 2;
constexpr int32_t kInsideTemperature = 74;
constexpr int32_t kLowerTankTemperature = 17;
constexpr int32_t kOutsideTemperature = 1;
constexpr int32_t kUpperTankTemperature = 18;

constexpr std::pair<int32_t, int32_t> kTemperatureRegisterRange{
    std::min({kCircuit1Temperature, kCircuit2Temperature, kCircuit3Temperature,
              kGroundInputTemperature, kGroundOutputTemperature,
              kHotGasTemperature, kInsideTemperature, kLowerTankTemperature,
              kOutsideTemperature, kUpperTankTemperature}),
    std::max({kCircuit1Temperature, kCircuit2Temperature, kCircuit3Temperature,
              kGroundInputTemperature, kGroundOutputTemperature,
              kHotGasTemperature, kInsideTemperature, kLowerTankTemperature,
              kOutsideTemperature, kUpperTankTemperature})};

// Tank limits
constexpr int32_t kLowerTankMinimum = 71;
constexpr int32_t kLowerTankMaximum = 72;
constexpr int32_t kUpperTankMinimum = 77;
constexpr int32_t kUpperTankMaximum = 78;
constexpr std::pair<int32_t, int32_t> kTankLimitRegisterRange{
    std::min({kLowerTankMinimum, kLowerTankMaximum, kUpperTankMinimum,
              kUpperTankMaximum}),
    std::max({kLowerTankMinimum, kLowerTankMaximum, kUpperTankMinimum,
              kUpperTankMaximum})};

// Miscellaneous
constexpr int32_t kActiveCircuitCount = 5100;
constexpr int32_t kCompressorActive = 5158;
constexpr int32_t kSchedulingEnabled = 134;

// Boosting schedule(s)
using WeekdayHours = std::pair<int32_t, int32_t>;

struct BoostingScheduleHourAddresses : public IAddressRange {
  constexpr BoostingScheduleHourAddresses(WeekdayHours monday_hours,
                                          WeekdayHours tuesday_hours,
                                          WeekdayHours wednesday_hours,
                                          WeekdayHours thursday_hours,
                                          WeekdayHours friday_hours,
                                          WeekdayHours saturday_hours,
                                          WeekdayHours sunday_hours)
      : monday{monday_hours},
        tuesday{tuesday_hours},
        wednesday{wednesday_hours},
        thursday{thursday_hours},
        friday{friday_hours},
        saturday{saturday_hours},
        sunday{sunday_hours} {}

  struct Weekday {
    constexpr explicit Weekday(WeekdayHours hours)
        : start_hour{hours.first}, end_hour{hours.second} {}

    int32_t start_hour{-1};
    int32_t end_hour{-1};
  };

  Weekday monday;
  Weekday tuesday;
  Weekday wednesday;
  Weekday thursday;
  Weekday friday;
  Weekday saturday;
  Weekday sunday;

  [[nodiscard]] constexpr std::pair<int32_t, int32_t> Range() const override {
    return {
        std::min({monday.start_hour, monday.end_hour, tuesday.start_hour,
                  tuesday.end_hour, wednesday.start_hour, wednesday.end_hour,
                  thursday.start_hour, thursday.end_hour, friday.start_hour,
                  friday.end_hour, saturday.start_hour, saturday.end_hour,
                  sunday.start_hour, sunday.end_hour}),
        std::max({monday.start_hour, monday.end_hour, tuesday.start_hour,
                  tuesday.end_hour, wednesday.start_hour, wednesday.end_hour,
                  thursday.start_hour, thursday.end_hour, friday.start_hour,
                  friday.end_hour, saturday.start_hour, saturday.end_hour,
                  sunday.start_hour, sunday.end_hour})};
  }
};

struct BoostingScheduleDeltaAddresses : public IAddressRange {
  constexpr BoostingScheduleDeltaAddresses(
      int32_t monday_delta, int32_t tuesday_delta, int32_t wednesday_delta,
      int32_t thursday_delta, int32_t friday_delta, int32_t saturday_delta,
      int32_t sunday_delta)
      : monday{monday_delta},
        tuesday{tuesday_delta},
        wednesday{wednesday_delta},
        thursday{thursday_delta},
        friday{friday_delta},
        saturday{saturday_delta},
        sunday{sunday_delta} {}

  struct Weekday {
    int32_t delta{-1};
  };

  Weekday monday{};
  Weekday tuesday{};
  Weekday wednesday{};
  Weekday thursday{};
  Weekday friday{};
  Weekday saturday{};
  Weekday sunday{};

  [[nodiscard]] constexpr std::pair<int32_t, int32_t> Range() const override {
    return {
        std::min({monday.delta, tuesday.delta, wednesday.delta, thursday.delta,
                  friday.delta, saturday.delta, sunday.delta}),
        std::max({monday.delta, tuesday.delta, wednesday.delta, thursday.delta,
                  friday.delta, saturday.delta, sunday.delta})};
  }
};

constexpr BoostingScheduleHourAddresses kCircuit3BoostingScheduleHours{
    {5214, 5213}, {5211, 5212}, {5220, 5221}, {5222, 5215},
    {5223, 5224}, {5216, 5217}, {5218, 5219}};

constexpr BoostingScheduleDeltaAddresses kCircuit3BoostingScheduleDeltas{
    107, 106, 110, 111, 112, 108, 109};

constexpr BoostingScheduleHourAddresses kLowerTankBoostingScheduleHours{
    {5014, 5021}, {5015, 5022}, {5016, 5023}, {5017, 5024},
    {5018, 5025}, {5019, 5026}, {5020, 5027},
};

constexpr BoostingScheduleDeltaAddresses kLowerTankBoostingScheduleDeltas{
    36, 37, 38, 39, 41, 42, 43};

}  // namespace communicator::registers
