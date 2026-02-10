FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/Harfistan.API/Harfistan.API.csproj", "Presentation/Harfistan.API/"]
COPY ["Core/Harfistan.Domain/Harfistan.Domain.csproj", "Core/Harfistan.Domain/"]
COPY ["Infrastructure/Harfistan.Persistence/Harfistan.Persistence.csproj", "Infrastructure/Harfistan.Persistence/"]
COPY ["Core/Harfistan.Application/Harfistan.Application.csproj", "Core/Harfistan.Application/"]
RUN dotnet restore "Presentation/Harfistan.API/Harfistan.API.csproj"
COPY . .
WORKDIR "/src/Presentation/Harfistan.API"
RUN dotnet build "./Harfistan.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Harfistan.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Harfistan.API.dll"]