# Testing

## Current State
The repository has frontend test tooling in the Angular project and backend test project scaffolds under `backend/tests`.

Known commands:

- Solution build: `dotnet build SmartMovieCatalog.slnx`
- Backend build: `dotnet build backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj`
- Frontend build: run `npm run build` from `frontend`
- Frontend tests: run `npm test -- --watch=false` from `frontend`
- Backend test project build: `dotnet build backend/tests/SmartMovieCatalog.Domain.Tests/SmartMovieCatalog.Domain.Tests.csproj`

PowerShell helpers:

- Build: `.\scripts\build.ps1`
- Test: `.\scripts\test.ps1`
- Run locally: `.\scripts\run-local.ps1`
- Clean generated output: `.\scripts\clean.ps1`

## Testing Policy
- New behavior should have tests when there is an applicable test structure.
- If no test structure exists for the changed area, prefer adding one when the behavior is important, shared, or regression-prone.
- Do not add broad or slow test infrastructure for trivial scaffold changes.

## Backend Testing Direction
Backend test projects live under `backend/tests`.

Recommended test types:

- Unit tests for pure business rules and validation.
- Integration tests for HTTP endpoints and dependency injection wiring.
- Contract-style tests for API request and response shape.

The current backend test projects are compile-only scaffolds without a test framework package. Do not introduce xUnit, TestContainers, EF Core, or database-backed tests until the backend behavior or user direction requires them.

## Frontend Testing Direction
- Keep component tests focused on behavior, rendering, and user-observable state.
- Avoid testing Angular internals.
- Mock API boundaries instead of coupling UI tests to backend runtime behavior.

## Verification Scope
- Run the narrowest command that validates the change.
- Run broader builds when project files, shared contracts, or cross-boundary behavior changes.
- If a command fails, inspect the error and changed code before attempting another fix.
