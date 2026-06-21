# ══════════════════════════════════════════════════════════════════════════════
#  Multi-stage build — .NET 9  (Alpine runtime ≈ 180 MB final image)
#
#  Stages:
#    restore     — NuGet + local tools cache layer (invalidated only on .csproj change)
#    development — hot-reload via dotnet watch (dev compose target)
#    migrator    — runs EF Core migrations then exits (both composes)
#    build       — Release publish
#    production  — minimal aspnet runtime, non-root user, healthcheck
# ══════════════════════════════════════════════════════════════════════════════

# ── Stage 1: restore ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS restore
WORKDIR /src

# Copy only dependency manifests first — layer is cached until they change
COPY ToDoApi.csproj .
COPY .config/ .config/
RUN dotnet restore \
 && dotnet tool restore

# ── Stage 2: development (hot-reload) ─────────────────────────────────────────
FROM restore AS development
COPY . .
EXPOSE 8080
ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://+:8080"]

# ── Stage 3: migrator ─────────────────────────────────────────────────────────
FROM restore AS migrator
COPY . .
ENTRYPOINT ["dotnet", "ef", "database", "update"]

# ── Stage 4: build / publish ─────────────────────────────────────────────────
FROM restore AS build
ARG BUILD_VERSION=0.0.0
COPY . .
RUN dotnet publish ToDoApi.csproj \
        -c Release \
        -o /app/publish \
        --no-restore \
        /p:Version=${BUILD_VERSION} \
        /p:UseAppHost=false

# ── Stage 5: production runtime ───────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS production

LABEL org.opencontainers.image.title="ToDoApi" \
      org.opencontainers.image.description=".NET 9 ToDo REST API" \
      org.opencontainers.image.licenses="MIT"

WORKDIR /app
COPY --from=build --chown=app:app /app/publish .

USER app
EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=15s --retries=3 \
    CMD wget -qO- http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "ToDoApi.dll"]
