# Feature Specification

## Feature Summary

Implement the first real movie catalog vertical slice: creating a movie with basic manually entered metadata through `POST /api/movies`.

The feature adds explicit public API contracts, validation, application orchestration, a minimal movie domain representation, EF Core persistence through the existing infrastructure layer, and a stable `201 Created` response containing the created movie identifier and main movie data. No authentication is required for this issue.

## Clarifications

### Session 2026-05-04

- Q: Which movie fields are required for the first create-movie slice? → A: Required fields are `title`, `releaseYear`, `countryCode`, and `originalLanguage`; all other request fields are optional.
- Q: What valid range should `releaseYear` use? → A: `releaseYear` must be between `1888` and the next calendar year.
- Q: What format should the public movie ID use? → A: Movie ID is a server-generated GUID returned as a JSON string.
- Q: What validation should `countryCode` and `originalLanguage` use? → A: `countryCode` must be two letters; `originalLanguage` is a trimmed language tag string.
- Q: What `201 Created` response behavior should the endpoint use? → A: Return `201 Created` with the movie body and `Location: /api/movies/{id}`; the read endpoint is deferred.

## Problem Statement

SmartMovieCatalog currently has authentication foundation behavior but no product catalog behavior. The application needs a small, production-shaped movie creation path to replace scaffold direction with an actual catalog use case while preserving Clean Architecture boundaries and current API conventions.

## Goals

- Add a `POST /api/movies` endpoint.
- Define explicit request and response DTOs in `SmartMovieCatalog.Contracts`.
- Validate required movie input at the API boundary.
- Move creation orchestration and business rules out of the endpoint handler.
- Add a minimal movie domain model or representation required for persisted creation.
- Persist movies using the current accepted persistence approach.
- Return `201 Created` with the created movie ID and main movie data.
- Keep API contracts separate from persistence models.
- Update API/domain/architecture documentation where behavior or contracts change.
- Add focused tests for domain/application/API/persistence behavior where existing test structure applies.

## Non-Goals

- Poster upload.
- Gemini Vision analysis.
- SignalR notifications.
- Authentication or authorization.
- Semantic search.
- Advanced duplicate detection.
- Bulk import.
- TMDb integration.
- New persistence providers.
- New external services.
- New Wolverine transports, distributed messaging, RAG, AI provider behavior, or event-driven processing.
- Frontend movie creation UI unless an existing movie creation page is found during implementation.

## User Stories

- As an API client, I can submit basic movie metadata and create a movie in the catalog.
- As an API client, I receive `201 Created` with the created movie ID and movie data when creation succeeds.
- As an API client, I receive a consistent validation response when required or malformed input is submitted.
- As a maintainer, I can review the create movie behavior through clear API contracts, application orchestration, domain rules, persistence mapping, and focused tests.

## Functional Requirements

- `POST /api/movies` MUST accept JSON with `title`, `originalTitle`, `releaseYear`, `countryCode`, `originalLanguage`, `genres`, `director`, `synopsis`, `durationMinutes`, and `ageRating`.
- `POST /api/movies` MUST validate required fields before executing application logic.
- `title`, `releaseYear`, `countryCode`, and `originalLanguage` MUST be required for movie creation.
- `originalTitle`, `genres`, `director`, `synopsis`, `durationMinutes`, and `ageRating` MUST be optional for movie creation.
- `title` MUST be required and non-blank after trimming.
- `releaseYear` MUST be required and between `1888` and the next calendar year.
- `countryCode` MUST be required, trimmed, validated as exactly two letters, and normalized to uppercase.
- `originalLanguage` MUST be required, trimmed, and preserved as a language tag string.
- `genres` MUST allow one or more non-blank genre names when supplied and MUST avoid persisting blank genre entries.
- `durationMinutes` MUST be positive when supplied.
- The endpoint MUST return `400 Bad Request` as `ValidationProblemDetails` for request-shape and field-validation failures.
- The endpoint MUST dispatch creation through the Application layer instead of placing business rules in the Minimal API handler.
- The Application layer MUST return a stable created movie result or an expected failure result.
- The Domain layer MUST define the minimal movie identity and invariant model needed by this behavior.
- Movie identity MUST use a server-generated GUID that is returned in API responses as a JSON string.
- The Infrastructure layer MUST persist created movies through the existing EF Core/PostgreSQL persistence approach.
- Public request and response DTOs MUST live in `SmartMovieCatalog.Contracts` and MUST NOT expose persistence entities.
- The response MUST include the created movie ID and main movie data.
- The successful response MUST use `201 Created`.
- The successful response MUST include a `Location` header with `/api/movies/{id}` even though `GET /api/movies/{id}` is deferred to a later slice.
- OpenAPI metadata MUST document success and validation responses.
- Documentation MUST be updated when API contracts, domain constraints, persistence shape, or architecture behavior changes.
- No authentication MUST be required for this issue.
- No Gemini, SignalR, CQRS, Wolverine, RAG, semantic search, or event-driven behavior is introduced by this issue.

