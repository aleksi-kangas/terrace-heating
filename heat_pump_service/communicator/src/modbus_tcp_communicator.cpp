#include "communicator/modbus_tcp_communicator.h"

#include <stdexcept>

namespace communicator {

ModbusTCPCommunicator::ModbusTCPCommunicator(const std::string& host,
                                             int32_t port) {
  context_ = modbus_new_tcp(host.c_str(), port);
  if (context_ == nullptr) {
    throw std::runtime_error{"Failed to create modbus context"};
  }
}

}  // namespace communicator
