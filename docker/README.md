# Docker

Docker orchestration is rooted at the repository root:

- `docker-compose.yml`
- `docker-compose.override.yml`
- `.env.example`

The backend image currently builds from `backend/src/SmartMovieCatalog.Api/Dockerfile` with repository root build context.

Use:

```powershell
docker compose up --build
```

The API is exposed on `http://localhost:5048` by default.
