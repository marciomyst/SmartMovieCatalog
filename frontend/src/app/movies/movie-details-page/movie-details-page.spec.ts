import { HttpErrorResponse } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, ParamMap, provideRouter } from '@angular/router';
import { BehaviorSubject, NEVER, of, throwError } from 'rxjs';
import { MovieDetails } from '../movie.models';
import { MoviesApi } from '../movies-api';
import { MovieDetailsPage } from './movie-details-page';

describe('MovieDetailsPage', () => {
  let fixture: ComponentFixture<MovieDetailsPage>;
  let paramMap: BehaviorSubject<ParamMap>;
  let moviesApi: {
    getMovieById: ReturnType<typeof vi.fn<(id: string) => ReturnType<MoviesApi['getMovieById']>>>;
  };

  const details: MovieDetails = {
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

  beforeEach(async () => {
    paramMap = new BehaviorSubject<ParamMap>(convertToParamMap({ id: 'movie-1' }));
    moviesApi = {
      getMovieById: vi.fn(() => of(details))
    };

    await TestBed.configureTestingModule({
      imports: [MovieDetailsPage],
      providers: [
        provideRouter([]),
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: paramMap.asObservable()
          }
        },
        { provide: MoviesApi, useValue: moviesApi }
      ]
    }).compileComponents();
  });

  function createComponent(): void {
    fixture = TestBed.createComponent(MovieDetailsPage);
    fixture.detectChanges();
  }

  it('should request details by route movie id and render success state', () => {
    createComponent();

    expect(moviesApi.getMovieById).toHaveBeenCalledWith('movie-1');
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Central do Brasil');
    expect(compiled.textContent).toContain('Walter Salles');
    expect(compiled.textContent).toContain('A retired teacher and a young boy');
  });

  it('should render loading state while waiting for response', () => {
    moviesApi.getMovieById.mockReturnValue(NEVER);
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Loading movie details');
  });

  it('should render not-found state for 404 responses', () => {
    moviesApi.getMovieById.mockReturnValue(throwError(() => new HttpErrorResponse({
      status: 404,
      statusText: 'Not Found'
    })));
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Movie not found');
  });

  it('should render generic error state for non-404 failures', () => {
    moviesApi.getMovieById.mockReturnValue(throwError(() => new HttpErrorResponse({
      status: 500,
      statusText: 'Server Error'
    })));
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Details unavailable');
    expect(compiled.textContent).toContain('Try again in a moment.');
  });

  it('should convert poster path to TMDB URL', () => {
    createComponent();

    const image = fixture.nativeElement.querySelector('.hero-poster img') as HTMLImageElement;
    expect(image.src).toBe('https://image.tmdb.org/t/p/w500/p/central.jpg');
  });

  it('should keep absolute poster URL unchanged', () => {
    moviesApi.getMovieById.mockReturnValue(of({
      ...details,
      posterUrl: 'https://image.tmdb.org/t/p/w500/direct.jpg'
    }));
    createComponent();

    const image = fixture.nativeElement.querySelector('.hero-poster img') as HTMLImageElement;
    expect(image.src).toBe('https://image.tmdb.org/t/p/w500/direct.jpg');
  });

  it('should render not-found state when route id is empty', () => {
    paramMap.next(convertToParamMap({ id: '   ' }));
    createComponent();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Movie not found');
  });

  it('should expose fallback values for missing metadata helpers', () => {
    moviesApi.getMovieById.mockReturnValue(of({
      ...details,
      originalTitle: null,
      director: null,
      synopsis: null,
      durationMinutes: null,
      ageRating: null,
      externalId: null,
      posterUrl: null,
      genres: []
    }));
    createComponent();

    const component = fixture.componentInstance as unknown as {
      metadataCompleteness: () => number;
      heroBackgroundStyle: (value: string | null) => string | null;
      valueOrFallback: (value: string | number | null | undefined) => string | number;
    };

    expect(component.metadataCompleteness()).toBe(0);
    expect(component.heroBackgroundStyle(null)).toBeNull();
    expect(component.valueOrFallback('')).toBe('Not informed');
  });
});
