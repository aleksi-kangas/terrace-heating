cmake_minimum_required(VERSION 3.22)
project(service_proto)

# TODO Move .proto files to a common directory...

find_package(gRPC CONFIG REQUIRED)
find_package(Protobuf CONFIG REQUIRED)

add_library(${PROJECT_NAME} heat_pump_service.proto)
get_target_property(_GRPC_CPP_PLUGIN_EXECUTABLE gRPC::grpc_cpp_plugin
        IMPORTED_LOCATION_RELEASE)
protobuf_generate(
        TARGET ${PROJECT_NAME}
        LANGUAGE cpp)
protobuf_generate(
        TARGET ${PROJECT_NAME}
        LANGUAGE grpc
        GENERATE_EXTENSIONS .grpc.pb.h .grpc.pb.cc
        PLUGIN "protoc-gen-grpc=${_GRPC_CPP_PLUGIN_EXECUTABLE}")
target_include_directories(${PROJECT_NAME} PUBLIC ${CMAKE_CURRENT_BINARY_DIR})
target_link_libraries(${PROJECT_NAME} PUBLIC
        gRPC::grpc++
        gRPC::grpc++_reflection
        protobuf::libprotobuf)
