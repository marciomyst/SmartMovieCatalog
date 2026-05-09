import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { of } from 'rxjs';
import { App } from './app';
import { LoginPage } from './auth/login-page/login-page';
import { CatalogPage } from './catalog/catalog-page/catalog-page';
import { MovieDetails, PagedMovieSummaryResponse } from './movies/movie.models';
import { MovieDetailsPage } from './movies/movie-details-page/movie-details-page';
import { MoviesApi } from './movies/movies-api';

describe('App', () => {
  let component: App;
  let fixture: ComponentFixture<App>;
  let router: Router;

  const emptyResponse: PagedMovieSummaryResponse = {
    items: [],
    page: 1,
    pageSize: 12,
    totalCount: 0,
    totalPages: 0,
    hasPreviousPage: false,
    hasNextPage: false
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

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideRouter([
          { path: '', component: LoginPage },
          { path: 'catalog', component: CatalogPage },
          { path: 'movies/:id', component: MovieDetailsPage }
        ]),
        {
          provide: MoviesApi,
          useValue: {
            listMovies: vi.fn(() => of(emptyResponse)),
            getMovieById: vi.fn(() => of(detailsResponse))
          }
        }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(App);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should render the app brand and catalog navigation link', () => {
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.brand-link')?.textContent).toContain('Smart Movie Catalog');
    expect(compiled.querySelector('a[routerLink="/catalog"]')?.textContent).toContain('Catalog');
  });

  it('should render the public catalog route without an auth guard', async () => {
    fixture.detectChanges();

    await router.navigateByUrl('/catalog');
    fixture.detectChanges();
    await fixture.whenStable();
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('app-catalog-page')).toBeTruthy();
    expect(compiled.textContent).toContain('Catalog');
  });

  it('should render the movie details route', async () => {
    fixture.detectChanges();

    await router.navigateByUrl('/movies/movie-1');
    fixture.detectChanges();
    await fixture.whenStable();
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('app-movie-details-page')).toBeTruthy();
    expect(compiled.textContent).toContain('Central do Brasil');
  });
});
