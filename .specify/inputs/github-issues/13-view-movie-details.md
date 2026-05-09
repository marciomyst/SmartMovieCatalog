# GitHub Issue Context

## Source

- Repository: marciomyst/SmartMovieCatalog
- Issue: #13
- URL: https://github.com/marciomyst/SmartMovieCatalog/issues/13
- State: OPEN
- Created: 2026-05-02T22:42:32Z
- Updated: 2026-05-02T22:58:10Z
- Milestone: M1 — Core Movie Catalog

## Title

View movie details

## Labels

- type:feature
- priority:high
- area:api
- area:backend
- area:frontend
- area:catalog
- v1

## Assignees

_No assignees_



## Issue Body

## Summary

Implement the movie details API and frontend details view.

This issue allows users to open a specific movie from a list or card and see its full metadata.

## Goal

Allow users to view the full details of a single movie.

## Scope

- Add an API endpoint to get a movie by ID.
- Return a detail DTO with the full movie metadata currently supported.
- Add a frontend details route/page.
- Add navigation from movie cards/list items to the details page.
- Handle loading, not found, and error states.
- Keep the UI aligned with the dark cinematic product direction.

## Suggested API

```http
GET /api/movies/{id}
```

Response media type:

```http
application/json
```

Example response shape:

```json
{
  "id": "movie-id",
  "title": "Central do Brasil",
  "originalTitle": "Central do Brasil",
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR",
  "genres": ["Drama"],
  "director": "Walter Salles",
  "cast": [],
  "synopsis": "A retired teacher and a young boy travel through Brazil in search of his father.",
  "durationMinutes": 110,
  "ageRating": "12",
  "posterUrl": null,
  "createdAt": "2026-05-02T00:00:00Z",
  "updatedAt": null
}
```

Example not found response:

```http
HTTP/1.1 404 Not Found
Content-Type: application/problem+json
```

```json
{
  "type": "https://smartmoviecatalog.dev/problems/movie-not-found",
  "title": "Movie not found.",
  "status": 404,
  "detail": "The movie with id '8b9c2b58-3f8e-48f6-ae0c-6b77f4f274b1' was not found.",
  "instance": "/api/movies/8b9c2b58-3f8e-48f6-ae0c-6b77f4f274b1"
}
```

## Acceptance Criteria

- A movie can be retrieved by ID.
- Existing movie returns `200 OK`.
- Successful response uses the `application/json` media type.
- Missing movie returns `404 Not Found`.
- Missing movie response uses `application/problem+json` with `type`, `title`, `status`, `detail`, and `instance`.
- The details page displays title, year, genres, director, synopsis, duration, age rating, and available metadata.
- The UI handles loading state.
- The UI handles not found state.
- The UI handles API failure state.
- Navigation from movie card/list to details works.
- No persistence model is exposed directly through the API.

## Technical Notes

- Use a dedicated detail response DTO.
- Use simple HTTP JSON contracts for request and response payloads.
- Use `application/problem+json` for not found failures.
- Do not include AI analysis fields until the AI analysis feature exists.
- Do not introduce edit behavior in this issue unless needed only as navigation to the edit flow.

## Out of Scope

- Editing movie data.
- Poster upload.
- AI poster analysis.
- Realtime updates.
- Similar movie recommendations.
- Semantic search.


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
