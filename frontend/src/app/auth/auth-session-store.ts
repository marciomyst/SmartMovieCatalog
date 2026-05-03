import { computed, Injectable, signal } from '@angular/core';
import { AuthSession, AuthenticateResponse, CurrentUserResponse } from './auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthSessionStore {
  private readonly sessionState = signal<AuthSession | null>(null);

  public readonly session = this.sessionState.asReadonly();
  public readonly currentUser = computed(() => this.sessionState()?.currentUser ?? null);
  public readonly isAuthenticated = computed(() => this.sessionState() !== null);

  public setSession(authenticateResponse: AuthenticateResponse, currentUser: CurrentUserResponse): void {
    this.sessionState.set({
      accessToken: authenticateResponse.accessToken,
      accessTokenExpiresAtUtc: authenticateResponse.accessTokenExpiresAtUtc,
      currentUser
    });
  }

  public clear(): void {
    this.sessionState.set(null);
  }
}
