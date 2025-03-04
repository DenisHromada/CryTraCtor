FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CryTraCtor.API/CryTraCtor.API.csproj", "src/CryTraCtor.API/"]
COPY ["src/CryTraCtor.Common/CryTraCtor.Common.csproj", "src/CryTraCtor.Common/"]
COPY ["src/CryTraCtor.Business/CryTraCtor.Business.csproj", "src/CryTraCtor.Business/"]
COPY ["src/CryTraCtor.Database/CryTraCtor.Database.csproj", "src/CryTraCtor.Database/"]
COPY ["src/CryTraCtor.Packet/CryTraCtor.Packet.csproj", "src/CryTraCtor.Packet/"]
RUN dotnet restore "src/CryTraCtor.API/CryTraCtor.API.csproj"
COPY . .
WORKDIR "/src/src/CryTraCtor.API"
RUN dotnet build "CryTraCtor.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CryTraCtor.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryTraCtor.API.dll"]
