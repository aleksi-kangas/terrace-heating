cmake_minimum_required(VERSION 3.22)
project(heat_pump_service)

option(ENABLE_TESTS "Enable tests" ON)
if (${ENABLE_TESTS})
    enable_testing()
endif ()

add_subdirectory(communicator)
add_subdirectory(service)

add_executable(${PROJECT_NAME} main.cpp)
target_link_libraries(${PROJECT_NAME} PRIVATE communicator service)
target_compile_options(${PROJECT_NAME} PRIVATE -Wall -Wextra)
target_compile_features(${PROJECT_NAME} PRIVATE cxx_std_20)
