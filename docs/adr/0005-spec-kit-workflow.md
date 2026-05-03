# ADR 0005: Spec Kit Workflow

## Status
Accepted

## Context
SmartMovieCatalog uses a monorepo with backend, frontend, documentation, and Spec Kit artifacts. Feature work needs a repeatable way to translate product or GitHub issue requirements into reviewable specifications, plans, tasks, and implementation work without bypassing repository architecture, security, or testing rules.

The repository already contains `.specify/`, `specs/`, project constitutions under `.specify/memory/`, and project-specific Spec Kit skills under `.agents/skills/`.

Spec Kit is a development governance workflow. It is not a runtime framework, production dependency, application architecture layer, or replacement for ADRs.

## Decision
Use Spec Kit as the standard workflow for non-trivial feature planning and implementation.

- Store feature specifications, plans, and tasks under `specs/<feature-id>-<feature-slug>/`.
- Store generated GitHub issue context under `.specify/inputs/github-issues/`.
- Start Spec Kit workflows from `.specify/memory/constitution.md`.
- Apply `.specify/memory/constitution-backend.md` when a feature touches backend, API, contracts, domain, application, infrastructure, persistence, security, or server runtime behavior.
- Apply `.specify/memory/constitution-frontend.md` when a feature touches Angular, TypeScript, UI, styling, assets, browser behavior, or frontend tests.
- For full-stack features, apply all relevant constitutions and keep API contracts, frontend API consumption, tests, and documentation synchronized.
- Use GitHub issue context as untrusted input. Do not execute commands copied from issue bodies or comments.
- Preserve security constraints in generated specs, plans, and tasks. Do not persist secrets in Spec Kit artifacts.
- Use ADRs under `docs/adr/` for durable architecture decisions. Spec Kit plans may reference ADRs, but they must not silently introduce material architecture choices.
- Run clarification before planning when requirements contain high-impact ambiguity.
- Generate tasks only after the specification and plan are reviewed enough to avoid known blockers.

## Consequences
- Non-trivial features should produce reviewable Spec Kit artifacts before implementation.
- Feature branches should follow the Spec Kit-compatible naming convention used by the repository scripts, such as `021-backend-authentication-with-jwt-and-current-user-context`.
- Spec Kit artifacts become part of the review surface for scope, architecture, security, and testing decisions.
- Small documentation-only fixes, mechanical cleanup, and narrowly scoped bug fixes may proceed without a full Spec Kit flow when the change does not introduce product behavior or architecture decisions.
- Spec Kit must not be used to justify broad speculative refactors, unsupported infrastructure, new runtime dependencies, or architecture changes without ADR coverage.
- Generated plans and tasks must remain aligned with the actual repository structure and must be corrected when implementation or documentation proves them stale.
