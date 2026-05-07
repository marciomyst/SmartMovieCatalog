# Data Model: Catalog Page

This feature adds frontend TypeScript models and UI state only. It does not add backend domain or persistence entities.

## MovieSummary

**Purpose**: Lightweight movie item rendered in the catalog grid/list.

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| `id` | string | Yes | Used for details route generation |
| `title` | string | Yes | Primary card title |
| `releaseYear` | number | Yes | Display metadata |
| `countryCode` | string | Yes | Display metadata when useful |
| `genres` | string[] | Yes | May be empty |
| `director` | string or null | No | Display when present |
| `posterUrl` | string or null | No | Use placeholder when absent |
| `createdAt` | string | Yes | ISO-like API timestamp from issue #12 |

## PagedMovieSummaryResponse

**Purpose**: API response consumed by the catalog page.

| Field | Type | Required | Validation / Use |
|-------|------|----------|------------------|
| `items` | `MovieSummary[]` | Yes | Rendered as catalog results |
| `page` | number | Yes | Current API page |
| `pageSize` | number | Yes | Current API page size |
| `totalCount` | number | Yes | Used for result summary |
| `totalPages` | number | Yes | Used for pagination display |
| `hasPreviousPage` | boolean | Yes | Enables previous control |
| `hasNextPage` | boolean | Yes | Enables next control |

## MovieListQuery

**Purpose**: Normalized request state passed to `MoviesApi`.

| Field | Type | Required | Validation / Normalization |
|-------|------|----------|----------------------------|
| `query` | string or undefined | No | Trim; omit when blank |
| `page` | number | Yes | Positive integer; default `1` |
| `pageSize` | number | Yes | Positive integer; default `12` |

## CatalogViewState

**Purpose**: UI state represented by URL query parameters and component state.

| Field | Type | Required | Source |
|-------|------|----------|--------|
| `query` | string | Yes | URL query parameter `query`; defaults to empty string |
| `page` | number | Yes | URL query parameter `page`; defaults to `1` |
| `pageSize` | number | Yes | URL query parameter `pageSize`; defaults to `12` |

## CatalogLoadState

**Purpose**: User-observable state for the catalog page.

Allowed states:

- `loading`
- `success`
- `emptyCatalog`
- `noResults`
- `error`

State rules:

- `emptyCatalog`: empty query and `totalCount` is `0`.
- `noResults`: non-empty query and no returned items.
- `error`: API request fails or cannot be processed.
- `success`: one or more movies are available for rendering.
