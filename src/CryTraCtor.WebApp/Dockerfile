FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CryTraCtor.WebApp/CryTraCtor.WebApp.csproj", "src/CryTraCtor.WebApp/"]
RUN dotnet restore "src/CryTraCtor.WebApp/CryTraCtor.WebApp.csproj"
COPY . .
WORKDIR "/src/src/CryTraCtor.WebApp"
RUN dotnet build "CryTraCtor.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CryTraCtor.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS watch
ENTRYPOINT ["dotnet", "watch", "--non-interactive"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryTraCtor.WebApp.dll"]
