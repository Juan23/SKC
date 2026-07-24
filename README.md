# SKC Desktop Apps

> **Archived 2026-07-24.** Development froze 2026-07-23 and this repo was removed from the
> active workspace; all feature work lives in [Juan23/skc-api](https://github.com/Juan23/skc-api)
> (API + webapp). The built exes remain installed and running until the webapp rollout replaces
> them, and the `SKC Admin CLI` remains the owner's live bulk-Excel admin tool (clone this repo
> to run it). WinForms-era workspace documentation is preserved in
> [`ARCHIVED-WORKSPACE-NOTES.md`](ARCHIVED-WORKSPACE-NOTES.md).

Windows desktop (WinForms) clients for the SKC bakery-supplies operations platform — the
office, branch, and owner apps that staff use day to day. Part of a larger system; the
central API, web app, and full overview live in
**[Juan23/skc-api](https://github.com/Juan23/skc-api)**.

## The apps

| App | Who uses it | What it does |
|---|---|---|
| **SKC Bakery Supplies** | Central office | Inventory, purchases, deliveries to branches, reports — plus an office-counter POS |
| **SKC Branch** | Each branch | Offline-first POS (its startup screen), delivery acceptance, production entry, end-of-day report |
| **SKC Admin** | Owner | Recipe, product-category, and selling-price administration |

One .NET 10 solution (`SKC.slnx`): the three apps above plus a small owner-only CLI
(`skcadmin`) for Excel-driven recipe and catalogue admin. Independent apps, each talking
to the API over HTTP.

## Highlights

- **Offline-first point-of-sale** — sales queue in a local SQLite store and sync to the
  API when online; idempotent on a client-minted GUID, so a crash mid-sync never loses or
  double-counts a sale.
- **Money kept exact** — decimals stored as text in SQLite to avoid float drift, and
  counter time preserved separately from sync time.
- **Deliberately no shared code** — each app owns its HTTP client and DTOs, a chosen
  convention for small, independently-installed binaries.

## Build

```
dotnet build SKC.slnx
```

Or open `SKC.slnx` in Visual Studio. Installed manually on each office/branch PC — no
auto-update.
