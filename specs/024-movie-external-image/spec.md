# Feature Specification: Movie External Identifier And Card Image

**Feature Branch**: `[024-movie-external-image]`  
**Created**: 2026-05-07  
**Status**: Draft  
**Input**: User description: "Na entidade Movies, adicione um campo ExternalId (Integer) nao obrigatorio para poder salvar o id do filme no TMDB. Adicione tambem um campo string Image para salvar o path da imagem do card, sendo que ela tambem nao e obrigatoria."

## Clarifications

### Session 2026-05-07

- Q: What image reference format should `image` accept? -> A: Accept only relative image paths, for example `/p/example-card.jpg`.
- Q: How should invalid `image` values such as absolute URLs be handled? -> A: Reject values that are not relative paths.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Save TMDB Metadata With A Movie (Priority: P1)

As a catalog maintainer or importer, I can save the TMDB movie identifier and card image path with a movie record so that catalog entries can preserve external metadata needed by later catalog views and import workflows.

**Why this priority**: This is the requested technical adjustment and enables movie records to keep the external provider identifier and card image reference without requiring a larger TMDB integration.

**Independent Test**: Can be tested by creating or storing a movie with both optional metadata values and confirming the saved movie exposes the same values.

**Acceptance Scenarios**:

1. **Given** a valid movie payload with `externalId` and `image`, **When** the movie is saved, **Then** the saved movie includes the supplied TMDB identifier and card image path.
2. **Given** a valid movie payload without `externalId` and without `image`, **When** the movie is saved, **Then** the movie is saved successfully and both values remain absent.

---

### User Story 2 - Preserve Existing Movie Creation Behavior (Priority: P2)

As an existing caller, I can continue creating movies without supplying TMDB metadata so that the new fields do not break current create-movie behavior.

**Why this priority**: Both new fields are optional, so backward compatibility is part of the feature.

**Independent Test**: Can be tested by sending the existing valid movie creation shape and confirming it still succeeds without the new fields.

**Acceptance Scenarios**:

1. **Given** an existing valid movie creation payload that omits the new fields, **When** the movie is saved, **Then** the request remains valid and the response represents the created movie.
2. **Given** existing movies that were saved before this change, **When** they are read or mapped by the system, **Then** missing `externalId` and `image` values are treated as absent metadata, not invalid data.

### Edge Cases

- `externalId` is omitted.
- `externalId` is supplied as zero or a negative value.
- `image` is omitted.
- `image` is supplied as an empty or whitespace-only value.
- `image` is supplied as an absolute URL or other non-relative path value.
- `externalId` is supplied without `image`, or `image` is supplied without `externalId`.
- Existing movies have no values for either field.
- The same TMDB identifier is supplied for more than one movie before duplicate-detection rules exist.
- `image` contains only a relative provider card-image path; no binary image data, upload, or absolute external URL is included.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Movie records MUST support an optional `externalId` value for storing the TMDB movie identifier.
- **FR-002**: `externalId` MUST be absent when it is not supplied by the caller or source workflow.
- **FR-003**: `externalId` MUST be accepted as an integer value when supplied.
- **FR-004**: `externalId` MUST be positive when supplied because TMDB movie identifiers are positive numeric identifiers.
- **FR-005**: Movie records MUST support an optional `image` value for storing the relative card image path associated with the movie.
- **FR-006**: `image` MUST be absent when it is not supplied or when the supplied value is blank after trimming.
- **FR-007**: `image` MUST store a relative path string only; the feature MUST NOT store absolute external URLs or image binary content.
- **FR-008**: Movie creation input MUST reject `image` values that are not relative paths.
- **FR-009**: Movie creation input MUST allow callers to provide `externalId` and `image` without making either field required.
- **FR-010**: Movie creation output MUST include `externalId` and `image` so clients can confirm the saved values.
- **FR-011**: Existing valid movie creation input that omits both fields MUST continue to be accepted.
- **FR-012**: The system MUST NOT require `externalId` uniqueness in this feature because broader duplicate-detection and provider-identity rules are out of scope.
- **FR-013**: The feature MUST NOT add TMDB fetching, authentication, provider credentials, image downloading, image upload, or external-service calls.
- **FR-014**: Documentation of movie metadata and public movie payloads MUST be updated to include the new optional fields.

### Key Entities

- **Movie**: A catalog movie record. Existing required metadata remains unchanged. New optional metadata includes `externalId`, the TMDB movie identifier, and `image`, the relative card image path string.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% of currently valid movie creation payloads that omit `externalId` and `image` remain valid.
- **SC-002**: A movie saved with both new values returns both values in the saved movie representation.
- **SC-003**: A movie saved without either new value represents both values as absent metadata.
- **SC-004**: Invalid non-positive `externalId` values are rejected before a movie is saved.
- **SC-005**: Invalid non-relative `image` values are rejected before a movie is saved.
- **SC-006**: The feature is verifiable through focused coverage of movie rules, saved metadata, and public movie payloads.

## Assumptions

- `ExternalId` means the TMDB movie identifier for this feature. A broader provider-qualified external identity model is out of scope.
- TMDB movie identifiers are positive integers.
- `Image` means a relative card image path string, not an absolute URL, uploaded image content, or a full image storage feature.
- Empty or whitespace-only image values should be treated the same as an omitted image.
- Existing duplicate movie behavior remains unchanged; this feature does not introduce TMDB-based uniqueness or duplicate detection.
- No frontend UI changes are required for this technical entity adjustment unless later implementation work explicitly extends catalog card rendering.
