# Contract: Authentication API Consumption

The login screen consumes existing backend auth endpoints through same-origin `/api` paths.

## `POST /api/auth/authenticate`

**Request**:

```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Success Response**: `200 OK`

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "email": "user@example.com",
  "accessToken": "<jwt>",
  "accessTokenExpiresAtUtc": "2026-05-03T02:00:00Z"
}
```

**Validation Failure**: `400 Bad Request`

- Response body is `ValidationProblemDetails`.
- Frontend maps `Email` and `Password` validation entries to field-level feedback.

**Authentication Failure**: `401 Unauthorized`

- Response body is `ProblemDetails`.
- Frontend presents generic failure messaging and does not disclose account existence or status.

## `GET /api/auth/me`

**Request Headers**:

```text
Authorization: Bearer <accessToken>
```

**Success Response**: `200 OK`

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "email": "user@example.com",
  "name": "Example User",
  "roles": ["User"],
  "mustChangePasswordOnFirstLogin": false
}
```

**Authentication Failure**: `401 Unauthorized`

- Missing, malformed, expired, incorrectly signed, missing-user, inactive-user, and removed-user tokens are treated as unauthorized.
- Frontend must not create a trusted session if current-user lookup fails.

## Frontend Consumption Rules

- Login flow calls authenticate first, then current-user lookup.
- UI components must not own raw `HttpClient` calls.
- Bearer token is kept in memory only.
- Local Angular development uses the existing `/api` proxy behavior.
