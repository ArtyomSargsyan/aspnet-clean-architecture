# ══════════════════════════════════════════════════════════════════════════════
#  Multi-stage build — .NET 9  (Alpine runtime ≈ 180 MB final image)
#
#  Stages:
#    restore     — NuGet + local tools cache layer (invalidated only on .csproj change)
#    development — hot-reload via dotnet watch (dev compose target)
#    build       — Release publish
#    runtime     — shared aspnet Alpine base (migrator + production both extend this)
#    migrator    — runs db.MigrateAsync() + DbSeeder.SeedAsync() then exits 0
#    production  — serves HTTP; /health is live from first request
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

# ── Stage 3: build / publish ─────────────────────────────────────────────────
FROM restore AS build
ARG BUILD_VERSION=0.0.0
COPY . .
RUN dotnet publish ToDoApi.csproj \
        -c Release \
        -o /app/publish \
        --no-restore \
        /p:Version=${BUILD_VERSION} \
        /p:UseAppHost=false

# ── Stage 4: shared runtime base ─────────────────────────────────────────────
#  Both migrator and production copy from the same build artifact.
#  Keeping it in a shared stage avoids duplicating the COPY instruction.
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app
COPY --from=build --chown=app:app /app/publish .
USER app

# ── Stage 5: migrator ─────────────────────────────────────────────────────────
#  Runs db.MigrateAsync() then DbSeeder.SeedAsync() via the --migrate flag,
#  then exits 0. No HTTP server starts — no /health race condition possible.
FROM runtime AS migrator
ENTRYPOINT ["dotnet", "ToDoApi.dll", "--migrate"]

# ── Stage 6: production ───────────────────────────────────────────────────────
FROM runtime AS production

LABEL org.opencontainers.image.title="ToDoApi" \
      org.opencontainers.image.description=".NET 9 ToDo REST API" \
      org.opencontainers.image.licenses="MIT"

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=15s --retries=3 \
    CMD wget -qO- http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "ToDoApi.dll"]
