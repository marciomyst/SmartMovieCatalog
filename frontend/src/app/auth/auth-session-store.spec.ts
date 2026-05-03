import { TestBed } from '@angular/core/testing';
import { AuthSessionStore } from './auth-session-store';
import { AuthenticateResponse, CurrentUserResponse } from './auth.models';

describe('AuthSessionStore', () => {
  afterEach(() => {
    vi.restoreAllMocks();
  });

  it('should keep the authenticated session in memory only', () => {
    const storageSetItem = vi.spyOn(Storage.prototype, 'setItem');
    TestBed.configureTestingModule({});
    const store = TestBed.inject(AuthSessionStore);
    const authenticateResponse: AuthenticateResponse = {
      userId: '00000000-0000-0000-0000-000000000001',
      email: 'admin@example.com',
      accessToken: 'access-token',
      accessTokenExpiresAtUtc: '2026-05-03T20:00:00Z'
    };
    const currentUser: CurrentUserResponse = {
      userId: authenticateResponse.userId,
      email: authenticateResponse.email,
      name: 'Admin',
      roles: ['Admin'],
      mustChangePasswordOnFirstLogin: false
    };

    store.setSession(authenticateResponse, currentUser);

    expect(store.isAuthenticated()).toBe(true);
    expect(store.session()).toEqual({
      accessToken: 'access-token',
      accessTokenExpiresAtUtc: '2026-05-03T20:00:00Z',
      currentUser
    });
    expect(storageSetItem).not.toHaveBeenCalled();
    expect(window.localStorage.getItem('accessToken')).toBeNull();
    expect(window.sessionStorage.getItem('accessToken')).toBeNull();
  });
});
