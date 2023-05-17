cmake_minimum_required(VERSION 3.22)
project(communicator_test)

include(FetchContent)
FetchContent_Declare(googletest
        GIT_REPOSITORY https://github.com/google/googletest
        GIT_TAG main)
FetchContent_MakeAvailable(googletest)
include(GoogleTest)

add_executable(${PROJECT_NAME}
        mock_modbus_tcp_server.h    mock_modbus_tcp_server.cpp
        modbus_tcp_communicator_test.cpp)
target_link_libraries(${PROJECT_NAME} PUBLIC communicator modbus gtest gmock gtest_main)
target_compile_options(${PROJECT_NAME} PRIVATE -Wall -Wextra)
target_compile_features(${PROJECT_NAME} PUBLIC cxx_std_20)

gtest_discover_tests(${PROJECT_NAME})
