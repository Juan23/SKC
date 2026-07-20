# SKC Desktop Apps

WinForms client apps for the SKC inventory / delivery / production / sales system,
used by SKC Bakery Supplies (central office) and its branches. The server side is the
separate `Juan23/skc-api` repo (ASP.NET Core API + Postgres on a droplet, reached over
Tailscale).

## Solution layout

One solution, `SKC.slnx`, three independent projects (all `net10.0-windows`, no
project references between them — see "Conventions" below):

| Project | Who runs it | What it does |
|---|---|---|
| **SKC Bakery Supplies** | Central office | Inventory, purchases, supplier deliveries to branches, reports. Also has an office-counter POS (`frmPos`) selling raw materials & miscellaneous items, since the office is a storefront too. |
| **SKC Branch** | Each branch PC | Starts on the POS screen; delivery acceptance, stock view, baking/decorating production entry, sales history, and a printable end-of-day sales report open from its nav. Branch identity is picked once and remembered in `%APPDATA%\SKC Branch\config.json`. |
| **SKC Admin** | Owner only | Recipe management, product category/UoM classification, selling-price administration. Access is enforced server-side (owner IP allowlist), not by the app. |

## Building

```
dotnet build SKC.slnx
```

Or open `SKC.slnx` in Visual Studio. There are no automated tests; verification is a
clean build plus curl-testing the API contracts a screen depends on (WinForms GUIs
can't be click-tested headlessly).

## Offline-first POS

Both POS screens (SKC Branch and the office POS in SKC Bakery Supplies) queue sales in
a local SQLite db (`%APPDATA%\<App Name>\pos.db`) via `PosLocalStore` + `PosSyncEngine`,
and sync to `POST /api/sales` on a 60s timer plus a manual button. Sync is
push-then-pull and idempotent on a client-minted sale GUID, so crashes mid-sync are
harmless. Two invariants to preserve in any change:

- **Money columns are TEXT, not REAL** — Microsoft.Data.Sqlite binds `decimal` as text;
  REAL would coerce through a double and lock in drift (`145.70 → 145.6999…`).
- **`sold_at` is counter time, not sync time** — offline sales sync late.

The two POS copies are intentionally separate code (see Conventions); the only
functional difference is the catalog filter — SKC Branch excludes `RawMaterial`,
the office POS shows only `RawMaterial` + `Miscellaneous`.

**No customer-receipt printing — ever.** This is a BIR (Bureau of Internal Revenue)
compliance constraint, not a missing feature: printing customer receipts from an
unregistered POS would expose the business to penalties, so receipts stay pen-and-paper.
Do not add one. The *internal* end-of-day sales report in SKC Branch (`frmSalesReport`
— date range, per-sale list, signed-off summary, CSV export for Excel) is a different
thing and carries no such exposure.

## Conventions

- **No shared code between apps.** Each app has its own static `*ApiClient`
  (`CentralApiClient.cs`, `BranchApiClient.cs`, …): a static `HttpClient`, 10s timeout,
  case-insensitive JSON, one method per endpoint, throws with the response body on
  non-success. New apps copy this shape rather than referencing a shared project —
  deliberate, not an oversight.
- **`ApiBaseUrl`** is hardcoded to the droplet (`http://100.84.79.35:7290`) with a
  commented-out localhost line for local dev — revert to the droplet address before
  committing.
- **Designer files:** layout lives in `.Designer.cs` (editable directly, but keep it
  in the dialect the VS Designer can round-trip — no logic in `InitializeComponent`).
  Main SKC Branch screens are designed at ClientSize 1280×640 on the 96-DPI (7F,15F)
  baseline.
- **Branch names are exact-match strings** repeated across apps and the DB
  (`Yoho`, `Gaisano`, `Liloy`, `Labason`) — adding or renaming a branch means updating
  every occurrence in the same change.

## Releasing

All three apps share `<Version>` in their csproj files (currently **2.4.0**) — bump it
together when shipping. There's no auto-update: builds are installed manually on each
PC. SKC Branch has a publish profile at
`SKC Branch/Properties/PublishProfiles/FolderProfile.pubxml`.
