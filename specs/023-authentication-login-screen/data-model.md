# Data Model: Authentication Login Screen

This feature introduces no new persisted data model. The following shapes document existing API and frontend session data used by the login flow.

## Authenticate Request

**Purpose**: Credentials submitted by the login form.

**Fields**:
- `email`: user email address, required, valid email format, maximum 320 characters in the frontend form.
- `password`: user password, required.

**Validation**:
- Empty email must block submission and show field-level feedback.
- Malformed email must block submission and show field-level feedback.
- Empty password must block submission and show field-level feedback.
- Backend `400 ValidationProblemDetails` maps server validation errors back to field-level messages.

## Authenticate Response

**Purpose**: Successful authentication result returned before current-user lookup.

**Fields**:
- `userId`: authenticated user identifier.
- `email`: authenticated user email address.
- `accessToken`: bearer access token.
- `accessTokenExpiresAtUtc`: UTC expiration timestamp for the access token.

**Rules**:
- The access token is used only to request current-user context.
- The access token must not be persisted to `localStorage` or `sessionStorage`.

## Current User Response

**Purpose**: Trusted current-user context loaded after successful authentication.

**Fields**:
- `userId`: authenticated user identifier.
- `email`: authenticated user email address.
- `name`: display name.
- `roles`: role names returned by the backend.
- `mustChangePasswordOnFirstLogin`: first-login password-change indicator.

## Auth Session

**Purpose**: Frontend-only in-memory authenticated session.

**Fields**:
- `accessToken`: bearer token retained in memory only.
- `accessTokenExpiresAtUtc`: token expiration timestamp.
- `currentUser`: current-user context.

**State Transitions**:
- `Unauthenticated` -> `Authenticating`: user submits valid local form.
- `Authenticating` -> `Authenticated`: authenticate succeeds and current-user lookup succeeds.
- `Authenticating` -> `Unauthenticated`: authenticate fails, current-user lookup fails, backend unavailable, or validation fails.
- `Authenticated` -> `Unauthenticated`: browser refresh clears memory, or future explicit session-clear behavior is introduced.

**Security Constraints**:
- No bearer token persistence in browser storage.
- No partial trusted session after authenticate succeeds but current-user lookup fails.
