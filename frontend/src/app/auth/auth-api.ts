import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticateRequest, AuthenticateResponse, CurrentUserResponse } from './auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthApi {
  private readonly http = inject(HttpClient);

  public authenticate(request: AuthenticateRequest): Observable<AuthenticateResponse> {
    return this.http.post<AuthenticateResponse>('/api/auth/authenticate', request);
  }

  public getCurrentUser(accessToken: string): Observable<CurrentUserResponse> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${accessToken}`
    });

    return this.http.get<CurrentUserResponse>('/api/auth/me', { headers });
  }
}
