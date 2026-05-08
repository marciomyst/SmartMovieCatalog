# GitHub Issue Context

## Source

- Repository: marciomyst/SmartMovieCatalog
- Issue: #18
- URL: https://github.com/marciomyst/SmartMovieCatalog/issues/18
- State: OPEN
- Created: 2026-05-02T23:34:28Z
- Updated: 2026-05-02T23:34:28Z
- Milestone: M1 — Core Movie Catalog

## Title

Catalog page

## Labels

- type:feature
- priority:high
- area:frontend
- area:catalog
- area:ui
- v1

## Assignees

_No assignees_



## Issue Body

## Summary

Implement the main catalog browsing page.

The catalog page should provide a dedicated surface for browsing all movies and using basic title search.

## Goal

Allow users to browse the movie catalog outside of the home page.

## Scope

- Add a catalog route/page.
- Fetch paginated movie summaries from the API.
- Display movies using cards or a consistent catalog layout.
- Integrate basic title search using the movie listing query parameter.
- Support loading, empty, error, and no-result states.
- Support explicit pagination.
- Link each movie to the details page.
- Keep the layout responsive.

## Suggested Route

```text
/catalog
```

## Suggested API Usage

```http
GET /api/movies?query=central&page=1&pageSize=12
```

The response should use the shared paginated movie list shape:

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

Invalid pagination or query parameters should use the shared invalid request response:

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

## Suggested Initial UI

- Page title: `Catalog`
- Search input
- Movie grid/list
- Pagination controls
- Empty state
- Error state

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

## Technical Notes

- Keep this page focused on basic browsing for V1.
- Do not introduce advanced filter UI unless backend support exists.
- Search integration should remain basic title search through `GET /api/movies?query=...`.
- Reuse the movie card component from issue #17 if available.
- Keep catalog state simple and maintainable.

## Out of Scope

- Genre filter.
- Year filter.
- AI analyzed filter.
- Semantic search.
- Vector search.
- Recommendations.
- Poster similarity.
- RAG.
- Authentication-specific catalog behavior.


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
