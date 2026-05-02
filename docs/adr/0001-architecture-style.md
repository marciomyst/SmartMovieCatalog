# ADR 0001: Architecture Style

## Status
Superseded by the current Clean Architecture project split.

## Context
The repository originally contained a single ASP.NET Core API project and an Angular SPA. The backend has now been split into Clean Architecture projects while keeping infrastructure choices unselected.

## Decision
Use the current Clean Architecture structure:

- API: `backend/src/SmartMovieCatalog.Api`
- Application: `backend/src/SmartMovieCatalog.Application`
- Domain: `backend/src/SmartMovieCatalog.Domain`
- Infrastructure: `backend/src/SmartMovieCatalog.Infrastructure`
- Contracts: `backend/src/SmartMovieCatalog.Contracts`
- Frontend: `frontend`
- Documentation: `docs`

Infrastructure choices such as authentication, persistence, messaging, CQRS, and external services remain unselected until accepted by separate ADRs.

## Consequences
- The project has explicit architectural boundaries before product behavior grows.
- The layers are initially thin and should not receive speculative abstractions.
- Future infrastructure decisions must be documented separately.
