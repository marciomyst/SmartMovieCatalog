# Implementation Plan: Home Movie Cards

**Feature Directory**: `specs/017-home-movie-cards` | **Date**: 2026-05-09 | **Spec**: `specs/017-home-movie-cards/spec.md`
**Input**: Feature specification from `specs/017-home-movie-cards/spec.md`

## Summary

Implement a public product home page at `/` that displays up to 6 real movie cards from the existing movie listing flow, preserves authentication through `/login`, supports loading/empty/error states, and links every rendered card to the movie details page.

This plan is frontend-owned for issue #17. Existing backend list and details endpoints are prerequisites; no backend contract or persistence changes are planned unless implementation proves the current listing behavior cannot provide the required movie summaries.

## Technical Context

**Language/Version**: TypeScript 5.9 with Angular 21; backend context is ASP.NET Core 10 / C# but no backend implementation is owned here.
**Primary Dependencies**: Angular core/common/router/http, RxJS 7.8, existing `MoviesApi`, existing movie models, local CSS aligned with `frontend/DESIGN.md`.
**Storage**: N/A. Home page state is transient component state; no browser persistence is introduced.
**Testing**: Angular unit/component tests through `npm test -- --watch=false` using Vitest and Angular testing utilities.
**Target Platform**: Browser SPA served through Angular dev server locally and ASP.NET Core SpaProxy/static fallback in integrated runtime.
**Project Type**: Web application frontend feature in the existing monorepo.
**Performance Goals**: Fetch and render at most 6 home movie cards for the initial home surface; avoid duplicate requests for a single home page load.
**Constraints**: No new frontend dependencies, no new UI library, no new design system, no direct `HttpClient` use in components, no unsupported AI/search/recommendation terminology, and login must remain reachable at `/login`.
**Scale/Scope**: One public home page route, one login route adjustment, up to 6 movie cards, focused reusable card extraction if it reduces duplication with catalog.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Actual architecture is the source of truth: pass. Current source has Angular 21, existing routes for login/catalog/details, `MoviesApi`, and catalog card rendering.
- Boundaries explicit: pass. Implementation stays under `frontend` plus documentation updates; backend work remains prerequisite-only.
- Contracts drive cross-boundary work: pass with dependency. The feature consumes the existing movie listing shape and details route without changing public API contracts.
- Security and secret hygiene: pass. No secrets, token persistence, new auth mechanism, or internal backend details are introduced.
- Small verified changes: pass. Work decomposes into route change, home page, optional card reuse, focused tests, and docs update for routing.
- Frontend design compliance: pass. UI must follow `frontend/DESIGN.md`, local CSS, Material Symbols, and the dark cinematic direction.

No constitution violations require justification.

## Project Structure

### Documentation (this feature)

```text
specs/017-home-movie-cards/
|-- spec.md
|-- plan.md
|-- research.md
|-- data-model.md
|-- quickstart.md
|-- contracts/
|   `-- home-ui.md
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
        |-- app.spec.ts
        |-- auth/
        |   `-- login-page/
        |       |-- login-page.ts
        |       |-- login-page.html
        |       |-- login-page.css
        |       `-- login-page.spec.ts
        |-- catalog/
        |   `-- catalog-page/
        |       |-- catalog-page.ts
        |       |-- catalog-page.html
        |       |-- catalog-page.css
        |       `-- catalog-page.spec.ts
        |-- home/
        |   `-- home-page/
        |       |-- home-page.ts
        |       |-- home-page.html
        |       |-- home-page.css
        |       `-- home-page.spec.ts
        |-- movies/
        |   |-- movie.models.ts
        |   |-- movies-api.ts
        |   |-- movies-api.spec.ts
        |   `-- movie-details-page/
        `-- shared/
            `-- movie-card/
                |-- movie-card.ts
                |-- movie-card.html
                |-- movie-card.css
                `-- movie-card.spec.ts
