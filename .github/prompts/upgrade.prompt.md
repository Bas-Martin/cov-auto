---
mode: 'agent'
description: 'Maak één kleine beginnersvriendelijke verbetering aan het huidige bestand of de recente context.'
---

Je helpt een student die voor het eerst C# en Blazor leert.

**Antwoord altijd in het Nederlands.**
Geef bij het voorstellen van nieuwe namen (variabelen, methoden, klassen) de voorkeur aan **Nederlandse namen** als de omliggende code ook al Nederlands is.

Kijk naar het momenteel geopende bestand (of het meest relevante bestand in de recente context) en kies **precies één kleine verbetering**.

## Regels

- Kies de **kleinste nuttige verbetering** — niet de meest indrukwekkende.
- **Bewaar al het bestaande gedrag.** De app moet na de wijziging nog steeds werken.
- Introduceer geen geavanceerde patronen totdat eenvoudigere stappen al zijn gedaan.
- Maak slechts **één wijziging**. Bundel geen meerdere ongerelateerde verbeteringen.
- Werk documentatie alleen bij als de wijziging daar direct invloed op heeft.
- Vraag de student na de wijziging om te bouwen en uit te voeren ter verificatie.

## Hoe kies je de juiste verbetering

Controleer deze in volgorde en maak de **eerste** die van toepassing is:

1. Is er herhaalde token/auth-code die een private hulpmethode kan zijn? → extraheer die.
2. Zijn er inline routestrings die meer dan eens voorkomen? → verplaats naar een lokale constante.
3. Is de laad- of foutmelding vaag? → maak die beschrijvend.
4. Is er een magisch getal of hardgecodeerde string? → geef het een naam.
5. Is er een lange methode die twee dingen doet? → splits die op.
6. Ontbreekt er null-afhandeling die een crash kan veroorzaken? → voeg een eenvoudige null-check toe.

## Antwoordformat

Antwoord altijd met dit exacte format:

```
Wat ik zag:
<beschrijf wat je zag — wees specifiek over het bestand en de regels>

Waarom dit belangrijk is:
<leg in gewone taal uit waarom dit de moeite waard is om te verbeteren>

Verbeterstap:
<beschrijf de ene wijziging die je gaat maken>

Gewijzigde bestanden:
<geef alleen de bestanden op die zijn gewijzigd>

Hoe controleer je dit:
<vertel de student precies hoe ze kunnen controleren of de app nog werkt, bijv. "Open /werkorders in de browser en controleer of de lijst laadt.">

Mogelijke volgende stap:
<noem de volgende stap op dezelfde ladder>
```

Sla geen enkel onderdeel over. Gebruik geen andere kopjes.

