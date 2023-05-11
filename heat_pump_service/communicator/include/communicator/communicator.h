#pragma once

namespace communicator {
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
  int32_t lower_tank_maximum{0};
  int32_t upper_tank_minimum{0};
  int32_t upper_tank_maximum{0};
};

class ICommunicator {
 public:
  virtual ~ICommunicator() = default;

  [[nodiscard]] virtual uint32_t ActiveCircuitCount() const = 0;
  [[nodiscard]] virtual Temperatures ReadTemperatures() const = 0;
  [[nodiscard]] virtual TankLimits ReadTankLimits() const = 0;
};
}  // namespace communicator
