# Implementation Plan: Catalog Page

**Feature Directory**: `specs/018-catalog-page` | **Date**: 2026-05-06 | **Spec**: `specs/018-catalog-page/spec.md`
**Input**: Feature specification from `specs/018-catalog-page/spec.md`

## Summary

Implement a public V1 Angular catalog page at `/catalog` that uses a typed `MoviesApi` service to load paginated movie summaries from `GET /api/movies`, supports basic title search, synchronizes `query`, `page`, and `pageSize` through URL query parameters, renders catalog states and pagination, and links each movie to the details route from issue #13.

This plan is frontend-owned for issue #18. Backend list/search/details endpoints remain prerequisite work from issues #12, #16, and #13 unless scope is explicitly expanded.

## Technical Context

**Language/Version**: TypeScript 5.9 with Angular 21; backend context is ASP.NET Core 10 / C# but no backend implementation is owned here.
**Primary Dependencies**: Angular core/common/forms/router, Angular `HttpClient`, RxJS 7.8, local CSS aligned with `frontend/DESIGN.md`.
**Storage**: N/A for this feature; catalog state is represented by browser URL query parameters.
**Testing**: Angular unit tests through `npm test -- --watch=false` using Vitest and Angular HTTP/component testing utilities.
**Target Platform**: Browser SPA served through Angular dev server locally and ASP.NET Core SpaProxy/static fallback in integrated runtime.
**Project Type**: Web application frontend feature in the existing monorepo.
**Performance Goals**: Avoid duplicate API requests for the same state transition; render one page of movie summaries with default `pageSize=12`.
**Constraints**: No new frontend dependencies, no new UI library, no new design system, no direct `HttpClient` use in components, no authentication requirement for `/catalog` in V1.
**Scale/Scope**: One catalog route, one typed movie API service, URL-backed list/search/pagination state, and focused frontend tests.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Actual architecture is the source of truth: pass. Current source has Angular 21 with `provideRouter([])`, typed auth API service patterns, and no catalog route yet.
- Boundaries explicit: pass. Issue #18 implementation stays under `frontend`; backend API work is a prerequisite, not hidden in this feature.
- Contracts drive cross-boundary work: pass with dependency. `GET /api/movies` response shape and details route must match issues #12/#16/#13 before implementation is complete.
- Security and secret hygiene: pass. No secrets, bearer-token persistence, auth guards, provider details, or sensitive backend internals are introduced.
- Small verified changes: pass. Work decomposes into route/app shell, typed API models/service, catalog UI state, tests, and documentation only if structure changes.
- Frontend design compliance: pass. UI uses `frontend/DESIGN.md`, local CSS, dark cinematic direction, Material Symbols, and no unsupported future architecture terminology.

No constitution violations require justification.

## Project Structure

### Documentation (this feature)

```text
specs/018-catalog-page/
|-- spec.md
|-- plan.md
|-- research.md
|-- data-model.md
|-- quickstart.md
|-- contracts/
|   `-- catalog-ui.md
`-- tasks.md
```

### Source Code (repository root)

```text
frontend/
`-- src/
    `-- app/
        |-- app.ts
        |-- app.html
        |-- app.css
        |-- catalog/
        |   `-- catalog-page/
        |       |-- catalog-page.ts
        |       |-- catalog-page.html
        |       |-- catalog-page.css
        |       `-- catalog-page.spec.ts
        `-- movies/
            |-- movie.models.ts
            |-- movies-api.ts
            `-- movies-api.spec.ts
```

**Structure Decision**: Keep API models/services under `frontend/src/app/movies` because movie list/search consumption is reusable by home/catalog/details-adjacent UI. Keep the page under `frontend/src/app/catalog/catalog-page` because catalog orchestration and state are feature-specific.

## Complexity Tracking

No complexity violations.

## Phase 0: Research

Research output: `specs/018-catalog-page/research.md`

Resolved decisions:

- Catalog state uses URL query parameters.
- `pageSize` defaults to 12, may be read from URL, and has no V1 selector.
- `/catalog` is public for V1 and does not require route guards.
- Components use a typed `MoviesApi`; components do not inject `HttpClient`.
- Invalid URL pagination parameters are normalized before API calls to avoid knowingly issuing invalid requests from the UI.
- The issue #17 movie card is reused if present; otherwise a catalog-local card is acceptable.

No `NEEDS CLARIFICATION` items remain.

## Phase 1: Design & Contracts

Design outputs:

- `specs/018-catalog-page/data-model.md`
- `specs/018-catalog-page/contracts/catalog-ui.md`
- `specs/018-catalog-page/quickstart.md`

Contract summary:

- Frontend consumes `GET /api/movies?page={page}&pageSize={pageSize}`.
- Frontend consumes `GET /api/movies?query={query}&page={page}&pageSize={pageSize}`.
- Expected response fields are `items`, `page`, `pageSize`, `totalCount`, `totalPages`, `hasPreviousPage`, and `hasNextPage`.
- Movie item links target `/movies/{id}` unless issue #13 establishes a different details route before implementation.

