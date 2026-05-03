# SmartMovieCatalog Frontend Constitution

## Scope

Apply this file when Spec Kit work touches Angular, TypeScript, UI components, templates, styles, routing, browser behavior, frontend tests, frontend build configuration, assets, API client usage, or visual design.

This file extends `.specify/memory/constitution.md`; it does not replace it.

## Frontend Source Of Truth

- Frontend root: `frontend`.
- Package manifest: `frontend/package.json`.
- Angular configuration: `frontend/angular.json`.
- App source: `frontend/src/app`.
- Visual rules: `frontend/DESIGN.md`.
- Frontend guidance: `docs/FRONTEND.md`.
- Testing policy: `docs/TESTING.md`.
- Security rules: `docs/SECURITY.md`.

## Frontend Principles

### I. Preserve The Existing Angular Stack

The frontend is an Angular 21 SPA. Follow the existing project structure, dependency set, and build/test scripts.

Do not introduce external UI libraries, new state management frameworks, custom design systems, new CSS palettes, Angular standalone/signals assumptions, or unrelated frontend dependencies without explicit approval and clear justification.

### II. Design System Compliance

Before changing UI, HTML, CSS, or TypeScript components, read `frontend/DESIGN.md`.

The current visual direction is dark-mode-only, cinematic, premium, and AI-focused. Use the documented Tailwind utility classes, tokens, typography, layout rules, component specs, and iconography. Do not create ad hoc palettes, spacing systems, or visual languages.

### III. Components Must Not Own Server Concerns

Components should focus on presentation, local UI state, and orchestration. They must not call `HttpClient` directly. Backend communication must go through typed API services and strongly typed models.

Feature-specific code should stay inside feature folders once features become large enough to justify that boundary. Reusable UI belongs under shared component locations when that structure exists or is introduced deliberately.

### IV. API Data Is Untrusted

Treat API responses, URL data, user input, and AI-generated values as untrusted. Avoid direct DOM injection. If HTML rendering becomes necessary, document sanitization and security implications before implementation.

Do not expose provider prompts, credentials, internal policies, connection strings, or sensitive backend details in the frontend bundle.

### V. User-Observable Behavior Drives Tests

Frontend tests should focus on rendered behavior, user-observable state, and API boundary mocking. Avoid coupling tests to Angular internals or backend runtime availability.

## Frontend Verification

Use the narrowest command that validates the frontend change:

- Build: run `npm run build` from `frontend`.
- Tests: run `npm test -- --watch=false` from `frontend`.

If a frontend change also modifies backend contracts, run the relevant backend verification from `.specify/memory/constitution-backend.md`.

## Frontend Documentation

Update documentation when frontend structure, visual rules, API consumption, security posture, or testing strategy changes.

Do not modify generated output in `dist`, `.angular/cache`, or `node_modules`.

**Version**: 1.0.0 | **Ratified**: 2026-05-02 | **Last Amended**: 2026-05-02
