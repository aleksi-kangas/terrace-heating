cmake_minimum_required(VERSION 3.22)
project(service)

option(BUILD_SERVICE_TESTS "Build tests for service" ${ENABLE_TESTS})

find_package(gRPC CONFIG REQUIRED)

add_subdirectory(proto)  # TODO Temporarily compile .proto here

add_library(${PROJECT_NAME}
        include/service/service.h   src/service.cpp)
target_include_directories(${PROJECT_NAME} PUBLIC include PRIVATE src)
target_link_libraries(${PROJECT_NAME} PUBLIC
        gRPC::grpc++
        gRPC::grpc++_reflection
        communicator
        ${PROJECT_NAME}_proto)
target_compile_options(${PROJECT_NAME} PRIVATE -Wall -Wextra)
target_compile_features(${PROJECT_NAME} PUBLIC cxx_std_20)

if (${BUILD_SERVICE_TESTS})
    add_subdirectory(test)
endif ()
