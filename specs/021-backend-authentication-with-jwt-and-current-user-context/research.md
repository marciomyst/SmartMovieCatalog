# Phase 0 Research: Backend Authentication With JWT And Current User Context

## Decision: Use local email/password authentication with JWT bearer access tokens

Rationale: ADR 0002 is accepted and selects backend-only local credentials plus JWT bearer tokens. This matches the feature scope and avoids introducing external identity providers, refresh-token storage, registration, frontend session behavior, or fine-grained authorization policies.

Alternatives considered:

- ASP.NET Core Identity end-to-end: rejected for this feature because the repository needs local authentication primitives, not Identity UI, registration, password recovery, external login, or Identity-specific persistence shape.
- External identity provider: rejected by the feature non-goals.
- Cookie authentication: rejected because the API contract calls for bearer tokens and there is no frontend session implementation in scope.

## Decision: Store local users in PostgreSQL through EF Core owned by Infrastructure

Rationale: ADR 0003 is accepted and selects EF Core with PostgreSQL, `ConnectionStrings:DefaultConnection`, migrations in `SmartMovieCatalog.Infrastructure`, and application persistence abstractions in `SmartMovieCatalog.Application`. The current `docker-compose.yml` already provides a PostgreSQL service and passes `ConnectionStrings__DefaultConnection` to the API container.

Alternatives considered:

- In-memory users only: rejected because the feature requires persisted active, removed, role, and password state.
- File-based local users: rejected because it conflicts with ADR 0003 and creates unsafe operational behavior for authentication.
- Adding persistence in the API project: rejected because it violates the Clean Architecture dependency direction and ADR 0003.

## Decision: Model user authentication state as a domain aggregate with infrastructure persistence mapping

Rationale: User state has product invariants: normalized email uniqueness, active/removal rejection, persisted roles, password hash metadata, and first-login password-change state. The domain model should express those invariants without EF Core attributes or ASP.NET Core dependencies; Infrastructure owns EF Core mappings and migrations.

Alternatives considered:

- Persistence entity as the only user model: rejected because active/removal and role rules would be hidden in infrastructure or HTTP endpoint code.
- Full identity domain with registration, password recovery, refresh tokens, and policy authorization: rejected because those behaviors are out of scope.

## Decision: Keep authentication orchestration in Application services/use cases

Rationale: Minimal API endpoints should handle HTTP concerns only. Application services should validate use-case flow, request repositories, verify passwords through an abstraction, issue tokens through an abstraction, and return explicit success/failure results. Infrastructure implements persistence, password hashing, JWT generation, and current request user access.

Alternatives considered:

- Authentication logic in HTTP endpoints: rejected because it mixes HTTP, validation, persistence, hashing, and token generation.
- CQRS command/query stack: rejected because the repository has not introduced CQRS and the feature does not require that abstraction.

## Decision: Use ASP.NET Core `ProblemDetails` and `ValidationProblemDetails` for product auth errors

Rationale: ADR 0004 is accepted and selects framework-standard problem responses for product endpoints. Authentication failures must stay generic to avoid account enumeration, while validation failures should use `ValidationProblemDetails`.

Alternatives considered:

- Existing `ApiErrorResponse`: rejected for this feature because ADR 0004 says new product endpoints should not depend on that custom envelope.
- Raw strings or ad hoc anonymous errors: rejected because they are not stable API contracts.

## Decision: Use framework password hashing primitives without adopting full ASP.NET Core Identity

Rationale: Passwords must never be stored in plaintext. A password hasher service can use maintained ASP.NET Core password hashing primitives while keeping Identity user management, UI, and schema out of scope. The persisted hash format must be opaque to API consumers and sufficient for verification and future rehash decisions.

Alternatives considered:

- Custom cryptography: rejected due to security risk and maintenance cost.
- Plain SHA or unsalted hashes: rejected as insecure.
- Full Identity stack: rejected as broader than this feature.

## Decision: Validate JWT and persistence configuration at startup

Rationale: JWT issuer, audience, signing key, token lifetime, and default database connection are security-sensitive runtime settings. Missing or invalid configuration must fail closed before the API can issue unusable or unsigned tokens.

Alternatives considered:

- Lazy validation on first login: rejected because it turns deployment misconfiguration into runtime authentication failures.
- Committed development secrets: rejected by repository security rules.

## Decision: Add focused backend tests using a deliberate test framework and API test host

Rationale: The feature adds security-sensitive authentication, persistence, token generation, and API behavior. Existing backend test projects are compile-only scaffolds, so implementation should explicitly add a test framework package and `Microsoft.AspNetCore.Mvc.Testing` for endpoint-level tests. Database-backed tests should avoid TestContainers unless a later decision explicitly accepts that dependency.

Alternatives considered:

- Build-only verification: rejected because authentication behavior is regression-prone.
- Database-backed TestContainers by default: rejected because the repository explicitly says not to assume TestContainers and ADR 0003 leaves database-backed integration test strategy unselected.
