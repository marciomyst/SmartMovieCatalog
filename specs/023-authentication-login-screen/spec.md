# Feature Specification

## Feature Summary

Document the implemented Angular authentication login screen for SmartMovieCatalog. The SPA now starts on a dark cinematic login page, validates local email/password input, authenticates against the existing backend auth endpoints, and stores the authenticated session in memory only.

## Problem Statement

The repository already contains a working frontend auth surface, backend auth endpoints, and supporting documentation. Without a feature spec, the behavior of the login screen, its API dependencies, and its security boundaries are easy to drift apart over time.

This feature spec establishes the login screen as a documented product surface and records the current contract between the Angular SPA and the backend auth API.

## Goals

- Document the Angular login screen as the current SPA entry surface.
- Document the user flow for local email/password authentication.
- Document the API contract between the frontend auth module and the backend auth endpoints.
- Document the in-memory session strategy and the security boundary around bearer token storage.
- Keep frontend documentation, backend API documentation, and auth security notes synchronized.

## Non-Goals

- No refresh token flow.
- No persistent browser session.
- No logout flow.
- No route guards or authenticated shell navigation.
- No registration flow.
- No password recovery flow.
- No external identity provider integration.
- No role-specific authorization UI.

## User Stories

### User Story 1 - Sign In (Priority: P1)

As a user, I can enter my email and password on the login screen and sign in to the application.

**Why this priority**: Authentication is the entry point for the SPA and unlocks the product flow.

**Independent Test**: Open the app, provide valid credentials, and verify that the session is created and the current user context is loaded.

**Acceptance Scenarios**:

1. **Given** the login screen is displayed, **When** I submit valid credentials, **Then** I am authenticated and the current user context is loaded.
2. **Given** the login screen is displayed, **When** I submit invalid credentials, **Then** I receive a generic authentication failure.

---

### User Story 2 - Validate Input (Priority: P1)

As a user, I get immediate validation feedback when the email or password is invalid.

**Why this priority**: The login form must prevent avoidable failed submissions and show clear field-level feedback.

**Independent Test**: Submit empty or malformed input and verify that the form shows field-level validation messages without authenticating.

**Acceptance Scenarios**:

1. **Given** the email field is empty or malformed, **When** I submit the form, **Then** the email field shows validation feedback.
2. **Given** the password field is empty, **When** I submit the form, **Then** the password field shows validation feedback.

---

### User Story 3 - Preserve Session Boundaries (Priority: P2)

As a security-conscious user, my login session is not persisted in browser storage.

**Why this priority**: The current scope uses in-memory session handling to avoid exposing bearer tokens in persistent browser storage.

**Independent Test**: Complete sign in, refresh the browser, and verify that the session is cleared.

**Acceptance Scenarios**:

1. **Given** I am authenticated, **When** I refresh the browser, **Then** the in-memory session is lost.
2. **Given** I am authenticated, **When** I inspect browser storage, **Then** no bearer token is stored in localStorage or sessionStorage.

## Functional Requirements

- **FR-001**: The application MUST present the authentication login screen as the current SPA entry surface.
- **FR-002**: The login screen MUST collect email and password using an Angular reactive form.
- **FR-003**: The login screen MUST provide client-side validation for required email and password input.
- **FR-004**: The login screen MUST display a password visibility toggle with accessible labeling.
- **FR-005**: The frontend auth module MUST submit credentials to `POST /api/auth/authenticate`.
- **FR-006**: The frontend auth module MUST call `GET /api/auth/me` after successful authentication to load the current user context.
- **FR-007**: The frontend auth module MUST keep `HttpClient` usage isolated from UI components.
- **FR-008**: The frontend auth module MUST store the authenticated session in memory only.
- **FR-009**: The frontend auth module MUST not persist bearer tokens in localStorage or sessionStorage.
- **FR-010**: The UI MUST map `400 ValidationProblemDetails` responses to field-level validation feedback.
- **FR-011**: The UI MUST present generic `401 Unauthorized` messaging without revealing whether the email exists.
- **FR-012**: Auth contracts in the frontend MUST remain aligned with `SmartMovieCatalog.Contracts.Auth`.
- **FR-013**: Frontend documentation MUST describe the auth module, proxy behavior, session model, and security boundaries.
- **FR-014**: Authentication tests MUST cover rendering, validation, backend validation-error mapping, password visibility, API calls, bearer header behavior, in-memory session storage, and unauthorized errors.

## Acceptance Criteria

- The login screen renders correctly as the SPA entry surface.
- Users can submit valid credentials and the application loads the current user context.
- Invalid or empty email and password inputs show field-level validation feedback.
- Backend validation failures show field-specific feedback in the frontend.
- Authentication failures remain generic and do not disclose whether the email exists.
- The bearer token is held in memory only and is not written to persistent browser storage.
- The login flow calls `POST /api/auth/authenticate` and then `GET /api/auth/me` with the bearer token.
- Frontend auth documentation and security documentation match the implemented behavior.
- Frontend auth tests pass for rendering, validation, API integration, and error handling.

## Edge Cases

- Empty email or password input must not trigger an auth request.
- Malformed email input must surface local validation feedback before submission.
- A `400` response from the backend must map to field-level validation feedback.
- A `401` response from the backend must remain generic and not disclose account existence.
- A browser refresh must clear the in-memory session.
- If the backend is unavailable, the login flow must fail without exposing internal details.
- A successful authenticate response followed by a failed current-user lookup must not leave the user with a partially trusted session.

## Clarifications Needed

None identified.

## Assumptions

- The implemented frontend auth flow is the source of truth for this spec.
- The earlier backend-only scope note in the issue body is historical and superseded by the implemented frontend auth session ADR.
- Angular development continues to use same-origin `/api` paths with the local proxy.
- The bearer token remains in memory until a separate security decision introduces persistence or refresh-token behavior.
- This spec documents the current product behavior rather than introducing new auth capabilities.

## Dependencies

- `POST /api/auth/authenticate` and `GET /api/auth/me` remain available and stable.
- `docs/FRONTEND.md`, `docs/API.md`, `docs/SECURITY.md`, and `docs/adr/0008-frontend-auth-session.md` stay synchronized with the implementation.
- The Angular auth module remains under `frontend/src/app/auth`.
- The local proxy continues to forward `/api` to the backend during frontend development.

## Risks

- Documentation drift if the auth flow changes without updating the spec.
- Future contract changes in `SmartMovieCatalog.Contracts.Auth` can invalidate the frontend documentation.
- Persisting the session in browser storage later would require a new security decision and spec update.
- Additional auth flows could be incorrectly assumed to be in scope if this spec is not read carefully.

## Out of Scope

- Refresh tokens.
- Persistent sessions.
- Logout.
- Registration.
- Password recovery.
- Route guards.
- External identity providers.
- Role-specific auth UI.
