# Feature Specification: Normalize Movie Genres

**Feature Branch**: `[025-normalize-movie-genres]`  
**Created**: 2026-05-07  
**Status**: Draft  
**Input**: User description: "vamos fazer mais uma spec, precisamos criar uma tabela Genres para salvar o nome dos generos e ajustar a MovieGenres para ser apenas uma associativa NxM com Movies"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Reuse Catalog Genres Across Movies (Priority: P1)

As a catalog maintainer or importer, I can save movie genres as reusable catalog entries so that the same genre name is represented once and can be shared by many movies.

**Why this priority**: This is the core data correction requested by the feature and prevents duplicated genre names from being stored as movie-specific values.

**Independent Test**: Can be tested by saving two movies with the same genre name and confirming both movies reference the same catalog genre while each movie still exposes the expected genre name.

**Acceptance Scenarios**:

1. **Given** no catalog genre named `Drama` exists, **When** a movie is saved with genre `Drama`, **Then** a reusable genre named `Drama` exists and the movie is associated with it.
2. **Given** a catalog genre named `Drama` already exists, **When** another movie is saved with genre `Drama`, **Then** the existing genre is reused instead of creating a duplicate genre entry.
3. **Given** two movies share the same genre, **When** either movie is read or represented by the system, **Then** each movie still lists that genre by name.

---

### User Story 2 - Keep Movie Genre Associations Accurate (Priority: P2)

As a catalog maintainer, I can rely on movie-to-genre associations to represent only which genres belong to each movie so that relationship data is not mixed with genre name data.

**Why this priority**: The feature explicitly requires `MovieGenres` to become only the many-to-many association between movies and genres.

**Independent Test**: Can be tested by saving a movie with multiple genres and confirming each association links the movie to a reusable genre without duplicating genre name data in the association.

**Acceptance Scenarios**:

1. **Given** a movie is saved with `Drama` and `Road Movie`, **When** the movie's genres are inspected through supported behavior, **Then** the movie is associated with both catalog genres.
2. **Given** a movie is saved with the same genre name more than once in one request or source payload, **When** the movie is saved, **Then** the movie has only one association to that genre.
3. **Given** a genre is associated with multiple movies, **When** one movie is associated with another genre, **Then** existing associations for other movies remain unchanged.

---

### User Story 3 - Preserve Existing Movie Genre Data (Priority: P3)

As a maintainer, I can transition existing movie genre data to the new normalized structure so that current catalog records do not lose genre information.

**Why this priority**: The repository already persists movie genre names, so the schema correction must preserve existing catalog data.

**Independent Test**: Can be tested by starting with persisted movies that have genre names, applying the transition, and confirming each movie still exposes the same distinct genre names afterward.

**Acceptance Scenarios**:

1. **Given** existing movies have persisted genre names, **When** the genre structure is normalized, **Then** every distinct valid genre name exists as a catalog genre.
2. **Given** multiple existing movies contain the same genre name, **When** the genre structure is normalized, **Then** the shared genre name is represented once and associated to each relevant movie.
3. **Given** existing movies are represented after normalization, **When** their genre lists are read, **Then** users and clients see the same genre names they saw before the change.

### Edge Cases

- A movie is saved with no genres.
- A movie is saved with an empty genre collection.
- A movie is saved with duplicate genre names in the same payload.
- Genre names contain leading or trailing whitespace.
- Genre names differ only by letter casing.
- Existing movies share the same genre name.
- Existing movies contain multiple distinct genre names.
- The same genre is associated with many movies.
- A genre exists before any new movie uses it.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST support a reusable `Genre` catalog concept that stores each genre name independently from any single movie.
- **FR-002**: Each genre MUST have a non-blank name after trimming.
- **FR-003**: Genre names MUST be unique by normalized name so equivalent names do not create duplicate catalog genres.
- **FR-004**: Each genre MUST have a stable internal identifier.
- **FR-005**: Each genre MUST support an optional positive TMDB external identifier.
- **FR-006**: A supplied TMDB external identifier MUST be unique across genres.
- **FR-007**: The system MUST associate movies and genres through a many-to-many relationship.
- **FR-008**: Movie-to-genre association records MUST represent only the relationship between a movie and a genre, not the genre's display name.
- **FR-009**: Creating or saving a movie with genre names MUST create missing reusable genres and reuse existing matching genres.
- **FR-010**: A movie MUST NOT have duplicate associations to the same normalized genre.
- **FR-011**: Movies with no supplied genres MUST remain valid and MUST have no genre associations.
- **FR-012**: Existing movie creation behavior that accepts genre names MUST remain compatible for callers.
- **FR-013**: Existing movie representations that expose genre names MUST continue to expose genre names, not internal association details.
- **FR-014**: Existing persisted movie genre names MUST be preserved by creating reusable genres and associating each movie with the matching genres.
- **FR-015**: Existing duplicate genre names across movies MUST be consolidated into shared reusable genres by normalized name.
- **FR-016**: The feature MUST NOT expose genre identifiers or genre external identifiers through the movie creation public contract.
- **FR-017**: The feature MUST NOT introduce user-specific genre ownership, genre deletion workflows, genre management screens, external provider synchronization, or recommendation behavior.
- **FR-018**: Documentation of movie genre domain rules and persistence shape MUST be updated to describe reusable genres and many-to-many movie associations.

### Key Entities

- **Movie**: A catalog movie record that may be associated with zero or more genres.
- **Genre**: A reusable catalog genre with a stable internal identifier, required name, normalized identity used to avoid duplicate genre entries, and optional TMDB external identifier.
- **MovieGenre**: The association between one movie and one genre; it records membership only and does not own the genre name.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% of currently valid movie creation inputs that omit genres remain valid.
- **SC-002**: 100% of currently valid movie creation inputs with distinct valid genre names still save and return the expected genre names.
- **SC-003**: Saving two movies with the same normalized genre name results in one reusable genre and two movie associations.
- **SC-004**: Saving one movie with duplicate equivalent genre names results in one association for that genre.
- **SC-005**: Existing persisted movie genre names remain visible on their movies after the normalization transition.
- **SC-006**: The movie-to-genre association contains no genre display-name ownership after the feature is complete.

## Assumptions

- Genre normalization means trimming whitespace and matching names case-insensitively for uniqueness while preserving a readable display name.
- The public movie contract can continue accepting and returning genre names; exposing separate genre identifiers is out of scope for this feature.
- `Genre.ExternalId` represents the TMDB genre identifier, is optional, must be positive when supplied, and is not part of the public movie contract in this feature.
- Genre management endpoints, frontend genre administration, filtering, search, recommendations, and external provider synchronization are out of scope.
- Existing persisted genre names are trusted enough to migrate as catalog genres if they are valid under the current movie genre rules.
- No authorization or user ownership behavior is introduced by this feature.
