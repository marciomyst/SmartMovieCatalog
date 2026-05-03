# Quickstart: Authentication Login Screen Validation

## 1. Review Scope

Confirm `specs/023-authentication-login-screen/spec.md` and `specs/023-authentication-login-screen/plan.md` describe the current login screen and do not introduce refresh tokens, persistent sessions, logout, registration, password recovery, route guards, or external identity providers.

## 2. Verify Documentation Alignment

Review these files for consistent auth behavior:

- `docs/API.md`
- `docs/FRONTEND.md`
- `docs/SECURITY.md`
- `docs/TESTING.md`
- `docs/adr/0008-frontend-auth-session.md`

Expected result:

- `POST /api/auth/authenticate` and `GET /api/auth/me` are the documented API dependencies.
- `400 ValidationProblemDetails` maps to field-level frontend feedback.
- `401 ProblemDetails` remains generic.
- Bearer tokens are not persisted to `localStorage` or `sessionStorage`.

## 3. Run Frontend Tests

From `frontend`:

```powershell
npm test -- --watch=false
```

Expected result:

- Auth API tests cover authenticate and current-user bearer header behavior.
- Login-page tests cover rendering, validation, password visibility, successful login flow, backend validation errors, and generic unauthorized errors.
- Session-store tests cover in-memory-only session storage.

## 4. Run Frontend Build

From `frontend`:

```powershell
npm run build
```

Expected result:

- Angular production build succeeds.

## 5. Optional Manual Browser Check

Run the app using the repository local development workflow. On the login screen:

1. Submit empty credentials and verify field-level feedback.
2. Submit malformed email and verify no authenticate request is made.
3. Submit invalid credentials and verify the failure message is generic.
4. Submit valid credentials and verify current-user context loads.
5. Refresh the browser and verify the frontend session is cleared.
6. Inspect browser storage and verify no bearer token exists in `localStorage` or `sessionStorage`.
