cmake_minimum_required(VERSION 3.22)
project(communicator)

option(BUILD_COMMUNICATOR_TESTS "Build tests for communicator" ${ENABLE_TESTS})

find_package(libmodbus CONFIG REQUIRED)

add_library(${PROJECT_NAME}
        include/communicator/addresses.h
        include/communicator/communicator.h
        include/communicator/modbus_tcp_communicator.h  src/modbus_tcp_communicator.cpp
        include/communicator/types.h                    src/types.cpp
        include/communicator/utils.h                    src/utils.cpp)
target_include_directories(${PROJECT_NAME} PUBLIC include PRIVATE src)
target_link_libraries(${PROJECT_NAME} PUBLIC modbus)
target_compile_options(${PROJECT_NAME} PRIVATE -Wall -Wextra)
target_compile_features(${PROJECT_NAME} PUBLIC cxx_std_20)

if (${BUILD_COMMUNICATOR_TESTS})
    add_subdirectory(test)
endif ()
