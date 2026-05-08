# Feature Specification

## Feature Summary

Implement a dedicated catalog browsing page at `/catalog` where users can browse movies, run basic title search, page through results, and navigate from a catalog item to the movie details page.

Issue source: https://github.com/marciomyst/SmartMovieCatalog/issues/18

## Clarifications

### Session 2026-05-06

- Q: Should search and pagination state be reflected in the browser URL query string for deep links, or can it remain component state for V1? -> A: Use URL query params, for example `/catalog?query=central&page=2&pageSize=12`.
- Q: Should the catalog expose a fixed page size of 12, as suggested by the issue, or offer a page-size selector? -> A: Read `pageSize` from the URL but do not expose a page-size selector.
- Q: Should `/catalog` be publicly reachable for V1 or require an authenticated frontend session? -> A: `/catalog` is publicly reachable for V1.

## Problem Statement

The application currently does not provide a dedicated catalog surface outside the home page. Users need a stable route for browsing all movies, searching by title, handling result states, and opening movie details.

## Goals

- Add a catalog route/page reachable from app navigation.
- Fetch paginated movie summaries through the movie listing API.
- Integrate basic title search using `GET /api/movies?query=...&page=...&pageSize=...`.
- Display movies in cards or a consistent catalog layout aligned with the dark cinematic frontend design system.
- Support loading, empty catalog, no-result, API error, and pagination states.
- Link each catalog item to the movie details route.
- Keep the page responsive across mobile and desktop.
- Keep implementation scoped to V1 catalog browsing without unsupported AI or advanced search terminology.

## Non-Goals

- Genre filtering.
- Year filtering.
- AI analyzed filtering.
- Semantic search.
- Vector search.
- Recommendations.
- Poster similarity.
- RAG.
- Authentication-specific catalog behavior.
- New UI libraries, state management frameworks, or design systems.
- Backend implementation of prerequisite movie list, search, or details endpoints unless those prerequisite issues are explicitly folded into the work.

## User Stories

- As a catalog user, I want to open a dedicated catalog page so that I can browse movies outside the home page.
- As a catalog user, I want to search movies by title so that I can find a specific movie quickly.
- As a catalog user, I want pagination controls so that I can move through large result sets deliberately.
- As a catalog user, I want clear loading, empty, no-result, and error states so that I understand what is happening.
- As a catalog user, I want to open a movie details page from a catalog item so that I can inspect full metadata.

## Functional Requirements

- The frontend MUST define a `/catalog` route.
- The catalog page MUST be reachable through app navigation.
- The catalog page MUST be publicly reachable in V1 and MUST NOT require frontend authentication or route guards.
- The catalog page MUST call a typed frontend API service rather than calling `HttpClient` directly from components.
- The typed API service MUST consume `GET /api/movies` with `query`, `page`, and `pageSize` parameters.
- The catalog page MUST display movie summaries returned by the API.
- The catalog page MUST support a basic title search input.
- The catalog page MUST synchronize `query`, `page`, and `pageSize` with URL query parameters so catalog state survives refreshes and supports shareable links.
- The catalog page MUST default to `pageSize=12` when no page size is present in the URL.
- The catalog page MUST honor a valid `pageSize` from the URL but MUST NOT expose a page-size selector in the V1 UI.
- Empty search query MUST return the default movie list behavior provided by the listing API.
- Search MUST remain basic title search and MUST NOT expose semantic, vector, RAG, CLIP, AI ranking, or recommendation behavior.
- The page MUST display a loading state while requests are pending.
- The page MUST display an empty catalog state when the API returns zero total movies for an empty query.
- The page MUST display a no-result state when the API returns zero items for a non-empty query.
- The page MUST display an API error state when the request fails.
- The page MUST provide explicit pagination controls driven by `page`, `pageSize`, `totalCount`, `totalPages`, `hasPreviousPage`, and `hasNextPage`.
- Pagination controls MUST not allow navigation before the first page or beyond the last page.
- Catalog items MUST navigate to the movie details route backed by issue #13.
- Catalog layout MUST be responsive and follow `frontend/DESIGN.md`.
- Reusable movie card behavior from issue #17 SHOULD be reused if it exists by implementation time.
- Components MUST keep server communication in typed API services and strongly typed models.

