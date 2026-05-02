# ADR 0004: API Error Contract

## Status
Proposed

## Context
The current API surface is scaffold-level. A stable error response contract has not been defined yet.

## Decision
No custom API error contract is selected yet.

Before adding product endpoints, define:

- Error response shape.
- Validation error shape.
- Correlation or trace identifier policy.
- Mapping rules for common failures.
- OpenAPI documentation expectations.

## Consequences
- Scaffold endpoints may continue using framework defaults.
- Product endpoints should not expose internal exception details.
- Once accepted, all API endpoints should follow the selected error contract.
