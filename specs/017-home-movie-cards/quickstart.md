# Quickstart: Home Movie Cards

## Preconditions

- The backend movie listing endpoint is available through the existing `/api/movies` behavior.
- The movie details route `/movies/{id}` is available.
- Frontend dependencies are installed in `frontend`.

## Manual Validation

1. Start the backend and frontend through the repository's normal local workflow.
2. Open the frontend root route `/`.
3. Verify the page is a public home page and not the login form.
4. Verify the home movie section shows a loading state while requesting data.
5. With at least 6 movies available, verify exactly 6 movie cards render.
6. With fewer than 6 movies available, verify only the available movies render.
7. With no movies available, verify the empty state renders.
8. Simulate an API failure and verify the generic error state renders.
9. Activate a movie card and verify navigation to `/movies/{id}`.
10. Open `/login` and verify the existing login screen still works.
11. Open `/catalog` and `/movies/{id}` and verify existing routes still work.
12. Check mobile and desktop widths for overlapping text or broken card layout.
13. Confirm the home UI does not show unsupported terms: RAG, Qdrant, vector search, semantic search, CLIP, recommendation, AI ranking, or agent framework.

## Automated Verification

Run from `frontend`:

```powershell
npm test -- --watch=false
```

Run from `frontend`:

```powershell
npm run build
```

Backend verification is only required if implementation expands scope into backend code or public API contracts.