```

**Structure Decision**: Add a `home/home-page` feature folder for route-specific orchestration. Reuse existing `movies` models/API service. Extract a small shared movie card only if it replaces duplicated card rendering between home and catalog without broad styling or behavior churn; otherwise keep home card markup local and defer extraction.

## Complexity Tracking

No complexity violations.

## Phase 0: Research

Research output: `specs/017-home-movie-cards/research.md`

Resolved decisions:

- `/` becomes the public home page; login moves to `/login`.
- The home page displays up to 6 cards from existing movie listing data.
- Home page loading uses component-local state and does not use URL-backed pagination/search state.
- Home cards link to `/movies/{id}`.
- Components use `MoviesApi`; no component injects `HttpClient`.
- Shared movie card extraction is conditional on reducing real duplication with catalog.

No `NEEDS CLARIFICATION` items remain.

## Phase 1: Design & Contracts

Design outputs:

- `specs/017-home-movie-cards/data-model.md`
- `specs/017-home-movie-cards/contracts/home-ui.md`
- `specs/017-home-movie-cards/quickstart.md`

Contract summary:

- Public route `/` renders home movie cards.
- Public route `/login` renders the existing login screen.
- Home page consumes the existing movie listing API through `MoviesApi` with a request sized to 6 items.
- Home cards target `/movies/{id}`.

Post-design constitution check: pass. The design does not add dependencies, backend behavior, persistence, external services, new auth storage, or unsupported product terminology.

## Technical Approach

1. Add a `HomePage` component under `frontend/src/app/home/home-page`.
2. Update routing so:
   - `/` renders `HomePage`.
   - `/login` renders `LoginPage`.
   - `/catalog` and `/movies/:id` keep current behavior.
3. Update app navigation so users can reach Home, Catalog, and Login as appropriate.
4. Load movie summaries through `MoviesApi.listMovies` using `page=1` and `pageSize=6`.
5. Render home states:
   - loading while the request is pending.
   - empty when zero movies are returned.
   - error when the request fails.
   - success when at least one movie is available.
6. Render up to 6 movie cards with title, release year, genres, director, poster/placeholder, and basic metadata.
7. Link each card to `/movies/{id}` and support keyboard activation.
8. Evaluate whether catalog card markup should be extracted into `shared/movie-card`; extract only if the resulting change is smaller and clearer than duplication.
9. Update `docs/FRONTEND.md` to describe `/` as home and `/login` as login.
10. Add focused tests for route behavior, home states, data loading, card count, and details links.

## Affected Areas

- `frontend/src/main.ts`
- `frontend/src/app/app.html`
- `frontend/src/app/app.css`
- `frontend/src/app/app.spec.ts`
- `frontend/src/app/home/home-page/*`
- `frontend/src/app/catalog/catalog-page/*` if card extraction is chosen
- `frontend/src/app/shared/movie-card/*` if card extraction is chosen
- `frontend/src/app/movies/movies-api.ts` only if a helper for home list parameters is warranted
- `docs/FRONTEND.md`
- `AGENTS.md` Spec Kit current-plan reference

## Data Model Changes

No backend domain, persistence, or public API contract changes.

Frontend data concepts are documented in `specs/017-home-movie-cards/data-model.md`:

- `HomeMovieSection`
- `HomeLoadState`
- `MovieSummary`
- `MovieCard`

## API Changes

No API changes are owned by issue #17.

Prerequisite API behavior:

- `GET /api/movies?page=1&pageSize=6` returns a paged movie summary response.
- Each returned item includes an ID suitable for `/movies/{id}` navigation.
- Optional metadata may be null or empty and must be rendered with safe fallbacks.

## Frontend Changes

- Add public home page at `/`.
- Move login route to `/login`.
- Keep `/catalog` as the full browsing surface.
- Keep `/movies/:id` as the details surface.
- Render a bounded 6-card movie section on the home page.
- Show loading, empty, and API error states.
- Keep UI copy free of unsupported terms: RAG, Qdrant, vector search, semantic search, CLIP, agent framework, recommendations, and AI ranking.
- Keep styling aligned with `frontend/DESIGN.md`.

## Backend Changes

None for issue #17.

## Testing Strategy

- App routing/shell tests:
  - root route maps to home.
  - `/login` maps to the login page.
  - navigation exposes Home/Catalog/Login entry points without breaking existing routes.
- Home page tests:
  - calls movie service with `page=1` and `pageSize=6`.
  - renders loading state.
  - renders empty state.
  - renders error state.
  - renders fewer than 6 cards when fewer movies exist.
  - renders exactly 6 cards when at least 6 movies exist.
  - cards link to `/movies/{id}`.
  - no prohibited terminology appears in rendered home text.
- Movie card tests if extraction occurs:
  - poster image and placeholder behavior.
  - metadata fallbacks.
  - accessible link label and keyboard-compatible activation.

Verification:

- `npm test -- --watch=false` from `frontend`
- `npm run build` from `frontend`

Backend verification is not required unless implementation expands scope into API contracts or backend code.

## Deployment Impact

- No new infrastructure.
- No new frontend dependencies.
- No new backend dependencies.
- ASP.NET Core SPA fallback should serve `/`, `/login`, `/catalog`, and `/movies/{id}` in integrated non-test runtime.

## Security Considerations

- `/` and `/catalog` are public catalog discovery surfaces for V1.
- `/login` remains the authentication entry point.
- Do not persist bearer tokens or change auth session storage.
- Treat API response data as untrusted and render through Angular bindings.
- Do not render untrusted HTML.
- Show generic user-facing API failure copy; do not expose raw backend errors.

## Observability / Logging

- No backend logging changes.
- No frontend production logging of raw API payloads.
- User-visible states communicate pending, empty, and failure conditions.

## Risks

- Routing docs and tests may still assume `/` is login and must be updated together.
- Extracting a shared movie card may create more churn than value if catalog and home card layouts diverge.
- Current catalog image URL normalization contains logic that should stay consistent if a second card surface is added.
- If the backend default ordering is not actually "recently added", home copy must avoid claiming recency beyond what the API guarantees.

## Rollout Plan

1. Update routing and docs for `/` home and `/login`.
2. Add home page component and tests for loading/empty/error/success states.
3. Implement home movie loading through `MoviesApi`.
4. Render up to 6 cards and details links.
5. Extract shared card only if it reduces duplication cleanly.
6. Run frontend tests and build.

## Implementation Decision (T030)

- Decision date: 2026-05-09.
- Result: shared `movie-card` extraction was not selected.
- Rationale: current home and catalog cards have different structure and visual emphasis; extracting now would increase churn and coupling without clear maintainability gain for issue #17 scope.
