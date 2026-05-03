# Data Model: Backend Authentication With JWT And Current User Context

## User

Domain aggregate representing a local application user.

Fields:

- `Id`: `Guid`, required, stable token subject.
- `Email`: string, required, stored in canonical display form.
- `NormalizedEmail`: string, required, uppercase/trimmed lookup key, unique.
- `Name`: string, required, human-readable display name.
- `PasswordHash`: string, required, opaque framework password hash.
- `IsActive`: bool, required, defaults to true for seeded active users.
- `RemovedAtUtc`: nullable `DateTimeOffset`, soft-delete marker; any value means the user is removed.
- `MustChangePasswordOnFirstLogin`: bool, required.
- `Roles`: collection of persisted role names; initial valid values are `Admin` and `User`.
- `CreatedAtUtc`: `DateTimeOffset`, required.
- `UpdatedAtUtc`: nullable `DateTimeOffset`.

Validation and invariants:

- Email must be syntactically valid enough for local identity lookup and must normalize consistently before persistence.
- `NormalizedEmail` must be unique in persistence.
- `PasswordHash` must never contain plaintext and must never be returned through API contracts.
- `Roles` must contain only recognized role values and must not be empty for active users.
- Removed users are not active for authentication or current-user lookup, even if `IsActive` remains true.

State transitions:

- Create optional active admin seed user when admin seed credentials are supplied from non-versioned configuration.
- Authenticate active, non-removed user after successful password verification.
- Deactivate user by setting `IsActive = false`.
- Soft-delete user by setting `RemovedAtUtc`.
- Change first-login requirement by setting `MustChangePasswordOnFirstLogin`.

## UserRole

Persisted role assignment for a user. It may be implemented as an owned collection, join table, or equivalent EF Core mapping that preserves the domain model and supports JWT role claims.

Fields:

- `UserId`: `Guid`, required.
- `Role`: string, required; allowed values are `Admin` and `User`.

Validation and invariants:

- Role names are case-stable and emitted as standard role claims.
- Unknown roles are rejected before persistence.

## Authentication Request

Public API input for `POST /api/auth/authenticate`.

Fields:

- `Email`: string, required.
- `Password`: string, required.

Validation:

- Missing, empty, whitespace-only, or malformed request fields return `400 Bad Request` with `ValidationProblemDetails`.
- Validation responses must not echo raw password values.

## Authentication Result

Application result for successful authentication.

Fields:

- `UserId`: `Guid`.
- `Email`: string.
- `AccessToken`: string, JWT bearer token.
- `AccessTokenExpiresAtUtc`: `DateTimeOffset`.

Validation:

- Returned only for active, non-removed users with a verified password.
- Token includes subject and persisted role claims.

## Current User Context

Application result for `GET /api/auth/me`.

Fields:

- `UserId`: `Guid`.
- `Email`: string.
- `Name`: string.
- `Roles`: string collection.
- `MustChangePasswordOnFirstLogin`: bool.

Validation:

- Requires an authenticated bearer principal with a valid subject.
- Subject must map to an existing active, non-removed user.
- Missing, invalid, expired, incorrectly signed, inactive-user, removed-user, or missing-user contexts return `401 Unauthorized`.

## Jwt Settings

Runtime-only configuration model.

Fields:

- `Issuer`: from `Jwt:Issuer`, required.
- `Audience`: from `Jwt:Audience`, required.
- `SigningKey`: from `Jwt:SigningKey`, required secret material.
- `AccessTokenLifetimeMinutes`: from `Jwt:AccessTokenLifetimeMinutes`, required, defaults to 60 when not otherwise supplied by safe configuration.

Validation:

- Missing issuer, audience, signing key, or invalid lifetime fails at startup.
- Signing key is never committed, logged, returned, or documented with a real value.
