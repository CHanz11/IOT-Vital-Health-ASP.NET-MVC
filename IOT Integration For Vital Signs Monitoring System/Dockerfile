# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Update the COPY and restore paths with the new project file name and folder
COPY ["IOT Integration For Vital Signs Monitoring System/IOT-Integration-VitalSigns.csproj", "IOT Integration For Vital Signs Monitoring System/"]
RUN dotnet restore "IOT Integration For Vital Signs Monitoring System/IOT-Integration-VitalSigns.csproj"

COPY . .
WORKDIR "/src/IOT Integration For Vital Signs Monitoring System"
RUN dotnet build "IOT-Integration-VitalSigns.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IOT-Integration-VitalSigns.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IOT-Integration-VitalSigns.dll"]
