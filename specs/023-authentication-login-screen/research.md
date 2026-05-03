# Research: Authentication Login Screen

## Decision: Treat This Feature As Documentation And Verification Alignment

**Rationale**: The spec describes an already implemented frontend login surface and existing backend auth endpoints. The highest-value work is preventing drift between the implementation, API docs, security docs, ADR 0008, and frontend tests.

**Alternatives considered**:
- Add new runtime auth behavior: rejected because refresh tokens, persistent sessions, logout, registration, password recovery, route guards, and external identity providers are explicitly out of scope.
- Rework backend authentication: rejected because backend endpoints already exist and are dependencies, not planned changes.

## Decision: Keep Frontend Session In Memory Only

**Rationale**: Existing security documentation and ADR 0008 require bearer tokens to stay out of `localStorage` and `sessionStorage`. A browser refresh clears the frontend session until a later security decision designs persistent sessions or refresh tokens.

**Alternatives considered**:
- `localStorage` bearer token persistence: rejected because it increases persistent token exposure.
- `sessionStorage` bearer token persistence: rejected because the current scope requires browser refresh to clear the session.
- Refresh-token flow: rejected because it is a future feature requiring separate security review.

## Decision: Keep Authentication Failures Generic

**Rationale**: `401 Unauthorized` responses must not disclose whether an email exists, whether the password is wrong, or whether a user is inactive or removed. The frontend therefore presents generic authentication failure messaging.

**Alternatives considered**:
- Specific credential/account status messages: rejected because they increase account-enumeration risk.

## Decision: Use Existing Frontend Test Structure

**Rationale**: The Angular project already contains focused auth tests for API calls and login-page behavior. The feature should extend or verify that coverage instead of adding broad end-to-end or backend test infrastructure.

**Alternatives considered**:
- New backend tests: rejected for this feature because no backend behavior changes are planned.
- Browser end-to-end test suite: deferred because current acceptance can be validated by focused frontend tests and manual quickstart checks.
