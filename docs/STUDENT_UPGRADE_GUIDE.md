# Studentengids voor de verbeterworkflow

Deze gids legt de `/upgrade`-leerworkflow uit die in deze repository is ingebouwd.
Hij laat je zien hoe je de code stap voor stap kunt verbeteren — één kleine stap tegelijk — met GitHub Copilot in VS Code.

---

## Wat is de verbeterworkflow?

De verbeterworkflow is een set **herbruikbare Copilot-prompts** die zijn opgeslagen in `.github/prompts/`.
Elke prompt vertelt Copilot om de code op een specifieke manier te verbeteren, zonder je te overweldigen met te veel wijzigingen tegelijk.

Je hoeft het "juiste antwoord" niet van tevoren te weten.
Je vraagt om een verbetering, Copilot maakt één kleine wijziging, je leest hem, voert de app uit, en vraagt daarna om de volgende.

---

## Waarom begint deze repo eenvoudig?

De code in deze repo is bewust op beginnerniveau gehouden.
Elke Blazor-pagina die de API aanroept herhaalt bijvoorbeeld dezelfde twee regels:

```csharp
var token = await AuthStateProvider.GetTokenAsync();
Http.DefaultRequestHeaders.Authorization =
    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
```

Dit is geen fout. Het is het **beginpunt**.
Je kunt precies zien wat er gebeurt. Niets is verborgen in een framework-klasse of een handler.

Naarmate je verbeteringen uitvoert, wordt de code stap voor stap schoner — en begrijp je *waarom* elke wijziging is gemaakt.

---

## Waarom we niet meteen naar het beste patroon springen

Stel je voor dat je vraagt "hoe ga ik om met JWT-tokens in Blazor?" en iemand laat je direct dit zien:

```csharp
public class TokenAuthHandler : DelegatingHandler
{
    private readonly ITokenService _tokenService;
    public TokenAuthHandler(ITokenService tokenService) { _tokenService = tokenService; }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetTokenAsync();
        if (token != null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            await _tokenService.LogoutAsync();
        return response;
    }
}
```

Dit is goede professionele code. Maar als je het nog nooit hebt gezien, weet je niet:
- Wat is een `DelegatingHandler`?
- Waarom erft het van die klasse?
- Waar wordt `SendAsync` aangeroepen?
- Wat is `ITokenService` en waar komt het vandaan?

In plaats daarvan neemt de verbeterworkflow je stap voor stap door de stappen:

```
Stap 1: Herhaalde token-code in elke pagina         ← huidige staat
Stap 2: Opmerkingen en een null-check toevoegen
Stap 3: Een private hulpmethode extraheren
Stap 4: Een statische TokenHulp-klasse extraheren
Stap 5: Een TokenService extraheren (geregistreerd in DI)
Stap 6: TokenAuthHandler : DelegatingHandler implementeren
Stap 7: Typed HttpClient met de handler registreren
Stap 8: 401-uitlog/doorstuurgedrag toevoegen
```

Tegen de tijd dat je stap 6 bereikt, weet je al *waarom* de handler bestaat.
Je hebt hetzelfde probleem al drie keer op drie verschillende manieren opgelost. De handler is gewoon de nette versie.

---

## Hoe gebruik je de prompts in VS Code Copilot Chat

De prompts staan in `.github/prompts/`. VS Code Copilot Chat kan ze gebruiken als **herbruikbare promptbestanden**.

### Stap 1 — Open Copilot Chat

Druk op `Ctrl+Alt+I` (Windows/Linux) of `Cmd+Alt+I` (Mac), of klik op het Copilot Chat-icoon in de zijbalk.

### Stap 2 — Open het bestand dat je wilt verbeteren

Open bijvoorbeeld `CovAuto.Client/Pages/WorkOrders.razor`.

### Stap 3 — Koppel het promptbestand

Klik in Copilot Chat op het **paperclip/bijlage**-icoon en selecteer:
```
.github/prompts/upgrade.prompt.md
```

Of typ `#` in het chatinvoerveld en zoek op de naam van het promptbestand.

### Stap 4 — Verstuur

Druk op Enter. Copilot leest het huidige bestand en de promptinstructies, en maakt daarna één kleine verbetering.

---

## Beschikbare promptopdrachten

| Promptbestand | Wat het doet |
|---------------|--------------|
| `upgrade.prompt.md` | Maakt één kleine verbetering aan het huidige bestand. Kiest de kleinste nuttige stap. |
| `upgrade-next.prompt.md` | Gaat één stap verder op dezelfde verbeterrichting. |
| `upgrade-explain.prompt.md` | Legt de volgende verbetering uit **zonder code te wijzigen**. Goed voor begrip vóór actie. |
| `upgrade-token-auth.prompt.md` | Richt zich alleen op JWT/token-authenticatie. Gebruikt de token-auth-ladder. |

### Hoe je ze uitvoert

**Optie A — Koppelen in Copilot Chat (aanbevolen)**
1. Open het bestand dat je wilt verbeteren.
2. Open Copilot Chat (`Ctrl+Alt+I`).
3. Klik op het paperclip-icoon → selecteer het promptbestand.
4. Druk op Enter.

