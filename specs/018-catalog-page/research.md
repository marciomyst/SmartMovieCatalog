# Research: Catalog Page

## Decision: Use URL Query Parameters For Catalog State

**Decision**: Store `query`, `page`, and `pageSize` in `/catalog` URL query parameters.

**Rationale**: The catalog is a primary browsing surface. URL-backed state supports refresh, browser back/forward behavior, and shareable links with minimal extra infrastructure.

**Alternatives considered**:

- Component-only state: rejected because it loses catalog position on refresh and cannot be shared.
- Hybrid initial URL read only: rejected because later pagination/search changes would still be non-shareable.

## Decision: Default Page Size 12 With No V1 Selector

**Decision**: Default `pageSize` to `12`, honor a valid URL `pageSize`, and do not expose a page-size selector in the V1 UI.

**Rationale**: The GitHub issue suggests `pageSize=12`, which fits a card grid and keeps V1 controls small. Reading URL `pageSize` preserves API flexibility and testability without adding selector UI.

**Alternatives considered**:

- Fixed page size only: rejected because the clarified spec requires reading `pageSize` from the URL.
- Visible selector: rejected because it adds UI, validation, and acceptance-test scope without a V1 need.

## Decision: Public Catalog Route For V1

**Decision**: `/catalog` is publicly reachable and does not require frontend authentication or route guards in V1.

**Rationale**: Authentication-specific catalog behavior is out of scope, route guards are not implemented, and the current movie create slice is unauthenticated.

**Alternatives considered**:

- Require an authenticated session: rejected because it would introduce auth routing behavior outside issue #18.
- Defer to future app shell: rejected because the catalog needs a clear implementation target now.

## Decision: Typed API Service Boundary

**Decision**: Add or reuse a typed `MoviesApi` service for `GET /api/movies`; catalog components must not inject `HttpClient` directly.

**Rationale**: Existing frontend auth code isolates HTTP calls in typed API services, and the frontend constitution requires components to avoid owning server concerns.

**Alternatives considered**:

- Direct `HttpClient` in the catalog page: rejected by project rules.
- Global state management library: rejected because the current dependency set does not include one and the feature does not justify it.

## Decision: Normalize Invalid URL Pagination Parameters Before API Calls

**Decision**: If `page` or `pageSize` URL values are absent, non-numeric, or less than 1, normalize them to defaults before calling the API.

**Rationale**: URL parameters are untrusted input. Normalization avoids knowingly sending malformed requests from the UI while still allowing the API to enforce its own validation for runtime edge cases.

**Alternatives considered**:

- Send invalid URL values to the API: rejected because it creates avoidable error states from client-side URL parsing.
- Block rendering with a validation page: rejected as too heavy for V1.

## Decision: Reuse Movie Card If Available

**Decision**: Reuse the issue #17 movie card component if it exists by implementation time; otherwise create a catalog-local card that can later be extracted.

**Rationale**: This keeps issue #18 independent of issue #17 ordering while preserving reuse when available.

**Alternatives considered**:

- Block on issue #17: rejected because it makes catalog implementation unnecessarily coupled to card extraction.
- Create a shared card immediately: rejected unless no reusable component exists and duplication becomes real.
