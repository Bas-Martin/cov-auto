# Beginnersgids CovAuto

Dit document legt uit hoe het project werkt, hoe je inlogt, en hoe je als beginner een kleine wijziging kunt doorvoeren.

---

## Wat doet dit project?

CovAuto is een demo-applicatie voor een servicebedrijf. Er zijn twee soorten gebruikers:

- **Planners** – kunnen alles zien: teams, werkorders, rapporten. Ze kunnen ook nieuwe werkorders aanmaken.
- **Monteurs** – kunnen alleen werkorders van hun eigen team zien.

De applicatie bestaat uit twee onderdelen:
1. **CovAuto.API** – de back-end (levert data via HTTP)
2. **CovAuto.Client** – de front-end (Blazor, draait in de browser)

---

## Hoe inloggen?

Wachtwoord voor alle testaccounts: **Demo1234!**

| Gebruikersnaam   | Rol     | Team          |
|------------------|---------|---------------|
| `planner.noord`  | Planner | Noord Service |
| `planner.zuid`   | Planner | Zuid Service  |
| `monteur.jan`    | Monteur | Noord Service |
| `monteur.fatma`  | Monteur | Noord Service |
| `monteur.sven`   | Monteur | Zuid Service  |
| `monteur.ayse`   | Monteur | Zuid Service  |

---

## Hoe werkt de authenticatie?

1. Je stuurt gebruikersnaam + wachtwoord naar `POST /auth/login`.
2. Als het klopt, krijg je een **JWT-token** terug.
3. Bij elke volgende API-aanvraag stuur je dat token mee in de header:
   ```
   Authorization: Bearer <jouw-token>
   ```
4. Het token bevat je naam, rol en team-ID. De API gebruikt dat om te controleren wat je mag zien.

In de Blazor client slaat `AuthTokenHandler.cs` het token automatisch op en voegt het toe aan elke aanvraag.

---

## Hoe voeg je een nieuw veld toe aan een werkorder?

Stel: je wilt een veld `ContactPhone` (telefoonnummer klant) toevoegen.

**Stap 1: Domain entity** (`CovAuto.API/Domain/Entities/WorkOrder.cs`)
```csharp
public string ContactPhone { get; set; } = string.Empty;
```

**Stap 2: DTO** (`CovAuto.API/Application/DTOs/WorkOrderDto.cs`)
```csharp
public string ContactPhone { get; set; } = string.Empty;
```

**Stap 3: CreateWorkOrderRequest** (`CovAuto.API/Application/DTOs/CreateWorkOrderRequest.cs`)
```csharp
public string ContactPhone { get; set; } = string.Empty;
```

**Stap 4: Mapping in WorkOrderService** (`CovAuto.API/Application/Services/WorkOrderService.cs`)  
Voeg het veld toe aan de `MapToDto`-methode én aan de `CreateWorkOrderAsync`-methode.

**Stap 5: Database migratie**
```bash
cd CovAuto.API
dotnet ef migrations add AddContactPhone
dotnet ef database update
```

**Stap 6: Client model** (`CovAuto.Client/Models/WorkOrderDto.cs`)
```csharp
public string ContactPhone { get; set; } = string.Empty;
```

**Stap 7: Blazor pagina** (`CovAuto.Client/Pages/WorkOrderDetail.razor`)  
Voeg een regeltje toe in de detail-weergave.

---

## Hoe voeg je een nieuw API-endpoint toe?

Voorbeeld: een endpoint dat alle werkorders van één klant ophaalt.

**Stap 1:** Voeg een methode toe aan `WorkOrderRepository`:
```csharp
public async Task<List<WorkOrder>> GetByCustomerNameAsync(string name)
{
    return await _context.WorkOrders
        .Where(w => w.CustomerName.Contains(name))
        .ToListAsync();
}
```

**Stap 2:** Voeg een methode toe aan `WorkOrderService`:
```csharp
public async Task<List<WorkOrderDto>> GetByCustomerAsync(string name)
{
    var workOrders = await _workOrderRepository.GetByCustomerNameAsync(name);
    return workOrders.Select(MapToDto).ToList();
}
```

**Stap 3:** Voeg een endpoint toe aan `WorkOrdersController`:
```csharp
[HttpGet("by-customer")]
public async Task<IActionResult> GetByCustomer([FromQuery] string name)
{
    var result = await _workOrderService.GetByCustomerAsync(name);
    return Ok(ApiResponse<List<WorkOrderDto>>.Ok(result));
}
```

Klaar! Start de API opnieuw en je endpoint is beschikbaar op `/workorders/by-customer?name=Jansen`.

---

## Hoe werkt de autorisatie?

De API gebruikt rollen die in het JWT-token zitten:
- `[Authorize(Roles = "Planner")]` → alleen planners
- `[Authorize]` → ingelogde gebruikers (planner én monteur)

In controllers wordt soms ook handmatig gecheckt of een monteur zijn eigen team-ID gebruikt:
```csharp
var role = User.FindFirstValue(ClaimTypes.Role);
if (role == "Monteur")
{
    var teamIdClaim = User.FindFirstValue("teamId");
    // Controleer of het gevraagde team overeenkomt met het team van de monteur
}
```
