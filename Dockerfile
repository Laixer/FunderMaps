FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# Copy and restore app
COPY . .
RUN dotnet restore

# Publish app and libraries
RUN dotnet publish -c release -o /app --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV DOTNET_PRINT_TELEMETRY_MESSAGE=false
WORKDIR /app
COPY --from=build /app .
EXPOSE 80/tcp
ENTRYPOINT ["/app/FunderMaps"]
