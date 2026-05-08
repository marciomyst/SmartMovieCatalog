# Data Model: Normalize Movie Genres

## Movie

- Existing catalog movie aggregate.
- Has zero or more `MovieGenre` associations.
- Continues to expose genre names to current movie creation responses.

## Genre

| Field | Required | Rules |
|-------|----------|-------|
| `Id` | Yes | Stable Guid identity |
| `Name` | Yes | Trimmed, non-blank, max 100 characters |
| `NormalizedName` | Yes | Uppercase invariant trimmed name, unique |
| `ExternalId` | No | Positive TMDB genre id when supplied, unique when non-null |

## MovieGenre

| Field | Required | Rules |
|-------|----------|-------|
| `MovieId` | Yes | Existing movie id |
| `GenreId` | Yes | Existing genre id |

- Primary identity is the pair `MovieId` + `GenreId`.
- Does not store or own a genre display name.
- A movie cannot have duplicate associations to the same genre.

## Relationships

- One `Movie` has many `MovieGenre` associations.
- One `Genre` can be associated with many movies through `MovieGenre`.
- Movie creation resolves incoming genre names to existing or newly created `Genre` records before creating associations.
