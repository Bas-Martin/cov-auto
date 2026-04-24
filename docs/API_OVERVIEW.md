# API Overzicht

Alle beschikbare endpoints, met voorbeelden.

Base URL: `http://localhost:5239`

---

## Authenticatie

De meeste endpoints vereisen een JWT-token. Stuur het token mee als header:
```
Authorization: Bearer <jouw-token>
```

### Inloggen
```
POST /auth/login
```

**Body:**
```json
{
  "username": "planner.noord",
  "password": "Demo1234!"
}
```

**Reactie:**
```json
{
  "success": true,
  "data": {
    "token": "eyJ...",
    "username": "planner.noord",
    "fullName": "Linda van Noord",
    "role": "Planner",
    "teamId": null
  }
}
```

---

## Teams

### Alle teams ophalen (alleen Planner)
```
GET /teams
Authorization: Bearer <token>
```

### Één team ophalen
```
GET /teams/1
Authorization: Bearer <token>
```

Monteurs mogen alleen hun eigen team opvragen.

---

## Werkorders

### Werkorders ophalen (met filters)
```
GET /workorders
Authorization: Bearer <token>
```

**Optionele query parameters:**

| Parameter | Beschrijving | Voorbeeld |
|-----------|-------------|---------|
| `status` | Filter op status | `Nieuw`, `Gepland`, `InUitvoering`, `Voltooid`, `Geannuleerd` |
| `priority` | Filter op prioriteit | `Laag`, `Normaal`, `Hoog`, `Kritiek` |
| `title` | Zoek op titeldeel | `ketel` |
| `customerName` | Zoek op klantnaam | `Jansen` |
| `minEstimatedHours` | Minimaal aantal geschatte uren | `2` |
| `maxEstimatedHours` | Maximaal aantal geschatte uren | `8` |
| `sortBy` | Sorteerveld | `createdAt`, `title`, `estimatedHours`, `scheduledFor` |
| `sortDirection` | Richting | `asc` of `desc` |
| `page` | Paginanummer (standaard: 1) | `2` |
| `pageSize` | Items per pagina (standaard: 10, max: 100) | `5` |

**Voorbeeld:**
```
GET /workorders?status=Nieuw&priority=Hoog&sortBy=estimatedHours&sortDirection=asc&page=1&pageSize=5
```

### Één werkorder ophalen
```
GET /workorders/3
Authorization: Bearer <token>
```

Monteurs mogen alleen werkorders van hun eigen team zien.

### Nieuwe werkorder aanmaken (alleen Planner)
```
POST /workorders
Authorization: Bearer <token>
```

**Body:**
```json
{
  "title": "Storing CV-ketel",
  "description": "Ketel geeft foutcode E5",
  "estimatedHours": 3.0,
  "status": "Nieuw",
  "priority": "Hoog",
  "scheduledFor": "2024-03-01T09:00:00Z",
  "customerName": "Familie Jansen",
  "address": "Hoofdstraat 12, Amsterdam",
  "serviceTeamId": 1
}
```

---

## Rapporten (alleen Planner)

### Rapport voor één team
```
POST /reports/workorders/team/1
Authorization: Bearer <token>
```

**Body:**
```json
{
  "from": "2024-01-01T00:00:00Z",
  "to": "2024-12-31T23:59:59Z"
}
```

### Bulk rapport (meerdere teams parallel)
```
POST /reports/workorders/bulk
Authorization: Bearer <token>
```

**Body:**
```json
{
  "teamIds": [1, 2],
  "from": "2024-01-01T00:00:00Z",
  "to": "2024-12-31T23:59:59Z"
}
```

### Performance vergelijking (sequentieel vs. parallel)
```
GET /reports/performance-comparison
Authorization: Bearer <token>
```

---

## Standaard response-formaat

Alle endpoints geven een `ApiResponse`-object terug:

**Succesvol:**
```json
{
  "success": true,
  "message": null,
  "data": { ... }
}
```

**Fout:**
```json
{
  "success": false,
  "message": "Team 99 niet gevonden.",
  "data": null
}
```

## Gepagineerde resultaten

Bij `GET /workorders` zit de data verpakt in een `PagedResult`:
```json
{
  "success": true,
  "data": {
    "items": [ ... ],
    "totalCount": 15,
    "totalPages": 2,
    "page": 1,
    "pageSize": 10
  }
}
```
