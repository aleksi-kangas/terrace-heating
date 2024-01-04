source ../.env
CONNECTION_STRING="host=${POSTGRES_HOST};database=${POSTGRES_DATABASE};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD}"
dotnet ef database update --startup-project ./src/HeatingGateway.API/HeatingGateway.API.csproj --connection $CONNECTION_STRING