# Feature Specification

## Feature Summary

Implement movie details retrieval and a frontend details page so users can open a movie from catalog/list surfaces and view full metadata for a single movie.

Issue source: https://github.com/marciomyst/SmartMovieCatalog/issues/13

## Problem Statement

The current product allows movie creation and movie listing, but does not provide a details endpoint or details page for a specific movie. Users cannot inspect full movie metadata after selecting a movie from list/card views.

## Goals

- Add `GET /api/movies/{id}` to retrieve one movie by ID.
- Return a dedicated details DTO with full metadata currently supported by the product.
- Add a frontend details route/page for movie details.
- Support loading, not found, and API error states on details UI.
- Keep backend and frontend contracts synchronized.
- Preserve the existing dark cinematic frontend direction.

## Clarifications

### Session 2026-05-08

- Q: The issue example includes `cast` and `updatedAt`. Should this feature keep scope to currently supported metadata only, or expand domain/persistence/contracts now? → A: Keep scope to currently supported metadata only; do not add `cast` or `updatedAt` in this issue.
- Q: For invalid `id` format (non-GUID), what should the API return? → A: Return `400 Bad Request` with `application/problem+json`.
- Q: Should navigation be implemented only in catalog or across all existing movie cards/list items in frontend? → A: Ensure all existing movie card/list entry points navigate to `/movies/{id}`.

## Non-Goals

- Editing movie data.
- Poster upload.
- AI poster analysis fields.
- Realtime updates.
- Similar movie recommendations.
- Semantic search or vector/RAG behavior.
- Introducing new infrastructure, dependencies, or architecture patterns.

## User Stories

- As a catalog user, I want to open a movie details page from a movie card/list item so I can inspect full metadata.
- As a catalog user, I want a clear not-found state when a movie does not exist.
- As a catalog user, I want a clear error state when the API request fails.

## Functional Requirements

- The backend MUST expose `GET /api/movies/{id}`.
- The endpoint MUST return `200 OK` with `application/json` for existing movies.
- The endpoint MUST return `404 Not Found` with `application/problem+json` and fields `type`, `title`, `status`, `detail`, and `instance` when a movie is missing.
- The endpoint MUST return `400 Bad Request` with `application/problem+json` when `{id}` is not a valid GUID.
- The details response MUST be a dedicated public contract DTO under `SmartMovieCatalog.Contracts`.
- The details response MUST include full metadata currently supported in the domain/API for movies.
- The details response MUST NOT introduce new persisted fields such as `cast` or `updatedAt` in this feature.
- The frontend MUST define a details route/page reachable through `/movies/{id}`.
- The details page MUST call a typed API service and MUST NOT call `HttpClient` directly in components.
- The details page MUST display title, release year, genres, director, synopsis, duration, age rating, and available metadata.
- The details page MUST handle loading state.
- The details page MUST handle not-found state.
- The details page MUST handle API failure state.
- Navigation from movie cards/list items to `/movies/{id}` MUST work.
- Persistence models MUST NOT be exposed directly through API responses.
- AI analysis fields MUST NOT be introduced in this feature.

## Acceptance Criteria

- A movie can be retrieved by ID.
- Existing movie returns `200 OK`.
- Successful response uses `application/json`.
- Invalid movie ID format returns `400 Bad Request` with `application/problem+json`.
- Missing movie returns `404 Not Found`.
- Missing movie response uses `application/problem+json` with `type`, `title`, `status`, `detail`, and `instance`.
- The details page displays title, year, genres, director, synopsis, duration, age rating, and available metadata.
- The UI handles loading state.
- The UI handles not found state.
- The UI handles API failure state.
- Navigation from movie card/list to details works.
- No persistence model is exposed directly through the API.

## Edge Cases

- Invalid movie ID format in route parameter.
- Valid GUID format with no corresponding movie in persistence.
- Movie exists with optional metadata missing (`director`, `synopsis`, `durationMinutes`, `ageRating`, `image`).
- Movie exists with zero genres.
- API returns transient server failures while fetching details.
- User lands directly on `/movies/{id}` via deep link.

## Clarifications Needed

- No open clarifications remain for this feature.

## Assumptions

- "Full metadata currently supported" means fields already present in movie domain/application/contracts, without introducing new data ownership concepts.
- `posterUrl` in details response can be derived from stored relative movie `image` path (or null when absent), matching listing behavior.
- The details route pattern remains `/movies/{id}`.

## Dependencies

- Existing movie persistence and listing foundation (`Movie`, `Genre`, EF Core repositories).
- Existing API error contract using `ProblemDetails` / `ValidationProblemDetails` (ADR 0004).
- Existing catalog/list navigation flow that already links to `/movies/{id}`.
- Angular routing and typed API service patterns already used in `frontend/src/app`.

## Risks

- Scope creep if `cast` and `updatedAt` are interpreted as mandatory new persisted fields.
- Backend/frontend contract drift if details DTO evolves without mirrored frontend models.
- Not-found behavior inconsistency if endpoint metadata and actual response shaping diverge.
- Route collisions or UX inconsistency if details page is added without navigation/state alignment.

## Conflicts

- Issue #13 example response includes `cast` and `updatedAt`, which conflicts with the current movie domain model that does not expose those fields; this feature keeps those fields out of scope by decision.

## Out of Scope

- Any create/update/delete movie behavior changes.
- Any AI analysis, recommendation, or semantic search behavior.
- Any authentication-specific details-page behavior beyond existing app defaults.
