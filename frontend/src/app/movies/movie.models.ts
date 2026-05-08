export interface MovieSummary {
  id: string;
  title: string;
  releaseYear: number;
  countryCode: string;
  genres: string[];
  director: string | null;
  posterUrl: string | null;
  createdAt: string;
}

export interface PagedMovieSummaryResponse {
  items: MovieSummary[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface MovieListQuery {
  query?: string;
  page: number;
  pageSize: number;
}

export interface CatalogViewState {
  query: string;
  page: number;
  pageSize: number;
}

export type CatalogLoadState = 'loading' | 'success' | 'emptyCatalog' | 'noResults' | 'error';
