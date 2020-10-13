# FunderMaps Ecosystem

# Build the application solution
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# Copy and restore app
COPY . .
RUN dotnet restore

# Publish app and libraries
RUN dotnet publish -c release -o /app --no-restore

# Build runtime image
#
# Any FunderMaps application in the repository can
# be called via the CMD=<application> environment
# variable.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV DOTNET_PRINT_TELEMETRY_MESSAGE=false
WORKDIR /app
COPY --from=build /app .
EXPOSE 80/tcp
ENTRYPOINT "/app/$CMD"
