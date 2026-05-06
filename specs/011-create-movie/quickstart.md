# Quickstart: Create Movie

## Prerequisites

- .NET SDK compatible with `net10.0`.
- PostgreSQL configuration available through `ConnectionStrings:DefaultConnection` or local user-secrets/environment variables.
- Existing backend JWT configuration remains required for API startup even though `POST /api/movies` does not require authentication.

## Implementation Verification

Run the full backend solution build after implementation:

```powershell
dotnet build SmartMovieCatalog.slnx
```

Run focused backend tests:

```powershell
dotnet test backend/tests/SmartMovieCatalog.Domain.Tests/SmartMovieCatalog.Domain.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Application.Tests/SmartMovieCatalog.Application.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Api.Tests/SmartMovieCatalog.Api.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Infrastructure.Tests/SmartMovieCatalog.Infrastructure.Tests.csproj
```

## Manual API Check

Start the API through the repository's normal local workflow, then submit:

```http
POST /api/movies HTTP/1.1
Content-Type: application/json

{
  "title": "Central do Brasil",
  "originalTitle": "Central do Brasil",
  "releaseYear": 1998,
  "countryCode": "br",
  "originalLanguage": "pt-BR",
  "genres": ["Drama"],
  "director": "Walter Salles",
  "synopsis": "A retired teacher and a young boy travel through Brazil in search of his father.",
  "durationMinutes": 110,
  "ageRating": "12"
}
```

Expected:

- Status: `201 Created`
- Header: `Location: /api/movies/{id}`
- Body includes a GUID string `id`
- Body has `countryCode` normalized to `BR`
- No `Authorization` header is required

## Validation Checks

Submit invalid payloads and expect `400 Bad Request` as `ValidationProblemDetails`:

- Missing body.
- Blank `title`.
- `releaseYear` less than `1888`.
- `releaseYear` greater than next calendar year.
- `countryCode` not exactly two letters.
- Blank `originalLanguage`.
- Blank genre value when `genres` is supplied.
- `durationMinutes` less than or equal to zero.

## Out Of Scope Checks

Implementation should not add:

- Authentication or authorization requirements for `POST /api/movies`.
- `GET /api/movies/{id}`.
- Frontend movie creation UI.
- AI, Gemini, SignalR, semantic search, RAG, TMDb, bulk import, duplicate detection, Wolverine transports, background workers, or new persistence providers.
