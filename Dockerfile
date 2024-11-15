# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything from the root context (where you run `docker build`), including SolarWatch and SolarWatchTests
COPY . .

# Restore dependencies using the solution file
RUN dotnet restore SolarWatch/SolarWatch.sln

# Build and publish the application
RUN dotnet publish SolarWatch/SolarWatch.csproj -c Release -o /app/out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "SolarWatch.dll"]


