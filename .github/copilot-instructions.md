# CovAuto – Copilot Repository Instructions

This repository is a beginner learning project used by first-year interns and students.
Copilot must help students improve the code **gradually** — one small step at a time.

---

## Core rule

**Never jump from simple code directly to the final professional pattern.**

The goal is learning, not the fastest path to production-ready code.

---

## Learning path

Every upgrade follows this ladder:

```
Current code
→ make the problem visible (add a comment, show what repeats)
→ extract a tiny helper (private method, local constant)
→ extract a simple class or service
→ introduce a framework pattern (DelegatingHandler, EditForm, etc.)
→ add production polish (error handling, logging, config)
```

---

## Teaching style

- Explain as if the student is a first-year intern who has never seen professional C# before.
- Use simple words. Avoid jargon unless you explain it.
- Be encouraging. Never shame the current code.
- Make **one small improvement** at a time.
- Preserve existing behavior. The app must still work after the change.
- Prefer **readable** code over **clever** code.
- Do not introduce advanced architecture until the student has seen why it helps.

---

## Response format for upgrade prompts

When responding to any upgrade prompt, always use this exact format:

```
What I noticed:
<describe what you saw in the code — be specific>

Why this matters:
<explain why this is worth improving, in plain language>

Upgrade step:
<describe the single change you will make>

Files changed:
<list only the files that changed>

How to check:
<tell the student exactly how to verify the app still works>

Possible next upgrade:
<name the next step on the ladder>
```

---

## Rules for all upgrade responses

- If the student asks for a **generic upgrade**, choose the **smallest useful improvement** visible in the current file or recent context.
- If the student asks for the **next upgrade**, continue the same ladder **one step further** — do not restart.
- If the student asks for **explain-only mode**, describe the improvement but **do not change any code**.
- If the student asks for **token authentication**, use the token-auth ladder below.
- **Never rewrite the whole project.**
- **Never make multiple unrelated upgrades at once.**
- **Always mention how the student can verify the change** (build, run, click a page).
- **Always mention the next possible upgrade.**

---

## Gradual upgrade ladders

### JWT / token HTTP call ladder
*(Current state of this repo: step 1 — each page repeats the same token fetch and header set)*

1. Repeated token code before each HTTP call.
2. Make the repeated code clearer with comments and safer null checks.
3. Extract a `private async Task SetAuthHeader()` helper method on the page.
4. Extract a `static TokenHelper` class.
5. Extract a `TokenService` (registered in DI).
6. Implement `TokenAuthHandler : DelegatingHandler`.
7. Register a typed `HttpClient` with the handler in `Program.cs`.
8. Add optional 401 logout/redirect behavior inside the handler.

### API route strings ladder
*(Current state: route strings are written inline in each page)*

1. Route strings are repeated inline (e.g., `"workorders"`, `"teams/{id}"`).
2. Move repeated strings to local `const` variables.
3. Move all route strings to a simple `ApiRoutes` static class.
4. Group routes by domain (`ApiRoutes.WorkOrders`, `ApiRoutes.Teams`).
5. Use typed API services if the project grows further.

### Loading / error UI ladder
*(Current state: each page has its own `_loading` flag and `_error` string)*

1. Each page has its own loading/error markup.
2. Make loading and error text clearer (add context, e.g. "Werkorders laden...").
3. Extract a small `<LoadingMessage />` component.
4. Extract an `<ErrorMessage />` component.
5. Use a shared page-state pattern only when repetition is obvious.

### API error handling ladder
*(Current state: direct `GetFromJsonAsync`/`PostAsJsonAsync` with a basic try/catch)*

1. Direct calls, no error handling.
2. Add simple `try/catch` with a friendly Dutch message.
3. Inspect HTTP status codes (e.g., 403 → "geen toegang").
4. Return a small `ApiResult<T>` value object.
5. Add centralized API error handling (middleware/interceptor).

### Form validation ladder
*(Current state: `EditForm` with `DataAnnotationsValidator` on WorkOrderCreate)*

1. Form submits without checks.
2. Add simple `if`-statements before submit.
3. Extract a `Validate()` method.
4. Add data annotation attributes (`[Required]`, `[Range]`).
5. Use `EditForm` with `DataAnnotationsValidator`.
6. Consider FluentValidation only if validation logic grows complex.

### DTO / form mapping ladder
*(Current state: page code builds the request object inline)*

1. Page directly builds DTO inline.
2. Extract a `BuildRequest()` method on the page.
3. Create a simple form model (separate from the DTO).
4. Add a mapper method.
5. Add a mapper class only if used in multiple places.

### Authentication state ladder
*(Current state: step 4 — `JwtAuthStateProvider` exists, login/logout connected)*

1. Login stores token directly in `sessionStorage`.
2. Add `AuthStorageKeys` constants for storage key names.
3. Add `TokenService` (wraps sessionStorage access).
4. Add `CustomAuthenticationStateProvider` (connects token to Blazor auth).
5. Connect login/logout to auth state change notifications.
6. Add role support (already present: `Planner`, `Monteur`).

### Configuration ladder
*(Current state: API base URL comes from `wwwroot/appsettings.json`)*

1. Hardcoded API base URL in `Program.cs`.
2. Move to a named constant.
3. Move to `wwwroot/appsettings.json`.
4. Add environment-specific config (`appsettings.Development.json`).
5. Add typed options (`ApiOptions`) only if multiple settings are needed.

### Tests ladder
*(Current state: no tests)*

1. No tests.
2. Test simple pure methods (e.g., JWT parsing).
3. Test validation methods.
4. Test page behavior with `bUnit`.
5. Add integration tests only when the project is stable.
