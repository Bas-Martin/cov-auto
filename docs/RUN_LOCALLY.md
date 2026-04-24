# Lokaal opstarten

Stap-voor-stap instructies om het project lokaal te draaien.

---

## Vereisten

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Een terminal (PowerShell, bash, etc.)

Controleer of .NET is geïnstalleerd:
```bash
dotnet --version
# Verwacht: 10.x.x
```

---

## Stap 1: Repository klonen

```bash
git clone https://github.com/Bas-Martin/cov-auto.git
cd cov-auto
```

---

## Stap 2: De API opstarten

```bash
cd CovAuto.API
dotnet run
```

De API draait nu op: **http://localhost:5239**  
Swagger UI (interactieve documentatie) is beschikbaar op: **http://localhost:5239**

Bij de eerste keer opstarten wordt automatisch:
- De SQLite-database aangemaakt (`covauto.db`)
- Testdata aangemaakt (teams, gebruikers, werkorders)

---

## Stap 3: De Blazor client opstarten

Open een **tweede terminal**:

```bash
cd CovAuto.Client
dotnet run
```

De client draait nu op: **http://localhost:5264**

---

## Inloggen

Ga naar `http://localhost:5264` en log in met:

| Gebruikersnaam   | Wachtwoord  | Rol     |
|------------------|-------------|---------|
| `planner.noord`  | `Demo1234!` | Planner |
| `monteur.jan`    | `Demo1234!` | Monteur |

---

## Veelgestelde problemen

**"Kan geen verbinding maken met de API"**  
Zorg dat de API draait (`dotnet run` in `CovAuto.API`).

**"Poort al in gebruik"**  
Stop de vorige instantie of gebruik een andere poort:
```bash
dotnet run --urls "http://localhost:5240"
```

**Database vernieuwen (alles weggooien en opnieuw beginnen)**
```bash
cd CovAuto.API
rm covauto.db
dotnet run
```

**Migraties toevoegen na een model-wijziging**
```bash
cd CovAuto.API
dotnet ef migrations add BeschrijfJeWijziging
dotnet ef database update
```

---

## Configuratie

De API-instellingen staan in `CovAuto.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=covauto.db"
  },
  "JwtSettings": {
    "Secret": "...",
    "Issuer": "CovAutoAPI",
    "Audience": "CovAutoClients"
  }
}
```

De client weet welke API-URL hij moet gebruiken via `CovAuto.Client/wwwroot/appsettings.json`.
