#include <cstdint>
#include <cstdlib>
#include <string>

#include <grpcpp/ext/proto_server_reflection_plugin.h>
#include <grpcpp/grpcpp.h>

#include "communicator/modbus_tcp_communicator.h"
#include "service/service.h"

std::string ReadEnvironmentVariable(const std::string& key) {
  char* value = std::getenv(key.c_str());
  return value != nullptr ? std::string{value} : std::string{""};
}

int main() {
  const std::string kHeatPumpIp = ReadEnvironmentVariable("HEAT_PUMP_IP");
  if (kHeatPumpIp.empty()) {
    throw std::runtime_error{"HEAT_PUMP_IP environment variable is not set"};
  }

  const int32_t kHeatPumpPort = []() {
    const std::string kHeatPumpPortStr = ReadEnvironmentVariable("HEAT_PUMP_PORT");
    if (kHeatPumpPortStr.empty()) {
      throw std::runtime_error{"HEAT_PUMP_PORT environment variable is not set"};
    }
    try {
      return std::stoi(kHeatPumpPortStr);
    } catch (const std::invalid_argument&) {
      throw std::runtime_error{"HEAT_PUMP_PORT environment variable is not a valid port"};
    }
  }();

  service::HeatPumpService service{
      std::make_unique<communicator::ModbusTCPCommunicator>(kHeatPumpIp, kHeatPumpPort)};

  grpc::reflection::InitProtoReflectionServerBuilderPlugin();
  grpc::ServerBuilder builder{};
  builder.AddListeningPort("0.0.0.0:50051", grpc::InsecureServerCredentials());
  builder.RegisterService(&service);
  std::unique_ptr<grpc::Server> server{builder.BuildAndStart()};
  server->Wait();
}
