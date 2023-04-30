cmake_minimum_required(VERSION 3.22)
project(communicator)

find_package(libmodbus CONFIG REQUIRED)

add_library(${PROJECT_NAME}
        include/communicator/communicator.h
        include/communicator/modbus_tcp_communicator.h  src/modbus_tcp_communicator.cpp)
target_include_directories(${PROJECT_NAME} PUBLIC include)
target_link_libraries(${PROJECT_NAME} PUBLIC modbus)
target_compile_options(${PROJECT_NAME} PRIVATE -Wall -Wextra)
target_compile_features(${PROJECT_NAME} PUBLIC cxx_std_20)
