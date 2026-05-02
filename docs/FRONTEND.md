# Frontend

## Current State
The frontend is an Angular 21 SPA in `frontend`.

Primary references:

- `frontend/package.json`
- `frontend/angular.json`
- `frontend/src/app`
- `frontend/DESIGN.md`

## Design System
Before changing UI, read `frontend/DESIGN.md`.

The current visual direction is:

- Dark mode only.
- Cinematic, premium, AI-focused interface.
- Tailwind utility-first conventions as documented in `DESIGN.md`.
- Material Symbols Outlined for iconography.

Do not introduce another design system, component library, custom palette, or visual language without explicit approval.

## Angular Rules
- Follow the existing Angular project structure.
- Keep components focused and modular.
- Keep API access isolated from presentation logic as the application grows.
- Prefer strongly typed models for API responses.
- Do not introduce global mutable state unless there is a clear product need.

## Styling Rules
- Prefer the documented Tailwind utility classes and tokens from `DESIGN.md`.
- Avoid ad hoc colors and one-off spacing systems.
- Keep responsive behavior explicit.
- Do not modify generated build output in `dist`.

## Assets
- Store durable static assets under the Angular project public/assets structure when introduced.
- Avoid hotlinking external media in production-facing UI.
- Document licensing or source for non-generated assets.
