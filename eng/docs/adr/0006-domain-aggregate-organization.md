# ADR 0006: Domain Aggregate Organization

## Status
Accepted

## Context
The Domain project now contains the first implemented aggregate for backend authentication:

- `backend/src/SmartMovieCatalog.Domain/Users/User.cs`
- `backend/src/SmartMovieCatalog.Domain/Users/UserId.cs`
- `backend/src/SmartMovieCatalog.Domain/Users/EmailAddress.cs`
- `backend/src/SmartMovieCatalog.Domain/Users/UserRole.cs`

`User` inherits from `AggregateRoot`, so it is an aggregate root by behavior and type hierarchy. The physical folder structure currently places the aggregate root and its closely related value objects in `Domain/Users/`.

The Domain project also contains generic scaffold folders such as `Aggregates`, `Entities`, `ValueObjects`, `Enums`, `Events`, and `Exceptions`. These folders can imply organization by technical type, which would scatter a single aggregate's model across unrelated directories.

## Decision
Organize domain model code by aggregate or domain concept folder, not by generic DDD technical type folders.

For the current user model:

- Keep the `User` aggregate root in `SmartMovieCatalog.Domain/Users`.
- Keep `UserId`, `EmailAddress`, and `UserRole` in `SmartMovieCatalog.Domain/Users` because they are part of the user aggregate boundary and are not shared domain primitives.
- Treat `SmartMovieCatalog.Domain/Common` as the place for framework-free base abstractions and small shared primitives such as `AggregateRoot`, `Entity`, `ValueObject`, `DomainEvent`, `Guard`, and `Result`.
- Do not move aggregate-specific files into generic folders such as `Aggregates` or `ValueObjects`.
- Add future domain concepts under their own cohesive folders, for example `Movies`, `Catalogs`, `Recommendations`, or another name that matches the ubiquitous language once the behavior exists.

Generic scaffold folders under Domain are not architectural guidance. They should not receive new aggregate-specific code unless a later ADR deliberately changes the organization strategy.

## Consequences
- Aggregate folders keep invariants, identifiers, and value objects close to the behavior they support.
- The codebase avoids scattering one aggregate across type-based folders.
- Namespace structure can continue to follow the domain concept, for example `SmartMovieCatalog.Domain.Users`.
- Shared abstractions remain available without turning `Common` into a dumping ground for aggregate-specific types.
- Future refactors should prefer removing unused scaffold folders or leaving them empty over using them inconsistently.
