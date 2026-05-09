import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { catchError, distinctUntilChanged, map, of, switchMap, tap } from 'rxjs';
import { MovieDetails, MovieDetailsLoadState } from '../movie.models';
import { MoviesApi } from '../movies-api';

interface MovieDetailsLoadResult {
  state: MovieDetailsLoadState;
  movie: MovieDetails | null;
}

const tmdbImageSize = 'w500';
const tmdbImageBaseUrl = `https://image.tmdb.org/t/p/${tmdbImageSize}`;

@Component({
  selector: 'app-movie-details-page',
  imports: [CommonModule, RouterLink],
  templateUrl: './movie-details-page.html',
  styleUrl: './movie-details-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MovieDetailsPage implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly moviesApi = inject(MoviesApi);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly loadState = signal<MovieDetailsLoadState>('loading');
  protected readonly movie = signal<MovieDetails | null>(null);

  protected readonly genresLabel = computed(() => {
    const genres = this.movie()?.genres;
    return genres && genres.length > 0 ? genres.join(' / ') : 'Not informed';
  });

  protected readonly metadataCompleteness = computed(() => {
    const details = this.movie();
    if (!details) {
      return 0;
    }

    const values = [
      details.originalTitle,
      details.director,
      details.synopsis,
      details.durationMinutes,
      details.ageRating,
      details.externalId,
      details.posterUrl,
      details.genres.length > 0 ? details.genres : null
    ];

    const availableValues = values.filter(value => value !== null && value !== undefined && value !== '').length;
    return Math.round((availableValues / values.length) * 100);
  });

  public ngOnInit(): void {
    this.route.paramMap.pipe(
      map(params => params.get('id')?.trim() ?? ''),
      distinctUntilChanged(),
      tap(() => this.prepareForLoad()),
      switchMap(id => {
        if (!id) {
          return of<MovieDetailsLoadResult>({ state: 'notFound', movie: null });
        }

        return this.moviesApi.getMovieById(id).pipe(
          map(movie => ({ state: 'success' as const, movie })),
          catchError((error: unknown) => {
            if (error instanceof HttpErrorResponse && error.status === 404) {
              return of<MovieDetailsLoadResult>({ state: 'notFound', movie: null });
            }

            return of<MovieDetailsLoadResult>({ state: 'error', movie: null });
          })
        );
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(result => this.applyResult(result));
  }

  protected posterImageUrl(posterPath: string): string {
    if (posterPath.startsWith('http://') || posterPath.startsWith('https://')) {
      return posterPath;
    }

    return `${tmdbImageBaseUrl}${posterPath.startsWith('/') ? posterPath : `/${posterPath}`}`;
  }

  protected heroBackgroundStyle(posterPath: string | null): string | null {
    if (!posterPath) {
      return null;
    }

    return `url('${this.posterImageUrl(posterPath)}')`;
  }

  protected valueOrFallback(value: string | number | null | undefined): string | number {
    if (value === null || value === undefined || value === '') {
      return 'Not informed';
    }

    return value;
  }

  protected progressWidth(value: number): string {
    return `${Math.min(Math.max(value, 0), 100)}%`;
  }

  private prepareForLoad(): void {
    this.loadState.set('loading');
    this.movie.set(null);
  }

  private applyResult(result: MovieDetailsLoadResult): void {
    this.loadState.set(result.state);
    this.movie.set(result.movie);
  }
}
