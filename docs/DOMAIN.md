# Domain

## Current Domain Scope
The product direction is AI-assisted movie cataloging and discovery. The repository now has a Domain project, but it is still in an early scaffold state and does not yet contain product-specific entities, aggregates, repositories, or persistence models.

## Candidate Domain Concepts
These are candidate concepts, not implemented contracts:

- Movie: catalog item with title, metadata, media assets, and classification data.
- Catalog: curated collection of movies.
- User Preference: user-provided or inferred taste profile.
- Recommendation: ranked movie suggestion generated from catalog and preference signals.
- AI Analysis: structured output produced by an AI process for a movie or catalog item.

## Domain Rules
- Domain rules must be explicit in code, not hidden in UI logic or infrastructure configuration.
- Do not persist or expose AI-generated data without a clear schema and provenance.
- Keep transport DTOs separate from domain models once domain models exist.
- Avoid adding domain abstractions before the first real behavior requires them.

## Invariants To Define Before Persistence
Before adding a database, define at minimum:

- Required movie identity and uniqueness rules.
- Allowed metadata sources and trust levels.
- Whether AI analysis can be regenerated, versioned, or manually corrected.
- Ownership and authorization rules for user-specific data.

## Non-Goals For Now
- No product-specific DDD aggregate model yet.
- No assumed EF Core entity model.
- No assumed CQRS command/query split.
- No assumed PostgreSQL schema.

Add these only when the implementation requires them or the user explicitly approves an architecture direction.
