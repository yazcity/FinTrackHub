# Use ASP.NET 8 runtime for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FinTrackHub.csproj", "./"]
RUN dotnet restore "./FinTrackHub.csproj"

COPY . .
RUN dotnet publish "FinTrackHub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinTrackHub.dll"]
