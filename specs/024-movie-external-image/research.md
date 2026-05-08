# Research: Movie External Identifier And Card Image

## Decision: Model `externalId` as optional positive movie metadata

**Rationale**: The requested field stores the TMDB movie identifier. TMDB movie IDs are numeric identifiers and the feature only needs to preserve a caller-supplied value. Making the field optional preserves existing movie creation behavior and existing rows.

**Alternatives considered**:

- Provider-qualified external identity object: rejected as unnecessary until multiple external providers or provenance rules exist.
- Required TMDB ID: rejected because the user explicitly requested a non-required field and current movie creation is manually supplied.
- Unique TMDB ID constraint: rejected because current duplicate detection and provider identity rules are out of scope.

## Decision: Validate `externalId` as positive when supplied

**Rationale**: A zero or negative TMDB movie identifier is not meaningful. Rejecting non-positive values at the request boundary and enforcing the invariant in the domain keeps bad metadata out of persisted records.

**Alternatives considered**:

- Accept any integer: rejected because it allows invalid provider identifiers.
- Treat zero as absent: rejected because it hides caller mistakes and creates inconsistent semantics.

## Decision: Model `image` as optional normalized relative path metadata

**Rationale**: The request asks to save the card image path, not image content. Storing a nullable relative path string that is trimmed and normalized to absent when blank matches existing optional movie string behavior while avoiding arbitrary external URLs in this feature.

**Alternatives considered**:

- Store binary image data: rejected as outside the requested path/reference scope.
- Download/store TMDB image assets: rejected because the feature explicitly avoids external service behavior.
- Require `image` when `externalId` exists: rejected because the user requested both fields as independently optional.
- Accept absolute URLs: rejected because the clarified contract only accepts relative image paths such as `/p/example-card.jpg`.

## Decision: Propagate fields through the existing create movie path

**Rationale**: Current movie creation already spans Contracts, Api validation/mapping, Application command/result, Domain aggregate creation, and Infrastructure persistence. Adding the fields consistently across that path preserves contract clarity and avoids hidden persistence-only metadata.

**Alternatives considered**:

- Domain/persistence-only fields: rejected because callers need to save and confirm the values.
- Response-only field with no request support: rejected because there is no TMDB integration or other workflow supplying the value.

## Decision: Add an EF Core migration for nullable columns

**Rationale**: Movies are already persisted through EF Core/PostgreSQL. Nullable columns preserve existing records and avoid data backfill requirements.

**Alternatives considered**:

- No migration: rejected because persisted movie records must retain the new metadata.
- New table for external metadata: rejected as over-modeled for one optional provider ID and one optional path string.

## Decision: No ADR required

**Rationale**: This feature extends an existing entity and accepted persistence approach. It does not introduce a new provider, integration model, persistence technology, architecture boundary, or durable infrastructure decision.

**Alternatives considered**:

- Add ADR for TMDB metadata: rejected because the plan stores caller-supplied metadata only and does not establish TMDB as an integration provider.
