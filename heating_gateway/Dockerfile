FROM mcr.microsoft.com/dotnet/sdk:7.0.103 AS build
WORKDIR /app

# Copy all .csproj files while preserving directory structure
COPY *.sln .
COPY src/*/*.csproj ./
RUN mkdir src
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/$file; done
COPY tests/*/*.csproj ./
RUN mkdir tests
RUN for file in $(ls *.csproj); do mkdir -p tests/${file%.*}/ && mv $file tests/${file%.*}/$file; done
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release --property PublishDir=/app/out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "HeatingGateway.API.dll"]

FROM build as tests
WORKDIR /app
COPY . ./
ENTRYPOINT ["dotnet", "test"]
