FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QuestsBot.Telegram/QuestsBot.Telegram.csproj", "QuestsBot.Telegram/"]
RUN dotnet restore "QuestsBot.Telegram/QuestsBot.Telegram.csproj"
COPY . .
WORKDIR "/src/QuestsBot.Telegram"
RUN dotnet build "QuestsBot.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "QuestsBot.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuestsBot.Telegram.dll"]
