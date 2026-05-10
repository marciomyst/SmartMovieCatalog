# Feature Specification

## Feature Summary

Add basic title search to the movie listing experience so users can filter catalog movies with a simple text query. The search must remain limited to the existing movie list flow and must not introduce semantic search, vector search, or AI-assisted discovery.

Issue source: https://github.com/marciomyst/SmartMovieCatalog/issues/16

## Problem Statement

Users need a predictable way to find movies by title without relying on advanced search infrastructure. The catalog should accept a simple query, keep pagination intact, and return the same paginated movie summary shape whether the query is empty or populated.

## Goals

- Allow users to search movies by title through the existing movie listing endpoint.
- Support case-insensitive matching when the storage/query stack supports it.
- Search against movie title and original title using the same simple query.
- Keep the paginated response shape stable for empty and filtered searches.
- Expose the search input in the frontend movie list experience with loading, empty, no-result, and error states.
- Keep the feature simple, predictable, and free of AI terminology.

## Non-Goals

- Semantic search.
- Vector search.
- RAG or retrieval-augmented discovery.
- CLIP or poster similarity search.
- Recommendations or ranked discovery.
- A dedicated search endpoint.
- Full-text search infrastructure.
- Genre, year, or other advanced filters.
- Authentication changes.
- New frontend design systems or UI libraries.

## User Stories

- As a catalog user, I want to search by movie title so that I can quickly find a movie I already know.
- As a catalog user, I want pagination to continue working with search so that I can browse larger filtered result sets.
- As a catalog user, I want clear empty and error states so that I know whether the search returned no matches or failed.

## Functional Requirements

- The movie listing endpoint MUST accept an optional `query` parameter.
- Blank or whitespace-only queries MUST be treated as no filter.
- The search MUST return the default movie list when the query is empty.
- The search MUST match against movie title and original title.
- The search SHOULD be case-insensitive when supported by the persistence/query implementation.
- Search results MUST use the same paginated response shape as the unfiltered movie listing response.
- Invalid pagination or query parameters MUST return `400 Bad Request` as `application/problem+json` with the repository's invalid-request contract.
- The frontend MUST expose a typed API service for the list/search request.
- The frontend MUST render a search input where movie lists are shown.
- The frontend MUST preserve loading, empty, no-result, and error states for searched and unsearched result sets.
- The frontend MUST keep search behavior simple and predictable.
- The frontend MUST NOT expose AI, vector, semantic, or recommendation terminology for this feature.

## Acceptance Criteria

- User can search movies by title.
- Search is case-insensitive if supported by the chosen implementation.
- Empty query returns the default movie list.
- No results displays a clear empty state.
- Search works with pagination.
- Search response uses the same paginated response shape as the movie listing endpoint.
- Invalid pagination or query parameters return `400 Bad Request` using `application/problem+json` with `invalid-request`.
- Frontend uses a typed API service.
- Search does not use AI, embeddings, Qdrant, RAG, CLIP, or semantic ranking.
- API response shape remains stable.

## Edge Cases

- Query contains leading or trailing whitespace.
- Query is empty after trimming.
- Query differs only by letter case.
- Query matches a movie's original title but not its primary title.
- Query returns zero items while the catalog is otherwise populated.
- The current page becomes empty after a filtered search.
- Pagination parameters are invalid or outside the accepted range.
- The backend or database cannot translate the case-insensitive search the same way in all environments.

## Clarifications Needed

None identified.

## Assumptions

- The catalog browsing surface is the primary frontend location for search input.
- Search is triggered explicitly through the existing form interaction rather than live-as-you-type debounce.
- The existing paginated movie list contract is the public response shape.
- Original-title matching is part of the same basic search filter rather than a separate feature.

## Dependencies

- Existing movie listing endpoint and paginated movie summary contract.
- Existing movie repository and application query pipeline.
- Existing typed frontend movies API service.
- Existing catalog browsing surface in the Angular frontend.
- Related catalog browsing work in issue #18 consumes this search behavior.

## Risks

- Case-insensitive matching can behave differently across database providers or query translation paths.
- Search and pagination state can drift if the frontend URL state and API request state are not kept synchronized.
- Adding broad matches against original title may return more rows than users expect from a title-only query.
- Contract drift between the API response shape and frontend models would break the search experience.

## Out of Scope

- Advanced search ranking.
- Separate search endpoints.
- AI-driven discovery.
- Search analytics.
- Filter faceting.
- Backend architecture changes unrelated to the existing list pipeline.

## Implementation Notes

- Validation on 2026-05-10 confirmed no contract drift between specification and current implementation for `GET /api/movies` search behavior.
- Search remains scoped to basic title/original-title filtering with stable paginated response shape and typed frontend API consumption.
