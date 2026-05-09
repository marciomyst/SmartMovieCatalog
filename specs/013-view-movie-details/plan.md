# Implementation Plan

## Overview

Implement issue #13 by adding a backend movie-details read endpoint (`GET /api/movies/{id}`), a dedicated details response contract, a typed frontend details API service/model, and a `/movies/{id}` details page with loading/not-found/error states.

This plan keeps scope constrained to currently supported movie metadata and explicitly excludes new persisted fields such as `cast` and `updatedAt`.

## Technical Context

- Language/Version:
  - Backend: C# / ASP.NET Core 10
  - Frontend: Angular 21 / TypeScript 5.9
- Frameworks/Libraries:
  - Backend: Minimal APIs, Wolverine mediator dispatch, EF Core/PostgreSQL in Infrastructure
  - Frontend: Angular router/forms/http, RxJS, local CSS design system
- Architecture:
  - Clean Architecture backend (`Api -> Application -> Domain`, `Infrastructure -> Application/Domain`)
  - Typed API services on frontend; components do not call `HttpClient` directly
- Persistence:
  - Existing movie and genre persistence via EF Core mappings/repositories
- Deployment model:
  - ASP.NET Core SPA integration with Angular
- Testing:
  - Backend xUnit test projects (Api/Application/Domain/Infrastructure)
  - Frontend Vitest (`npm test -- --watch=false`)

## Constitution Check

- Architecture fidelity: pass. Plan follows existing Minimal API + Application handler + repository pattern and Angular typed-service pattern.
- Boundary adherence: pass. Backend and frontend changes remain in their owned directories.
- Contract synchronization: pass with required cross-boundary updates (Contracts + frontend models/services).
- Security hygiene: pass. No secrets, no new trust boundaries, and explicit not-found/error response shaping.
- Scope control: pass. Clarifications resolved with no domain expansion.

No unjustified constitution violations identified.

## Technical Approach

1. Backend contracts:
   - Add a dedicated movie details response DTO in `SmartMovieCatalog.Contracts`.
2. Application layer:
   - Add query + handler for "get movie by id" use case and map to details model.
   - Add failure model for missing movie and map to API `404 ProblemDetails`.
3. Persistence/repository:
   - Extend movie repository abstraction with lookup by ID including related genres.
   - Implement lookup in EF repository.
4. API layer:
   - Add `GET /api/movies/{id}` endpoint slice under `Features/Movies`.
   - Validate route ID shape and return:
     - `200` for success
     - `404` problem response for missing movie
5. Frontend data access:
   - Add details models and typed API service method (`getMovieById(id)`).
6. Frontend routing/page:
   - Register `/movies/:id` route.
   - Create details page component (loading/not-found/error/success states).
7. Navigation alignment:
   - Reuse existing links from catalog/list; verify route resolves.
8. Tests:
   - Backend endpoint + handler + repository tests.
   - Frontend API service + component/route behavior tests.

## Affected Areas

- Backend:
  - `backend/src/SmartMovieCatalog.Contracts/Movies/`
  - `backend/src/SmartMovieCatalog.Application/Features/Movies/`
  - `backend/src/SmartMovieCatalog.Application/Abstractions/Persistence/`
  - `backend/src/SmartMovieCatalog.Infrastructure/Persistence/`
  - `backend/src/SmartMovieCatalog.Api/Features/Movies/`
- Frontend:
  - `frontend/src/main.ts`
  - `frontend/src/app/movies/`
  - `frontend/src/app/` (new details feature folder)
- Tests:
  - `backend/tests/...`
  - `frontend/src/app/...*.spec.ts`
- Documentation:
  - `docs/API.md`
  - `docs/FRONTEND.md` (if routing/API usage descriptions change)

## Data Model Changes

- No mandatory domain entity expansion is planned by default.
- The details response will project existing movie metadata:
  - `id`, `title`, `originalTitle`, `releaseYear`, `countryCode`, `originalLanguage`,
    `genres`, `director`, `synopsis`, `durationMinutes`, `ageRating`, `posterUrl`/`image`, `createdAt`.
- `cast` and `updatedAt` are explicitly out of scope and will not be added in this feature.

## API Changes

- Add:
  - `GET /api/movies/{id}`
- Responses:
  - `200 application/json` with details DTO
  - `404 application/problem+json` for missing movie
- Keep existing list/create endpoints unchanged.

## Frontend Changes

- Add details route: `/movies/:id`.
- Add details page UI aligned with `frontend/DESIGN.md`.
- Add typed details API method in existing movie API service boundary.
- Render:
  - loading state while fetching
  - not-found state for 404
  - generic error state for other failures
  - details metadata on success

## Backend Changes

- Add details query + handler in Application.
- Add repository lookup abstraction + EF implementation.
- Add Minimal API endpoint in Api layer.
- Add ProblemDetails mapping for missing movie.

## Testing Strategy

- Backend:
  - API tests for `GET /api/movies/{id}` success + 404 + response shape.
  - Application tests for handler success/failure mapping.
  - Infrastructure tests for repository lookup behavior.
- Frontend:
  - API service tests for details GET call and response mapping.
  - Details page tests for loading/not-found/error/success states.
  - Route test for `/movies/:id`.
- Verification commands:
  - `dotnet build SmartMovieCatalog.slnx`
  - `dotnet test SmartMovieCatalog.slnx --no-build`
  - `npm test -- --watch=false` (from `frontend`)
  - `npm run build` (from `frontend`)

## Deployment Impact

- No infrastructure/deployment topology changes expected.
- No new external services or secrets.
- API surface expands with one read endpoint.

## Security Considerations

- Validate and parse route ID safely.
- Return generic, non-sensitive problem details for missing resources.
- Do not leak persistence internals or stack traces.
- Treat route/query/API data as untrusted on frontend rendering paths.

## Observability / Logging

- No new logging stack required.
- Reuse existing ASP.NET request/response diagnostics.
- Optional: add lightweight endpoint-level log context if current conventions allow.

## Risks

- Scope-growth risk is mitigated by explicit exclusion of `cast` and `updatedAt`.
- Contract drift risk between backend DTO and frontend model.
- UX consistency risk if details route exists but list/card surfaces diverge in navigation behavior.

## Open Questions

- No open functional questions remain for issue #13.

## Rollout Plan

1. Implement backend details endpoint with tests.
2. Implement frontend details route/page with tests.
3. Synchronize docs (`docs/API.md`, `docs/FRONTEND.md`).
4. Run backend and frontend verification commands.
5. Submit for review as a single scoped feature branch.
