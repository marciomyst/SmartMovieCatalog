# Data Model: Movie External Identifier And Card Image

## Entity: Movie

Represents a movie catalog record.

### Existing Fields

- `id`: server-generated movie identity.
- `title`: required, non-blank after trimming.
- `originalTitle`: optional, blank normalized to absent.
- `releaseYear`: required, between 1888 and the next calendar year.
- `countryCode`: required, exactly two letters after trimming, normalized to uppercase.
- `originalLanguage`: required, non-blank after trimming.
- `genres`: optional collection of non-blank normalized genre names.
- `director`: optional, blank normalized to absent.
- `synopsis`: optional, blank normalized to absent.
- `durationMinutes`: optional, positive when supplied.
- `ageRating`: optional, blank normalized to absent.
- `createdAtUtc`: required creation timestamp.

### New Fields

- `externalId`: optional integer TMDB movie identifier.
- `image`: optional string relative card image path.

### Validation Rules

- `externalId` may be omitted.
- `externalId` must be positive when supplied.
- `image` may be omitted.
- `image` must be trimmed when supplied.
- `image` must be treated as absent when the supplied value is empty or whitespace-only.
- `image` must be a relative path when supplied.
- `image` must reject absolute URLs and other non-relative path values.
- `image` stores a path string only; it must not contain binary image content.
- `externalId` and `image` are independent optional fields. Either one may be supplied without the other.
- `externalId` is not unique in this feature.

### Persistence Notes

- Add nullable storage for `externalId` on persisted movies.
- Add nullable storage for relative `image` paths on persisted movies.
- Existing movie rows require no backfill.
- Existing movie genres remain unchanged.

### Public Payload Notes

- Movie creation input accepts optional `externalId` and `image`.
- Movie creation output returns `externalId` and `image`.
- Omitted `externalId` and omitted/blank `image` are represented as absent values in the saved movie representation.
- Non-relative `image` values are rejected before a movie is saved.

### Out Of Scope

- TMDB API calls.
- TMDB provider credentials.
- Provider provenance or multi-provider external identity modeling.
- TMDB-based uniqueness or duplicate detection.
- Image upload, binary storage, download, caching, resizing, or CDN behavior.
- Absolute external image URLs.
- Frontend rendering changes.
