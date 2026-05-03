# GitHub Issue Context

## Source

- Repository: marciomyst/SmartMovieCatalog
- Issue: #21
- URL: https://github.com/marciomyst/SmartMovieCatalog/issues/21
- State: OPEN
- Created: 05/03/2026 00:59:57
- Updated: 05/03/2026 01:00:54
- Milestone: M1 — Core Movie Catalog

## Title

Backend authentication with JWT and current user context

## Labels

- type:feature
- priority:high
- area:api
- area:backend
- area:database
- area:architecture
- area:security
- needs:adr

## Assignees

- marciomyst



## Issue Body

## Descricao
Implementar autenticacao backend no SmartMovieCatalog com `POST /api/auth/authenticate` e `GET /api/auth/me`, adaptando o desenho analisado em `MercaSafra` para a arquitetura Clean Architecture deste repositorio.

O escopo e somente backend API. Nao incluir frontend Angular nesta issue.

## Escopo Tecnico
- Aceitar a estrategia de autenticacao em `docs/adr/0002-authentication-strategy.md`: autenticacao local com email/senha e JWT bearer.
- Aceitar a estrategia de persistencia em `docs/adr/0003-database-strategy.md`: EF Core com PostgreSQL usando `ConnectionStrings:DefaultConnection`.
- Definir/atualizar contrato de erro em `docs/adr/0004-api-error-contract.md`.
- Adicionar entidade/aggregate de usuario no dominio, abstracoes de aplicacao e implementacoes de infraestrutura.
- Configurar EF Core, migrations iniciais, hashing de senha, geracao JWT e leitura do usuario autenticado.
- Configurar `AddAuthentication().AddJwtBearer(...)` e `UseAuthentication()` antes de `UseAuthorization()`.

## API Contracts
### `POST /api/auth/authenticate`
Request:
```json
{ "email": "user@example.com", "password": "Password123!" }
```

Responses:
- `200 OK`: `{ userId, email, accessToken, accessTokenExpiresAtUtc }`
- `400 Bad Request`: entrada invalida
- `401 Unauthorized`: credenciais invalidas, usuario inexistente ou inativo

### `GET /api/auth/me`
Requer bearer token.

Responses:
- `200 OK`: `{ userId, email, name, roles, mustChangePasswordOnFirstLogin }`
- `401 Unauthorized`: token ausente/invalido ou usuario inexistente/inativo

## Fora De Escopo
- Frontend login/session UI.
- Refresh token.
- Registro de usuario.
- Recuperacao de senha.
- Provedor externo de identidade.
- Organizacoes/tenancy.
- Autorizacao granular alem de roles basicas no token.

## Documentacao
Atualizar:
- `docs/adr/0002-authentication-strategy.md`
- `docs/adr/0003-database-strategy.md`
- `docs/adr/0004-api-error-contract.md`
- `docs/API.md`
- `docs/SECURITY.md`
- `README.md`
- `backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.http`

## Testes
Application tests:
- autenticacao valida retorna token;
- senha invalida retorna falha nao autenticada;
- usuario inexistente/inativo nao autentica;
- contexto atual exige usuario autenticado;
- contexto atual rejeita usuario removido/inativo.

API tests:
- `POST /api/auth/authenticate` cobre `200`, `400`, `401`;
- `GET /api/auth/me` cobre `200` com token e `401` sem token.

Verificacao final:
- `dotnet build SmartMovieCatalog.slnx`
- executar testes backend adicionados.

## Assumptions
- PostgreSQL e o provider escolhido porque ja existe em `docker-compose.yml`.
- Segredos JWT ficam em environment variables ou user-secrets, nunca em arquivos versionados.
- O scaffold `WeatherForecast` pode ser removido quando os endpoints reais forem introduzidos.

## Comments

_No comments_

## Instructions for Spec Kit

Use this GitHub issue as the primary source of truth.

Convert the issue into a Spec Kit feature specification before creating the implementation plan.

Preserve:

- business goal;
- user stories;
- acceptance criteria;
- technical constraints;
- non-goals;
- dependencies;
- open questions.

If information is missing, add it under a clearly marked **Clarifications Needed** section instead of inventing requirements.

If the issue conflicts with existing project documentation, explicitly call out the conflict.

Prefer a small, incremental implementation plan aligned with the repository's existing architecture, folder structure, language, framework, and conventions.
