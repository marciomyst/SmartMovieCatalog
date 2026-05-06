# GitHub Issue Context

## Source

- Repository: marciomyst/SmartMovieCatalog
- Issue: #11
- URL: https://github.com/marciomyst/SmartMovieCatalog/issues/11
- State: OPEN
- Created: 05/02/2026 22:05:54
- Updated: 05/02/2026 22:25:51
- Milestone: M1 — Core Movie Catalog

## Title

Create movie

## Labels

- type:feature
- priority:high
- area:api
- area:backend
- area:catalog
- v1

## Assignees

- marciomyst



## Issue Body

## Summary

Implement the first real product vertical slice for creating a movie in Smart Movie Catalog.

This issue replaces part of the scaffold direction with an actual movie catalog behavior, while keeping the implementation intentionally simple and aligned with the current Clean Architecture structure.

## Goal

Allow a user to create a movie with basic metadata through the application.

## Scope

- Add a create movie API endpoint.
- Define explicit request/response DTOs in `SmartMovieCatalog.Contracts`.
- Add minimal application use case/orchestration in `SmartMovieCatalog.Application`.
- Add minimal domain model or domain representation only if required by the behavior.
- Persist or store the movie using the currently accepted persistence approach.
- Return a stable response containing the created movie identifier and main movie data.
- Add/update basic frontend flow if the movie creation page already exists or is part of this slice.
- Update documentation if API contracts or architecture behavior changes.

## Suggested API

```http
POST /api/movies

{
  "title": "Central do Brasil",
  "originalTitle": "Central do Brasil",
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR",
  "genres": ["Drama"],
  "director": "Walter Salles",
  "synopsis": "A retired teacher and a young boy travel through Brazil in search of his father.",
  "durationMinutes": 110,
  "ageRating": "12"
}
```


### Acceptance Criteria

- A movie can be created through the API.
- The endpoint returns `201 Created` when creation succeeds.
- The response includes the created movie ID.
- Required fields are validated.
- Invalid input returns a consistent validation/error response.
- Business rules do not live directly in controllers.
- API contracts do not expose persistence models.
- The implementation respects Clean Architecture dependency direction.
- No authentication is required in this issue.
- No Gemini, SignalR, CQRS, Wolverine, RAG, semantic search, or event-driven behavior is introduced.

### Technical Notes

- Keep the first implementation small.
- Prefer explicit DTOs.
- Do not introduce speculative abstractions.
- If persistence is not fully implemented yet, this issue depends on the persistence foundation decision or must use a clearly documented temporary storage approach.
- Do not leak database-specific details through API responses.

### Out of Scope

- Poster upload.
- Gemini Vision analysis.
- SignalR notifications.
- Authentication/authorization.
- Semantic search.
- Advanced duplicate detection.
- Bulk import.
- TMDb integration.

## Comments

_No comments_

## Instructions for Spec Kit

Use this GitHub issue as the primary source of truth.

Convert the issue into a Spec Kit feature specification before creating the implementation plan.

Preserve:

- business goal;
- user stories;
- acceptance criteria;
- technical constraints;
- non-goals;
- dependencies;
- open questions.

If information is missing, add it under a clearly marked **Clarifications Needed** section instead of inventing requirements.

If the issue conflicts with existing project documentation, explicitly call out the conflict.

Prefer a small, incremental implementation plan aligned with the repository's existing architecture, folder structure, language, framework, and conventions.
