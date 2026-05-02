# Design

This file defines the root-level product design policy for AI Flix / SmartMovieCatalog.

For implementation-level frontend tokens and component rules, read:

- `frontend/DESIGN.md`
- `docs/FRONTEND.md`

## Product Identity
- Product type: AI-assisted movie catalog and discovery experience.
- Visual direction: cinematic, premium, dark-mode-first, AI-focused.
- Interface priority: fast scanning, rich media presentation, clear AI metadata, and low-friction discovery.

## Design Source Of Truth
- Root product design policy: `DESIGN.md`.
- Angular implementation design system: `frontend/DESIGN.md`.
- Frontend engineering guidance: `docs/FRONTEND.md`.

If these documents conflict, prefer the most specific document:

1. `frontend/DESIGN.md` for component-level implementation.
2. `docs/FRONTEND.md` for frontend architecture and conventions.
3. `DESIGN.md` for product-level visual direction.

## Experience Principles
- Show the product experience directly. Avoid marketing-style landing pages unless explicitly requested.
- Keep navigation predictable and dense enough for repeated use.
- Make movie posters, titles, metadata, and AI analysis the primary visual signals.
- Use AI affordances deliberately. AI badges, match scores, analysis panels, and progress states should communicate useful information, not decoration.
- Keep states explicit: loading, empty, error, partial data, processing, and unavailable.

## Visual Rules
- Use dark mode as the default and only supported visual mode unless a light theme is explicitly added.
- Follow the Tailwind-based palette and token rules in `frontend/DESIGN.md`.
- Do not introduce a separate color palette, component library, or visual framework without approval.
- Prefer real movie/media imagery or durable product assets over abstract decorative graphics.
- Avoid visual clutter, excessive gradients, nested cards, and ornamental backgrounds.

## Layout Rules
- Keep primary app surfaces optimized for browsing, filtering, and comparing movies.
- Use constrained content widths for reading-heavy areas and denser layouts for catalog surfaces.
- Ensure responsive layouts work on mobile, tablet, and desktop.
- Do not let text overflow buttons, cards, navigation, badges, or metadata panels.
- Keep reusable UI patterns consistent across routes.

## Component Rules
- Use icons for common actions when an established icon exists.
- Use buttons only for actions and links only for navigation.
- Use typed, reusable UI components when a pattern appears more than once.
- Keep feature-specific UI inside feature boundaries when the app grows enough to justify them.
- Keep shared components generic and free of product-specific business logic.

## Content Rules
- Keep labels short and action-oriented.
- Avoid visible instructional copy that explains obvious UI mechanics.
- Prefer product language over implementation language.
- Distinguish user-provided metadata, catalog metadata, and AI-generated metadata when all three exist.

## Accessibility
- Maintain sufficient contrast against dark surfaces.
- Preserve keyboard navigation for interactive controls.
- Provide accessible names for icon-only buttons.
- Do not rely on color alone for status, errors, or AI confidence.
- Keep focus states visible.

## Change Control
- Update this file when product-level visual direction changes.
- Update `frontend/DESIGN.md` when implementation tokens, component specs, or frontend styling rules change.
- Update `docs/FRONTEND.md` when frontend structure, state management, or integration conventions change.
