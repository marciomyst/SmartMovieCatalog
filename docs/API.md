# API

## Current State
The backend is an ASP.NET Core Web API project in `backend/src/SmartMovieCatalog.Api`.
HTTP endpoints are implemented as Minimal API feature slices under `backend/src/SmartMovieCatalog.Api/Features`.
Endpoint handlers dispatch backend use cases through Wolverine `IMessageBus` and keep HTTP-only concerns in the Api layer.

Current backend endpoints include:

- `POST /api/auth/authenticate`
- `GET /api/auth/me`

The WeatherForecast scaffold endpoint has been removed.

## Authentication Endpoints

### `POST /api/auth/authenticate`

Accepts:

```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

Returns `200 OK` with:

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "email": "user@example.com",
  "accessToken": "<jwt>",
  "accessTokenExpiresAtUtc": "2026-05-03T02:00:00Z"
}
```

Invalid request shape returns `400 Bad Request` as `ValidationProblemDetails`.
Invalid credentials, nonexistent users, inactive users, and removed users return a generic `401 Unauthorized` as `ProblemDetails`.

### `GET /api/auth/me`

Requires `Authorization: Bearer <jwt>`.

Returns `200 OK` with:

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "email": "user@example.com",
  "name": "Example User",
  "roles": ["User"],
  "mustChangePasswordOnFirstLogin": false
}
```

Missing, malformed, expired, incorrectly signed, missing-user, inactive-user, and removed-user tokens return `401 Unauthorized` as `ProblemDetails`.

## Frontend Consumption
- The Angular login module calls `POST /api/auth/authenticate` with `email` and `password`.
- On successful authentication, it calls `GET /api/auth/me` using the returned bearer token.
- The frontend relies on same-origin `/api` paths. During local `ng serve`, `frontend/src/proxy.conf.js` forwards `/api` to the backend.
- The frontend maps `400 ValidationProblemDetails` to field-level validation and keeps `401 ProblemDetails` generic to avoid account enumeration.

## API Design Rules
- Keep endpoint contracts explicit and stable.
- Use request and response DTOs for public API contracts when behavior grows beyond scaffold examples.
- Return consistent error responses.
- Validate request input at the API boundary with feature-local FluentValidation validators.
- Keep business rules out of HTTP endpoints; endpoints should handle routing, binding, validation, authorization metadata, status codes, and response shaping.
- Group API endpoints by feature under `SmartMovieCatalog.Api/Features/<Feature>/<UseCase>` when adding product endpoints.
- Dispatch Application commands and queries through Wolverine instead of invoking use case services directly from HTTP endpoints.
- Avoid leaking persistence models, configuration objects, or AI provider payloads directly through HTTP responses.

## Routing
- Prefer clear resource-oriented routes.
- Use plural resource names when modeling collections, for example `/api/movies`.
- Avoid action-style routes unless the operation is not naturally resource-based.

## Status Codes
- `200 OK` for successful reads and updates with response bodies.
- `201 Created` for successful creates with a route to the created resource.
- `204 No Content` for successful deletes or updates without response bodies.
- `400 Bad Request` for syntactically invalid input.
- `401 Unauthorized` when authentication is required and missing or invalid.
- `403 Forbidden` when the caller is authenticated but not allowed.
- `404 Not Found` when the resource does not exist or must not be disclosed.
- `409 Conflict` for uniqueness or state conflicts.
- `422 Unprocessable Entity` for semantically invalid input when validation is detailed.

## OpenAPI
The backend references `Microsoft.AspNetCore.OpenApi`. Keep OpenAPI metadata accurate when adding endpoints.

## Versioning
No API versioning strategy is implemented yet. Do not introduce versioned routes or packages until the API has external consumers or compatibility requirements.
