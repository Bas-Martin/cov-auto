# Project Map

Een overzicht van alle bestanden en wat ze doen.

---

## Overzicht van de mappenstructuur

```
cov-auto/
├── CovAuto.API/               ← Back-end (Web API)
│   ├── Controllers/           ← HTTP endpoints (wat de buitenwereld ziet)
│   ├── Application/
│   │   ├── DTOs/              ← Data-objecten voor in/uit de API
│   │   ├── Services/          ← Businesslogica
│   │   └── QueryParameters/   ← Filter/sort/pagina parameters
│   ├── Domain/
│   │   ├── Entities/          ← Database-tabellen als klassen
│   │   └── Enums/             ← Vaste waarden (status, prioriteit, rol)
│   ├── Infrastructure/
│   │   ├── Data/              ← Database context + seed data
│   │   └── Repositories/      ← Database-queries
│   ├── Common/                ← Gedeelde hulpklassen
│   ├── Migrations/            ← Database-migraties (automatisch gegenereerd)
│   ├── Program.cs             ← Startpunt van de API
│   └── appsettings.json       ← Configuratie (database, JWT, logging)
│
├── CovAuto.Client/            ← Front-end (Blazor WebAssembly)
│   ├── Pages/                 ← Blazor pagina's (bevatten ook de HTTP-aanroepen)
│   ├── Auth/                  ← JWT opslaan en meesturen
│   ├── Models/                ← Data-objecten (zelfde structuur als API DTOs)
│   ├── Layout/                ← Navigatiebalk en layout
│   ├── Shared/                ← Herbruikbare componenten
│   └── Program.cs             ← Startpunt van de client
│
└── docs/                      ← Documentatie
```

---

## Bestanden per onderdeel

### Back-end: Controllers (HTTP endpoints)

| Bestand | Doel |
|---------|------|
| `Controllers/AuthController.cs` | `POST /auth/login` – inloggen, JWT teruggeven |
| `Controllers/TeamsController.cs` | `GET /teams` en `GET /teams/{id}` |
| `Controllers/WorkOrdersController.cs` | `GET /workorders`, `GET /workorders/{id}`, `POST /workorders` |
| `Controllers/ReportsController.cs` | Rapporten genereren (alleen voor planners) |

### Back-end: Services (businesslogica)

| Bestand | Doel |
|---------|------|
| `Application/Services/AuthService.cs` | Wachtwoord controleren, JWT aanmaken |
| `Application/Services/ServiceTeamService.cs` | Teams ophalen en omzetten naar DTOs |
| `Application/Services/WorkOrderService.cs` | Werkorders ophalen, aanmaken, omzetten naar DTOs |
| `Application/Services/ReportService.cs` | Rapporten genereren (sequentieel én parallel) |

### Back-end: Repositories (database-queries)

| Bestand | Doel |
|---------|------|
| `Infrastructure/Repositories/UserRepository.cs` | Gebruiker opzoeken op gebruikersnaam |
| `Infrastructure/Repositories/ServiceTeamRepository.cs` | Teams ophalen uit database |
| `Infrastructure/Repositories/WorkOrderRepository.cs` | Werkorders ophalen met filtering/paging |

### Back-end: Domain (datamodel)

| Bestand | Doel |
|---------|------|
| `Domain/Entities/User.cs` | Gebruiker (monteur of planner) |
| `Domain/Entities/ServiceTeam.cs` | Serviceteam met monteurs en werkorders |
| `Domain/Entities/WorkOrder.cs` | Werkorder (taak voor een team) |
| `Domain/Enums/UserRole.cs` | `Monteur` of `Planner` |
| `Domain/Enums/WorkOrderStatus.cs` | `Nieuw`, `Gepland`, `InUitvoering`, `Voltooid`, `Geannuleerd` |
| `Domain/Enums/WorkOrderPriority.cs` | `Laag`, `Normaal`, `Hoog`, `Kritiek` |

### Back-end: DTOs (data-objecten)

| Bestand | Doel |
|---------|------|
| `Application/DTOs/LoginRequest.cs` | Gebruikersnaam + wachtwoord insturen |
| `Application/DTOs/LoginResponse.cs` | Token, naam, rol terugkrijgen |
| `Application/DTOs/WorkOrderDto.cs` | Werkorder-data tonen |
| `Application/DTOs/CreateWorkOrderRequest.cs` | Nieuwe werkorder aanmaken |
| `Application/DTOs/ServiceTeamDto.cs` | Team-data tonen |
| `Application/DTOs/ReportDto.cs` | Rapport-data tonen |

