import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpErrorResponse } from '@angular/common/http';
import { of, throwError } from 'rxjs';
import { AuthApi } from '../auth-api';
import { AuthenticateRequest, AuthenticateResponse, CurrentUserResponse } from '../auth.models';
import { AuthSessionStore } from '../auth-session-store';
import { LoginPage } from './login-page';

describe('LoginPage', () => {
  let fixture: ComponentFixture<LoginPage>;
  let authApi: {
    authenticate: ReturnType<typeof vi.fn<(request: AuthenticateRequest) => ReturnType<AuthApi['authenticate']>>>;
    getCurrentUser: ReturnType<typeof vi.fn<(accessToken: string) => ReturnType<AuthApi['getCurrentUser']>>>;
  };
  let authSessionStore: {
    setSession: ReturnType<typeof vi.fn<(auth: AuthenticateResponse, currentUser: CurrentUserResponse) => void>>;
    currentUser: () => CurrentUserResponse | null;
  };

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

  beforeEach(async () => {
    authApi = {
      authenticate: vi.fn(() => of(authenticateResponse)),
      getCurrentUser: vi.fn(() => of(currentUser))
    };
    authSessionStore = {
      setSession: vi.fn(),
      currentUser: () => null
    };

    await TestBed.configureTestingModule({
      imports: [LoginPage],
      providers: [
        { provide: AuthApi, useValue: authApi },
        { provide: AuthSessionStore, useValue: authSessionStore }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginPage);
  });

  it('should render the Smart Movie Catalog auth form', () => {
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Smart Movie Catalog');
    expect(compiled.querySelector('input[type="email"]')).toBeTruthy();
    expect(compiled.querySelector('button[type="submit"]')?.textContent).toContain('Entrar');
  });

  it('should show required validation messages after submit', () => {
    fixture.detectChanges();

    const form = fixture.nativeElement.querySelector('form') as HTMLFormElement;
    form.dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Informe seu e-mail.');
    expect(compiled.textContent).toContain('Informe sua senha.');
  });

  it('should show malformed email validation without authenticating', () => {
    fixture.detectChanges();

    const email = fixture.nativeElement.querySelector('#email') as HTMLInputElement;
    const password = fixture.nativeElement.querySelector('#password') as HTMLInputElement;
    email.value = 'not-an-email';
    email.dispatchEvent(new Event('input'));
    password.value = 'Password123!';
    password.dispatchEvent(new Event('input'));

    const form = fixture.nativeElement.querySelector('form') as HTMLFormElement;
    form.dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Use um e-mail valido.');
    expect(authApi.authenticate).not.toHaveBeenCalled();
  });

  it('should toggle password visibility', () => {
    fixture.detectChanges();

    const button = fixture.nativeElement.querySelector('.icon-button') as HTMLButtonElement;
    const input = fixture.nativeElement.querySelector('#password') as HTMLInputElement;
    expect(input.type).toBe('password');

    button.click();
    fixture.detectChanges();

    expect(input.type).toBe('text');
  });

  it('should authenticate with the API and store the session', () => {
    fixture.detectChanges();

    const email = fixture.nativeElement.querySelector('#email') as HTMLInputElement;
    const password = fixture.nativeElement.querySelector('#password') as HTMLInputElement;
    email.value = 'admin@example.com';
    email.dispatchEvent(new Event('input'));
    password.value = 'Password123!';
    password.dispatchEvent(new Event('input'));

    const form = fixture.nativeElement.querySelector('form') as HTMLFormElement;
    form.dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    expect(authApi.authenticate).toHaveBeenCalledWith({
      email: 'admin@example.com',
      password: 'Password123!'
    });
    expect(authApi.getCurrentUser).toHaveBeenCalledWith('access-token');
    expect(authSessionStore.setSession).toHaveBeenCalledWith(authenticateResponse, currentUser);
  });

  it('should not store a trusted session when current user lookup fails', () => {
    authApi.getCurrentUser.mockReturnValue(throwError(() => new HttpErrorResponse({ status: 401 })));
    fixture.detectChanges();

    const email = fixture.nativeElement.querySelector('#email') as HTMLInputElement;
    const password = fixture.nativeElement.querySelector('#password') as HTMLInputElement;
    email.value = 'admin@example.com';
    email.dispatchEvent(new Event('input'));
    password.value = 'Password123!';
    password.dispatchEvent(new Event('input'));

    const form = fixture.nativeElement.querySelector('form') as HTMLFormElement;
    form.dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    expect(authApi.authenticate).toHaveBeenCalledWith({
      email: 'admin@example.com',
      password: 'Password123!'
    });
    expect(authApi.getCurrentUser).toHaveBeenCalledWith('access-token');
    expect(authSessionStore.setSession).not.toHaveBeenCalled();
  });

  it('should show a generic authentication error for unauthorized responses', () => {
    authApi.authenticate.mockReturnValue(throwError(() => new HttpErrorResponse({ status: 401 })));
    fixture.detectChanges();

    const email = fixture.nativeElement.querySelector('#email') as HTMLInputElement;
    const password = fixture.nativeElement.querySelector('#password') as HTMLInputElement;
    email.value = 'admin@example.com';
    email.dispatchEvent(new Event('input'));
    password.value = 'wrong-password';
    password.dispatchEvent(new Event('input'));

    const form = fixture.nativeElement.querySelector('form') as HTMLFormElement;
    form.dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Nao foi possivel autenticar as credenciais.');
  });

  it('should map backend validation problem details to field messages', () => {
    authApi.authenticate.mockReturnValue(throwError(() => new HttpErrorResponse({
      status: 400,
      error: {
        errors: {
          Email: ['E-mail rejeitado pelo servidor.'],
          Password: ['Senha rejeitada pelo servidor.']
        }
      }
    })));
    fixture.detectChanges();

    const email = fixture.nativeElement.querySelector('#email') as HTMLInputElement;
    const password = fixture.nativeElement.querySelector('#password') as HTMLInputElement;
    email.value = 'admin@example.com';
    email.dispatchEvent(new Event('input'));
    password.value = 'Password123!';
    password.dispatchEvent(new Event('input'));

    const form = fixture.nativeElement.querySelector('form') as HTMLFormElement;
    form.dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('E-mail rejeitado pelo servidor.');
    expect(compiled.textContent).toContain('Senha rejeitada pelo servidor.');
    expect(compiled.textContent).toContain('Revise os dados informados.');
  });
});
