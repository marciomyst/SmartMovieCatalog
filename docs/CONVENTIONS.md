# Conventions

## General
- Keep changes scoped to the requested behavior.
- Prefer simple code over speculative abstraction.
- Avoid unrelated refactors.
- Do not leave `TODO` comments for core business logic.
- Preserve existing user changes in the working tree.

## Naming
- Use names that describe domain behavior, not implementation mechanics.
- Avoid abbreviations unless they are established in the project.
- Keep API DTO names explicit when contracts become public.

## C#
- Keep nullable reference types respected.
- Prefer explicit types where they improve readability.
- Use `var` only where the type is obvious, anonymous, or clearer because of a long generic type.
- Keep Minimal API endpoint handlers thin; move orchestration and business behavior into Application/Domain.
- Group product HTTP endpoints by feature under `SmartMovieCatalog.Api/Features`.
- Use feature-local FluentValidation validators for HTTP request shape validation.
- Use Wolverine command/query handlers in Application for backend use case dispatch.
- Return `Result<TValue, TError>` from Application command/query handlers when a use case has expected success/failure outcomes.
- Do not introduce persistence or infrastructure packages without an architecture decision.

## Angular / TypeScript
- Follow the existing Angular project structure and package set.
- Keep component logic cohesive.
- Prefer typed API models.
- Keep styling aligned with `frontend/DESIGN.md`.
- Do not introduce external UI libraries without approval.

## Commits
Use Conventional Commits when creating commits:

- `feat:` for user-visible features.
- `fix:` for bug fixes.
- `docs:` for documentation-only changes.
- `test:` for test-only changes.
- `refactor:` for behavior-preserving code restructuring.
- `chore:` for tooling, dependency, or maintenance changes.

## Documentation
- Update docs when changing architecture, API contracts, security posture, testing strategy, or frontend design rules.
- Use `docs/adr` for material architecture decisions.
