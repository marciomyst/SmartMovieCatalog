# Data Model: View Movie Details

## MovieDetails (read projection)

Purpose: response model for `GET /api/movies/{id}` using existing movie metadata.

### Fields
- `id` (`string`, GUID serialized)
- `title` (`string`, required)
- `originalTitle` (`string | null`)
- `releaseYear` (`number`, required)
- `countryCode` (`string`, required, 2-letter uppercase)
- `originalLanguage` (`string`, required)
- `genres` (`string[]`, required, can be empty)
- `director` (`string | null`)
- `synopsis` (`string | null`)
- `durationMinutes` (`number | null`)
- `ageRating` (`string | null`)
- `externalId` (`number | null`, positive when present)
- `posterUrl` (`string | null`, relative path currently stored by domain)
- `createdAt` (`string`, ISO-8601 timestamp)

### Validation and constraints
- Invalid route `id` format is rejected at API boundary with `400 application/problem+json`.
- Valid GUID with no matching movie returns `404 application/problem+json`.
- No new persisted fields are introduced (`cast` and `updatedAt` remain out of scope).

### Relationships
- Backed by existing `Movie` aggregate and related `MovieGenre -> Genre` associations.
- Genres are projected as public string names only.
