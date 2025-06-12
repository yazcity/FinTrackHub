<<<<<<< HEAD
﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FinTrackHub/FinTrackHub.csproj", "FinTrackHub/"]
RUN dotnet restore "FinTrackHub/FinTrackHub.csproj"

COPY . .
WORKDIR /src/FinTrackHub
RUN dotnet publish "FinTrackHub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinTrackHub.dll"]
=======
# Base runtime image for .NET 8 (Linux)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK image for building your app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project file and restore dependencies
COPY ["YazzApi.csproj", "."]
RUN dotnet restore "./YazzApi.csproj"

# Copy the rest of the app source code
COPY . .
WORKDIR "/src/."

# Build the app
RUN dotnet build "./YazzApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app to a folder
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YazzApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy the published app from previous stage
COPY --from=publish /app/publish .

# Run the app
ENTRYPOINT ["dotnet", "YazzApi.dll"]
>>>>>>> 948ea83 (final code)
