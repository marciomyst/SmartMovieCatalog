import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { MovieSummary } from '../../movies/movie.models';
import { MoviesApi } from '../../movies/movies-api';

type HomeLoadState = 'loading' | 'success' | 'empty' | 'error';

const homeMovieLimit = 6;
const tmdbImageSize = 'w342';
const tmdbImageBaseUrl = `https://image.tmdb.org/t/p/${tmdbImageSize}`;

@Component({
  selector: 'app-home-page',
  imports: [CommonModule, RouterLink],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomePage implements OnInit {
  private readonly moviesApi = inject(MoviesApi);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly loadState = signal<HomeLoadState>('loading');
  protected readonly movies = signal<MovieSummary[]>([]);
  protected readonly hasMovies = computed(() => this.movies().length > 0);

  public ngOnInit(): void {
    this.moviesApi.listMovies({ page: 1, pageSize: homeMovieLimit }).pipe(
      map(response => response.items.slice(0, homeMovieLimit)),
      catchError(() => {
        this.loadState.set('error');
        this.movies.set([]);
        return of<MovieSummary[]>([]);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(items => {
      if (this.loadState() === 'error') {
        return;
      }

      this.movies.set(items);
      this.loadState.set(items.length > 0 ? 'success' : 'empty');
    });
  }

  protected detailsLink(movie: MovieSummary): string[] {
    return ['/movies', movie.id];
  }

  protected posterImageUrl(posterPath: string): string {
    if (posterPath.startsWith('http://') || posterPath.startsWith('https://')) {
      return posterPath;
    }

    const normalizedPosterPath = posterPath.startsWith('/') ? posterPath : '/' + posterPath;
    return tmdbImageBaseUrl + normalizedPosterPath;
  }

  protected trackByMovieId(_index: number, movie: MovieSummary): string {
    return movie.id;
  }
}
