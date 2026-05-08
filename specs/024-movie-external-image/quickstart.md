# Quickstart: Movie External Identifier And Card Image

## Prerequisites

- .NET SDK compatible with the repository.
- Existing local configuration for backend builds.
- PostgreSQL configuration only if running the API against the real development database.

## Implementation Checklist

1. Add optional `ExternalId` and `Image` properties to the `Movie` aggregate.
2. Extend `Movie.Create` to accept both values and enforce:
   - `ExternalId` is positive when supplied.
   - `Image` is trimmed, blank values become absent, and non-relative paths are rejected.
3. Extend `CreateMovieCommand`, `CreatedMovie`, and `CreateMovieHandler` to carry both values.
4. Extend `CreateMovieRequest` and `MovieResponse` to expose both optional fields.
5. Extend `CreateMovieEndpoint` mapping between request, command, created result, and response.
6. Extend `CreateMovieRequestValidator` to reject non-positive `ExternalId` and non-relative `Image` values when supplied.
7. Extend `MovieConfiguration` with nullable persistence mapping for both fields.
8. Add an EF Core migration that adds nullable `ExternalId` and `Image` columns to `Movies`.
9. Update focused backend tests:
   - Domain validation and normalization.
   - Application handler persistence/result mapping.
   - API validation for invalid `ExternalId`.
   - API contract response shape for new optional fields.
10. Update `docs/API.md` and `docs/DOMAIN.md`.

## Verification

Run the narrowest useful tests during implementation, then the solution build because this crosses backend layers and contracts.

```powershell
dotnet test backend/tests/SmartMovieCatalog.Domain.Tests/SmartMovieCatalog.Domain.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Application.Tests/SmartMovieCatalog.Application.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Api.Tests/SmartMovieCatalog.Api.Tests.csproj
dotnet build SmartMovieCatalog.slnx
```

## Manual API Check

After running the API locally, create a movie with the new optional metadata:

```http
POST /api/movies
Content-Type: application/json

{
  "title": "Central do Brasil",
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR",
  "externalId": 666,
  "image": "/p/example-card.jpg"
}
```

Expected result:

- Status `201 Created`.
- `Location` points to `/api/movies/{id}`.
- Response body includes `externalId: 666`.
- Response body includes `image: "/p/example-card.jpg"`.

Also verify that the existing minimal payload still succeeds:

```http
POST /api/movies
Content-Type: application/json

{
  "title": "Central do Brasil",
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR"
}
```

Expected result:

- Status `201 Created`.
- Response body includes `externalId: null`.
- Response body includes `image: null`.
