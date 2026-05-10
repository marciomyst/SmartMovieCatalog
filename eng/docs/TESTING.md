# Testing

## Current State
The repository has frontend test tooling in the Angular project and backend test project scaffolds under `backend/tests`.

Known commands:

- NUKE compile: `dotnet run --project eng/build/_build.csproj -- Compile`
- NUKE tests (backend + frontend): `dotnet run --project eng/build/_build.csproj -- Test`
- NUKE frontend build: `dotnet run --project eng/build/_build.csproj -- BuildFrontend`
- NUKE coverage: `dotnet run --project eng/build/_build.csproj -- Coverage`
- NUKE mutation: `dotnet run --project eng/build/_build.csproj -- Mutation --MutationScope both`
- NUKE local run: `dotnet run --project eng/build/_build.csproj -- RunLocal`
- Direct solution build: `dotnet build SmartMovieCatalog.slnx`
- Architecture tests only: `dotnet test backend/tests/SmartMovieCatalog.Architecture.Tests/SmartMovieCatalog.Architecture.Tests.csproj`

PowerShell entrypoint:

- Root bootstrapper: `.\eng\build.ps1 <Target>`

Legacy wrappers (compatibility):

- Build: `.\eng\scripts\build.ps1`
- Test: `.\eng\scripts\test.ps1`
- Mutation: `.\eng\scripts\test-mutation.ps1`
- Run locally: `.\eng\scripts\run-local.ps1`
- Clean generated output: `.\eng\scripts\clean.ps1`

## Testing Policy
- New behavior should have tests when there is an applicable test structure.
- If no test structure exists for the changed area, prefer adding one when the behavior is important, shared, or regression-prone.
- Do not add broad or slow test infrastructure for trivial scaffold changes.

## Backend Testing Direction
Backend test projects live under `backend/tests`.

Backend coverage uses `backend/tests/coverage.runsettings` so coverage reports focus on product code and exclude generated files, EF migrations, design-time factories, and marker reference classes. Do not compare raw coverage runs that include `obj/` output or migrations against the configured coverage number.

Recommended test types:

- Unit tests for pure business rules and validation.
- Integration tests for HTTP endpoints and dependency injection wiring.
- Contract-style tests for API request and response shape.
- Architecture tests for Clean Architecture dependency direction and layer boundaries.

Backend authentication tests use xUnit and `Microsoft.AspNetCore.Mvc.Testing`.

Current auth coverage:

- Application tests cover successful authentication, generic authentication failures, current-user lookup, and stale-user rejection.
- API tests cover `POST /api/auth/authenticate` success, validation failure, generic unauthorized failures, `GET /api/auth/me` success, missing/malformed/expired/incorrectly signed tokens, and stale persisted users.
- API tests use in-memory fakes for user persistence and password verification. They do not require TestContainers or a real database.

Do not introduce TestContainers or database-backed tests until a separate testing decision accepts that cost.

## Frontend Testing Direction
- Keep component tests focused on behavior, rendering, and user-observable state.
- Avoid testing Angular internals.
- Mock API boundaries instead of coupling UI tests to backend runtime behavior.
- Auth frontend tests cover login form rendering, validation messages, backend validation-error mapping, password visibility toggling, API authentication calls, current-user lookup, bearer header behavior, in-memory session storage, and generic unauthorized errors.

## Verification Scope
- Run the narrowest command that validates the change.
- Run broader builds when project files, shared contracts, or cross-boundary behavior changes.
- If a command fails, inspect the error and changed code before attempting another fix.

## Mutation Testing (Stryker)
- Stryker is configured for:
  - `backend/tests/SmartMovieCatalog.Domain.Tests/stryker-config.json`
  - `backend/tests/SmartMovieCatalog.Application.Tests/stryker-config.json`
- Ensure local tools are restored before running mutation tests: `dotnet tool restore`.
- Mutation testing is intentionally slower than regular tests. Use it for quality gates and focused hardening, not for every rapid edit loop.