### Back-end: Gemeenschappelijk

| Bestand | Doel |
|---------|------|
| `Common/ApiResponse.cs` | Standaard wrapper: `{ success, message, data }` |
| `Common/PagedResult.cs` | Gepagineerde lijst: `{ items, totalCount, page, ... }` |

### Back-end: Database

| Bestand | Doel |
|---------|------|
| `Infrastructure/Data/AppDbContext.cs` | EF Core context met tabellen en seed data |
| `Migrations/` | Automatisch gegenereerde migratiebestanden |
| `appsettings.json` | Database-verbinding, JWT-instellingen |

---

### Front-end: Pagina's

| Bestand | Route | Doel |
|---------|-------|------|
| `Pages/Home.razor` | `/` | Dashboard na inloggen |
| `Pages/Login.razor` | `/login` | Inlogformulier |
| `Pages/WorkOrders.razor` | `/werkorders` | Lijst van werkorders met filters |
| `Pages/WorkOrderDetail.razor` | `/werkorders/{id}` | Detail van één werkorder |
| `Pages/WorkOrderCreate.razor` | `/werkorders/nieuw` | Nieuwe werkorder aanmaken |
| `Pages/Teams.razor` | `/teams` | Lijst van teams (alleen planner) |
| `Pages/TeamDetail.razor` | `/teams/{id}` | Detail van één team |
| `Pages/Reports.razor` | `/rapporten` | Rapporten genereren (alleen planner) |

### Front-end: Pagina's (bevatten ook de HTTP-aanroepen)

| Bestand | Route | Doel |
|---------|-------|------|
| `Pages/Home.razor` | `/` | Dashboard na inloggen |
| `Pages/Login.razor` | `/login` | Inlogformulier – stuurt POST naar `auth/login` en slaat het token op |
| `Pages/WorkOrders.razor` | `/werkorders` | Lijst van werkorders met filters – haalt werkorders op via `GET /workorders` |
| `Pages/WorkOrderDetail.razor` | `/werkorders/{id}` | Detail van één werkorder – haalt op via `GET /workorders/{id}` |
| `Pages/WorkOrderCreate.razor` | `/werkorders/nieuw` | Nieuwe werkorder aanmaken – POST naar `workorders` |
| `Pages/Teams.razor` | `/teams` | Lijst van teams – haalt op via `GET /teams` |
| `Pages/TeamDetail.razor` | `/teams/{id}` | Detail van één team – haalt op via `GET /teams/{id}` |
| `Pages/Reports.razor` | `/rapporten` | Rapporten genereren – POST naar rapport-endpoints |

### Front-end: Authenticatie

| Bestand | Doel |
|---------|------|
| `Auth/JwtAuthStateProvider.cs` | Slaat het JWT op in browser `sessionStorage` en vertelt Blazor wie er ingelogd is |

Elke pagina injecteert `HttpClient` en `JwtAuthStateProvider` direct. Vóór elke API-aanroep haalt de pagina het token op en zet het als `Authorization: Bearer ...` header.

---

## Hoe stroomt data door de applicatie?

```
Browser → Blazor pagina → HTTP-aanroep naar API
                                   ↓
API Controller → Service → Repository → Database
                                   ↓
                Entity → DTO → JSON → terug naar browser
```

**Voorbeeld: werkorders laden**
1. `WorkOrders.razor` haalt het JWT-token op via `JwtAuthStateProvider`
2. `WorkOrders.razor` doet `GET /workorders?...` met het token als `Authorization: Bearer` header
3. `WorkOrdersController.GetWorkOrders()` ontvangt de aanvraag
4. `WorkOrderService.GetWorkOrdersAsync()` wordt aangeroepen
5. `WorkOrderRepository.GetPagedAsync()` haalt data op uit SQLite via EF Core
6. De resultaten worden omgezet van `WorkOrder`-entiteit naar `WorkOrderDto`
7. De DTO's worden verpakt in `ApiResponse<PagedResult<WorkOrderDto>>`
8. De JSON komt terug in de Blazor pagina en wordt getoond in een tabel
