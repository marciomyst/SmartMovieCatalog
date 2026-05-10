# GitHub Issue Context

## Source

- Repository: marciomyst/SmartMovieCatalog
- Issue: #16
- URL: https://github.com/marciomyst/SmartMovieCatalog/issues/16
- State: OPEN
- Created: 05/02/2026 23:08:24
- Updated: 05/02/2026 23:08:25
- Milestone: M1 — Core Movie Catalog

## Title

Basic title search

## Labels

- type:feature
- priority:high
- area:api
- area:backend
- area:frontend
- area:catalog
- area:search
- v1

## Assignees

_No assignees_



## Issue Body

## Summary

Implement basic movie search by title.

This is a simple catalog search feature for V1 and must not be confused with semantic search, RAG, vector search, or AI-based discovery.

## Goal

Allow users to find movies by title using a basic text query.

## Scope

- Add title query support to the movie listing endpoint.
- Search against movie title and optionally original title.
- Add frontend search input where movie lists are displayed.
- Show loading, empty result, and error states.
- Keep search behavior simple and predictable.

## Suggested API

```http
GET /api/movies?query=central&page=1&pageSize=12
```

Response media type:

```http
application/json
```

The response should use the same paginated response shape as `GET /api/movies`:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 12,
  "totalCount": 0,
  "totalPages": 0,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

Example invalid request response:

```http
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json
```

```json
{
  "type": "https://smartmoviecatalog.dev/problems/invalid-request",
  "title": "Invalid request.",
  "status": 400,
  "detail": "The request could not be processed because it is malformed or contains invalid parameters.",
  "instance": "/api/movies",
  "errors": {
    "page": [
      "Page must be greater than or equal to 1."
    ]
  }
}
```

## Acceptance Criteria

- User can search movies by title.
- Search is case-insensitive if supported by the chosen implementation.
- Empty query returns the default movie list.
- No results displays a clear empty state.
- Search works with pagination.
- Search response uses the same paginated response shape as the movie listing endpoint.
- Invalid pagination or query parameters return `400 Bad Request` using `application/problem+json` with `invalid-request`.
- Frontend uses a typed API service.
- Search does not use AI, embeddings, Qdrant, RAG, CLIP, or semantic ranking.
- API response shape remains stable.

## Technical Notes

- Keep this as basic title search only.
- Avoid introducing search infrastructure prematurely.
- Do not add a dedicated search endpoint for V1.
- If persistence is not implemented yet, search should use the current storage mechanism consistently.
- Debounce on the frontend only if it fits the existing Angular patterns.

## Out of Scope

- Semantic search.
- AI tags search.
- Full-text search engine.
- Ranking algorithms.
- Genre/year filters.
- Poster visual search.


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
