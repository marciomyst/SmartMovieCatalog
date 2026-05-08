# Quickstart: Normalize Movie Genres

## Verify

1. Build the backend solution:

```powershell
dotnet build SmartMovieCatalog.slnx
```

2. Create a movie with genres through the existing movie endpoint.

3. Confirm the response still returns genre names:

```json
{
  "genres": ["Drama"]
}
```

4. Confirm persistence shape:

- `Genres` contains one row per normalized genre name.
- `MovieGenres` contains only `MovieId` and `GenreId` relationship data.
- `MovieGenres` does not store a genre name column.

## Out Of Scope

- Genre management endpoints.
- Frontend changes.
- TMDB genre synchronization.
- Recommendation or filtering behavior.
