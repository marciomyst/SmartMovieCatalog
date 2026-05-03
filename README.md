# Smart Movie Catalog / SmartMovieCatalog

Smart Movie Catalog is an early-stage movie catalog application with an ASP.NET Core backend and an Angular frontend.

## Stack
- Backend: ASP.NET Core 10 / C# in `backend/src/SmartMovieCatalog.Api`.
- Frontend: Angular 21 / TypeScript in `frontend`.
- SPA integration: ASP.NET Core SpaProxy.
- Solution: `SmartMovieCatalog.slnx`.

Persistence, local JWT authentication, Clean Architecture layers, and Wolverine-based in-process CQRS are implemented and documented. Do not assume external messaging, refresh tokens, registration, password recovery, or AI-provider integration until those choices are implemented or documented through an architecture decision.

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

The Angular login screen is available at the SPA root. It calls `POST /api/auth/authenticate`, then `GET /api/auth/me`, through same-origin `/api` routes. In local frontend development, `frontend/src/proxy.conf.js` proxies `/api` to the backend.

Health check:

```bash
curl http://localhost:5048/health
```

From `frontend`:

```bash
npm run build
npm test -- --watch=false
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

On API startup, non-test environments apply EF Core migrations and can seed an optional admin user by supplying non-versioned configuration:

- `ADMIN_SEED_EMAIL`
- `ADMIN_SEED_PASSWORD`
- `ADMIN_SEED_NAME`

These `.env` keys are mapped by the local scripts and Docker Compose to `AdminSeedUser:*` configuration. Leave `ADMIN_SEED_EMAIL` and `ADMIN_SEED_PASSWORD` empty to disable the seed. Never commit real admin credentials.

Example local admin seed:

```env
ADMIN_SEED_EMAIL=admin@example.com
ADMIN_SEED_PASSWORD=Password123!
ADMIN_SEED_NAME=Admin
```

The seed runs only during API startup. After changing `.env`, restart the API process or recreate the Docker Compose API container:

```powershell
.\scripts\run-local.ps1
docker compose up -d --force-recreate api
```

## Development Notes
- Backend entry point: `backend/src/SmartMovieCatalog.Api/Program.cs`.
- Backend HTTP endpoints: Minimal API feature slices under `backend/src/SmartMovieCatalog.Api/Features`.
- Auth endpoint mapping: `backend/src/SmartMovieCatalog.Api/Features/Auth`.
- Frontend application: `frontend/src/app`.
- Frontend auth module: `frontend/src/app/auth`.
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