## Acceptance Criteria

- Catalog page is reachable through app navigation.
- Catalog page lists movies from the API.
- Catalog page consumes the paginated movie list endpoint from issue #12.
- Catalog page integrates title search from issue #16.
- User can navigate from catalog item to movie details.
- Catalog item navigation targets the movie details route backed by issue #13.
- Catalog page handles loading state.
- Catalog page handles empty catalog state.
- Catalog page handles no search results state.
- Catalog page handles API error state.
- Pagination controls work consistently using `page`, `pageSize`, `totalCount`, `totalPages`, `hasPreviousPage`, and `hasNextPage`.
- Layout is responsive.
- Components use typed API services instead of direct `HttpClient`.
- No unsupported future architecture terms are shown in the UI.

## Edge Cases

- API returns zero items and zero total count for an empty query.
- API returns zero items for a non-empty query.
- API returns an invalid request problem response for invalid pagination or query parameters.
- User changes the search term while a request is in flight.
- User pages after a search query changes.
- User opens `/catalog` with URL query parameters for `query`, `page`, or `pageSize`.
- User opens `/catalog` with an invalid `pageSize` query parameter.
- User lands on a page number that no longer has results after data changes.
- API returns partial optional movie metadata such as missing `director`, `genres`, or `posterUrl`.
- Movie card component from issue #17 is unavailable at implementation time.
- Details route from issue #13 is unavailable at implementation time.

## Clarifications Needed

None identified.

## Assumptions

- Issue #12 will provide `GET /api/movies` with the shared paginated movie summary response before issue #18 implementation is completed.
- Issue #16 will provide basic title search through the same listing endpoint before issue #18 implementation is completed.
- Issue #13 will provide the movie details route before catalog item navigation is completed.
- Issue #17 will provide a reusable movie card component if it is implemented first; otherwise issue #18 may create a small catalog item/card component within the catalog feature boundary.
- The default page size is 12 when `pageSize` is absent from the URL.
- Catalog search and pagination state use URL query parameters for V1.
- Catalog browsing is public for V1.

## Dependencies

- Issue #12: List movies.
- Issue #16: Basic title search.
- Issue #13: View movie details.
- Issue #17: Movie cards on the home page, optional reuse dependency.
- Existing Angular 21 frontend in `frontend`.
- Existing same-origin `/api` proxy behavior documented in `docs/FRONTEND.md`.
- Existing design system in `frontend/DESIGN.md`.

## Risks

- The current source only maps `POST /api/movies`; `GET /api/movies` and `GET /api/movies/{id}` are not present yet.
- The current `SmartMovieCatalog.Contracts.Common.PagedResponse<T>` uses `PageNumber` and lacks `totalPages`, `hasPreviousPage`, and `hasNextPage`, which conflicts with the issue #12/#16/#18 expected response shape.
- The current root Angular app renders only the login page and has an empty router configuration, so navigation and route layout work may be larger than a single page component if not delivered by prerequisite issues.
- Reusing issue #17 movie card depends on implementation order.

## Conflicts

- `docs/API.md` currently documents only `POST /api/movies`, while issue #18 requires consuming `GET /api/movies` from issue #12 and title search from issue #16.
- Current `PagedResponse<T>` source shape does not match the paginated response shape required by issue #18. This should be resolved by issue #12/#16 or explicitly included before implementation of this page.

## Out of Scope

- Implementing advanced catalog filters.
- Implementing semantic, vector, RAG, AI ranking, or recommendation search.
- Adding authentication-specific behavior to catalog browsing.
- Adding poster upload or poster similarity.
- Changing backend architecture, persistence provider, authentication model, or frontend design system.
