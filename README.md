# Overflow Distributed Service

A .NET Aspire-powered distributed sample that combines a minimal API service, a Blazor Server frontend, shared service defaults (health, telemetry, resilience), and an Aspire AppHost that wires everything together with Redis caching and a Keycloak identity container.

## Section Summaries
- **Section 2**: 
    * We setup and configured Aspire and used it to run and orchestrate and Identity Provider (Keycloak)
    * Ran the Aspire services and took an early look at dashboard. 
    * Logged in to Keycloak using Postman to retrieve a token. 

## Architecture
- **AppHost**: Orchestrates services with Aspire, provisioning Redis (`cache`) and Keycloak (`keycloak:6001`), wiring service discovery, health checks, and startup ordering. See [Overflow.AppHost/AppHost.cs](Overflow/AppHost/AppHost.cs).
- **API Service**: Minimal API with sample `/weatherforecast` and root liveness message; includes Aspire defaults and OpenAPI in development. See [Overflow.ApiService/Program.cs](Overflow/Overflow.ApiService/Program.cs).
- **Web Frontend**: Blazor Server app using Aspire service discovery; consumes the API via `WeatherApiClient` with Redis output caching. See [Overflow.Web/Program.cs](Overflow/Overflow.Web/Program.cs) and [Overflow.Web/WeatherApiClient.cs](Overflow/Overflow.Web/WeatherApiClient.cs).
- **Service Defaults**: Shared extensions for OpenTelemetry, health checks, service discovery, and resilient HTTP clients. See [Overflow.ServiceDefaults/Extensions.cs](Overflow/Overflow.ServiceDefaults/Extensions.cs).
- **Tests**: Aspire integration test that boots the distributed app and verifies the web frontend responds. See [Overflow.Tests/WebTests.cs](Overflow/Overflow.Tests/WebTests.cs).

## Prerequisites
- .NET SDK 10.0+
- Docker (for Redis and Keycloak containers started by Aspire)

## Getting Started
1) Restore packages:
   ```bash
   dotnet restore Overflow/Overflow.sln
   ```
2) Run the distributed app via the AppHost:
   ```bash
   dotnet run --project Overflow/Overflow.AppHost/Overflow.AppHost.csproj
   ```
   - Web frontend: http://localhost:5000 (or assigned port)
   - API service: http://localhost:5001 (or assigned port) → `/weatherforecast`
   - Keycloak: http://localhost:6001
3) Run tests:
   ```bash
   dotnet test Overflow/Overflow.Tests/Overflow.Tests.csproj
   ```

## Notable Features
- Aspire service discovery with `https+http://` scheme resolution for preferred HTTPS.
- OpenTelemetry metrics/tracing enabled by default; OTLP exporter activates when `OTEL_EXPORTER_OTLP_ENDPOINT` is set.
- Health endpoints mapped in development: `/health` (readiness) and `/alive` (liveness).
- Redis output caching in the web frontend and standard resilience handlers for outbound HTTP.

## Repo Layout
- `Overflow/Overflow.sln` — solution file
- `Overflow/Overflow.AppHost/` — Aspire orchestrator
- `Overflow/Overflow.ApiService/` — minimal API
- `Overflow/Overflow.Web/` — Blazor Server frontend
- `Overflow/Overflow.ServiceDefaults/` — shared infrastructure defaults
- `Overflow/Overflow.Tests/` — integration tests
