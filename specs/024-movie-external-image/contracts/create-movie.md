# Contract: Create Movie With Optional TMDB Metadata

## Endpoint

`POST /api/movies`

Authentication remains unchanged: this movie creation slice does not require authentication.

## Request Body

```json
{
  "title": "Central do Brasil",
  "originalTitle": "Central do Brasil",
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR",
  "genres": ["Drama"],
  "director": "Walter Salles",
  "synopsis": "A retired teacher and a young boy travel through Brazil in search of his father.",
  "durationMinutes": 110,
  "ageRating": "12",
  "externalId": 666,
  "image": "/p/example-card.jpg"
}
```

## Required Fields

- `title`: non-blank after trimming.
- `releaseYear`: between 1888 and the next calendar year.
- `countryCode`: exactly two letters after trimming; normalized to uppercase in the response.
- `originalLanguage`: non-blank after trimming.

## Optional Fields

- `originalTitle`
- `genres`
- `director`
- `synopsis`
- `durationMinutes`: positive when supplied.
- `ageRating`
- `externalId`: positive integer when supplied.
- `image`: relative card image path string; blank values are normalized to absent.

## Success Response

Status: `201 Created`  
Header: `Location: /api/movies/{id}`

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "title": "Central do Brasil",
  "originalTitle": "Central do Brasil",
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR",
  "genres": ["Drama"],
  "director": "Walter Salles",
  "synopsis": "A retired teacher and a young boy travel through Brazil in search of his father.",
  "durationMinutes": 110,
  "ageRating": "12",
  "externalId": 666,
  "image": "/p/example-card.jpg"
}
```

## Success Response When Optional Fields Are Omitted

Status: `201 Created`

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "title": "Central do Brasil",
  "originalTitle": null,
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR",
  "genres": [],
  "director": null,
  "synopsis": null,
  "durationMinutes": null,
  "ageRating": null,
  "externalId": null,
  "image": null
}
```

## Validation Failure

Invalid request shape and field-validation failures return `400 Bad Request` as `ValidationProblemDetails`.

Additional validation for this feature:

- `externalId` must be greater than zero when supplied.
- `image` is optional; a blank value is accepted only if normalized to absent before persistence/response.
- `image` must be a relative path when supplied. Absolute URLs and other non-relative values return `400 Bad Request` as `ValidationProblemDetails`.

## Non-Contract Behavior

- The endpoint does not call TMDB.
- The endpoint does not download or upload images.
- The endpoint does not accept absolute external image URLs.
- The endpoint does not require `externalId` uniqueness.
- The endpoint does not require `image` when `externalId` is supplied.
