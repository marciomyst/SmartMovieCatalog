# Contract: Catalog Page UI And API Consumption

## Route Contract

```text
GET /catalog
GET /catalog?query={query}&page={page}&pageSize={pageSize}
```

Rules:

- `/catalog` is publicly reachable for V1.
- No frontend authentication guard is required for this route.
- `query` is optional and represents basic title search.
- `page` is optional, defaults to `1`, and must be normalized to a positive integer before API calls.
- `pageSize` is optional, defaults to `12`, and must be normalized to a positive integer before API calls.
- The UI must not expose a page-size selector in V1.

## Frontend API Service Contract

`MoviesApi` exposes movie listing through a typed method equivalent to:

```ts
listMovies(query: MovieListQuery): Observable<PagedMovieSummaryResponse>
```

Request mapping:

```http
GET /api/movies?page=1&pageSize=12
GET /api/movies?query=central&page=1&pageSize=12
```

Rules:

- Components must call `MoviesApi`, not `HttpClient` directly.
- Blank search query is omitted from the API request.
- URL query parameters are normalized before creating the API request.

## Expected API Response

```json
{
  "items": [
    {
      "id": "movie-id",
      "title": "Central do Brasil",
      "releaseYear": 1998,
      "countryCode": "BR",
      "genres": ["Drama"],
      "director": "Walter Salles",
      "posterUrl": null,
      "createdAt": "2026-05-02T00:00:00Z"
    }
  ],
  "page": 1,
  "pageSize": 12,
  "totalCount": 47,
  "totalPages": 4,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

## Error Contract

Invalid API query or pagination parameters from the backend use `application/problem+json`:

```json
{
  "type": "https://smartmoviecatalog.dev/problems/invalid-request",
  "title": "Invalid request.",
  "status": 400,
  "detail": "The request could not be processed because it is malformed or contains invalid parameters.",
  "instance": "/api/movies",
  "errors": {
    "page": ["Page must be greater than or equal to 1."]
  }
}
```

UI rule:

- Display a generic catalog error state.
- Do not expose internal backend exception details.

## Navigation Contract

Catalog item links target the movie details route from issue #13:

```text
/movies/{id}
```

If issue #13 establishes a different route before implementation, update this contract and the catalog tests to match.
