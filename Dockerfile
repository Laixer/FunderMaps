# FunderMaps Ecosystem

# Build the application solution
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /source

ARG subtool

RUN test -n "$subtool" || (echo "subtool argument not set" && false)

# Copy and restore app
COPY . .

# Substitute magic variables with version
RUN find . -type f -exec sed -i "s/@@VERSION@@/$(git describe --long --always)/" {} +
RUN find . -type f -exec sed -i "s/@@COMMIT@@/$(git rev-parse HEAD)/" {} +

# Publish app and libraries
RUN echo "Building $subtool"
WORKDIR "/source/src/$subtool"
RUN dotnet publish -c release -o /app \
    && git describe --long --always > /app/VERSION \
    && git rev-parse HEAD > /app/COMMIT \
    && echo "$subtool" > /app/SUBTOOL

WORKDIR /app
RUN ln -s "$subtool" "entry"

# Runtime image
#
# Any FunderMaps application in the repository can
# be called via the CMD=<application> environment
# variable.
FROM mcr.microsoft.com/dotnet/aspnet:latest
ENV DOTNET_PRINT_TELEMETRY_MESSAGE=false
WORKDIR /app
COPY --from=build /app .
EXPOSE 80/tcp
ENTRYPOINT "/app/$CMD"
