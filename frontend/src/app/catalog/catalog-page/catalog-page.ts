import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, ParamMap, Router, RouterLink } from '@angular/router';
import { catchError, distinctUntilChanged, map, of, switchMap, tap } from 'rxjs';
import {
  CatalogLoadState,
  CatalogViewState,
  MovieListQuery,
  MovieSummary,
  PagedMovieSummaryResponse
} from '../../movies/movie.models';
import { MoviesApi } from '../../movies/movies-api';

interface CatalogLoadResult {
  state: CatalogViewState;
  response: PagedMovieSummaryResponse | null;
  failed: boolean;
}

@Component({
  selector: 'app-catalog-page',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './catalog-page.html',
  styleUrl: './catalog-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CatalogPage implements OnInit {
  private readonly moviesApi = inject(MoviesApi);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly searchControl = new FormControl('', { nonNullable: true });
  protected readonly viewState = signal<CatalogViewState>({
    query: '',
    page: 1,
    pageSize: 12
  });
  protected readonly loadState = signal<CatalogLoadState>('loading');
  protected readonly response = signal<PagedMovieSummaryResponse | null>(null);

  protected readonly movies = computed(() => this.response()?.items ?? []);
  protected readonly resultSummary = computed(() => {
    const currentResponse = this.response();
    if (!currentResponse) {
      return '';
    }

    const label = currentResponse.totalCount === 1 ? 'movie' : 'movies';
    return `${currentResponse.totalCount} ${label}`;
  });

  public ngOnInit(): void {
    this.route.queryParamMap.pipe(
      map(params => this.toCatalogViewState(params)),
      distinctUntilChanged((left, right) => this.sameViewState(left, right)),
      tap(state => this.prepareForLoad(state)),
      switchMap(state => this.moviesApi.listMovies(this.toMovieListQuery(state)).pipe(
        map(response => ({ state, response, failed: false })),
        catchError(() => of({ state, response: null, failed: true }))
      )),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(result => this.applyLoadResult(result));
  }

  protected submitSearch(): void {
    const query = this.searchControl.value.trim();
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        query: query || null,
        page: 1,
        pageSize: this.viewState().pageSize
      },
      queryParamsHandling: 'merge'
    });
  }

  protected previousPage(): void {
    const currentResponse = this.response();
    if (!currentResponse?.hasPreviousPage) {
      return;
    }

    this.navigateToPage(this.viewState().page - 1);
  }

  protected nextPage(): void {
    const currentResponse = this.response();
    if (!currentResponse?.hasNextPage) {
      return;
    }

    this.navigateToPage(this.viewState().page + 1);
  }

  protected detailsLink(movie: MovieSummary): string[] {
    return ['/movies', movie.id];
  }

  protected trackByMovieId(_index: number, movie: MovieSummary): string {
    return movie.id;
  }

  private navigateToPage(page: number): void {
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        page,
        pageSize: this.viewState().pageSize,
        query: this.viewState().query || null
      },
      queryParamsHandling: 'merge'
    });
  }

  private prepareForLoad(state: CatalogViewState): void {
    this.viewState.set(state);
    this.searchControl.setValue(state.query, { emitEvent: false });
    this.loadState.set('loading');
    this.response.set(null);
  }

  private applyLoadResult(result: CatalogLoadResult): void {
    if (result.failed || !result.response) {
      this.loadState.set('error');
      this.response.set(null);
      return;
    }

    this.response.set(result.response);

    if (result.response.items.length > 0) {
      this.loadState.set('success');
      return;
    }

    this.loadState.set(result.state.query ? 'noResults' : 'emptyCatalog');
  }

  private toMovieListQuery(state: CatalogViewState): MovieListQuery {
    return {
      query: state.query || undefined,
      page: state.page,
      pageSize: state.pageSize
    };
  }

  private toCatalogViewState(params: ParamMap): CatalogViewState {
    return {
      query: params.get('query')?.trim() ?? '',
      page: this.parsePositiveInteger(params.get('page'), 1),
      pageSize: this.parsePositiveInteger(params.get('pageSize'), 12)
    };
  }

  private parsePositiveInteger(value: string | null, fallback: number): number {
    if (!value) {
      return fallback;
    }

    const parsed = Number(value);
    return Number.isInteger(parsed) && parsed >= 1 ? parsed : fallback;
  }

  private sameViewState(left: CatalogViewState, right: CatalogViewState): boolean {
    return left.query === right.query &&
      left.page === right.page &&
      left.pageSize === right.pageSize;
  }
}
