# ADR 0003: Database Strategy

## Status
Proposed

## Context
Persistence has not been implemented yet. The repository does not currently contain EF Core, migrations, repository abstractions, database containers, or a schema.

## Decision
No database provider is selected yet.

Before implementation, choose and document:

- Database provider.
- Migration strategy.
- Local development database setup.
- Test database strategy.
- Backup and data retention assumptions.

## Consequences
- Do not add EF Core, PostgreSQL, migrations, repositories, or TestContainers until this ADR is accepted with a concrete strategy.
- Domain and API contracts should not imply persistence behavior that has not been chosen.
