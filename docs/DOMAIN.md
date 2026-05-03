# Domain

## Current Domain Scope
The product direction is AI-assisted movie cataloging and discovery. The Domain project is still early, but it now contains one implemented backend security aggregate:

- `User`: local authentication user aggregate under `backend/src/SmartMovieCatalog.Domain/Users`.

Product-specific movie cataloging aggregates have not been implemented yet.

## Candidate Domain Concepts
These are candidate concepts, not implemented contracts:

- Movie: catalog item with title, metadata, media assets, and classification data.
- Catalog: curated collection of movies.
- User Preference: user-provided or inferred taste profile.
- Recommendation: ranked movie suggestion generated from catalog and preference signals.
- AI Analysis: structured output produced by an AI process for a movie or catalog item.

## Domain Rules
- Domain rules must be explicit in code, not hidden in UI logic or infrastructure configuration.
- Domain model code is organized by aggregate or domain concept folder, as documented in `docs/adr/0006-domain-aggregate-organization.md`.
- Do not persist or expose AI-generated data without a clear schema and provenance.
- Keep transport DTOs separate from domain models once domain models exist.
- Avoid adding domain abstractions before the first real behavior requires them.

## Invariants To Define Before Movie Persistence
Before adding persisted movie catalog behavior, define at minimum:

- Required movie identity and uniqueness rules.
- Allowed metadata sources and trust levels.
- Whether AI analysis can be regenerated, versioned, or manually corrected.
- Ownership and authorization rules for user-specific data.

## Non-Goals For Now
- No product-specific movie cataloging aggregate model yet.
- No domain-layer CQRS abstraction. CQRS command/query dispatch is an Application/API concern documented in `docs/adr/0007-wolverine-cqrs-mediator.md`.

Add these only when the implementation requires them or the user explicitly approves an architecture direction.
