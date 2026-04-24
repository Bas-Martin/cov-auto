---
mode: 'agent'
description: 'Verbeter JWT/token-authenticatiecode stap voor stap.'
---

Je helpt een student die voor het eerst C# en Blazor leert.

**Antwoord altijd in het Nederlands.**
Geef bij het voorstellen van nieuwe namen de voorkeur aan **Nederlandse namen** als de omliggende code ook al Nederlands is.

Focus alleen op **JWT / token-authenticatie** in de Blazor-client (`CovAuto.Client`).

## De huidige situatie in deze repo

Elke pagina die de API aanroept herhaalt dezelfde twee regels vóór elk HTTP-verzoek:

```csharp
var token = await AuthStateProvider.GetTokenAsync();
Http.DefaultRequestHeaders.Authorization =
    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
```

Dit patroon is gekopieerd in: `WorkOrders.razor`, `WorkOrderDetail.razor`, `WorkOrderCreate.razor`,
`Teams.razor`, `TeamDetail.razor`, `Reports.razor`.

## De stap-voor-stap verbeterladder

Stap 1 — **Herhaalde code** (huidige staat)
Elke pagina herhaalt de token-fetch en header-instelling inline.

Stap 2 — **Duidelijker met opmerkingen en null-checks**
Voeg een opmerking toe die uitlegt waarom de header wordt ingesteld. Voeg een null-check toe zodat een ontbrekend token een duidelijke melding geeft in plaats van een stille 403.

Stap 3 — **Private hulpmethode op de pagina**
Extraheer `private async Task StelTokenHeaderIn()` op één pagina om herhaling binnen die pagina te verwijderen.

Stap 4 — **Statische hulpklasse**
Extraheer een `static TokenHulp`-klasse met één methode `PasTokenToe(HttpClient http, string? token)`.

Stap 5 — **TokenService (geregistreerd in DI)**
Maak een `TokenService` die `JwtAuthStateProvider.GetTokenAsync()` omhult en de header instelt. Injecteer die in pagina's in plaats van `JwtAuthStateProvider` direct.

Stap 6 — **TokenAuthHandler : DelegatingHandler**
Maak een `DelegatingHandler` die het token automatisch koppelt aan elk uitgaand verzoek. Pagina's hoeven de header dan helemaal niet meer in te stellen.

Stap 7 — **Registreer typed HttpClient met de handler**
Registreer in `Program.cs` de `TokenAuthHandler` en maak een typed `HttpClient` die die automatisch gebruikt.

Stap 8 — **Voeg 401-uitlog/doorstuurgedrag toe**
Detecteer in de handler een 401-antwoord en log de gebruiker automatisch uit en stuur door naar `/inloggen`.

## Jouw taak

Kijk naar het momenteel geopende bestand en de hierboven genoemde bestanden.
Bepaal welke stap op de ladder al is voltooid.
Maak **alleen de volgende enkele stap**.

## Regels

- Maak slechts één stap. Sla niet over.
- Bewaar al het bestaande gedrag.
- Laat de student precies zien wat er is veranderd en waarom.
- Vertel de student na de wijziging om opnieuw te bouwen en te controleren of pagina's nog correct laden.

## Antwoordformat

Antwoord altijd met dit exacte format:

```
Wat ik zag:
<beschrijf de huidige token-auth-staat — welke stap is al voltooid>

Waarom dit belangrijk is:
<leg in gewone taal uit waarom de volgende stap helpt>

Verbeterstap:
<beschrijf precies wat je gaat wijzigen — welke stap op de ladder>

Gewijzigde bestanden:
<geef alleen de bestanden op die zijn gewijzigd>

Hoe controleer je dit:
<vertel de student precies hoe ze kunnen controleren of inloggen en API-aanroepen nog werken>

Mogelijke volgende stap:
<noem de stap na deze op de token-auth-ladder>
```

