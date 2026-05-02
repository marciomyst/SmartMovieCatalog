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
