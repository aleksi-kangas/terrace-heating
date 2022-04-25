# Terrace Heating

> A full stack application for monitoring and controlling a terrace heating system.

This repository contains a full stack application for monitoring and controlling the heating
system for an enclosed terrace at my parents' home.

#### Motivation

At my parents' home, there is an enclosed terrace for which the heating (ground & air) is provided by a *Lämpöässä Vmi
9* ground source heat-pump. There didn't exist a clear and easy way of controlling the heating system, since it's tied
to one of the heat distribution circuits. The purpose of this project is to provide a simple UI for monitoring and
controlling the heating of the terrace.

[Backend Repository](https://github.com/aleksi-kangas/terrace-heating-backend)

[Frontend Repository](https://github.com/aleksi-kangas/terrace-heating-frontend)

## Features

- Periodical querying of heat-pump values with [Modbus](https://en.wikipedia.org/wiki/Modbus) TCP connection
  using [libmodbus](https://libmodbus.org/)-library.
- [Protocol Buffers](https://developers.google.com/protocol-buffers) and [gRPC](https://grpc.io/) enable fast and
  efficient communication between the backend and frontend.
- Frontend offers a simple Next.js based UI for monitoring and controlling the heating system.

## Overall Architecture

The overall architecture is illustrated by the following image:

![Architecture](/architecture.png)

In essence, the frontend communicates with the backend through the Envoy-proxy which handles transcoding of gRPC-Web
calls to pure gRPC and vice versa.

### Building

The project utilizes Docker and Docker Compose for building and dependency management.
Notes for me (and anyone curious) for getting the project up and running can be found from [BUILDING.md](BUILDING.md)
