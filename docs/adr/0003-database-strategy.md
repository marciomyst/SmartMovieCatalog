# ADR 0003: Database Strategy

## Status
Accepted

## Context
Persistence has not been implemented yet. Issue #21 requires persisted local users for backend authentication.

The repository already includes a PostgreSQL service in `docker-compose.yml` and passes `ConnectionStrings__DefaultConnection` to the API container. The current scaffold does not use persistence yet.

## Decision
Use EF Core with PostgreSQL for backend persistence.

- Use `ConnectionStrings:DefaultConnection` as the application connection string key.
- Use PostgreSQL as the database provider.
- Use EF Core migrations owned by `SmartMovieCatalog.Infrastructure`.
- Keep persistence implementation under `backend/src/SmartMovieCatalog.Infrastructure/Persistence`.
- Keep application persistence abstractions under `backend/src/SmartMovieCatalog.Application/Abstractions/Persistence`.
- Keep domain models independent of EF Core and PostgreSQL-specific concerns.
- Keep public API contracts separate from persistence entities.
- Store local development database settings through Docker Compose environment variables and `.env` values; do not commit real credentials.

The first schema introduced by this decision is expected to support local user authentication, including user identity, normalized email uniqueness, password hash state, active/removal state, roles, and first-login password-change state.

Database-backed integration test strategy is not selected by this ADR. If tests require a real PostgreSQL instance, that decision must be documented in the relevant test plan or a later ADR.

Backup, retention, and production operations policy are not selected by this ADR.

## Consequences
- Backend implementation may add EF Core and the PostgreSQL provider as production dependencies.
- Backend implementation may add DbContext configuration, entity mappings, repositories, and migrations in Infrastructure.
- The API must validate `ConnectionStrings:DefaultConnection` when persistence is required and fail clearly when missing or invalid.
- Docker Compose remains the local PostgreSQL path for development.
- Domain code must not depend on EF Core attributes, DbContext, database provider APIs, migrations, or connection-string concerns.
- Public API contracts must not leak persistence entities or database-specific details.
