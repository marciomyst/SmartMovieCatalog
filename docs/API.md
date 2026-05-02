# API

## Current State
The backend is an ASP.NET Core Web API project in `backend/src/SmartMovieCatalog.Api`.

Current scaffold files include:

- `Program.cs`
- `Controllers/WeatherForecastController.cs`
- `WeatherForecast.cs`
- `SmartMovieCatalog.Api.http`

The current API surface is scaffold-level and should not be treated as final product API design.

## API Design Rules
- Keep endpoint contracts explicit and stable.
- Use request and response DTOs for public API contracts when behavior grows beyond scaffold examples.
- Return consistent error responses.
- Validate input at the boundary.
- Keep business rules out of controllers when they become non-trivial.
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
