# AI Flix / SmartMovieCatalog

AI Flix is an early-stage movie catalog application with an ASP.NET Core backend and an Angular frontend.

## Stack
- Backend: ASP.NET Core 10 / C# in `backend/src/SmartMovieCatalog.Api`.
- Frontend: Angular 21 / TypeScript in `frontend`.
- SPA integration: ASP.NET Core SpaProxy.
- Solution: `SmartMovieCatalog.slnx`.

Do not assume persistence, CQRS, DDD layers, messaging, authentication, or AI-provider integration until those choices are implemented or documented through an architecture decision.

## Repository Layout
```text
.
├── AGENTS.md
├── CONTEXT.md
├── DESIGN.md
├── README.md
├── SmartMovieCatalog.slnx
├── docker-compose.yml
├── docker-compose.override.yml
├── docker/
│   └── README.md
├── backend/
│   ├── src/
│   │   ├── SmartMovieCatalog.Api/
│   │   ├── SmartMovieCatalog.Application/
│   │   ├── SmartMovieCatalog.Contracts/
│   │   ├── SmartMovieCatalog.Domain/
│   │   └── SmartMovieCatalog.Infrastructure/
│   └── tests/
│       ├── SmartMovieCatalog.Api.Tests/
│       ├── SmartMovieCatalog.Application.Tests/
│       ├── SmartMovieCatalog.Domain.Tests/
│       └── SmartMovieCatalog.Infrastructure.Tests/
├── frontend/
├── docs/
│   ├── ARCHITECTURE.md
│   ├── API.md
│   ├── CONVENTIONS.md
│   ├── DOMAIN.md
│   ├── FRONTEND.md
│   ├── ROADMAP.md
│   ├── SECURITY.md
│   ├── TESTING.md
│   └── adr/
├── scripts/
│   ├── build.ps1
│   ├── clean.ps1
│   ├── run-local.ps1
│   └── test.ps1
```

## Prerequisites
- .NET SDK compatible with `net10.0`.
- Node.js and npm compatible with the Angular project.
- Installed frontend dependencies under `frontend`.
- Docker Desktop or another Docker Engine compatible with Docker Compose.

## Essential Commands
From the repository root:

```bash
dotnet build SmartMovieCatalog.slnx
dotnet build backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj
```

PowerShell helpers:

```powershell
.\scripts\build.ps1
.\scripts\test.ps1
.\scripts\run-local.ps1
.\scripts\clean.ps1
```

Docker:

```bash
docker compose up --build
```

This starts:

- ASP.NET Core API on `http://localhost:5048`.
- PostgreSQL on `localhost:5432`.

The API container receives `ConnectionStrings__DefaultConnection` and JWT settings through environment variables. Set `JWT_SIGNING_KEY` in `.env` or your shell before starting the API; do not commit real signing keys or database credentials.

The backend auth endpoints are:

- `POST /api/auth/authenticate`
- `GET /api/auth/me`

Health check:

```bash
curl http://localhost:5048/health
```

From `frontend`:

```bash
npm run build
npm test
npm start
```

Local environment:

```bash
cp .env.example .env
```

The example values are for local development only. Do not commit real secrets in `.env`.

Useful PostgreSQL checks:

```bash
docker compose exec postgres pg_isready -U smartmovie -d smart_movie_catalog
docker compose exec postgres psql -U smartmovie -d smart_movie_catalog -c "select version();"
```

Apply EF Core migrations after configuring the local connection string:

```bash
dotnet ef database update --project backend/src/SmartMovieCatalog.Infrastructure --startup-project backend/src/SmartMovieCatalog.Api
```

Development/Test seed users are supplied through non-versioned configuration under `DevelopmentSeedUser:*`. Production user provisioning is operational/manual outside application startup.

## Development Notes
- Backend entry point: `backend/src/SmartMovieCatalog.Api/Program.cs`.
- Backend API scaffold: `backend/src/SmartMovieCatalog.Api/Controllers`.
- Frontend application: `frontend/src/app`.
- Frontend visual system: `frontend/DESIGN.md`.
- Root design policy: `DESIGN.md`.

## Documentation
- Architecture: `docs/ARCHITECTURE.md`
- Domain direction: `docs/DOMAIN.md`
- API conventions: `docs/API.md`
- Frontend conventions: `docs/FRONTEND.md`
- Testing policy: `docs/TESTING.md`
- Security rules: `docs/SECURITY.md`
- Engineering conventions: `docs/CONVENTIONS.md`
- Roadmap: `docs/ROADMAP.md`
- Architecture decisions: `docs/adr`

## Safety
- Do not commit secrets, credentials, certificates, connection strings, or private keys.
- Do not edit generated or vendor output such as `node_modules`, `bin`, `obj`, `dist`, `.vs`, or `.angular/cache`.
- Keep backend and frontend contracts synchronized when changing API behavior.
