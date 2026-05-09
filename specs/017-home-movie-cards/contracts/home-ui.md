# UI Contract: Home Movie Cards

## Routes

### `/`

Purpose: Public product home page with a 6-card movie discovery section.

Expected behavior:

- Loads up to 6 movie summaries.
- Displays loading, empty, error, and success states.
- Displays movie cards when one or more movies are available.
- Each movie card links to `/movies/{id}`.
- Does not show unsupported V1 AI/search terminology.

### `/login`

Purpose: Authentication entry point.

Expected behavior:

- Displays the existing login screen.
- Preserves existing login behavior and generic authentication error handling.

### Existing Routes Preserved

- `/catalog`: full catalog browsing page.
- `/movies/{id}`: movie details page.

## Data Request Contract

Home page uses the existing movie listing behavior through the frontend movie API service.

Expected request semantics:

```http
GET /api/movies?page=1&pageSize=6
```

Expected response semantics:

- `items`: movie summaries to display.
- `page`: current page number.
- `pageSize`: requested page size.
- `totalCount`: total matching movies.
- `totalPages`: total available pages.
- `hasPreviousPage`: previous-page indicator.
- `hasNextPage`: next-page indicator.

The home page uses `items` for card rendering and does not expose pagination controls.

## Rendered State Contract

### Loading

Condition: movie request is pending.

Expected UI:

- A visible loading state in the movie section.
- No stale movie cards.

### Empty

Condition: request succeeds with zero movies.

Expected UI:

- A visible empty state.
- No card grid.

### Error

Condition: request fails.

Expected UI:

- A visible generic error state.
- No raw technical error details.

### Success

Condition: request succeeds with at least one movie.

Expected UI:

- One card per returned movie, up to 6 cards.
- Card content uses available title, release year, genres, director, poster/placeholder, and basic metadata.
- Each card target is `/movies/{id}`.
