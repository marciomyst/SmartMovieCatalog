# Research: View Movie Details

## Decision 1: Scope of metadata
- **Decision**: Keep the details response limited to metadata already supported by current domain/API.
- **Rationale**: Avoids schema and domain expansion in issue #13 and keeps implementation aligned with existing entities/contracts.
- **Alternatives considered**:
  - Add `updatedAt` only.
  - Add `cast` and `updatedAt`.

## Decision 2: Invalid ID handling
- **Decision**: Return `400 Bad Request` with `application/problem+json` when `{id}` is not a valid GUID.
- **Rationale**: Produces explicit client feedback for malformed route input and testable API behavior.
- **Alternatives considered**:
  - Return `404 Not Found` for invalid format.
  - Leave behavior implicit to framework defaults.

## Decision 3: Image field in details contract
- **Decision**: Use `posterUrl` as the canonical details image field.
- **Rationale**: Matches existing list contract naming and keeps frontend consumption consistent.
- **Alternatives considered**:
  - Expose only relative `image` path.
  - Expose both `posterUrl` and `image`.

## Decision 4: Frontend navigation scope
- **Decision**: Ensure all existing movie card/list entry points in frontend navigate to `/movies/{id}`.
- **Rationale**: Removes UX drift and fulfills issue scope for details-page access.
- **Alternatives considered**:
  - Scope navigation changes to catalog only.
  - Split additional entry points into another issue.
