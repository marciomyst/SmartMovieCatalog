# Quickstart: View Movie Details

## 1) Backend verification

```bash
dotnet build SmartMovieCatalog.slnx
dotnet test SmartMovieCatalog.slnx --no-build
```

## 2) Frontend verification

```bash
cd frontend
npm test -- --watch=false
npm run build
```

## 3) Manual API checks

### Existing movie
1. Create a movie with `POST /api/movies`.
2. Call `GET /api/movies/{id}` with returned id.
3. Expect `200 application/json` with full details payload.

### Invalid id format
1. Call `GET /api/movies/not-a-guid`.
2. Expect `400 application/problem+json`.

### Missing movie
1. Call `GET /api/movies/{guid}` with unknown GUID.
2. Expect `404 application/problem+json`.

## 4) Manual frontend checks
1. Open `/catalog`.
2. Open details from available movie cards/lists.
3. Validate `/movies/{id}` renders metadata.
4. Validate loading, not-found, and error states.
