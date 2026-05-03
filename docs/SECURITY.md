# Security

## Secret Handling
- Never commit secrets, tokens, connection strings, private keys, certificates, or credentials.
- Do not place secrets in `appsettings*.json`, source files, Dockerfiles, frontend environment files, or documentation.
- Use development user-secrets or environment variables for local secret material.
- Treat generated logs and diagnostic output as potentially sensitive.

## Configuration
- Keep development and production settings clearly separated.
- Do not expose internal URLs, credentials, or infrastructure details to the frontend bundle.
- Validate configuration at startup when missing or invalid settings could cause unsafe behavior.

## API Security
- Validate all request input at the API boundary.
- Do not leak stack traces, provider payloads, connection strings, or internal exception details in HTTP responses.
- Use consistent authorization checks once authenticated features exist.
- Avoid returning data that belongs to another user or tenant.

## Backend Authentication
- Local users authenticate with email/password through `POST /api/auth/authenticate`.
- Passwords are persisted only as opaque PBKDF2-SHA256 hashes with per-password salts; plaintext passwords must never be stored, logged, returned, or placed in configuration.
- JWT settings must be supplied through environment variables, user-secrets, or another non-versioned provider:
  - `Jwt:Issuer`
  - `Jwt:Audience`
  - `Jwt:SigningKey`
  - `Jwt:AccessTokenLifetimeMinutes`
- `Jwt:SigningKey` must not be committed and must be at least 32 bytes.
- Database connection strings must use `ConnectionStrings:DefaultConnection` from environment variables or user-secrets.
- Authentication failures intentionally return generic `401 Unauthorized` responses and must not disclose whether an email exists, a password is wrong, or a user is inactive or removed.
- Authentication logs must not contain passwords, password hashes, bearer tokens, signing keys, raw authorization headers, or account-enumeration details.
- Development/Test seed users may be provisioned only from non-versioned configuration. Production users are provisioned operationally outside application startup and migrations.

## Frontend Security
- Do not store long-lived secrets in browser storage.
- Treat all data from APIs, URLs, and user input as untrusted.
- Avoid direct DOM injection. If HTML rendering is required, document sanitization.
- Do not expose AI provider prompts, credentials, or sensitive internal policies in the client.

## AI-Specific Security
- Treat AI output as untrusted data.
- Validate and constrain AI-generated structured output before persistence or display.
- Avoid sending secrets, private user data, or unnecessary internal data to AI providers.
- Record provenance for AI-generated metadata when the product starts persisting it.

## Dependency Hygiene
- Prefer existing dependencies.
- Add new packages only when they provide clear value and have acceptable maintenance and security posture.
- Do not modify generated or vendored dependency directories directly.
