import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { NEVER, of, throwError } from 'rxjs';
import { MovieListQuery, PagedMovieSummaryResponse } from '../../movies/movie.models';
import { MoviesApi } from '../../movies/movies-api';
import { HomePage } from './home-page';

describe('HomePage', () => {
  let fixture: ComponentFixture<HomePage>;
  let moviesApi: {
    listMovies: ReturnType<typeof vi.fn<(query: MovieListQuery) => ReturnType<MoviesApi['listMovies']>>>;
  };

  function createResponse(count: number): PagedMovieSummaryResponse {
    return {
      items: Array.from({ length: count }, (_item, index) => ({
        id: `movie-${index + 1}`,
        title: `Movie ${index + 1}`,
        releaseYear: 2000 + index,
        countryCode: 'BR',
        genres: index % 2 === 0 ? ['Drama'] : [],
        director: index % 2 === 0 ? `Director ${index + 1}` : null,
        posterUrl: index % 3 === 0 ? `/p/poster-${index + 1}.jpg` : null,
        createdAt: '2026-05-09T00:00:00Z'
      })),
      page: 1,
      pageSize: 6,
      totalCount: count,
      totalPages: count > 0 ? 1 : 0,
      hasPreviousPage: false,
      hasNextPage: false
    };
  }

  beforeEach(async () => {
    moviesApi = {
      listMovies: vi.fn(() => of(createResponse(6)))
    };

    await TestBed.configureTestingModule({
      imports: [HomePage],
      providers: [
        provideRouter([]),
        { provide: MoviesApi, useValue: moviesApi }
      ]
    }).compileComponents();
  });

  function createComponent(): void {
    fixture = TestBed.createComponent(HomePage);
    fixture.detectChanges();
  }

  it('should request movies with page=1 and pageSize=6', () => {
    createComponent();

    expect(moviesApi.listMovies).toHaveBeenCalledWith({ page: 1, pageSize: 6 });
  });

  it('should render loading state while request is pending', () => {
    moviesApi.listMovies.mockReturnValue(NEVER);
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Loading movies');
  });

  it('should render exactly six cards when six or more movies are available', () => {
    moviesApi.listMovies.mockReturnValue(of(createResponse(8)));
    createComponent();

    const cards = fixture.nativeElement.querySelectorAll('.movie-card');
    expect(cards).toHaveLength(6);
  });

  it('should render fewer cards when fewer movies are available', () => {
    moviesApi.listMovies.mockReturnValue(of(createResponse(3)));
    createComponent();

    const cards = fixture.nativeElement.querySelectorAll('.movie-card');
    expect(cards).toHaveLength(3);
  });

  it('should render a poster placeholder when posterUrl is absent', () => {
    moviesApi.listMovies.mockReturnValue(of(createResponse(2)));
    createComponent();

    const placeholder = fixture.nativeElement.querySelector('.poster-placeholder');
    expect(placeholder).toBeTruthy();
  });

  it('should render empty state when no movies are available', () => {
    moviesApi.listMovies.mockReturnValue(of(createResponse(0)));
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('No movies yet');
  });

  it('should render generic error state on request failure', () => {
    moviesApi.listMovies.mockReturnValue(throwError(() => new Error('network')));
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Catalog unavailable');
    expect(compiled.textContent).toContain('Try again in a moment.');
  });

  it('should link each card to the corresponding details route', () => {
    moviesApi.listMovies.mockReturnValue(of(createResponse(2)));
    createComponent();

    const link = fixture.nativeElement.querySelector('.movie-card') as HTMLAnchorElement;
    expect(link.getAttribute('href')).toBe('/movies/movie-1');
    expect(link.getAttribute('aria-label')).toContain('Open details for Movie 1');
  });

  it('should render cards as keyboard and pointer accessible links', () => {
    moviesApi.listMovies.mockReturnValue(of(createResponse(2)));
    createComponent();

    const links = fixture.nativeElement.querySelectorAll('.movie-card');
    expect(links).toHaveLength(2);

    for (const link of links) {
      expect(link.tagName).toBe('A');
      expect(link.getAttribute('href')).toContain('/movies/');
      expect(link.getAttribute('aria-label')).toContain('Open details for');
    }
  });

  it('should avoid unsupported product terminology in home copy', () => {
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    const text = (compiled.textContent ?? '').toLowerCase();

    expect(text).not.toContain('rag');
    expect(text).not.toContain('qdrant');
    expect(text).not.toContain('vector search');
    expect(text).not.toContain('semantic search');
    expect(text).not.toContain('clip');
    expect(text).not.toContain('agent framework');
    expect(text).not.toContain('recommendation');
  });
});
