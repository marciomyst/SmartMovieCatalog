# Implementation Plan

## Overview

Implement basic title search on the existing movie listing path so the backend accepts a simple `query` parameter, the repository filters by title and original title, and the Angular catalog UI submits the query through the typed movies API while preserving paging and user-visible states.

The repository already contains the required endpoint, repository query path, typed API service, and catalog search UI. This plan records the current delivery approach and keeps issue #16 aligned with the implemented architecture.

## Technical Context

**Language/Version**: ASP.NET Core 10 / C# for the backend; Angular 21 / TypeScript 5.9 for the frontend.
**Primary Dependencies**: Minimal API feature slices, Wolverine message bus dispatch, EF Core/PostgreSQL persistence, Angular Router, Reactive Forms, RxJS, Angular `HttpClient`, local component CSS.
**Storage**: PostgreSQL through EF Core for movie catalog data.
**Testing**: Backend build and test coverage through the repository's .NET build/test flow; frontend behavior tests through Angular/Vitest and `npm test`; frontend build through `npm run build`.
**Target Platform**: Browser SPA served through Angular dev server and ASP.NET Core SpaProxy/static hosting.
**Project Type**: Full-stack catalog feature spanning the API and Angular frontend.
**Constraints**: Keep search basic and title-based, preserve the existing paginated response shape, do not introduce a separate search endpoint, do not add new dependencies or UI frameworks, and do not expose AI/search terminology in the product UI.
**Scale/Scope**: One list endpoint, one repository query path, one typed API service, and one catalog search form with state handling.

## Constitution Check

*GATE: Must pass before planning is considered complete.*

- Actual architecture is the source of truth: pass. The repo already uses ASP.NET Core 10, EF Core/PostgreSQL, Wolverine, and Angular 21, and the feature fits those existing choices.
- Boundaries explicit: pass. Backend behavior stays under `backend/src`; frontend behavior stays under `frontend`.
- Contracts drive cross-boundary work: pass. The search query, pagination, and paginated response shape are defined on the API boundary and consumed by the typed frontend service.
- Security and secret hygiene: pass. No secrets, token storage, or internal provider details are introduced.
- Small verified changes: pass. The feature is scoped to the current list/search pipeline and catalog UI.
- Frontend design compliance: pass. The catalog search UI follows the repository's dark cinematic frontend guidance.

No constitution violations require justification.

## Technical Approach

1. Keep the movie listing endpoint as the single public search surface.
2. Normalize the incoming `query` by trimming whitespace and treating blank values as null.
3. Apply case-insensitive filtering in the movie repository against both `Title` and `OriginalTitle`.
4. Preserve the existing paginated response contract so filtered and unfiltered requests return the same shape.
5. Keep the frontend `MoviesApi` responsible for serializing `query`, `page`, and `pageSize`.
6. Keep the catalog page search form as the user entry point for search queries.
7. Reset paging to the first page when a new search term is submitted.
8. Preserve loading, empty, no-result, and API error states in the catalog page.
9. Keep UI copy and labels free of AI and semantic-search terminology.
10. Verify the feature with the narrowest build/test commands that cover the touched backend and frontend surfaces.

## Affected Areas

- `backend/src/SmartMovieCatalog.Api/Features/Movies/ListMovies/ListMoviesEndpoint.cs`
- `backend/src/SmartMovieCatalog.Api/Features/Movies/ListMovies/ListMoviesRequest.cs`
- `backend/src/SmartMovieCatalog.Api/Features/Movies/ListMovies/ListMoviesRequestValidator.cs`
- `backend/src/SmartMovieCatalog.Application/Features/Movies/ListMoviesQuery.cs`
- `backend/src/SmartMovieCatalog.Application/Features/Movies/ListMoviesHandler.cs`
- `backend/src/SmartMovieCatalog.Infrastructure/Persistence/EfMovieRepository.cs`
- `backend/src/SmartMovieCatalog.Contracts/Movies/PagedMovieSummaryResponse.cs`
- `frontend/src/app/movies/movie.models.ts`
- `frontend/src/app/movies/movies-api.ts`
- `frontend/src/app/catalog/catalog-page/*`
- `frontend/src/app/home/home-page/*` only if the same search contract is reused by a separate list surface later

## Data Model Changes

No new backend domain entities or persistence tables are required.

The public movie summary contract remains stable:

- `items`
- `page`
- `pageSize`
- `totalCount`
- `totalPages`
- `hasPreviousPage`
- `hasNextPage`

## API Changes

No new endpoint is required.

Existing behavior on `GET /api/movies`:

- `query` is optional.
- `page` defaults to `1`.
- `pageSize` defaults to `12`.
- Blank `query` is treated as absent.
- Invalid pagination or malformed input returns `400 Bad Request` with `application/problem+json`.
- The response remains a paginated movie summary payload.

## Frontend Changes

- Keep the catalog page search input bound to the typed movies API.
- Keep query state synchronized with the URL query string where the catalog list is rendered.
- Keep result states explicit for loading, empty catalog, no results, and error.
- Keep the UI copy focused on basic title search.
- Keep the frontend free of direct `HttpClient` calls from components.

## Backend Changes

- Preserve the endpoint-level normalization of the `query` parameter.
- Preserve the repository filter over title and original title.
- Preserve the paginated projection to the public contract.
- Keep validation focused on request shape, not search semantics.

## Testing Strategy

- Backend:
  - Validate the `GET /api/movies` path continues to accept optional `query`.
  - Validate blank queries are normalized away.
  - Validate invalid pagination returns validation problems.
  - Validate the repository query matches both title and original title.
- Frontend:
  - Validate `MoviesApi.listMovies` sends `query`, `page`, and `pageSize` correctly.
  - Validate blank search input is omitted from the API request.
  - Validate the catalog page renders loading, empty, no-result, error, and success states.
  - Validate pagination and URL synchronization remain intact.

Verification:

- `dotnet build backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj`
- `dotnet build SmartMovieCatalog.slnx`
- `npm test -- --watch=false` from `frontend`
- `npm run build` from `frontend`

## Deployment Impact

- No new infrastructure.
- No new packages.
- No new environment variables.
- No route or hosting changes beyond the existing catalog and list behavior.

## Security Considerations

- Treat query string input as untrusted.
- Do not echo raw backend errors in the UI.
- Keep search generic and do not expose provider, vector, or AI details.
- Do not persist search state or tokens in browser storage.

## Observability / Logging

- No new logging surface is required.
- Search requests should remain free of sensitive payloads.
- User-visible states should distinguish loading, empty, no-result, and error conditions without exposing internal exceptions.

## Risks

- Case-insensitive matching may behave differently depending on database/provider translation.
- The frontend and backend can drift if the paginated response contract changes without updating the shared model.
- Query normalization mistakes can produce duplicate requests or stale URL state.
- Because the feature is already implemented in the repository, verification is the primary risk area rather than code design.

## Open Questions

None identified.

## Implementation Notes

- Verified on 2026-05-10 that implemented behavior matches this plan: no API contract drift, no frontend typed-model drift, and no architecture deviations from the defined scope.
- Endpoint/query normalization, repository filtering (`Title`/`OriginalTitle`), catalog UI state handling, and pagination metadata behavior are aligned with planned constraints.

## Rollout Plan

1. Keep the existing endpoint, repository filter, and frontend search form aligned with the issue contract.
2. Run the backend and frontend verification commands.
3. Confirm the catalog search UX still behaves correctly for empty queries, no results, and pagination.
4. Carry the feature forward as a dependency for the broader catalog browsing work.
