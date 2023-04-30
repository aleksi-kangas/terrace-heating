cmake_minimum_required(VERSION 3.22)
project(heat_pump_service)

find_package(gRPC CONFIG REQUIRED)

add_subdirectory(communicator)

add_executable(${PROJECT_NAME} main.cpp)
target_link_libraries(${PROJECT_NAME} gRPC::grpc++ communicator)
