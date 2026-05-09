# Research: Home Movie Cards

## Decision: Root route becomes public home

**Decision**: Route `/` renders the public home page with movie cards, and the existing login screen moves to `/login`.

**Rationale**: Issue #17 explicitly asks for movie cards on the home page. The clarification session resolved the current documentation conflict by making the root route the product-facing entry surface while preserving authentication access.

**Alternatives considered**:

- Keep `/` as login and add `/home`: rejected because it weakens the issue's "home page" requirement.
- Embed movie cards in the login page: rejected because it mixes authentication and catalog discovery responsibilities.

## Decision: Home page shows up to 6 cards

**Decision**: The home page requests and displays up to 6 movie summaries.

**Rationale**: Six cards provide a meaningful preview without duplicating the full catalog page. This also keeps mobile rendering and first-load work bounded.

**Alternatives considered**:

- Four cards: rejected because it may underrepresent the catalog on desktop.
- Twelve cards or full first page: rejected because it duplicates the catalog browsing surface.

## Decision: Use existing movie listing service boundary

**Decision**: Home page loads movies through the existing typed movie API service and does not call network APIs directly from presentation components.

**Rationale**: This follows current frontend rules and keeps server communication centralized in typed services.

**Alternatives considered**:

- Direct component network calls: rejected by repository frontend rules.
- New home-specific API service: rejected unless implementation discovers distinct home-only behavior.

## Decision: Keep home state local

**Decision**: Home loading, empty, error, and success states are local page state. Search, pagination controls, and URL query synchronization remain catalog-page concerns.

**Rationale**: The home page is a bounded discovery surface, not the full browsing interface. The catalog page already owns full browsing state.

**Alternatives considered**:

- Reuse catalog URL query state: rejected because it adds unnecessary complexity and user-visible controls to the home page.
- Add home pagination: rejected as out of scope.

## Decision: Conditional movie card extraction

**Decision**: Extract a shared movie card only if it clearly reduces duplication between home and catalog without broad styling churn.

**Rationale**: The issue suggests a reusable card when it will also be used by catalog. The catalog already has card markup; extraction is useful only if the shared component remains small and preserves each page's visual needs.

**Alternatives considered**:

- Always extract first: rejected because it can enlarge the change and force catalog refactoring before proving reuse value.
- Never extract: rejected because duplicate card behavior may create drift in navigation, poster fallback, and metadata display.

## Decision: Avoid unsupported product terminology

**Decision**: Home page copy must not include unsupported terms such as RAG, Qdrant, vector search, semantic search, CLIP, recommendation, AI ranking, or agent framework.

**Rationale**: The issue and existing frontend documentation explicitly restrict these V1 claims. The feature is catalog discovery, not AI discovery.

**Alternatives considered**:

- Use AI-themed marketing copy: rejected because it overstates current product behavior.
