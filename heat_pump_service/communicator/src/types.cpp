#include "communicator/types.h"

namespace communicator {
bool WeekdayBoostingSchedule::IsValid() const {
  return start_hour <= 24 && end_hour <= 24 && -10 <= temperature_delta &&
         temperature_delta <= 10;
}

bool BoostingSchedule::IsValid() const {
  return monday.IsValid() && tuesday.IsValid() && wednesday.IsValid() &&
         thursday.IsValid() && friday.IsValid() && saturday.IsValid() &&
         sunday.IsValid();
}

}  // namespace communicator
