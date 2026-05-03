import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize, map, switchMap } from 'rxjs';
import { AuthApi } from '../auth-api';
import { ValidationProblemDetails } from '../auth.models';
import { AuthSessionStore } from '../auth-session-store';

@Component({
  selector: 'app-login-page',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginPage {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authApi = inject(AuthApi);
  private readonly authSessionStore = inject(AuthSessionStore);

  protected readonly passwordVisible = signal(false);
  protected readonly submitted = signal(false);
  protected readonly isSubmitting = signal(false);
  protected readonly authErrorMessage = signal<string | null>(null);
  protected readonly authSuccessMessage = computed(() => {
    const currentUser = this.authSessionStore.currentUser();
    return currentUser ? `Bem-vindo, ${currentUser.name}.` : null;
  });
  protected readonly serverErrors = signal<Partial<Record<'email' | 'password', string>>>({});

  protected readonly loginForm = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email, Validators.maxLength(320)]],
    password: ['', [Validators.required]]
  });

  protected readonly passwordInputType = computed(() => this.passwordVisible() ? 'text' : 'password');
  protected readonly passwordIcon = computed(() => this.passwordVisible() ? 'visibility_off' : 'visibility');

  protected togglePasswordVisibility(): void {
    this.passwordVisible.update(value => !value);
  }

  protected submit(): void {
    this.submitted.set(true);
    this.loginForm.markAllAsTouched();
    this.authErrorMessage.set(null);
    this.serverErrors.set({});

    if (this.loginForm.invalid || this.isSubmitting()) {
      return;
    }

    this.isSubmitting.set(true);

    const credentials = this.loginForm.getRawValue();
    this.authApi.authenticate(credentials).pipe(
      switchMap(authenticateResponse =>
        this.authApi.getCurrentUser(authenticateResponse.accessToken).pipe(
          map(currentUser => ({ authenticateResponse, currentUser }))
        )
      ),
      finalize(() => this.isSubmitting.set(false))
    ).subscribe({
      next: ({ authenticateResponse, currentUser }) => {
        this.authSessionStore.setSession(authenticateResponse, currentUser);
      },
      error: error => this.handleAuthenticationError(error)
    });
  }

  protected hasError(controlName: 'email' | 'password', error: string): boolean {
    const control = this.loginForm.controls[controlName];
    return control.hasError(error) && (control.touched || this.submitted());
  }

  protected clearFieldServerError(controlName: 'email' | 'password'): void {
    const errors = this.serverErrors();
    if (!errors[controlName]) {
      return;
    }

    this.serverErrors.set({
      ...errors,
      [controlName]: undefined
    });
    this.authErrorMessage.set(null);
  }

  protected emailErrorMessage(): string | null {
    const serverMessage = this.serverErrors().email;
    if (serverMessage) {
      return serverMessage;
    }

    if (this.hasError('email', 'required')) {
      return 'Informe seu e-mail.';
    }

    if (this.hasError('email', 'email')) {
      return 'Use um e-mail valido.';
    }

    if (this.hasError('email', 'maxlength')) {
      return 'Use ate 320 caracteres.';
    }

    return null;
  }

  protected passwordErrorMessage(): string | null {
    const serverMessage = this.serverErrors().password;
    if (serverMessage) {
      return serverMessage;
    }

    if (this.hasError('password', 'required')) {
      return 'Informe sua senha.';
    }

    return null;
  }

  private handleAuthenticationError(error: unknown): void {
    if (error instanceof HttpErrorResponse && error.status === 400) {
      this.applyValidationProblem(error.error);
      return;
    }

    if (error instanceof HttpErrorResponse && error.status === 401) {
      this.authErrorMessage.set('Nao foi possivel autenticar as credenciais.');
      return;
    }

    this.authErrorMessage.set('Nao foi possivel autenticar agora.');
  }

  private applyValidationProblem(error: unknown): void {
    const validationProblem = error as ValidationProblemDetails;
    const errors = validationProblem.errors;

    this.serverErrors.set({
      email: errors?.['Email']?.[0],
      password: errors?.['Password']?.[0]
    });

    this.authErrorMessage.set('Revise os dados informados.');
  }
}
