# Implementation Plan: Authentication Login Screen

**Branch**: `023-authentication-login-screen` | **Date**: 2026-05-03 | **Spec**: `specs/023-authentication-login-screen/spec.md`
**Input**: Feature specification from `specs/023-authentication-login-screen/spec.md`

## Summary 

Document and validate the already implemented Angular authentication login screen as the current SPA entry surface. The feature records the existing local email/password sign-in flow, backend auth API dependencies, frontend validation/error behavior, and in-memory-only bearer-token session boundary. No new runtime capability is introduced by this planning slice.

## Technical Context

**Language/Version**: ASP.NET Core 10 / C# backend; Angular 21 / TypeScript frontend  
**Primary Dependencies**: Existing backend auth endpoints, Angular reactive forms, Angular `HttpClient`, Vitest/Angular test tooling  
**Storage**: Backend users are already persisted by the existing auth foundation; frontend session storage is in-memory only with no `localStorage` or `sessionStorage` token persistence  
**Testing**: Frontend auth behavior tests via `npm test -- --watch=false`; frontend build via `npm run build`  
**Target Platform**: Web application served as Angular SPA with ASP.NET Core SpaProxy/same-origin API integration  
**Project Type**: Full-stack web application documentation/verification slice  
**Performance Goals**: No new runtime performance target; existing login UI should remain responsive under normal browser interaction  
**Constraints**: No refresh tokens, persistent sessions, logout, registration, password recovery, route guards, external identity providers, new dependencies, or new backend behavior  
**Scale/Scope**: One implemented login screen, two existing auth API endpoints, one in-memory frontend session model, synchronized documentation and tests

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Actual architecture source of truth**: PASS. Plan reflects existing ASP.NET Core 10, Angular 21, JWT auth, and frontend auth module behavior from repository docs and code.
- **Explicit boundaries**: PASS. Runtime code stays under `frontend/src/app/auth`; backend contract references stay under `backend/src/SmartMovieCatalog.Contracts/Auth`; documentation remains under `docs` and `specs`.
- **Contracts drive cross-boundary work**: PASS. Existing `POST /api/auth/authenticate` and `GET /api/auth/me` contracts are documented in `contracts/auth-api.md` and must remain aligned with frontend models.
- **Security and secret hygiene**: PASS. Plan preserves in-memory-only bearer token handling and generic auth failures; no secret material is introduced.
- **Small, verified, reviewable changes**: PASS. Scope is documentation/test alignment for existing behavior. Verification uses frontend tests/build only unless backend contracts change.
- **Frontend design compliance**: PASS. No UI redesign or design-system change is planned.
- **Backend architecture compliance**: PASS. No new backend code, persistence, messaging, external services, or infrastructure changes are planned.

## Project Structure

### Documentation (this feature)

```text
specs/023-authentication-login-screen/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── auth-api.md
├── spec.md
└── tasks.md
```

### Source Code (repository root)

```text
backend/
└── src/
    └── SmartMovieCatalog.Contracts/
        └── Auth/
            ├── AuthenticateRequest.cs
            ├── AuthenticateResponse.cs
            └── CurrentUserResponse.cs

frontend/
└── src/
    └── app/
        └── auth/
            ├── auth-api.ts
            ├── auth-api.spec.ts
            ├── auth-session-store.ts
            ├── auth-session-store.spec.ts
            ├── auth.models.ts
            └── login-page/
                ├── login-page.css
                ├── login-page.html
                ├── login-page.spec.ts
                └── login-page.ts

docs/
├── API.md
├── FRONTEND.md
├── SECURITY.md
├── TESTING.md
└── adr/
    └── 0008-frontend-auth-session.md
```

**Structure Decision**: Use the existing monorepo layout. This feature does not add projects or directories outside Spec Kit planning artifacts; application-code references are for validation and documentation alignment only.

## Phase 0: Research Summary

Research confirms that the feature documents current behavior rather than selecting new technology. The accepted decisions are:

- Preserve the existing Angular auth module and same-origin `/api` backend integration.
- Preserve in-memory-only bearer-token storage.
- Preserve generic authentication failure messaging for `401 Unauthorized`.
- Use focused frontend tests and documentation review as the primary validation mechanism.

See `specs/023-authentication-login-screen/research.md`.

## Phase 1: Design Summary

Design artifacts are intentionally lightweight:

- `data-model.md` documents request/response/session shapes and validation boundaries without introducing persistence.
- `contracts/auth-api.md` records the existing auth API contract and frontend consumption expectations.
- `quickstart.md` gives verification steps for docs, tests, and build.

## Constitution Check: Post-Design

- **No gate violations introduced**: PASS.
- **No unresolved clarification remains**: PASS.
- **No new dependency or infrastructure choice introduced**: PASS.
- **Security boundary remains explicit**: PASS.

## Complexity Tracking

No constitution violations require justification.
