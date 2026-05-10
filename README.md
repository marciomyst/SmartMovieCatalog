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
├── eng/
│   ├── build/
│   ├── docs/
│   ├── scripts/
│   ├── docker/
│   └── .env.example
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
```

## Prerequisites
- .NET SDK compatible with `net10.0`.
- Node.js and npm compatible with the Angular project.
- Installed frontend dependencies under `frontend`.
- Docker Desktop or another Docker Engine compatible with Docker Compose.

## Essential Commands
From the repository root:

```bash
dotnet run --project eng/build/_build.csproj -- Compile
dotnet run --project eng/build/_build.csproj -- Test
dotnet run --project eng/build/_build.csproj -- BuildFrontend
```

NUKE targets:

```powershell
.\eng\build.ps1 Compile
.\eng\build.ps1 Test
.\eng\build.ps1 Coverage
.\eng\build.ps1 Mutation --MutationScope both
.\eng\build.ps1 Clean
.\eng\build.ps1 RunLocal
```

Legacy wrappers are available in `eng/scripts/*.ps1`.

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

The Angular home page is available at the SPA root (`/`). The Angular login screen is available at `/login`. It calls `POST /api/auth/authenticate`, then `GET /api/auth/me`, through same-origin `/api` routes. In local frontend development, `frontend/src/proxy.conf.js` proxies `/api` to the backend.

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
cp eng/.env.example .env
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
.\eng\scripts\run-local.ps1
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
- Architecture: `eng/docs/ARCHITECTURE.md`
- Domain direction: `eng/docs/DOMAIN.md`
- API conventions: `eng/docs/API.md`
- Frontend conventions: `eng/docs/FRONTEND.md`
- Testing policy: `eng/docs/TESTING.md`
- Security rules: `eng/docs/SECURITY.md`
- Engineering conventions: `eng/docs/CONVENTIONS.md`
- Roadmap: `eng/docs/ROADMAP.md`
- Architecture decisions: `eng/docs/adr`

## Safety
- Do not commit secrets, credentials, certificates, connection strings, or private keys.
- Do not edit generated or vendor output such as `node_modules`, `bin`, `obj`, `dist`, `.vs`, or `.angular/cache`.
- Keep backend and frontend contracts synchronized when changing API behavior.
