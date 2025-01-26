# Dotnet 8 project build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080

COPY back-end-api/*.csproj .

# Restore project
RUN dotnet restore

# Build project
COPY back-end-api/. .
RUN dotnet build BackEndApi.csproj -c Release

# Use the official ASP.NET Core runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

COPY --from=build /app/bin/Release/net8.0/ .
COPY back-end-api/.env .

ENV ASPNETCORE_ENVIRONMENT=Development

# Command to run the application when the container starts
ENTRYPOINT ["dotnet", "BackEndApi.dll"]

#CMD ["sleep", "infinity"]