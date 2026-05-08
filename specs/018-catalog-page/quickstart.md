# Quickstart: Catalog Page

## Prerequisites

- Frontend dependencies installed under `frontend`.
- Movie listing endpoint from issue #12 available:
  - `GET /api/movies?page=1&pageSize=12`
- Basic title search from issue #16 available:
  - `GET /api/movies?query=central&page=1&pageSize=12`
- Movie details route from issue #13 agreed or implemented for catalog links.

## Implementation Verification

Run frontend tests:

```powershell
cd frontend
npm test -- --watch=false
```

Run frontend build:

```powershell
cd frontend
npm run build
```

## Manual Browser Checks

Start the frontend through the normal local workflow, then verify:

```text
/catalog
/catalog?query=central&page=1&pageSize=12
/catalog?page=2&pageSize=12
```

Expected behavior:

- `/catalog` is reachable without frontend authentication.
- App navigation includes a Catalog link.
- Catalog shows loading while the API request is pending.
- Empty catalog state appears when an empty query returns zero total movies.
- No-result state appears when a non-empty query returns no items.
- Error state appears when the API request fails.
- Search updates the URL query string and resets `page` to `1`.
- Pagination updates the URL query string.
- `pageSize` defaults to `12` when absent.
- A valid URL `pageSize` in the range `1..100` is honored.
- No page-size selector appears in the V1 UI.
- Catalog item links target the movie details route.

## Out Of Scope Checks

Implementation should not add:

- Backend list/search/details endpoint code inside issue #18.
- Route guards or authentication requirements for `/catalog`.
- Genre/year/AI filters.
- Semantic search, vector search, RAG, CLIP, recommendations, or AI ranking UI.
- New UI libraries, state management frameworks, or design systems.
