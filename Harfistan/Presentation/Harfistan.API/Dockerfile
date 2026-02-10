FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER root
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Harfistan/Presentation/Harfistan.API/Harfistan.API.csproj", "Harfistan/Presentation/Harfistan.API/"]
COPY ["Harfistan/Core/Harfistan.Domain/Harfistan.Domain.csproj", "Harfistan/Core/Harfistan.Domain/"]
COPY ["Harfistan/Infrastructure/Harfistan.Persistence/Harfistan.Persistence.csproj", "Harfistan/Infrastructure/Harfistan.Persistence/"]
COPY ["Harfistan/Core/Harfistan.Application/Harfistan.Application.csproj", "Harfistan/Core/Harfistan.Application/"]

RUN dotnet restore "Harfistan/Presentation/Harfistan.API/Harfistan.API.csproj"
COPY . .
WORKDIR "/src/Harfistan/Presentation/Harfistan.API"
RUN dotnet build "./Harfistan.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Harfistan.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Harfistan.API.dll"]
