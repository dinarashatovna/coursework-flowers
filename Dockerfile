# ---------- Stage 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Сначала восстановим зависимости (для кеша)
COPY GardenWalk.csproj ./
RUN dotnet restore GardenWalk.csproj

# Затем остальной код и сборка
COPY . .
RUN dotnet publish GardenWalk.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---------- Stage 2: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Установка wget
RUN apt-get update \
    && apt-get install -y --no-install-recommends wget \
    && rm -rf /var/lib/apt/lists/*

# Папка для SQLite (монтируется как volume в docker-compose)
RUN mkdir -p /app/data
VOLUME ["/app/data"]

ENV ASPNETCORE_URLS=http://+:8007 \
    ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8007

# Простой healthcheck — приложение должно отвечать на корень
HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
    CMD wget -qO- http://localhost:8007/health >/dev/null 2>&1 || exit 1

ENTRYPOINT ["dotnet", "GardenWalk.dll"]
