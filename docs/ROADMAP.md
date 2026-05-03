# Roadmap

This roadmap is a planning document, not an implementation promise.

## Phase 0 - Stabilize Scaffold
- Keep repository documentation aligned with the actual project structure.
- WeatherForecast API scaffold removed when backend auth endpoints were introduced.
- Frontend build command confirmed.
- Backend test framework added for authentication behavior.

## Phase 1 - Product Foundation
- Define core movie catalog use cases.
- Define initial API contracts for movie search, movie details, and catalog browsing.
- Create durable frontend navigation and layout based on `DESIGN.md`.
- Establish shared request/response models or generated client strategy if API growth justifies it.

## Phase 2 - Domain And Persistence
- Persistence provider decided as EF Core with PostgreSQL and documented with an ADR.
- Define movie identity, metadata ownership, and uniqueness rules.
- Introduce persistence behind clear application boundaries if needed.
- Add integration tests for persistence-backed endpoints.

## Phase 3 - AI Features
- Define AI analysis input/output schemas.
- Add validation for AI-generated structured output.
- Track provenance, versioning, and regeneration rules for AI metadata.
- Add safeguards against prompt/data leakage.

## Phase 4 - Security And Operations
- Authentication model defined as local email/password with JWT bearer access tokens.
- Production JWT and database configuration validation added.
- Establish deployment topology.
- Add observability for API errors, AI provider failures, and frontend runtime issues.

## Open Decisions
- API versioning strategy.
- AI provider and data retention policy.
