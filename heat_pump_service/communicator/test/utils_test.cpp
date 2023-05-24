#include "communicator/utils.h"

#include <gtest/gtest.h>

using namespace ::testing;
using namespace communicator::utils;

TEST(Utils, ExtractContiguousAddressRanges) {
  constexpr uint16_t kValueIgnored = 0;

  ScheduleAddressValueMappings input{{
      {1, kValueIgnored},
      {2, kValueIgnored},
      {3, kValueIgnored},
      {5, kValueIgnored},
      {6, kValueIgnored},
      {8, kValueIgnored},
      {10, kValueIgnored},
      {11, kValueIgnored},
      {12, kValueIgnored},
      {13, kValueIgnored},
      {15, kValueIgnored},
      {16, kValueIgnored},
      {18, kValueIgnored},
      {20, kValueIgnored},
      {21, kValueIgnored},
      {22, kValueIgnored},
      {23, kValueIgnored},
      {24, kValueIgnored},
      {26, kValueIgnored},
      {27, kValueIgnored},
      {29, kValueIgnored}
  }};

  const auto result = ExtractContiguousAddressRanges(input);
  const std::vector<std::vector<ScheduleAddressValueMapping>> kExpected{
      {
          {1, kValueIgnored},
          {2, kValueIgnored},
          {3, kValueIgnored}
      },
      {
          {5, kValueIgnored},
          {6, kValueIgnored}
      },
      {
          {8, kValueIgnored}
      },
      {
          {10, kValueIgnored},
          {11, kValueIgnored},
          {12, kValueIgnored},
          {13, kValueIgnored}
      },
      {
          {15, kValueIgnored},
          {16, kValueIgnored}
      },
      {
          {18, kValueIgnored}
      },
      {
          {20, kValueIgnored},
          {21, kValueIgnored},
          {22, kValueIgnored},
          {23, kValueIgnored},
          {24, kValueIgnored}
      },
      {
          {26, kValueIgnored},
          {27, kValueIgnored}
      },
      {
          {29, kValueIgnored}
      }
  };
  EXPECT_EQ(result, kExpected);
}
