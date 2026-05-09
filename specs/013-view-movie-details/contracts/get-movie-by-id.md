# Contract: GET /api/movies/{id}

## Endpoint
- Method: `GET`
- Route: `/api/movies/{id}`
- Auth: not required

## Path parameters
- `id` (required): movie identifier as GUID string.

## 200 OK (`application/json`)

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "title": "Central do Brasil",
  "originalTitle": "Central Station",
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR",
  "genres": ["Drama"],
  "director": "Walter Salles",
  "synopsis": "A retired teacher and a young boy travel through Brazil in search of his father.",
  "durationMinutes": 110,
  "ageRating": "12",
  "externalId": 666,
  "posterUrl": "/p/example-card.jpg",
  "createdAt": "2026-05-04T12:00:00+00:00"
}
```

## 400 Bad Request (`application/problem+json`)
- Trigger: `{id}` is not a valid GUID.
- Fields: `type`, `title`, `status`, `detail`, `instance`.

## 404 Not Found (`application/problem+json`)
- Trigger: valid GUID format with no matching movie.
- Fields: `type`, `title`, `status`, `detail`, `instance`.
