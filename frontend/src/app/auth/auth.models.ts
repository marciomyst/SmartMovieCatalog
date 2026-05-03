export interface AuthenticateRequest {
  email: string;
  password: string;
}

export interface AuthenticateResponse {
  userId: string;
  email: string;
  accessToken: string;
  accessTokenExpiresAtUtc: string;
}

export interface CurrentUserResponse {
  userId: string;
  email: string;
  name: string;
  roles: string[];
  mustChangePasswordOnFirstLogin: boolean;
}

export interface AuthSession {
  accessToken: string;
  accessTokenExpiresAtUtc: string;
  currentUser: CurrentUserResponse;
}

export interface ValidationProblemDetails {
  errors?: Record<string, string[]>;
}