**Optie B — Typ `#` in het chatinvoerveld**
1. Typ in Copilot Chat `#` en begin de promptnaam te typen (bijv. `upgrade`).
2. Selecteer het overeenkomende promptbestand uit het dropdown-menu.
3. Druk op Enter.

**Optie C — Copilot Edits (VS Code 1.93+)**
1. Open Copilot Edits (`Ctrl+Shift+I`).
2. Sleep het promptbestand naar de chat, of gebruik `#` om het te koppelen.
3. Copilot Edits past de wijziging direct toe op het bestand.

> **Opmerking over `/upgrade` als slash-opdracht:**
> VS Code Copilot ondersteunt geen registratie van willekeurige `/upgrade`-slash-opdrachten vanuit promptbestanden.
> De prompts in `.github/prompts/` zijn herbruikbare promptbestanden — geen echte slash-opdrachten.
> De `/`-opdrachten in Copilot Chat zijn ingebouwde opdrachten (zoals `/explain`, `/fix`, `/tests`).
> Gebruik altijd één van de bovenstaande opties om een verbetering uit te voeren.

---

## Wat je doet nadat Copilot code heeft gewijzigd

1. **Lees de diff.** Open het bronbeheer-paneel (`Ctrl+Shift+G`) en lees wat er is veranderd.
2. **Vraag "waarom?"** Als je iets niet begrijpt, vraag Copilot: "Waarom heb je dit gewijzigd?"
3. **Bouw het project.**
   ```
   dotnet build CovAuto.sln
   ```
4. **Voer de app uit** en controleer of de pagina nog werkt. Open de browser en klik door de gewijzigde pagina.
5. **Als er iets kapot is,** gebruik `Ctrl+Z` of `git checkout -- <bestand>` om ongedaan te maken, en vraag Copilot wat er mis ging.
6. **Commit de wijziging** als je er tevreden mee bent:
   ```
   git add .
   git commit -m "verbetering: StelTokenHeaderIn-hulpfunctie extraheren"
   ```

---

## Hoe je een wijziging beoordeelt

Controleer nadat Copilot een verbetering heeft aangebracht:

- Bouwt de app nog steeds? (`dotnet build`)
- Laadt de gewijzigde pagina nog in de browser?
- Ziet het gedrag er hetzelfde uit? (zelfde data, zelfde foutmeldingen, zelfde knoppen)
- Kun je in je eigen woorden uitleggen wat er is veranderd?

Als je het niet kunt uitleggen, gebruik dan `upgrade-explain` om Copilot het te laten doorlopen.

---

## Waarom bouwen en testen belangrijk is

Een codewijziging lezen is niet genoeg.
Een wijziging kan er *goed uitzien* maar iets subtiel kapot maken — een ontbrekende `await`, een verkeerde variabelenaam, een ontbrekende `null`-check.

Bouw en test altijd na een verbetering. Dit is ook wat professionele ontwikkelaars doen.

---

## Vraag "waarom?" wanneer je iets niet begrijpt

Elke verbetering in deze workflow is ontworpen om begrijpelijk te zijn.
Als Copilot iets doet dat je niet volgt, vraag dan:

> "Kun je uitleggen waarom je dit hebt gewijzigd? Gebruik eenvoudige woorden."

Of gebruik de `upgrade-explain`-prompt vóór je überhaupt een wijziging maakt.

Er zijn geen domme vragen. Het doel is begrip, niet alleen nette code.

---

## Voorbeeldsessie

**Student opent `WorkOrders.razor` en koppelt `upgrade.prompt.md`:**

> **Copilot:**
>
> Wat ik zag:
> In `WorkOrders.razor`, regels 156–157, wordt het token opgehaald en de Authorization-header ingesteld.
> Dezelfde twee regels staan ook in `WorkOrderDetail.razor`, `Teams.razor` en 3 andere pagina's.
>
> Waarom dit belangrijk is:
> Als de manier waarop tokens worden gekoppeld ooit verandert (bijv. een andere headernaam), moet je 6 bestanden bijwerken.
>
> Verbeterstap:
> Ik extraheer een private `StelTokenHeaderIn()`-methode in `WorkOrders.razor` zodat de herhaling binnen deze ene pagina verdwijnt.
>
> Gewijzigde bestanden:
> - `CovAuto.Client/Pages/WorkOrders.razor`
>
> Hoe controleer je dit:
> Open `/werkorders` in de browser en controleer of de lijst nog correct laadt.
>
> Mogelijke volgende stap:
> Verplaats `StelTokenHeaderIn` naar een gedeelde statische `TokenHulp`-klasse zodat alle pagina's hem kunnen gebruiken.

**Student bouwt, controleert de pagina, commit, en koppelt daarna `upgrade-next.prompt.md`:**

> **Copilot:**
> (gaat verder naar stap 4 — extraheert de statische hulpklasse)

En zo verder, één stap tegelijk.

