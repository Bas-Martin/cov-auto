# CovAuto

Demo-applicatie voor het beheren van werkorders en servicetechnici (monteurs).

## Wat doet dit project?

CovAuto is een voorbeeld-systeem voor een servicebedrijf. Er zijn twee rollen:

- **Planners** – zien alles, kunnen werkorders aanmaken en rapporten genereren
- **Monteurs** – zien alleen werkorders van hun eigen team

De applicatie laat zien hoe je een .NET Web API bouwt met:
- JWT-authenticatie en rolgebaseerde autorisatie
- Filtering, sortering en paginering op lijsten
- Async verwerking: sequentieel vs. parallel rapporten genereren
- Een Blazor WebAssembly front-end

## Snel starten

**Terminal 1 – API:**
```bash
cd CovAuto.API
dotnet run
```
Swagger UI: http://localhost:5239

**Terminal 2 – Client:**
```bash
cd CovAuto.Client
dotnet run
```
Applicatie: http://localhost:5264

**Inloggen** (wachtwoord voor alle accounts: `Demo1234!`):

| Gebruikersnaam   | Rol     |
|------------------|---------|
| `planner.noord`  | Planner |
| `planner.zuid`   | Planner |
| `monteur.jan`    | Monteur |
| `monteur.fatma`  | Monteur |
| `monteur.sven`   | Monteur |
| `monteur.ayse`   | Monteur |

## Projectstructuur

```
CovAuto.API/
├── Controllers/        # HTTP endpoints
├── Application/
│   ├── DTOs/           # Data-objecten in/uit de API
│   ├── Services/       # Businesslogica
│   └── QueryParameters/# Filter/sort/pagina parameters
├── Domain/
│   ├── Entities/       # Database-entiteiten
│   └── Enums/          # Vaste waarden (status, prioriteit, rol)
├── Infrastructure/
│   ├── Data/           # AppDbContext + seed data
│   └── Repositories/   # Database-queries
└── Common/             # ApiResponse, PagedResult

CovAuto.Client/
├── Pages/              # Blazor pagina's
├── Services/           # HTTP-aanroepen naar de API
├── Auth/               # JWT opslaan en meesturen
└── Models/             # Data-objecten
```

## Documentatie

| Document | Beschrijving |
|----------|-------------|
| [docs/BEGINNER_GUIDE.md](docs/BEGINNER_GUIDE.md) | Hoe authenticatie werkt, hoe je een veld of endpoint toevoegt |
| [docs/PROJECT_MAP.md](docs/PROJECT_MAP.md) | Overzicht van alle bestanden en hun doel |
| [docs/RUN_LOCALLY.md](docs/RUN_LOCALLY.md) | Stap-voor-stap opstartinstructies |
| [docs/API_OVERVIEW.md](docs/API_OVERVIEW.md) | Alle endpoints met voorbeelden |

## Endpoints (kort overzicht)

| Methode | URL | Rol | Beschrijving |
|---------|-----|-----|-------------|
| `POST` | `/auth/login` | Iedereen | Inloggen, JWT ophalen |
| `GET` | `/teams` | Planner | Alle teams |
| `GET` | `/teams/{id}` | Planner/Monteur | Één team |
| `GET` | `/workorders` | Planner/Monteur | Werkorders (met filters) |
| `GET` | `/workorders/{id}` | Planner/Monteur | Één werkorder |
| `POST` | `/workorders` | Planner | Nieuwe werkorder aanmaken |
| `POST` | `/reports/workorders/team/{id}` | Planner | Rapport voor één team |
| `POST` | `/reports/workorders/bulk` | Planner | Bulk rapporten parallel |
| `GET` | `/reports/performance-comparison` | Planner | Sequentieel vs. parallel |

