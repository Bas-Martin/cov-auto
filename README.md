# CovAuto API

Een .NET 8 Web API voor het beheren van werkorders (werkorders) en servicetechnici (monteurs).

## Wat doet dit project?

CovAuto API biedt een REST API voor een bedrijf met servicetechnici en werkorders. De API demonstreert:

- **JWT-authenticatie** en rolgebaseerde autorisatie
- **Filtering, sorting en pagination** op werkorders
- **Asynchrone verwerking**: sequentieel vs. parallel rapporten genereren (`Task.WhenAll`)
- **Clean Architecture** structuur: Domain → Application → Infrastructure

## Rollen

| Rol | Beschrijving |
|-----|-------------|
| **Planner** | Ziet alle teams en werkorders, kan werkorders aanmaken, genereert rapporten |
| **Monteur** | Ziet alleen werkorders van het eigen team |

## Starten

```bash
cd CovAuto.API
dotnet run
```

De Swagger UI is beschikbaar op: `http://localhost:5000`

## Testgebruikers

Alle gebruikers hebben wachtwoord: **Demo1234!**

| Gebruikersnaam     | Rol     | Team          |
|--------------------|---------|---------------|
| `planner.noord`    | Planner | Noord Service |
| `planner.zuid`     | Planner | Zuid Service  |
| `monteur.jan`      | Monteur | Noord Service |
| `monteur.fatma`    | Monteur | Noord Service |
| `monteur.sven`     | Monteur | Zuid Service  |
| `monteur.ayse`     | Monteur | Zuid Service  |

## Endpoints

### Authenticatie
| Methode | URL | Beschrijving |
|---------|-----|-------------|
| `POST` | `/auth/login` | Inloggen en JWT token ophalen |

### Teams
| Methode | URL | Rol | Beschrijving |
|---------|-----|-----|-------------|
| `GET` | `/teams` | Planner | Alle teams ophalen |
| `GET` | `/teams/{id}` | Planner/Monteur* | Één team ophalen |

*Monteur mag alleen zijn eigen team opvragen.

### Werkorders
| Methode | URL | Rol | Beschrijving |
|---------|-----|-----|-------------|
| `GET` | `/workorders` | Planner/Monteur | Werkorders met filtering/sorting/pagination |
| `GET` | `/workorders/{id}` | Planner/Monteur | Één werkorder ophalen |
| `POST` | `/workorders` | Planner | Nieuwe werkorder aanmaken |

### Rapporten
| Methode | URL | Rol | Beschrijving |
|---------|-----|-----|-------------|
| `POST` | `/reports/workorders/team/{id}` | Planner | Rapport voor één team |
| `POST` | `/reports/workorders/bulk` | Planner | Bulk rapporten parallel |
| `GET` | `/reports/performance-comparison` | Planner | Sequentieel vs. parallel vergelijking |

## Filtering, Sorting & Pagination

De `GET /workorders` endpoint ondersteunt de volgende query parameters:

**Filtering:**
- `title` – Zoek op titel (bevat)
- `status` – Filter op status: `Nieuw`, `Gepland`, `InUitvoering`, `Voltooid`, `Geannuleerd`
- `priority` – Filter op prioriteit: `Laag`, `Normaal`, `Hoog`, `Kritiek`
- `customerName` – Zoek op klantnaam
- `minEstimatedHours` / `maxEstimatedHours` – Filter op geschatte uren

**Sortering:**
- `sortBy` – `title`, `estimatedHours`, `createdAt`, `scheduledFor` (standaard: `createdAt`)
- `sortDirection` – `asc` of `desc` (standaard: `desc`)

**Pagination:**
- `page` – Paginanummer (standaard: `1`)
- `pageSize` – Items per pagina (standaard: `10`, max: `100`)

**Voorbeeld:**
```
GET /workorders?status=Nieuw&priority=Hoog&sortBy=estimatedHours&sortDirection=asc&page=1&pageSize=5
```

## Project structuur

```
CovAuto.API/
├── Controllers/          # HTTP endpoints
├── Application/
│   ├── DTOs/             # Data Transfer Objects
│   ├── Interfaces/       # Service interfaces
│   ├── Services/         # Businesslogica
│   └── QueryParameters/  # Filter/sort/pagination parameters
├── Domain/
│   ├── Entities/         # Database entiteiten
│   └── Enums/            # Enumeraties
├── Infrastructure/
│   └── Data/             # AppDbContext + seed data
└── Common/               # Gedeelde klassen (ApiResponse, PagedResult)
```

Monteurs hebben werkorders nodig.
