FROM ubuntu:latest as dependencies

RUN apt-get update && apt-get install -y \
    gcc \
    g++ \
    cmake \
    git \
    pkg-config \
    ninja-build \
    curl \
    zip \
    unzip \
    tar \
    python3 \
    protobuf-compiler \
    gdb

RUN export VCPKG_FORCE_SYSTEM_BINARIES=1 && \
    git clone https://github.com/Microsoft/vcpkg.git && \
    cd vcpkg && \
    ./bootstrap-vcpkg.sh

RUN export VCPKG_FORCE_SYSTEM_BINARIES=1 && \
    vcpkg/vcpkg install grpc && \
    vcpkg/vcpkg install libmodbus

FROM dependencies as build
ARG BUILD_TYPE=Release

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

COPY . .

RUN mkdir -p build && \
    cd build && \
    cmake -DCMAKE_TOOLCHAIN_FILE=/vcpkg/scripts/buildsystems/vcpkg.cmake .. && \
    cmake --build . --config ${BUILD_TYPE}

FROM ubuntu:latest as runtime

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

COPY --from=build /usr/src/app/build/heat_pump_service .

CMD ["./heat_pump_service"]

FROM dependencies as tests
ARG BUILD_TYPE=Release

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

COPY . .

RUN mkdir -p build && \
    cd build && \
    cmake -DCMAKE_TOOLCHAIN_FILE=/vcpkg/scripts/buildsystems/vcpkg.cmake .. && \
    cmake --build . --config ${BUILD_TYPE}

WORKDIR /usr/src/app/build
CMD ["ctest", "-C", "${BUILD_TYPE}", "--progress", "--output-on-failure"]
