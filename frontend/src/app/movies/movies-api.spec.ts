import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MovieDetails, PagedMovieSummaryResponse } from './movie.models';
import { MoviesApi } from './movies-api';

describe('MoviesApi', () => {
  let moviesApi: MoviesApi;
  let httpTestingController: HttpTestingController;

  const response: PagedMovieSummaryResponse = {
    items: [
      {
        id: 'movie-1',
        title: 'Central do Brasil',
        releaseYear: 1998,
        countryCode: 'BR',
        genres: ['Drama'],
        director: 'Walter Salles',
        posterUrl: null,
        createdAt: '2026-05-02T00:00:00Z'
      }
    ],
    page: 2,
    pageSize: 12,
    totalCount: 47,
    totalPages: 4,
    hasPreviousPage: true,
    hasNextPage: true
  };

  const detailsResponse: MovieDetails = {
    id: 'movie-1',
    title: 'Central do Brasil',
    originalTitle: 'Central Station',
    releaseYear: 1998,
    countryCode: 'BR',
    originalLanguage: 'pt-BR',
    genres: ['Drama'],
    director: 'Walter Salles',
    synopsis: 'A retired teacher and a young boy travel through Brazil in search of his father.',
    durationMinutes: 110,
    ageRating: '12',
    externalId: 666,
    posterUrl: '/p/central.jpg',
    createdAt: '2026-05-04T12:00:00Z'
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        MoviesApi,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    moviesApi = TestBed.inject(MoviesApi);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should request movies with page, pageSize, and title query parameters', () => {
    moviesApi.listMovies({ query: 'central', page: 2, pageSize: 12 }).subscribe(result => {
      expect(result).toEqual(response);
    });

    const request = httpTestingController.expectOne(request =>
      request.url === '/api/movies' &&
      request.params.get('query') === 'central' &&
      request.params.get('page') === '2' &&
      request.params.get('pageSize') === '12'
    );

    expect(request.request.method).toBe('GET');
    request.flush(response);
  });

  it('should omit blank title query parameters', () => {
    moviesApi.listMovies({ query: '   ', page: 1, pageSize: 12 }).subscribe();

    const request = httpTestingController.expectOne(request =>
      request.url === '/api/movies' &&
      !request.params.has('query') &&
      request.params.get('page') === '1' &&
      request.params.get('pageSize') === '12'
    );

    request.flush({
      ...response,
      items: [],
      page: 1,
      totalCount: 0,
      totalPages: 0,
      hasPreviousPage: false,
      hasNextPage: false
    });
  });

  it('should map the paginated movie summary response shape', () => {
    moviesApi.listMovies({ page: 2, pageSize: 12 }).subscribe(result => {
      expect(result.items[0]?.title).toBe('Central do Brasil');
      expect(result.page).toBe(2);
      expect(result.pageSize).toBe(12);
      expect(result.totalCount).toBe(47);
      expect(result.totalPages).toBe(4);
      expect(result.hasPreviousPage).toBe(true);
      expect(result.hasNextPage).toBe(true);
    });

    const request = httpTestingController.expectOne(request =>
      request.url === '/api/movies' &&
      request.params.get('page') === '2' &&
      request.params.get('pageSize') === '12'
    );

    request.flush(response);
  });

  it('should request movie details by id', () => {
    moviesApi.getMovieById('movie-1').subscribe(result => {
      expect(result).toEqual(detailsResponse);
    });

    const request = httpTestingController.expectOne('/api/movies/movie-1');
    expect(request.request.method).toBe('GET');
    request.flush(detailsResponse);
  });
});
