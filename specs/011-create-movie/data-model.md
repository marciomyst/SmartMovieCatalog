# Data Model: Create Movie

## Movie

**Purpose**: Represents a global movie catalog record created from manually supplied, unverified metadata.

**Identity**:

- `id`: server-generated GUID, exposed by API as a JSON string.
- Uniqueness is by `id` only in this slice.
- Duplicate title/year/country metadata is allowed.

**Fields**:

| Field | Type | Required | Validation / Normalization |
|-------|------|----------|----------------------------|
| `id` | GUID | Yes | Server-generated only |
| `title` | string | Yes | Trim; non-blank |
| `originalTitle` | string | No | Trim; null/empty when omitted |
| `releaseYear` | integer | Yes | `1888` through next calendar year |
| `countryCode` | string | Yes | Trim; exactly two letters; uppercase-normalized |
| `originalLanguage` | string | Yes | Trim; non-blank language tag string |
| `genres` | list of `MovieGenre` | No | Each genre trimmed and non-blank; blank entries rejected |
| `director` | string | No | Trim; null/empty when omitted |
| `synopsis` | string | No | Trim; null/empty when omitted |
| `durationMinutes` | integer | No | Positive when supplied |
| `ageRating` | string | No | Trim; null/empty when omitted |
| `createdAtUtc` | datetime offset | Yes | Set server-side if current persistence conventions support it |

**Relationships**:

- `Movie` owns zero or more `MovieGenre` values for persistence.
- No user owner relationship is added because authentication/authorization is out of scope.
- No AI analysis, poster, TMDb, catalog collection, recommendation, or search relationship is added.

**Lifecycle**:

- Initial state: created and persisted.
- Update, delete, publish, moderation, ownership, and AI-analysis lifecycle states are out of scope.

**Trust / Provenance**:

- Metadata source is manually supplied by API caller.
- Metadata trust level is unverified.
- No AI provenance or external provider provenance is persisted in this slice.

## MovieGenre

**Purpose**: Normalized genre label owned by a movie.

**Fields**:

| Field | Type | Required | Validation / Normalization |
|-------|------|----------|----------------------------|
| `name` | string | Yes | Trim; non-blank |

**Persistence Notes**:

- Persist as a child collection/table associated with `Movie`.
- Preserve API shape as `genres: string[]`.
- Do not expose persistence table keys or EF Core implementation details through contracts.

## CreateMovieRequest

**Purpose**: Public API input DTO for `POST /api/movies`.

**Fields**:

| Field | Type | Required |
|-------|------|----------|
| `title` | string | Yes |
| `originalTitle` | string | No |
| `releaseYear` | integer | Yes |
| `countryCode` | string | Yes |
| `originalLanguage` | string | Yes |
| `genres` | string array | No |
| `director` | string | No |
| `synopsis` | string | No |
| `durationMinutes` | integer | No |
| `ageRating` | string | No |

## MovieResponse

**Purpose**: Public API output DTO for successful movie creation.

**Fields**:

| Field | Type | Required |
|-------|------|----------|
| `id` | string GUID | Yes |
| `title` | string | Yes |
| `originalTitle` | string or null | No |
| `releaseYear` | integer | Yes |
| `countryCode` | string | Yes |
| `originalLanguage` | string | Yes |
| `genres` | string array | Yes, can be empty |
| `director` | string or null | No |
| `synopsis` | string or null | No |
| `durationMinutes` | integer or null | No |
| `ageRating` | string or null | No |

## Validation Failure Model

Validation failures use ASP.NET Core `ValidationProblemDetails`.

Expected validation failures:

- Missing request body.
- Blank `title`.
- Missing or out-of-range `releaseYear`.
- Blank or non-two-letter `countryCode`.
- Blank `originalLanguage`.
- Blank genre entries when `genres` is supplied.
- Non-positive `durationMinutes` when supplied.
