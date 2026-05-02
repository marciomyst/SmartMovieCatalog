# Backend Agent Instructions

- Backend source lives under `backend/src`.
- Use `SmartMovieCatalog.Api` as the canonical backend project name.
- Build with `dotnet build backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj` from the repository root.
- Keep the Clean Architecture dependency direction explicit:
  - Api references Application, Infrastructure, Contracts, and frontend for SPA integration.
  - Infrastructure references Application and Domain.
  - Application references Domain.
  - Domain references no internal projects.
  - Contracts references no internal projects.
- Do not introduce persistence, authentication, messaging, CQRS, or background workers without an accepted ADR or explicit user approval.
- Do not put secrets in `appsettings*.json`, Dockerfiles, source files, or documentation.
