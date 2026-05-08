# Frontend

## Current State
The frontend is an Angular 21 SPA in `frontend`. The root route renders the authentication login page, and `/catalog` renders the public movie catalog browsing page.

Primary references:

- `frontend/package.json`
- `frontend/angular.json`
- `frontend/src/app`
- `frontend/DESIGN.md`

## Routing
- `/` displays the authentication login page.
- `/catalog` displays the public V1 catalog page.
- `/catalog` reads `query`, `page`, and `pageSize` from URL query parameters.
- Catalog `pageSize` defaults to `12` when omitted or invalid. A valid URL `pageSize` in the range `1..100` is honored, but V1 does not show a page-size selector.
- Catalog item links target the movie details route pattern `/movies/{id}` established by the movie details feature.

## Design System
Before changing UI, read `frontend/DESIGN.md`.

The current visual direction is:

- Dark mode only.
- Cinematic, premium, AI-focused interface.
- Tailwind-inspired token conventions as documented in `DESIGN.md`. Tailwind is not installed in the Angular project today, so current components use local CSS aligned with those tokens.
- Material Symbols Outlined for iconography.

Do not introduce another design system, component library, custom palette, or visual language without explicit approval.

## Angular Rules
- Follow the existing Angular project structure.
- Keep components focused and modular.
- Keep API access isolated from presentation logic as the application grows.
- Prefer strongly typed models for API responses.
- Do not introduce global mutable state unless there is a clear product need.

## Movie Catalog UI
- The catalog page lives under `frontend/src/app/catalog/catalog-page`.
- Movie catalog HTTP calls are isolated in `frontend/src/app/movies/movies-api.ts`.
- Movie catalog contracts are mirrored as TypeScript interfaces in `frontend/src/app/movies/movie.models.ts` and must stay aligned with the movie listing/search API contracts.
- Catalog components must not call `HttpClient` directly.
- The V1 catalog route is public and does not require route guards or an authenticated frontend session.
- Search is basic title search through `GET /api/movies?query=...`; do not show semantic search, vector search, RAG, CLIP, recommendation, or AI ranking terminology for this feature.

## Authentication UI
- The login screen lives under `frontend/src/app/auth/login-page`.
- Auth HTTP calls are isolated in `frontend/src/app/auth/auth-api.ts`; components must not call `HttpClient` directly.
- `frontend/src/app/auth/auth-session-store.ts` owns the in-memory authenticated session.
- Auth contracts are mirrored as TypeScript interfaces in `frontend/src/app/auth/auth.models.ts` and must stay aligned with `SmartMovieCatalog.Contracts.Auth`.
- The Angular dev server proxies `/api` to the backend through `frontend/src/proxy.conf.js`; production uses same-origin `/api` when the SPA is served by ASP.NET Core.
- The access token returned by `POST /api/auth/authenticate` is kept in memory through `AuthSessionStore`. Do not persist bearer tokens in `localStorage` or `sessionStorage`.
- The login flow calls `POST /api/auth/authenticate` first, then calls `GET /api/auth/me` with the returned bearer token before storing the session.
- Registration, password recovery, refresh tokens, persistent sessions, and route guards are not implemented.

## Styling Rules
- Prefer the documented Tailwind-inspired tokens from `DESIGN.md` and `frontend/DESIGN.md`; implement them with local CSS while Tailwind is not installed.
- Avoid ad hoc colors and one-off spacing systems.
- Keep responsive behavior explicit.
- Do not modify generated build output in `dist`.

## Assets
- Store durable static assets under the Angular project public/assets structure when introduced.
- Avoid hotlinking external media in production-facing UI.
- Document licensing or source for non-generated assets.
