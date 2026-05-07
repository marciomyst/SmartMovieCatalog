import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { of } from 'rxjs';
import { App } from './app';
import { LoginPage } from './auth/login-page/login-page';
import { CatalogPage } from './catalog/catalog-page/catalog-page';
import { PagedMovieSummaryResponse } from './movies/movie.models';
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

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideRouter([
          { path: '', component: LoginPage },
          { path: 'catalog', component: CatalogPage }
        ]),
        {
          provide: MoviesApi,
          useValue: {
            listMovies: vi.fn(() => of(emptyResponse))
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
});
