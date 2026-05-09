# Feature Specification: Home Movie Cards

**Feature Branch**: `017-home-movie-cards`  
**Created**: 2026-05-09  
**Status**: Draft  
**Input**: GitHub issue #17: "Movie cards on the home page" - https://github.com/marciomyst/SmartMovieCatalog/issues/17

## Clarifications

### Session 2026-05-09

- Q: How should the product home page coexist with the current login screen at the root route? -> A: Make `/` the public home page with movie cards, and move login to `/login`.
- Q: How many movie cards should the home page show initially? -> A: Show 6 movie cards.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Browse Movies From Home (Priority: P1)

As a visitor opening the application, I want the home page to show real movie cards so that I can immediately discover catalog content without first navigating to a separate browsing page.

**Why this priority**: This is the core value of the feature. The current product entry experience does not expose catalog content on the home page.

**Independent Test**: Seed or mock catalog data, open the home page, and verify that movie cards are rendered with real movie summary data and no scaffold/sample-only content.

**Acceptance Scenarios**:

1. **Given** movies exist in the catalog, **When** a visitor opens the home page, **Then** the visitor sees a responsive section of movie cards populated from catalog data.
2. **Given** a movie has title, release year, genres, director, and poster data, **When** its card is shown on the home page, **Then** those available values are visible without exposing unsupported product claims.
3. **Given** a movie has no poster image, **When** its card is shown, **Then** a polished poster placeholder is displayed instead of a broken image.

---

### User Story 2 - Understand Home Page States (Priority: P2)

As a visitor, I want clear loading, empty, and error states so that I understand whether the catalog is loading, empty, or temporarily unavailable.

**Why this priority**: The home page must remain credible and usable when data is unavailable or delayed.

**Independent Test**: Mock loading, empty, and failed catalog responses, then verify that each state renders a distinct user-facing message and does not display stale or misleading movie cards.

**Acceptance Scenarios**:

1. **Given** catalog data is being requested, **When** the home page is loading, **Then** a loading state is displayed in the movie section.
2. **Given** the catalog contains no movies, **When** the home page finishes loading, **Then** an empty state explains that no movies are available yet.
3. **Given** the catalog request fails, **When** the home page finishes loading, **Then** an error state is displayed without exposing internal error details.

---

### User Story 3 - Open Movie Details From Home (Priority: P3)

As a visitor, I want to click a movie card on the home page so that I can open the movie details page for that movie.

**Why this priority**: Details navigation makes the home page cards actionable and connects this feature to the catalog exploration flow.

**Independent Test**: Render home page cards with movie IDs, activate a card, and verify that the application navigates to the corresponding movie details page.

**Acceptance Scenarios**:

1. **Given** movie cards are visible on the home page, **When** a visitor activates a card, **Then** the visitor is taken to that movie's details page.
2. **Given** a visitor uses keyboard navigation, **When** focus reaches a movie card, **Then** the card can be activated without requiring a mouse.

### Edge Cases

- Catalog data is empty.
- Catalog data is slow to load.
- Catalog data request fails.
- A movie summary contains missing optional metadata such as director, genres, or poster URL.
- A poster URL fails to load or is absent.
- A long movie title, director name, or genre list must not break the card layout.
- A visitor opens the home page on a narrow mobile viewport.
- The current root route is already used by the login screen; the login screen must move to `/login` so `/` can become the public home page.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The application MUST provide a product-facing home page surface that can display movie catalog content.
- **FR-002**: The home page MUST include a movie card section populated from real catalog movie summary data.
- **FR-003**: The home page movie section MUST show 6 movie cards when at least 6 movies are available.
- **FR-004**: The home page movie section MUST display fewer than 6 movie cards when fewer than 6 movies are available but at least one movie exists.
- **FR-005**: The home page movie section MUST display available card content for each movie, including title, release year, genres, director, poster image or poster placeholder, and basic metadata.
- **FR-006**: The home page movie section MUST display a loading state while movie data is being retrieved.
- **FR-007**: The home page movie section MUST display an empty state when no movies are available.
- **FR-008**: The home page movie section MUST display an error state when movie data cannot be loaded.
- **FR-009**: Each movie card MUST provide navigation to that movie's details page.
- **FR-010**: Movie card navigation MUST support pointer and keyboard activation.
- **FR-011**: The home page movie section MUST remain responsive across desktop and mobile viewports.
- **FR-012**: The home page UI MUST follow the existing dark cinematic product direction.
- **FR-013**: The home page UI MUST NOT expose unsupported V1 terms or claims such as RAG, Qdrant, vector search, semantic search, CLIP, recommendation, or agent framework.
- **FR-014**: Presentation components MUST use the established frontend data access boundary for catalog data and MUST NOT own direct network communication.
- **FR-015**: The root route `/` MUST display the public home page with movie cards.
- **FR-016**: The login screen MUST remain reachable through `/login`.
- **FR-017**: Reusable movie card behavior SHOULD be shared with other catalog surfaces when doing so avoids meaningful duplication without broad refactoring.

### Key Entities

- **Movie Summary**: A catalog item preview shown on a card. Key visible attributes include title, release year, genres, director, country or similar basic metadata, poster image reference, and identifier for details navigation.
- **Home Movie Section**: The product-facing home page area that presents movie summaries and its loading, empty, and error states.
- **Movie Card**: A navigable visual representation of one movie summary, including poster or placeholder and available metadata.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: A visitor can open the home page and see catalog movie content or a clear state message in under 3 seconds under normal local development conditions.
- **SC-002**: The home page shows 6 movie cards when at least 6 movies are available.
- **SC-003**: 100% of rendered home page movie cards navigate to the matching details page when activated.
- **SC-004**: The home page movie section presents loading, empty, and error states with distinct user-facing messages.
- **SC-005**: The home page movie section remains usable at common mobile and desktop viewport widths without overlapping text or broken card layout.
- **SC-006**: No unsupported V1 AI, RAG, vector, semantic search, CLIP, recommendation, Qdrant, or agent terminology appears in the home page card experience.

## Assumptions

- "Home page" means the public root route `/`, not the existing catalog page.
- The current catalog listing behavior provides the movie summary data needed for home page cards.
- The default catalog ordering is acceptable for "recently added" display unless a dedicated recent-movies behavior is defined later.
- The initial home page movie section shows up to 6 movies rather than full catalog pagination.
- The movie details page route from issue #13 is available for card navigation.
- The catalog page from issue #18 remains available as the full browsing surface.
- Authentication remains reachable through `/login`.

## Dependencies

- Existing movie listing data and movie summary shape.
- Existing movie details route backed by issue #13.
- Existing catalog UI behavior from issue #18 that already displays movie cards and state handling.
- Existing frontend visual system and dark cinematic design direction.

## Conflicts

- `docs/FRONTEND.md` currently states that `/` displays the authentication login page, while issue #17 asks for movie cards on the home page. This feature resolves the conflict by making `/` the public home page and moving login to `/login`.

## Out of Scope

- Poster upload.
- Flip-card AI analysis back side.
- AI analyzed badges.
- Recommendations.
- Infinite scrolling.
- Advanced filters.
- Authentication redesign.
- Backend contract changes unless the existing movie listing behavior is insufficient for home page cards.
