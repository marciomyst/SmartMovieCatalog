# GitHub Issue Context

## Source

- Repository: marciomyst/SmartMovieCatalog
- Issue: #23
- URL: https://github.com/marciomyst/SmartMovieCatalog/issues/23
- State: OPEN
- Created: 05/03/2026 19:37:28
- Updated: 05/03/2026 19:44:54
- Milestone: M1 — Core Movie Catalog

## Title

Authentication login screen

## Labels

- type:feature
- priority:high
- area:api
- area:frontend
- area:ui
- area:security
- v1

## Assignees

_No assignees_



## Issue Body

## Summary

Implement and document the Angular authentication login screen for Smart Movie Catalog.

<img width="1338" height="737" alt="Image" src="https://github.com/user-attachments/assets/1580050b-6599-47c0-b4bd-a2fc7736ca96" />

This issue covers the first frontend authentication surface: a dark cinematic login page that authenticates against the existing backend auth API and creates an in-memory frontend session.

## Goal

Allow users to authenticate from the Angular SPA using local email/password credentials and load their current user context before entering authenticated product flows.

## Scope

- Add the authentication login screen as the current SPA entry surface.
- Keep the UI aligned with `frontend/DESIGN.md`: dark mode, cinematic visual direction, responsive layout, and Material Symbols icons.
- Use an Angular reactive form with client-side validation for email and password.
- Consume `POST /api/auth/authenticate` through a typed frontend API service.
- After successful authentication, call `GET /api/auth/me` using the returned bearer token.
- Keep `HttpClient` calls isolated outside UI components.
- Store the authenticated session in memory only through an auth session store.
- Map backend `400 ValidationProblemDetails` to field-level frontend validation feedback.
- Keep backend `401 Unauthorized` responses generic in the UI to avoid account enumeration.
- Update documentation for frontend auth structure, security posture, API consumption, testing, and ADR coverage.

## API Usage

```http
POST /api/auth/authenticate
```

Request:

```json
{ "email": "user@example.com", "password": "Password123!" }
```

Successful login then calls:

```http
GET /api/auth/me
Authorization: Bearer <accessToken>
```

Local Angular development should use same-origin `/api` paths through `frontend/src/proxy.conf.js`.

## Acceptance Criteria

- Login screen renders as the SPA entry screen.
- The page is responsive on mobile and desktop.
- Email and password fields use Angular reactive form validation.
- Password visibility can be toggled with an accessible icon button.
- Submit calls `POST /api/auth/authenticate` with `email` and `password`.
- Successful authentication calls `GET /api/auth/me` with the bearer token.
- Successful authentication stores `accessToken`, `accessTokenExpiresAtUtc`, and current user data in memory.
- Bearer token is not persisted in `localStorage` or `sessionStorage`.
- `400 ValidationProblemDetails` displays field-level feedback.
- `401 Unauthorized` displays a generic authentication error.
- UI components do not call `HttpClient` directly.
- Frontend auth contracts stay aligned with `SmartMovieCatalog.Contracts.Auth`.
- Tests cover rendering, validation, password visibility, API calls, bearer header behavior, and unauthorized errors.
- Documentation reflects the implemented auth screen and in-memory session strategy.

## Technical Notes

- Frontend auth code should live under `frontend/src/app/auth`.
- Use typed API boundaries such as `AuthApi` and `auth.models.ts`.
- Use an in-memory session store rather than browser storage.
- Keep `/api` as the frontend request base so the same code works with Angular proxy locally and ASP.NET Core SPA hosting in runtime.
- Any persistent session or refresh-token behavior requires a separate security decision.

## Out of Scope

- Refresh tokens.
- Persistent browser sessions.
- Route guards and authenticated app shell navigation.
- Logout flow.
- Registration.
- Password recovery.
- External identity providers.
- Role-specific authorization UI.


## Comments

_No comments_

## Instructions for Spec Kit

Use this GitHub issue as the primary source of truth.

Convert the issue into a Spec Kit feature specification before creating the implementation plan.

Preserve:

- business goal;
- user stories;
- acceptance criteria;
- technical constraints;
- non-goals;
- dependencies;
- open questions.

If information is missing, add it under a clearly marked **Clarifications Needed** section instead of inventing requirements.

If the issue conflicts with existing project documentation, explicitly call out the conflict.

Prefer a small, incremental implementation plan aligned with the repository's existing architecture, folder structure, language, framework, and conventions.
