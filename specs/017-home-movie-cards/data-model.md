# Data Model: Home Movie Cards

## HomeMovieSection

Represents the movie discovery area on the public home page.

### Fields

- `loadState`: current section state.
- `movies`: ordered collection of movie summaries displayed as cards.
- `maxCards`: fixed value of 6 for this feature.

### Rules

- Shows loading state while data is being retrieved.
- Shows empty state when the catalog contains no movies.
- Shows error state when data cannot be loaded.
- Shows success state when at least one movie summary is available.
- Displays at most 6 movie summaries.

## HomeLoadState

Represents the user-visible state of the home movie section.

### Values

- `loading`: data request is pending.
- `success`: at least one movie is available.
- `empty`: no movies are available.
- `error`: movie data cannot be loaded.

### Transitions

- Initial state is `loading`.
- `loading` -> `success` when the request returns one or more movies.
- `loading` -> `empty` when the request returns zero movies.
- `loading` -> `error` when the request fails.

## MovieSummary

Represents a movie preview returned by the existing movie listing behavior.

### Fields Used By This Feature

- `id`: stable movie identifier used for details navigation.
- `title`: primary card title.
- `releaseYear`: visible year metadata.
- `countryCode`: visible basic metadata when available.
- `genres`: visible genre labels when available.
- `director`: visible director metadata when available.
- `posterUrl`: poster image reference when available.

### Rules

- `id` and `title` are required for a navigable card.
- Optional fields may be absent and must not break layout.
- `posterUrl` may be absent; card must show a placeholder.

## MovieCard

Represents one navigable home card.

### Fields

- `movie`: movie summary represented by the card.
- `detailsTarget`: route target for the movie details page.
- `posterDisplay`: either poster image or placeholder.

### Rules

- Activating the card navigates to `/movies/{id}`.
- Card must be pointer and keyboard accessible.
- Card text must fit on mobile and desktop without overlapping content.
- Card must not expose unsupported V1 AI/search terminology.
