# Smart Movie Catalog - Global Monorepo Context

This repository contains an ASP.NET Core backend and an Angular frontend.

Use this file to prevent context bleeding between backend and frontend work.

## 1. Backend Boundary (.NET / C#)
- Backend files live under `backend/src`.
- Before changing backend behavior, inspect the relevant C# project and layer.
- The backend uses Clean Architecture projects: Api, Application, Domain, Infrastructure, and Contracts.
- EF Core/PostgreSQL persistence, JWT authentication, and Wolverine in-process CQRS exist in the backend. Do not assume distributed messaging, additional persistence stores, or external services unless they exist in the codebase or the user explicitly requests that architecture.
- Keep backend code focused on explicit boundaries, clear request handling, dependency injection, configuration hygiene, and nullable-safe C#.
- Do not put secrets in `appsettings*.json`, Dockerfiles, source files, or committed documentation.

## 2. Frontend Boundary (Angular)
- Frontend files live under `frontend`.
- Before changing UI, HTML, CSS, or TypeScript components, read `frontend/DESIGN.md`.
- Follow the existing Angular project conventions and dependencies in `frontend/package.json`.
- Do not introduce external component libraries, custom palettes, or unrelated visual systems without explicit approval.
- Auth UI and API access live under `frontend/src/app/auth`; components must not call `HttpClient` directly.

## 3. Generated And Vendor Paths
Do not read or modify these paths unless explicitly required:
- `node_modules/`
- `bin/`
- `obj/`
- `dist/`
- `.vs/`
- `.angular/cache/`

## 4. General Workflow
- Verify the target directory before creating files.
- Do not mix contexts: keep backend concerns in backend files and UI concerns in frontend files.
- Prefer the narrowest build or test command that validates the change.
- If a referenced design or architecture document is missing, do not invent its rules. Use the current code structure as the source of truth and ask for direction only when the missing context blocks the task.
