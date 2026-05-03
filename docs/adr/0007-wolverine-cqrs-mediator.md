# ADR 0007: Wolverine CQRS Mediator

## Status
Accepted

## Context
The backend API now exposes product HTTP endpoints as Minimal API feature slices. Auth behavior already lives outside HTTP handlers in the Application layer, but endpoint handlers were invoking application use case services directly.

The project needs a consistent command/query dispatch model for backend use cases while preserving Clean Architecture boundaries and avoiding premature distributed messaging.

## Decision
Use Wolverine as the backend command/query mediator.

- Register Wolverine in the API host.
- Discover handlers from `SmartMovieCatalog.Application`.
- Dispatch commands and queries from HTTP endpoints through `IMessageBus.InvokeAsync<T>()`.
- Keep command/query message types and handlers in the Application layer.
- Keep HTTP routing, validation, authorization metadata, status codes, and response shaping in the Api layer.
- Disable remote invocation for now so Wolverine is used as local in-process CQRS, not distributed request/reply messaging.
- Do not introduce Wolverine transports, outbox, retries beyond local invocation defaults, sagas, or external messaging until a separate feature and ADR explicitly require them.

## Consequences
- API endpoint handlers no longer invoke application use case services directly.
- Application handlers become the command/query entry point for new backend use cases.
- Existing application services can be adapted behind Wolverine handlers during migration.
- Handler discovery must include the Application assembly.
- Tests should continue to verify HTTP behavior and application behavior independently.
