# Quickstart: Backend Authentication With JWT And Current User Context

## Prerequisites

- .NET 10 SDK.
- Docker available for the local PostgreSQL service, or another PostgreSQL instance configured through `ConnectionStrings:DefaultConnection`.
- JWT configuration supplied through user-secrets, environment variables, or another non-versioned provider.

## Local Configuration

From `backend/src/SmartMovieCatalog.Api`:

```powershell
dotnet user-secrets set "Jwt:Issuer" "SmartMovieCatalog"
dotnet user-secrets set "Jwt:Audience" "SmartMovieCatalog.Api"
dotnet user-secrets set "Jwt:SigningKey" "<local-development-signing-key>"
dotnet user-secrets set "Jwt:AccessTokenLifetimeMinutes" "60"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=smart_movie_catalog;Username=smartmovie;Password=<local-development-password>"
```

Use local-only values. Do not commit real signing keys, database credentials, user passwords, access tokens, or `.env` files.

## Development Database

Start PostgreSQL from the repository root:

```powershell
docker compose up -d postgres
```

Apply the authentication migration after implementation adds EF Core migrations:

```powershell
dotnet ef database update --project backend/src/SmartMovieCatalog.Infrastructure --startup-project backend/src/SmartMovieCatalog.Api
```

Non-test API startup applies EF Core migrations and can seed an optional admin user by supplying `AdminSeedUser:Email` and `AdminSeedUser:Password` from non-versioned configuration. Leave both values empty to disable admin seeding.

## Run The API

```powershell
dotnet run --project backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj
```

## Authenticate

```http
POST http://localhost:5048/api/auth/authenticate
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "<local-development-password>"
}
```

Expected success:

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "email": "user@example.com",
  "accessToken": "<jwt>",
  "accessTokenExpiresAtUtc": "2026-05-03T02:00:00Z"
}
```

Invalid input returns `400 Bad Request` with `ValidationProblemDetails`. Invalid credentials, nonexistent users, inactive users, and removed users return generic `401 Unauthorized` with `ProblemDetails`.

## Current User

```http
GET http://localhost:5048/api/auth/me
Authorization: Bearer <jwt>
Accept: application/json
```

Expected success:

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "email": "user@example.com",
  "name": "Example User",
  "roles": ["User"],
  "mustChangePasswordOnFirstLogin": false
}
```

Missing, invalid, expired, incorrectly signed, inactive-user, removed-user, or missing-user tokens return `401 Unauthorized`.

## Verification

Run the narrowest checks during implementation, then final full backend verification:

```powershell
dotnet build SmartMovieCatalog.slnx
dotnet test SmartMovieCatalog.slnx
```
