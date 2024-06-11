# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the official ASP.NET Core build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SnipSmart/SnipSmart.csproj", "SnipSmart/"]
RUN dotnet restore "SnipSmart/SnipSmart.csproj"
COPY . .
WORKDIR "/src/SnipSmart"
RUN dotnet build "SnipSmart.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SnipSmart.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY SnipSmart/Database/Database.db /app/Database/Database.db

# Change ownership and permissions of the database file
RUN chmod 666 /app/Database/Database.db

# Optionally, if you are running with a specific non-root user, ensure permissions are set correctly:
# USER your_username

ENTRYPOINT ["dotnet", "SnipSmart.dll"]