## Acceptance Criteria

- A movie can be created through the API.
- The endpoint returns `201 Created` when creation succeeds.
- The response includes the created movie ID.
- The successful response includes a `Location` header in the form `/api/movies/{id}`.
- Required fields are validated.
- Invalid input returns a consistent validation/error response.
- Business rules do not live directly in controllers.
- API contracts do not expose persistence models.
- The implementation respects Clean Architecture dependency direction.
- No authentication is required in this issue.
- No Gemini, SignalR, CQRS, Wolverine, RAG, semantic search, or event-driven behavior is introduced.

## Edge Cases

- Missing request body returns `400 Bad Request` as `ValidationProblemDetails`.
- Blank `title`, `countryCode`, or `originalLanguage` returns validation errors.
- Whitespace around string fields is normalized consistently before persistence and response.
- Invalid `releaseYear` values before `1888` or after the next calendar year return validation errors.
- `countryCode` values that are not exactly two letters return validation errors.
- Non-positive `durationMinutes` values return validation errors.
- Empty or blank genre entries are rejected or normalized consistently.
- Duplicate movies are allowed in this first slice unless a future feature defines duplicate detection.
- Database-specific or internal exception details are not exposed through API responses.
- No user ownership is assigned because authentication and authorization are out of scope.

## Clarifications Needed

None identified.

## Assumptions

- Movie IDs are server-generated GUIDs and stable, using a domain identifier that can be returned as a public JSON string ID.
- Movie metadata in this issue is manually supplied by the API caller, not imported from TMDb or generated by AI.
- Metadata trust level is "user supplied / unverified" for this first slice; AI provenance and external-source provenance are out of scope.
- The first slice does not enforce movie title/year uniqueness because advanced duplicate detection is explicitly out of scope.
- Created movies are global catalog records with no owner because this issue explicitly requires no authentication.
- Existing EF Core/PostgreSQL persistence is the accepted persistence approach for this repository.
- Existing Wolverine in-process dispatch may be used only as the repository's current Application mediator pattern; this issue must not introduce new Wolverine dependencies, transports, distributed messaging, or event-driven behavior.
- No frontend create movie page currently exists, so frontend work is not part of the planned implementation unless implementation discovers an existing page intended for this slice.

## Dependencies

- Existing ASP.NET Core 10 API project and Minimal API feature-slice structure.
- Existing `SmartMovieCatalog.Contracts` project for public DTOs.
- Existing `SmartMovieCatalog.Application` project for use-case orchestration.
- Existing `SmartMovieCatalog.Domain` project for domain model and invariants.
- Existing `SmartMovieCatalog.Infrastructure` EF Core/PostgreSQL persistence setup.
- Existing backend test projects under `backend/tests`.
- `docs/API.md`, `docs/DOMAIN.md`, `docs/ARCHITECTURE.md`, `docs/SECURITY.md`, and `docs/TESTING.md` for repository rules.

## Risks

- Movie domain constraints are still early; over-modeling the aggregate could create speculative complexity.
- Under-modeling identity, source trust, and duplicate behavior could violate `docs/DOMAIN.md`; this spec resolves those as first-slice assumptions.
- Adding persisted movie behavior requires EF Core model and migration changes; migration quality matters even for a small slice.
- The issue says not to introduce CQRS/Wolverine behavior while current API documentation requires dispatch through Wolverine. The plan treats existing in-process Wolverine usage as an existing architectural pattern, not a new introduction.
- No authentication means created movie records are not user-owned; a later authenticated catalog feature may need ownership migration or additional fields.

## Out of Scope

- Poster or file upload.
- AI analysis or Gemini integration.
- SignalR notifications.
- Authentication, authorization, route guards, or user ownership.
- Semantic search, RAG, embeddings, recommendation, or similarity behavior.
- Duplicate detection beyond allowing records with distinct server-generated IDs.
- Bulk import.
- TMDb or other external metadata integration.
- Frontend implementation unless an existing movie creation page is already part of the codebase.
