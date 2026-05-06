# Domain

## Current Domain Scope

The product direction is AI-assisted movie cataloging and discovery. The Domain project is still early, but it now contains these implemented domain concepts:

- `User`: local authentication user aggregate under `backend/src/SmartMovieCatalog.Domain/Users`.
- `Movie`: movie catalog record under `backend/src/SmartMovieCatalog.Domain/Movies`.

## Candidate Domain Concepts

These are candidate concepts, not implemented contracts:

- Catalog: curated collection of movies.
- User Preference: user-provided or inferred taste profile.
- Recommendation: ranked movie suggestion generated from catalog and preference signals.
- AI Analysis: structured output produced by an AI process for a movie or catalog item.

## Movie Domain Decisions

The first persisted movie behavior is `POST /api/movies`.

- Movie identity uses a server-generated GUID.
- Public API responses expose the movie ID as a JSON string.
- Uniqueness is by ID only in the first slice; duplicate title/year/country metadata is allowed.
- Required movie metadata is `title`, `releaseYear`, `countryCode`, and `originalLanguage`.
- `releaseYear` must be between `1888` and the next calendar year.
- `countryCode` is exactly two letters and is uppercase-normalized.
- `originalLanguage` is a trimmed language tag string.
- Genres are optional, trimmed, non-blank values and are persisted as a normalized child collection.
- Metadata source is manually supplied by the API caller.
- Metadata trust level is unverified.
- Movies are global catalog records with no user owner because authentication and authorization are out of scope for the first create slice.
- AI analysis, external provider provenance, duplicate detection, update/delete lifecycle, ownership, and authorization are deferred.

## Domain Rules

- Domain rules must be explicit in code, not hidden in UI logic or infrastructure configuration.
- Domain model code is organized by aggregate or domain concept folder, as documented in `docs/adr/0006-domain-aggregate-organization.md`.
- Do not persist or expose AI-generated data without a clear schema and provenance.
- Keep transport DTOs separate from domain models once domain models exist.
- Avoid adding domain abstractions before the first real behavior requires them.

## Non-Goals For Now

- No domain-layer CQRS abstraction. CQRS command/query dispatch is an Application/API concern documented in `docs/adr/0007-wolverine-cqrs-mediator.md`.
- No AI analysis schema, regeneration, versioning, or manual correction behavior yet.
- No user-specific movie ownership rules yet.

Add these only when the implementation requires them or the user explicitly approves an architecture direction.
