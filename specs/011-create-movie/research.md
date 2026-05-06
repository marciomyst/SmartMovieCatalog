# Research: Create Movie

## Decision: Use Existing Clean Architecture Feature Slice

**Decision**: Implement `POST /api/movies` as a Minimal API feature slice under `SmartMovieCatalog.Api/Features/Movies/CreateMovie`, dispatching to an Application handler through the existing Wolverine `IMessageBus`.

**Rationale**: `docs/API.md` and current auth endpoints use Minimal API feature slices and existing in-process Wolverine dispatch. Reusing this pattern keeps HTTP concerns in Api and orchestration in Application without introducing a new architectural style.

**Alternatives considered**:

- Directly invoke an Application service from the endpoint: rejected because current API rules prefer Wolverine dispatch.
- Add controller-based MVC: rejected because current product endpoints are Minimal API feature slices.
- Add new CQRS/messaging infrastructure: rejected because the issue explicitly forbids new CQRS/Wolverine/event-driven behavior.

## Decision: Define Explicit Public Contracts

**Decision**: Add `CreateMovieRequest` and `MovieResponse` under `SmartMovieCatalog.Contracts/Movies`.

**Rationale**: Product endpoints must use stable DTOs and must not leak persistence entities. Contracts keeps API shape independent from Domain and Infrastructure.

**Alternatives considered**:

- Bind directly to Domain `Movie`: rejected because transport DTOs must remain separate from domain models.
- Return EF Core entities: rejected by API and constitution rules.

## Decision: Minimal Movie Aggregate

**Decision**: Add a `Movie` aggregate/concept under `SmartMovieCatalog.Domain/Movies` with `MovieId`, normalized required metadata, optional metadata, and genre normalization.

**Rationale**: `docs/DOMAIN.md` requires movie identity, uniqueness, metadata source/trust, AI provenance, and ownership implications before movie persistence. The spec resolves these for the first slice: GUID identity, duplicates allowed, manually supplied unverified metadata, no AI provenance, and no owner.

**Alternatives considered**:

- Store movie as an infrastructure-only persistence entity: rejected because movie identity and invariants are domain concerns.
- Model posters, AI analysis, ownership, recommendations, or source provenance tables now: rejected as speculative and out of scope.

## Decision: Server-Generated GUID Movie ID

**Decision**: Movie identity uses a server-generated GUID returned as a JSON string.

**Rationale**: Existing auth contracts expose GUID user IDs. GUID IDs avoid sequential database implementation leakage and require no new ID generation scheme.

**Alternatives considered**:

- Sequential numeric IDs: rejected because they expose persistence shape and are less consistent with existing contracts.
- Opaque arbitrary string IDs: rejected because it adds unnecessary ID format ambiguity.

## Decision: Required Fields And Validation

**Decision**: Require `title`, `releaseYear`, `countryCode`, and `originalLanguage`; treat `originalTitle`, `genres`, `director`, `synopsis`, `durationMinutes`, and `ageRating` as optional.

**Rationale**: The issue asks for basic metadata and an intentionally small implementation. Required title/year/country/language provide enough identity and catalog context without blocking creation on descriptive metadata.

**Alternatives considered**:

- Require `genres`: rejected because genre classification may be unavailable or subjective in the first slice.
- Require all suggested payload fields: rejected as too strict for a basic create endpoint.

## Decision: Release Year Range

**Decision**: Validate `releaseYear` between `1888` and the next calendar year.

**Rationale**: This range permits early film history and near-future announced releases while rejecting clearly invalid values.

**Alternatives considered**:

- `1900` through current year: rejected because it excludes early films and announced near-future catalog entries.
- Positive four-digit year: rejected because it admits unrealistic years.

## Decision: Country And Language Validation

**Decision**: Validate `countryCode` as exactly two letters, trim it, and uppercase-normalize it. Trim and preserve `originalLanguage` as a language tag string without strict BCP-47 validation.

**Rationale**: Strict country shape prevents noisy data. Full BCP-47 validation is unnecessary for this first slice and may require more nuance than current scope needs.

**Alternatives considered**:

- Free-form country/language strings: rejected because country data would be too noisy.
- Strict BCP-47 parser for language: rejected as unnecessary complexity.

## Decision: Duplicate Handling

**Decision**: Allow duplicate movie metadata in the first slice and enforce uniqueness only by movie ID.

**Rationale**: Advanced duplicate detection is explicitly out of scope. Title/year/country duplicates can be legitimate and need a separate product decision.

**Alternatives considered**:

- Unique index on title and release year: rejected because it is a weak global uniqueness rule.
- Fuzzy duplicate detection: rejected as explicitly out of scope.

## Decision: Genre Persistence Shape

**Decision**: Persist genres as a normalized child collection/table owned by the movie persistence mapping while keeping API contracts as a string array.

**Rationale**: Genres are likely to become filter/search criteria soon. A normalized child collection avoids committing to provider-specific array behavior and keeps the public contract independent from persistence shape.

**Alternatives considered**:

- JSON/serialized string column: rejected because filtering and normalization become harder.
- PostgreSQL text array: rejected because it is provider-specific and unnecessary for the first model.

## Decision: Created Response

**Decision**: Return `201 Created` with `MovieResponse` body and `Location: /api/movies/{id}` while deferring `GET /api/movies/{id}`.

**Rationale**: This preserves normal create semantics and gives clients a stable future resource URI without expanding the slice into read behavior.

**Alternatives considered**:

- Body only with no `Location`: rejected because API rules prefer a route to the created resource for creates.
- Add read endpoint now: rejected as scope expansion beyond issue #11.

## Decision: Testing Approach

**Decision**: Add focused tests in existing backend test projects: Domain tests for invariants, Application tests with fake repository, API tests through WebApplicationFactory/fakes, and Infrastructure tests for EF Core mapping.

**Rationale**: Existing backend test projects and auth tests establish this pattern. The feature crosses contracts, domain, application, API, and persistence, so broader backend coverage is justified.

**Alternatives considered**:

- Only API tests: rejected because domain/application/persistence regressions would be under-specified.
- TestContainers/database-backed tests: rejected because docs explicitly defer that testing cost.
