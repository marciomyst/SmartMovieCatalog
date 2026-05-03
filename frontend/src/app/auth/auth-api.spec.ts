import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AuthApi } from './auth-api';
import { AuthenticateResponse, CurrentUserResponse } from './auth.models';

describe('AuthApi', () => {
  let authApi: AuthApi;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AuthApi,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    authApi = TestBed.inject(AuthApi);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should post credentials to the authenticate endpoint', () => {
    const response: AuthenticateResponse = {
      userId: '00000000-0000-0000-0000-000000000001',
      email: 'admin@example.com',
      accessToken: 'access-token',
      accessTokenExpiresAtUtc: '2026-05-03T20:00:00Z'
    };

    authApi.authenticate({ email: 'admin@example.com', password: 'Password123!' }).subscribe(result => {
      expect(result).toEqual(response);
    });

    const request = httpTestingController.expectOne('/api/auth/authenticate');
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({
      email: 'admin@example.com',
      password: 'Password123!'
    });
    request.flush(response);
  });

  it('should send the bearer token when requesting the current user', () => {
    const response: CurrentUserResponse = {
      userId: '00000000-0000-0000-0000-000000000001',
      email: 'admin@example.com',
      name: 'Admin',
      roles: ['Admin'],
      mustChangePasswordOnFirstLogin: false
    };

    authApi.getCurrentUser('access-token').subscribe(result => {
      expect(result).toEqual(response);
    });

    const request = httpTestingController.expectOne('/api/auth/me');
    expect(request.request.method).toBe('GET');
    expect(request.request.headers.get('Authorization')).toBe('Bearer access-token');
    request.flush(response);
  });
});