Post-design constitution check: pass. The design does not add dependencies, backend behavior, authentication requirements, persistence, or unsupported AI/search architecture.

## Technical Approach

1. Establish Angular routing for `/catalog` and root app navigation.
2. Add typed movie models for movie summaries, paginated responses, and list query state.
3. Add `MoviesApi` for `GET /api/movies`, using Angular query parameter construction.
4. Parse URL query parameters into normalized catalog state:
   - `query`: trimmed string, omitted when blank.
   - `page`: positive integer, default `1` when absent or invalid.
   - `pageSize`: positive integer, default `12` when absent or invalid.
5. Synchronize search and pagination changes back to URL query parameters.
6. Load movies from `MoviesApi` and render loading, empty catalog, no-result, error, results, and pagination states.
7. Link catalog items to the movie details route.
8. Add focused API service and component tests with mocked API boundaries.

## Affected Areas

- `frontend/src/main.ts`
- `frontend/src/app/app.ts`
- `frontend/src/app/app.html`
- `frontend/src/app/app.css`
- `frontend/src/app/catalog/catalog-page/*`
- `frontend/src/app/movies/*`
- `docs/FRONTEND.md` only if implementation changes documented routing/API structure.
- `AGENTS.md` Spec Kit current-plan reference.

## Data Model Changes

No backend domain or persistence model changes.

Frontend models are documented in `specs/018-catalog-page/data-model.md`:

- `MovieSummary`
- `PagedMovieSummaryResponse`
- `MovieListQuery`
- `CatalogViewState`
- `CatalogLoadState`

## API Changes

No API changes are owned by issue #18.

Prerequisite API behavior:

- `GET /api/movies?page=1&pageSize=12`
- `GET /api/movies?query=central&page=1&pageSize=12`
- `400 Bad Request` as `application/problem+json` for invalid request parameters.
- details route target backed by `GET /api/movies/{id}` from issue #13.

Current source still maps only `POST /api/movies`; implementation must confirm issues #12/#16/#13 are complete or explicitly expand scope before relying on runtime API behavior.

## Frontend Changes

- Add public `/catalog` route.
- Add app navigation entry to Catalog.
- Add URL-backed catalog state for `query`, `page`, and `pageSize`.
- Use default `pageSize=12`; no V1 page-size selector.
- Render movie summaries in a responsive grid/list following `frontend/DESIGN.md`.
- Render loading, empty catalog, no-result, API error, and pagination states.
- Reuse issue #17 movie card if available, otherwise use a catalog-local card.
- Keep UI copy free of unsupported terms: RAG, Qdrant, vector search, semantic search, CLIP, agent framework, recommendations, and AI ranking.

## Backend Changes

None for issue #18.

## Testing Strategy

- `MoviesApi` tests:
  - builds `GET /api/movies` with `page`, `pageSize`, and optional `query`.
  - omits blank `query`.
  - maps the expected paginated response shape.
- Catalog page tests:
  - route/app shell renders Catalog navigation.
  - reads `query`, `page`, and `pageSize` from URL.
  - defaults invalid or absent `page`/`pageSize` before API calls.
  - search updates URL state and resets `page` to 1.
  - pagination updates URL state and respects `hasPreviousPage`/`hasNextPage`.
  - renders loading, empty catalog, no-result, API error, and success states.
  - details links target the expected details route.

Verification:

- `npm test -- --watch=false` from `frontend`
- `npm run build` from `frontend`
- Run backend build only if prerequisite API contracts are changed as part of a broader scope.

## Deployment Impact

- No new infrastructure.
- No new frontend dependencies.
- No new backend dependencies.
- Uses same-origin `/api` and existing Angular proxy behavior.
- ASP.NET Core SPA fallback should serve `/catalog` in integrated non-test runtime.

## Security Considerations

- `/catalog` is public for V1 by clarified product decision.
- Treat URL query parameters and API responses as untrusted.
- Normalize URL pagination parameters before API calls.
- Use Angular binding; do not render untrusted HTML.
- Do not persist bearer tokens or introduce route guards for this feature.
- Do not expose internal backend errors; show generic API error state.

## Observability / Logging

- No backend logging changes.
- No frontend production logging of raw API payloads or search terms.
- User-visible states communicate pending, empty, no-result, and failure conditions.

## Risks

- Issues #12, #16, and #13 may not be implemented when issue #18 starts.
- Current `PagedResponse<T>` backend contract shape does not match issue #18 expectations.
- Current Angular app has only the login page and an empty router configuration.
- Reusable movie card availability depends on issue #17 implementation order.

## Rollout Plan

1. Confirm prerequisite API response shape and details route.
2. Add typed movie models and `MoviesApi`.
3. Add public `/catalog` route and navigation.
4. Implement URL-backed catalog state.
5. Implement catalog rendering, states, pagination, and details links.
6. Add focused tests.
7. Run frontend tests and build.
8. Update docs only if implementation changes documented frontend structure or API consumption.
