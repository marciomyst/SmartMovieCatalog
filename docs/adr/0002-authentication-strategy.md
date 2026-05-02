# ADR 0002: Authentication Strategy

## Status
Proposed

## Context
Authentication has not been implemented yet. The repository has no confirmed identity provider, token strategy, cookie strategy, authorization model, or user persistence model.

## Decision
No authentication mechanism is selected yet.

Before implementation, choose and document:

- Authentication mechanism.
- Token or cookie handling.
- User identity source.
- Authorization policy model.
- Frontend session storage rules.

## Consequences
- Do not introduce auth packages, login flows, JWT handling, or user tables until this ADR is accepted with a concrete strategy.
- Frontend auth folders or services should not be created speculatively.
