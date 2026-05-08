# Contract: Movie Genre Compatibility

## `POST /api/movies`

The public movie creation contract remains unchanged for genres.

### Request

```json
{
  "title": "Central do Brasil",
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR",
  "genres": ["Drama"]
}
```

### Response

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "title": "Central do Brasil",
  "releaseYear": 1998,
  "countryCode": "BR",
  "originalLanguage": "pt-BR",
  "genres": ["Drama"],
  "externalId": null,
  "image": null
}
```

## Contract Rules

- `genres` remains a collection of names.
- Genre identifiers are not accepted or returned by this feature.
- `Genre.ExternalId` is internal backend metadata and is not part of the movie creation contract.
- Duplicate equivalent genre names in a request are normalized to one movie association.
