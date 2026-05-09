import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { ActivatedRoute, convertToParamMap, ParamMap, Router, provideRouter } from '@angular/router';
import { BehaviorSubject, NEVER, of, throwError } from 'rxjs';
import { MovieListQuery, PagedMovieSummaryResponse } from '../../movies/movie.models';
import { MoviesApi } from '../../movies/movies-api';
import { CatalogPage } from './catalog-page';

describe('CatalogPage', () => {
  let fixture: ComponentFixture<CatalogPage>;
  let queryParamMap: BehaviorSubject<ParamMap>;
  let moviesApi: {
    listMovies: ReturnType<typeof vi.fn<(query: MovieListQuery) => ReturnType<MoviesApi['listMovies']>>>;
  };
  let router: Router;

  const movieResponse: PagedMovieSummaryResponse = {
    items: [
      {
        id: 'movie-1',
        title: 'Central do Brasil',
        releaseYear: 1998,
        countryCode: 'BR',
        genres: ['Drama'],
        director: 'Walter Salles',
        posterUrl: '/p/central.jpg',
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

  const emptyResponse: PagedMovieSummaryResponse = {
    items: [],
    page: 1,
    pageSize: 12,
    totalCount: 0,
    totalPages: 0,
    hasPreviousPage: false,
    hasNextPage: false
  };

  beforeEach(async () => {
    queryParamMap = new BehaviorSubject<ParamMap>(convertToParamMap({}));
    moviesApi = {
      listMovies: vi.fn(() => of(movieResponse))
    };

    await TestBed.configureTestingModule({
      imports: [CatalogPage],
      providers: [
        provideRouter([]),
        {
          provide: ActivatedRoute,
          useValue: {
            queryParamMap: queryParamMap.asObservable()
          }
        },
        { provide: MoviesApi, useValue: moviesApi }
      ]
    }).compileComponents();

    router = TestBed.inject(Router);
    vi.spyOn(router, 'navigate').mockResolvedValue(true);
  });

  function createComponent(): void {
    fixture = TestBed.createComponent(CatalogPage);
    fixture.detectChanges();
  }

  it('should render the initial catalog page shell', () => {
    moviesApi.listMovies.mockReturnValue(NEVER);
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Catalog');
    expect(compiled.querySelector('#movie-search')).toBeTruthy();
    expect(compiled.textContent).toContain('Loading catalog');
  });

  it('should request and render movie summaries from MoviesApi', () => {
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(moviesApi.listMovies).toHaveBeenCalledWith({ page: 1, pageSize: 12, query: undefined });
    expect(compiled.textContent).toContain('Central do Brasil');
    expect(compiled.textContent).toContain('Walter Salles');
  });

  it('should render TMDB poster image URLs for poster paths', () => {
    createComponent();

    const image = fixture.nativeElement.querySelector('.poster-frame img') as HTMLImageElement;
    expect(image.src).toBe('https://image.tmdb.org/t/p/w342/p/central.jpg');
    expect(image.alt).toBe('Central do Brasil poster');
  });

  it('should use MoviesApi as the catalog API boundary', () => {
    createComponent();

    expect(moviesApi.listMovies).toHaveBeenCalledTimes(1);
  });

  it('should read query, page, and pageSize from URL query parameters', () => {
    queryParamMap.next(convertToParamMap({
      query: 'central',
      page: '2',
      pageSize: '24'
    }));
    createComponent();

    const input = fixture.nativeElement.querySelector('#movie-search') as HTMLInputElement;
    expect(input.value).toBe('central');
    expect(moviesApi.listMovies).toHaveBeenCalledWith({
      query: 'central',
      page: 2,
      pageSize: 24
    });
  });

  it('should update URL query parameters and reset page when searching', () => {
    queryParamMap.next(convertToParamMap({
      query: 'central',
      page: '2',
      pageSize: '12'
    }));
    createComponent();

    const input = fixture.nativeElement.querySelector('#movie-search') as HTMLInputElement;
    input.value = 'matrix';
    input.dispatchEvent(new Event('input'));

    fixture.debugElement.query(By.css('form')).triggerEventHandler('ngSubmit');

    expect(router.navigate).toHaveBeenCalledWith([], {
      relativeTo: expect.anything(),
      queryParams: {
        query: 'matrix',
        page: 1,
        pageSize: 12
      },
      queryParamsHandling: 'merge'
    });
  });

  it('should omit blank search query from URL state', () => {
    createComponent();

    const input = fixture.nativeElement.querySelector('#movie-search') as HTMLInputElement;
    input.value = '   ';
    input.dispatchEvent(new Event('input'));

    fixture.debugElement.query(By.css('form')).triggerEventHandler('ngSubmit');

    expect(router.navigate).toHaveBeenCalledWith([], {
      relativeTo: expect.anything(),
      queryParams: {
        query: null,
        page: 1,
        pageSize: 12
      },
      queryParamsHandling: 'merge'
    });
  });

  it('should render empty catalog state for an empty default list', () => {
    moviesApi.listMovies.mockReturnValue(of(emptyResponse));
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('No movies yet');
  });

  it('should render no-result state for an empty title search', () => {
    queryParamMap.next(convertToParamMap({ query: 'missing' }));
    moviesApi.listMovies.mockReturnValue(of(emptyResponse));
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('No results');
  });

  it('should render generic API error state', () => {
    moviesApi.listMovies.mockReturnValue(throwError(() => new Error('failed')));
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Catalog unavailable');
    expect(compiled.textContent).toContain('Try again in a moment.');
  });

  it('should update URL state through pagination controls', () => {
    queryParamMap.next(convertToParamMap({
      query: 'central',
      page: '2',
      pageSize: '12'
    }));
    createComponent();

    const buttons = fixture.nativeElement.querySelectorAll('.pagination button') as NodeListOf<HTMLButtonElement>;
    buttons[1]?.click();

    expect(router.navigate).toHaveBeenCalledWith([], {
      relativeTo: expect.anything(),
      queryParams: {
        query: 'central',
        page: 3,
        pageSize: 12
      },
      queryParamsHandling: 'merge'
    });
  });

  it('should normalize invalid URL page and pageSize before API calls', () => {
    queryParamMap.next(convertToParamMap({
      page: 'zero',
      pageSize: '101'
    }));
    createComponent();

    expect(moviesApi.listMovies).toHaveBeenCalledWith({
      query: undefined,
      page: 1,
      pageSize: 12
    });
  });

  it('should not render a page-size selector', () => {
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('select')).toBeNull();
  });

  it('should link catalog items to movie details', () => {
    createComponent();

    const link = fixture.nativeElement.querySelector('.movie-card') as HTMLAnchorElement;
    expect(link.getAttribute('href')).toBe('/movies/movie-1');
  });

  it('should render singular result summary when only one movie is available', () => {
    moviesApi.listMovies.mockReturnValue(of({
      ...movieResponse,
      totalCount: 1
    }));
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('1 movie');
  });

  it('should not navigate to previous/next page when pagination is unavailable', () => {
    moviesApi.listMovies.mockReturnValue(of({
      ...movieResponse,
      hasPreviousPage: false,
      hasNextPage: false
    }));
    createComponent();

    const component = fixture.componentInstance as unknown as {
      previousPage: () => void;
      nextPage: () => void;
    };

    component.previousPage();
    component.nextPage();

    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should keep absolute poster URLs unchanged', () => {
    moviesApi.listMovies.mockReturnValue(of({
      ...movieResponse,
      items: [{
        ...movieResponse.items[0],
        posterUrl: 'https://image.tmdb.org/t/p/w342/external.jpg'
      }]
    }));
    createComponent();

    const image = fixture.nativeElement.querySelector('.poster-frame img') as HTMLImageElement;
    expect(image.src).toBe('https://image.tmdb.org/t/p/w342/external.jpg');
  });

  it('should avoid duplicate API calls when query state does not change', () => {
    createComponent();
    moviesApi.listMovies.mockClear();

    queryParamMap.next(convertToParamMap({
      query: '',
      page: '1',
      pageSize: '12'
    }));

    expect(moviesApi.listMovies).not.toHaveBeenCalled();
  });
});
