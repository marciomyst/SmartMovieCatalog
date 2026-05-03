# SmartMovieCatalog Constitution

## Scope And Routing

This repository is a monorepo with two runtime surfaces:

- Backend: ASP.NET Core 10 / C# Clean Architecture projects under `backend/src`.
- Frontend: Angular 21 / TypeScript SPA under `frontend`.

All Spec Kit work must start from this file. Then apply the specialized constitution based on the requested change:

- Backend, API, contracts, domain, application, infrastructure, persistence, security, or server runtime work must also follow `.specify/memory/constitution-backend.md`.
- Frontend, UI, Angular, TypeScript, component, styling, accessibility, or browser behavior work must also follow `.specify/memory/constitution-frontend.md`.
- Full-stack product features must follow all three constitution files and keep backend contracts, frontend API usage, tests, and documentation synchronized.

## Core Principles

### I. Actual Architecture Is The Source Of Truth

Specs, plans, and tasks must reflect the current repository architecture. Do not assume Wolverine, CQRS, EF Core, PostgreSQL, xUnit, TestContainers, Angular standalone components, Angular signals, authentication, messaging, background workers, or external AI providers unless they already exist in the codebase or the user explicitly requests an architecture change.

When an architecture document and implementation disagree, inspect the implementation and surface the conflict instead of inventing rules. Material durable choices require an ADR under `docs/adr`.

### II. Boundaries Must Stay Explicit

Backend concerns stay under `backend/src` and backend tests under `backend/tests`. Frontend concerns stay under `frontend`. Documentation stays under `docs`. Spec Kit artifacts stay under `.specify` and `specs`.

Generated and vendor output is not architectural input and must not be read or modified unless explicitly required: `node_modules`, `bin`, `obj`, `dist`, `.vs`, `.angular/cache`.

### III. Contracts Drive Cross-Boundary Work

Any feature that crosses backend and frontend must define the API behavior, request/response shape, validation behavior, error behavior, and frontend consumption model before implementation tasks are generated.

Backend and frontend changes must remain synchronized. Public API contracts must not leak persistence models, provider payloads, secrets, internal exception details, or implementation-specific configuration.

### IV. Security And Secret Hygiene Are Non-Negotiable

Never introduce secrets, credentials, certificates, private keys, connection strings, provider tokens, or private user data into source files, Dockerfiles, frontend bundles, committed docs, or Spec Kit artifacts.

Treat all external input, API input, user input, URL data, and AI output as untrusted. Validate at boundaries and document security-sensitive assumptions in the spec or plan.

### V. Small, Verified, Reviewable Changes

Plans and tasks must prefer small, reviewable changes over broad speculative refactors. Do not add production dependencies, infrastructure packages, persistence, external services, or new design systems without explicit justification and architectural impact review.

Verification must use the narrowest command that covers the change, with broader checks when shared contracts, project files, or cross-boundary behavior changes.

## Required Context

Before planning or implementing a feature, read only the context relevant to the requested scope:

- Always read `CONTEXT.md`, `docs/ARCHITECTURE.md`, `docs/CONVENTIONS.md`, and this constitution.
- Backend scope: also read `docs/API.md`, `docs/DOMAIN.md`, `docs/TESTING.md`, `docs/SECURITY.md`, and `.specify/memory/constitution-backend.md`.
- Frontend scope: also read `docs/FRONTEND.md`, `frontend/DESIGN.md`, `docs/TESTING.md`, `docs/SECURITY.md`, and `.specify/memory/constitution-frontend.md`.
- Full-stack scope: read both specialized constitutions and all relevant backend/frontend documents.

## Quality Gates

Use the narrowest feasible verification:

- Backend API/layer changes: `dotnet build backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj`.
- Backend shared contracts, project references, or cross-layer changes: `dotnet build SmartMovieCatalog.slnx`.
- Frontend changes: run `npm run build` from `frontend`.
- Frontend behavior tests: run `npm test -- --watch=false` from `frontend`.
- Full-stack or shared contract changes: run the backend and frontend checks that cover both sides.

If a verification command fails, inspect the error and the changed files before attempting another fix.

## Governance

This constitution governs Spec Kit specs, plans, tasks, and implementation decisions for this repository. More specific constitution files may add constraints for backend or frontend work, but they must not weaken these common principles.

Amend this file when repository-wide workflow, architecture rules, security posture, verification policy, or cross-boundary conventions change. Amend the specialized constitution files when backend-only or frontend-only rules change.

**Version**: 1.0.0 | **Ratified**: 2026-05-02 | **Last Amended**: 2026-05-02
