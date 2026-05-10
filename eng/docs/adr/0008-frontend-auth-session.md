# ADR 0008: Frontend Auth Session

## Status
Accepted

## Context
The Angular SPA now includes a login screen for the existing backend authentication endpoints.

The backend exposes local email/password authentication through `POST /api/auth/authenticate` and current-user lookup through `GET /api/auth/me`. The frontend needs a minimal session model that is secure enough for the current scope without introducing refresh tokens, registration, password recovery, route guards, or a global state library.

## Decision
Use a feature-local Angular authentication module under `frontend/src/app/auth`.

- Keep HTTP calls in `AuthApi`, not in UI components.
- Mirror the backend auth contracts with TypeScript interfaces in `auth.models.ts`.
- Call `POST /api/auth/authenticate` with `email` and `password`.
- After successful authentication, call `GET /api/auth/me` with `Authorization: Bearer <accessToken>`.
- Store the authenticated session in `AuthSessionStore` using Angular signals.
- Keep the bearer access token in memory only.
- Use same-origin `/api` paths. Local Angular development uses `frontend/src/proxy.conf.js` to proxy API calls to the backend.

## Consequences
- The login component remains presentation and orchestration code; it does not own raw HTTP details.
- Browser refresh clears the frontend session because the token is not persisted.
- This avoids local-storage token exposure for the current scope.
- Refresh tokens, persistent sessions, route guards, registration, and password recovery require separate feature work and security review.
