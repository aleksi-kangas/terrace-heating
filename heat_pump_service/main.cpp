#include <cstdint>
#include <cstdlib>
#include <filesystem>
#include <fstream>
#include <iostream>
#include <sstream>
#include <string>

#include <grpcpp/ext/proto_server_reflection_plugin.h>
#include <grpcpp/grpcpp.h>

#include "communicator/modbus_tcp_communicator.h"
#include "service/service.h"

std::string ReadEnvironmentVariable(const std::string& key) {
  char* value = std::getenv(key.c_str());
  return value != nullptr ? std::string{value} : std::string{""};
}

std::shared_ptr<grpc::ServerCredentials> MakeSslServerCredentials(const std::filesystem::path& server_private_key_file,
                                                                  const std::filesystem::path& server_certificate_file,
                                                                  const std::filesystem::path& root_certificate_file) {
  auto ReadFile = [](const std::filesystem::path& file) -> std::string {
    std::ifstream file_stream{file};
    std::stringstream ss{};
    ss << file_stream.rdbuf();
    return ss.str();
  };
  const std::string server_private_key = ReadFile(server_private_key_file);
  const std::string server_certificate = ReadFile(server_certificate_file);
  std::string root_certificate = ReadFile(root_certificate_file);
  grpc::SslServerCredentialsOptions::PemKeyCertPair pem_key_cert_pair{.private_key = server_private_key,
                                                                      .cert_chain = server_certificate};
  grpc::SslServerCredentialsOptions ssl_server_credential_options{};
  ssl_server_credential_options.client_certificate_request = GRPC_SSL_REQUEST_AND_REQUIRE_CLIENT_CERTIFICATE_AND_VERIFY;
  ssl_server_credential_options.force_client_auth = true;
  ssl_server_credential_options.pem_key_cert_pairs = {std::move(pem_key_cert_pair)};
  ssl_server_credential_options.pem_root_certs = std::move(root_certificate);
  return grpc::SslServerCredentials(ssl_server_credential_options);
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

  grpc::reflection::InitProtoReflectionServerBuilderPlugin();
  grpc::ServerBuilder builder{};
  auto server_credentials =
      MakeSslServerCredentials("/usr/src/app/certs/heat-pump-service-key.pem",
                               "/usr/src/app/certs/heat-pump-service-cert.pem", "/usr/src/app/certs/ca-cert.pem");
  if (server_credentials == nullptr)
    throw std::runtime_error{"Failed to create SSL credentials"};
  builder.AddListeningPort("0.0.0.0:50051", server_credentials);

  service::HeatPumpService service{std::make_unique<communicator::ModbusTCPCommunicator>(kHeatPumpIp, kHeatPumpPort)};
  builder.RegisterService(&service);

  std::unique_ptr<grpc::Server> server{builder.BuildAndStart()};
  std::cout << "Service listening at port 50051" << std::endl;
  server->Wait();
}
