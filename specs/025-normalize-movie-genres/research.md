# Research: Normalize Movie Genres

## Decision: Genre Identity

**Decision**: `Genre` uses a stable Guid identity, a required display `Name`, a required `NormalizedName`, and an optional positive `ExternalId` for the TMDB genre identifier.

**Rationale**: A Guid identity matches the repository's current aggregate identity approach and keeps movie associations stable if a genre display name changes later. `NormalizedName` supports case-insensitive reuse while preserving a readable display name. `ExternalId` is optional because manual movie creation only supplies genre names today.

**Alternatives considered**: Using the name as the primary key was simpler but would make rename and display-name preservation risky. Exposing provider-qualified external identity was deferred because only TMDB was requested.

## Decision: Public Movie Contract Compatibility

**Decision**: Keep movie creation input and output as `genres: string[]`.

**Rationale**: Current callers submit and receive genre names. The normalization is an internal persistence/domain improvement and should not force clients to know genre identifiers.

**Alternatives considered**: Returning genre objects or accepting `{ name, externalId }` entries would expose a broader genre-management contract before the product has that workflow.

## Decision: Genre Resolution Boundary

**Decision**: Resolve genre names to reusable `Genre` entities in the Application layer through a persistence abstraction implemented by Infrastructure.

**Rationale**: The API stays focused on HTTP concerns, the Domain receives valid `Genre` entities for movie associations, and Infrastructure owns database lookup and persistence.

**Alternatives considered**: Resolving genres in the endpoint would violate API layering. Creating genres inside `Movie` from strings would hide persistence lookup and make reuse impossible.

## Decision: Migration Shape

**Decision**: Create `Genres`, backfill distinct existing movie genre names into it, and rebuild `MovieGenres` as `MovieId`/`GenreId`.

**Rationale**: Existing persisted genre names must remain visible after the schema change, and duplicate names across movies must consolidate into reusable genre rows.

**Alternatives considered**: Dropping and recreating `MovieGenres` without backfill would lose catalog data. Keeping `Name` on `MovieGenres` would fail the requested normalized shape.
