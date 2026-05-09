import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MovieDetails, MovieListQuery, PagedMovieSummaryResponse } from './movie.models';

@Injectable({
  providedIn: 'root'
})
export class MoviesApi {
  private readonly http = inject(HttpClient);

  public listMovies(query: MovieListQuery): Observable<PagedMovieSummaryResponse> {
    let params = new HttpParams()
      .set('page', query.page.toString())
      .set('pageSize', query.pageSize.toString());

    const titleQuery = query.query?.trim();
    if (titleQuery) {
      params = params.set('query', titleQuery);
    }

    return this.http.get<PagedMovieSummaryResponse>('/api/movies', { params });
  }

  public getMovieById(id: string): Observable<MovieDetails> {
    return this.http.get<MovieDetails>(`/api/movies/${id}`);
  }
}
