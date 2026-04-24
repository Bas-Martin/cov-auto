# Student Upgrade Guide

This guide explains the `/upgrade` learning workflow built into this repository.
It shows you how to make the code gradually better — one small step at a time — using GitHub Copilot in VS Code.

---

## What is the upgrade workflow?

The upgrade workflow is a set of **reusable Copilot prompts** stored in `.github/prompts/`.
Each prompt tells Copilot to improve the code in a specific way, without overwhelming you with too many changes at once.

You do not need to know the "right answer" before you start.
You ask for an upgrade, Copilot makes one small change, you read it, run the app, and then ask for the next one.

---

## Why does this repo start simple?

The code in this repo is intentionally kept at a beginner level.
For example, every Blazor page that calls the API repeats the same two lines:

```csharp
var token = await AuthStateProvider.GetTokenAsync();
Http.DefaultRequestHeaders.Authorization =
    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
```

This is not a mistake. It is the **starting point**.
You can see exactly what happens. Nothing is hidden inside a framework class or a handler.

As you run upgrades, the code gets cleaner step by step — and you understand *why* each change was made.

---

## Why we do not jump straight to the best pattern

Imagine you asked "how do I handle JWT tokens in Blazor?" and someone immediately showed you this:

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

This is good professional code. But if you have never seen it before, you would not know:
- What is a `DelegatingHandler`?
- Why does it inherit that class?
- Where does `SendAsync` get called?
- What is `ITokenService` and where does it come from?

Instead, the upgrade workflow takes you through the steps one by one:

```
Step 1: Repeated token code in every page         ← current state
Step 2: Add comments and a null guard
Step 3: Extract a private helper method
Step 4: Extract a static TokenHelper class
Step 5: Extract a TokenService (registered in DI)
Step 6: Implement TokenAuthHandler : DelegatingHandler
Step 7: Register typed HttpClient with the handler
Step 8: Add 401 logout/redirect behavior
```

By the time you reach step 6, you already know *why* the handler exists.
You have already solved the same problem three different ways. The handler is just the clean version.

---

## How to use the prompts in VS Code Copilot Chat

The prompts live in `.github/prompts/`. VS Code Copilot Chat can use them as **reusable prompt files**.

### Step 1 — Open Copilot Chat

Press `Ctrl+Alt+I` (Windows/Linux) or `Cmd+Alt+I` (Mac), or click the Copilot Chat icon in the sidebar.

### Step 2 — Open the file you want to improve

For example, open `CovAuto.Client/Pages/WorkOrders.razor`.

### Step 3 — Attach the prompt file

In Copilot Chat, click the **paperclip / attach** icon and select:
```
.github/prompts/upgrade.prompt.md
```

Or type `#` in the chat input and search for the prompt file by name.

### Step 4 — Send

Press Enter. Copilot will read the current file and the prompt instructions, then make one small improvement.

---

## Available prompt commands

| Prompt file | What it does |
|-------------|--------------|
| `upgrade.prompt.md` | Makes one small improvement to the current file. Choose the smallest useful step. |
| `upgrade-next.prompt.md` | Continues the same improvement direction one step further. |
| `upgrade-explain.prompt.md` | Explains the next improvement **without changing any code**. Good for understanding before acting. |
| `upgrade-token-auth.prompt.md` | Focuses only on JWT/token authentication. Uses the token auth ladder. |

### How to run them

**Option A — Attach in Copilot Chat (recommended)**
1. Open the file you want to improve.
2. Open Copilot Chat (`Ctrl+Alt+I`).
3. Click the paperclip icon → select the prompt file.
4. Press Enter.

**Option B — Type `#` in the chat input**
1. In Copilot Chat, type `#` and start typing the prompt name (e.g., `upgrade`).
2. Select the matching prompt file from the dropdown.
3. Press Enter.

**Option C — Copilot Edits (VS Code 1.93+)**
1. Open Copilot Edits (`Ctrl+Shift+I`).
2. Drag the prompt file into the chat, or use `#` to attach it.
3. Copilot Edits will apply the change directly to the file.

> **Note about `/upgrade` as a slash command:**
> VS Code Copilot does not support registering arbitrary `/upgrade` slash commands from prompt files.
> The prompts in `.github/prompts/` are reusable prompt files — not true slash commands.
> The `/` commands in Copilot Chat are built-in commands (like `/explain`, `/fix`, `/tests`).
> To run an upgrade, always attach the prompt file using one of the options above.

---

## What to do after Copilot changes code

1. **Read the diff.** Open the Source Control panel (`Ctrl+Shift+G`) and read what changed.
2. **Ask "why?"** If you do not understand something, ask Copilot: "Why did you change this?"
3. **Build the project.**
   ```
   dotnet build CovAuto.sln
   ```
4. **Run the app** and verify the page still works. Open the browser and click through the changed page.
5. **If something broke,** use `Ctrl+Z` or `git checkout -- <file>` to undo, and ask Copilot to explain what went wrong.
6. **Commit the change** if you are happy with it:
   ```
   git add .
   git commit -m "upgrade: extract SetAuthHeader helper"
   ```

---

## How to review a change

After Copilot applies an upgrade, check:

- Does the app still build? (`dotnet build`)
- Does the changed page still load in the browser?
- Does the behavior look the same? (same data, same errors, same buttons)
- Can you explain what changed in your own words?

If you cannot explain it, use `upgrade-explain` to ask Copilot to walk through it.

---

## Why running and building matters

Reading a code change is not enough.
A change can *look* correct but break something subtle — a missing `await`, a wrong variable name, a missing `null` check.

Always build and test after an upgrade. This is what professional developers do too.

---

## Ask "why" whenever you do not understand

Every upgrade in this workflow is designed to be understandable.
If Copilot does something you do not follow, ask:

> "Can you explain why you changed this? Use simple words."

Or use the `upgrade-explain` prompt before making any change at all.

There are no bad questions. The goal is understanding, not just having clean code.

---

## Example session

**Student opens `WorkOrders.razor` and attaches `upgrade.prompt.md`:**

> **Copilot:**
>
> What I noticed:
> In `WorkOrders.razor`, lines 156–157 fetch the token and set the Authorization header.
> The same two lines appear in `WorkOrderDetail.razor`, `Teams.razor`, and 3 other pages.
>
> Why this matters:
> If the way tokens are attached ever changes (e.g., a different header name), you would need to update 6 files.
>
> Upgrade step:
> I will extract a private `SetAuthHeaderAsync()` method in `WorkOrders.razor` so the duplication is gone within this one page.
>
> Files changed:
> - `CovAuto.Client/Pages/WorkOrders.razor`
>
> How to check:
> Open `/werkorders` in the browser and check that the list still loads correctly.
>
> Possible next upgrade:
> Move `SetAuthHeaderAsync` to a shared static `TokenHelper` class so all pages can use it.

**Student builds, checks the page, commits, then attaches `upgrade-next.prompt.md`:**

> **Copilot:**
> (continues to step 4 — extracts the static helper class)

And so on, one step at a time.
