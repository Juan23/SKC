# ARCHIVED-WORKSPACE-NOTES

WinForms-exclusive documentation extracted from the SKC workspace docs (`CLAUDE.md`,
`spec-status.md`, `bug-track.md`, `spec.md`) on **2026-07-24**, when this repo was archived
and removed from the active workspace. Development froze 2026-07-23; all feature work moved
to [`Juan23/skc-api`](https://github.com/Juan23/skc-api) (API + webapp) and
[`Juan23/skc-web`](https://github.com/Juan23/skc-web).

Context worth keeping in mind when reading (or resurrecting) anything below:

- **The built exes remained installed and running** at the office and branch PCs at archive
  time — v2.4.0, calling the plain-HTTP API (`http://100.84.79.35:7290`) cookie-less through
  the IP/device gates. The server keeps those wire contracts stable until the webapp rollout
  replaces every installed client.
- **The office-counter POS (`frmPos` in SKC Bakery Supplies) was still the live office till**
  at archive time — the web POS was Branch-login-only and excluded `RawMaterial`.
- **SKC Admin CLI (`skcadmin`) is NOT part of the freeze** — it remains the owner's live bulk
  Excel admin path. Its docs are preserved below with the rest; re-clone this repo to run it.
- Everything below is verbatim as it stood in the workspace docs, grouped by source file.
  Section headers say where each chunk came from.

---

> The full original `spec.md` (the multi-repo pre-implementation design doc, majority WinForms-era) is preserved byte-for-byte as [`ARCHIVED-spec-original.md`](ARCHIVED-spec-original.md) in this repo, since its escaped-markdown formatting made chunk extraction lossy. The chunks below cover the other three workspace docs.

## From spec-status.md — Phase 1 — WinForms client work (Central + Branch apps)

**Central app (`SKC Bakery Supplies`, commit `aa81933`)**
- `DeliveryTicketSummary` now shows `Status` in the ViewDeliveries grid.
- Delete failures (e.g. blocked deletes on accepted tickets) surface as a message box instead of an unhandled error.

**New branch-side app (`SKC Branch`, commit `aa81933`)**
- First-run branch picker (persisted to local config) — no login/identity, matches spec step 5 not being built yet.
- Pending-deliveries list per branch, delivery line detail, and an Accept action that records who accepted.

## From spec-status.md — Phase 2 — WinForms client work (Central + Branch apps)

**Central app (`SKC Bakery Supplies`, commit `291436f`)**
- New "Branch Inventory Report" screen (branch dropdown + live stock grid + print).

**SKC Branch app (commit `291436f`)**
- New "My Stock" view showing the logged-in branch's own current stock.

## From spec-status.md — Post-Phase-2 — WinForms client work (Central + Branch apps)

**Central app (`SKC Bakery Supplies`, commit `211a5e7`)**
- **Adjust Stock** on the Branch Inventory Report — reconciles a branch's physical count (reuses `frmAdjustInventory`, now branch-aware).
- **Edit** on View Deliveries for InTransit tickets — deletes the ticket (restocking Office) and reopens Create Delivery pre-filled, so the admin amends without retyping. Edit/Delete disable once accepted; blocked-delete shows the plain message, not raw ProblemDetails JSON.
- Title bar shows `<Version>` (2.1.0).

**SKC Branch app (commit `211a5e7`)**
- Distinct "This delivery was changed by the office — please review" prompt when an accept 404s (ticket amended/deleted). Version in title bar. Single-file publish profile added; `frmMain.resx` now tracked so a clean clone builds.


## From spec-status.md — Baking/decorating production module — WinForms client work (SKC Admin / Branch / Central apps)

**New app: SKC Admin (`repos`, commit `e8c0c4e`)**
- Small WinForms app (no shared references, same shape as SKC Branch), reachable only from the owner's IP (enforced server-side, not client-side). Two tabs: Products (set category/UoM per SKU) and Recipes (create/edit/deactivate, with a line editor for inputs).

**SKC Branch app (commit `461d455`, bumped to 2.2.0)**
- New "Bake / Decorate" screen: pick a recipe (filtered by kind), set a batch multiplier and actual output qty (editable for yield variance), record who did it, preview what will be consumed from the branch's own stock, submit. A small history dialog lists past batches.

**Central app (`SKC Bakery Supplies`, commit `7d284ce`, bumped to 2.2.0)**
- Purchases screen: a "Buy by pack" toggle appears when the selected product has a pack conversion set (e.g. "Sack (25kg)" = 25000 g). Qty/cost are entered as pack-count/cost-per-pack and converted to base units before the line is added — everything downstream (lots, recipes) stays in base units.

## From spec-status.md — Offline-first POS — WinForms client work (SKC Branch / Admin / Bakery Supplies)

**SKC Branch (commit `f41ea10`, 2.3.0)**
- POS screen: type-ahead search over the local catalog cache (only priced items findable — the "no such item" forcing function to keep production recorded), cart, discount button, cash→change, big Complete Sale. Low-stock warning with proceed/cancel.
- `PosLocalStore` (`%APPDATA%\SKC Branch\pos.db`, Microsoft.Data.Sqlite + Dapper — a deliberate, documented deviation from the no-packages convention): catalog cache, durable pending-sales queue, day log. Cached stock decremented locally per sale so intra-day offline sales inform the warning; overwritten by the authoritative pull each sync.
- `PosSyncEngine`: push-then-pull on a 60s timer + manual Sync Now; rejected sales leave the queue but stay visible in the day log with the server's reason.
- Day log ("Today's Sales") = the poor man's Z-report; offline startup no longer throws blocking modals.

**SKC Admin (commit `f41ea10`, 2.3.0)** — Set Price on the Products tab (with a derived Sellable column).

**SKC Bakery Supplies (commit `f41ea10`, 2.3.0)** — Branch Sales Report: branch + date range, sale line detail, shortfall-flagged sales highlighted, day totals.

## From spec-status.md — 2026-07-18 second audit — branch-side sales history (WinForms frmBranchSalesHistory)

- **Branch-side sales history** — a new `frmBranchSalesHistory` screen in SKC Branch (nav button on the POS) giving each branch its own date-range sales view + line detail, reusing the existing `GET /api/sales` endpoints (no new server read). Uses the inclusive-end date pattern.

## From spec-status.md — Office POS — WinForms client details (SKC Branch filter, office frmPos, rollout step)

- **SKC Branch POS** now excludes `RawMaterial` from `GetSellableCatalog()` even when priced — previously only `Price > 0` gated the counter, so a priced raw material would incorrectly have been sellable at a branch.
- **New office POS** (`SKC Bakery Supplies`) — `frmPos`/`frmPosDayLog`/`frmPosSalesHistory` + their own `PosLocalStore`/`PosSyncEngine` (own `%APPDATA%\SKC Bakery Supplies\pos.db`), reusing the existing `BakeryProduct`/`BranchSaleSummary`/`BranchSaleLine` DTOs and adding `PosDtos.cs` for the rest. `CentralApiClient` gained `PushSalesAsync`/`VoidSaleAsync` (the read-side methods already existed, used by the existing Branch Sales Report). Branch is hardcoded to the const `"Office"` — no picker, since there's one office counter — which is the same `branch_name` purchases/deliveries already write to `inventory_lots`, so office sales draw from and reconcile with real office stock rather than a disconnected pool. Sellable filter is `Price > 0 AND Category IN ('RawMaterial','Miscellaneous')`, the inverse of the branch filter. `Home.cs` gained a "POS" launcher button; `BranchSalesReport.cs`'s branch dropdown gained "Office" so office-counter sales are visible in the existing report.
- Manual step still needed before rollout: the owner must reclassify existing sellable-but-mistagged `RawMaterial` items (candles, cellophane, etc.) to `Miscellaneous` via SKC Admin, or they'll disappear from the branch POS the moment this ships (SKC Branch's new `RawMaterial` exclusion would otherwise hide them).

## From spec-status.md — Branch end-of-day sales report — WinForms client details (frmSalesReport)

- **New `frmSalesReport` in SKC Branch**, opened from a "Sales Report / Print..." button on the POS rail (the rail's totals block shifted down to make room). Date range defaulting to today; per-sale grid; `PrintDocument` preview + print rendering the sale list, a summary block, and "Counted by / Verified by" signature lines; and a **CSV export** (UTF-8 BOM, invariant decimals, RFC-style quoting) for the owner's Excel analytics workbook.
- **Offline fallback, today-only** — if the server is unreachable, a today-only range prints from the local POS SQLite store, banner-flagged on screen and on paper, since closing time is exactly when a branch may be offline. Past ranges can't fall back (the local store keeps a day log, not history). **CSV export has no fallback** — item lines for synced sales exist only server-side.
- Reviewed by `skc-code-tester`, which found three real bugs, all fixed and re-verified: a printed header that read the live date pickers instead of the loaded range (so it could disagree with the rows beneath it), a stale offline flag that blocked valid exports, and a print-footer reservation that overflowed the page margin once a range contained both a voided and a shortfall sale. As always, **the WinForms screens themselves were not click-tested** — no GUI session available.

## From CLAUDE.md — WinForms development stopped notice (2026-07-23, superseded by archival)

## ⚠️ WinForms development stopped — 2026-07-23

**As of 2026-07-23, `repos/` (the WinForms client apps) is stopped.** The owner has explicitly said to stop working on the WinForms GUI (SKC Admin's condition for this, noted below, has now been met) — **all future feature work goes into `skc-api/` (API + webapp) only.** This is a deliberate resourcing decision, not a technical dead-end: the webapp already mirrors most WinForms screens and is the actively-developed front end going forward.

Practical implications:
- Don't propose or make changes to `repos/` (SKC Bakery Supplies, SKC Branch, SKC Admin) unless the owner explicitly asks for one — treat it as frozen/maintenance-only.
- **SKC Admin CLI (`skcadmin`) is unaffected** — it's part of `skc-api`'s owner-tooling story, not the frozen WinForms GUI stack, and stays a live option for recipe/product admin.
- Any schema/API change should be designed webapp-first; backward compatibility for the frozen WinForms clients (so they keep working, not so they gain new capabilities) is a nice-to-have, not a requirement that should shape the design.
- If a feature genuinely can't be done without a WinForms change, flag that explicitly and ask before touching `repos/`.

## From CLAUDE.md — Workspace layout — repos/ bullet

- `repos/` — git repo `Juan23/SKC.git`, a Visual Studio solution (`SKC.slnx` at the repo root) with four .NET projects (the client side: three WinForms apps plus the SKC Admin CLI console app). **Frozen as of 2026-07-23 — see the notice above; no new feature work here.**

## From CLAUDE.md — repos/ — client apps section (conventions, POS local store, gotchas)

## `repos/` — client apps

> **Frozen 2026-07-23** — reference/maintenance documentation only. No new feature work goes here; see the notice near the top of this file.

Four .NET projects under one solution (`SKC.slnx`) — three WinForms apps targeting `net10.0-windows` plus one console app targeting `net10.0` — none referencing each other:

- **SKC Bakery Supplies** — the central office app (inventory, purchases, deliveries, reports incl. Branch Sales Report; delivery "Edit Ticket" amends an unaccepted ticket via delete-at-resubmit + prefilled recreate). Also has its own **office-counter POS** (`frmPos`, opened from Home's "POS" button, not a startup screen like SKC Branch's) — the office is both the delivery hub and a storefront that sells `RawMaterial` + `Miscellaneous` items. It's a purpose-built copy of SKC Branch's POS pattern (own `PosLocalStore`/`PosSyncEngine`, own `%APPDATA%\SKC Bakery Supplies\pos.db`), trimmed of the Deliveries/Bake-Decorate nav that doesn't apply to the office; `branchName` is hardcoded to the const `"Office"` rather than picked, since there's exactly one office counter.
- **SKC Branch** — the branch-side app. **Startup screen is the POS (`frmPos`)**; delivery acceptance, stock view, and baking/decorating production entry open from its nav buttons. Branch identity is remembered in `%APPDATA%\SKC Branch\config.json` (`BranchConfig`); `frmBranchPicker` only shows on first run. Takes the NuGet packages the POS store needs (Dapper + Microsoft.Data.Sqlite) but no project references; deliberately copies patterns from the office app instead of sharing code (see below). Its `PosLocalStore.GetSellableCatalog()` excludes `RawMaterial` even when priced — raw materials only ever sell from the office POS.
- **SKC Admin** — owner-only app for recipe management, product category/UoM classification, and selling-price administration. Access is enforced server-side (`IsOwnerCaller`), not by the app itself — same no-shared-code convention as SKC Branch.
- **SKC Admin CLI** (`skcadmin`, added 2026-07-20) — owner-only console app, originally piloted **alongside** the SKC Admin WinForms app. **As of 2026-07-23 the WinForms GUI is frozen (see the notice above) — `skcadmin` is now the live path for recipe/product admin**, not just a pilot. Same trust model and no-shared-code convention (own `AdminApiClient`/DTO copies); takes NuGet packages ClosedXML + Spectre.Console. Commands: `health`, `products [search]`, `products template <xlsx>` / `products import <xlsx> [--dry-run] [--yes]` (same Excel round-trip pattern for the catalog's editable fields — Price via the owner-gated price endpoint, Category/UoM/PackMultiplier via the office-gated classification endpoint; match by SKU, diff against the live catalog, push changed rows only), `recipes`, `recipes template <xlsx>`, `recipes import <xlsx> [--dry-run] [--yes]`, `recipes deactivate <id>`; bare `skcadmin` in a real terminal opens an interactive arrow-key menu (a thin dispatch layer over those same command methods — by design it also serves as the owner's feature map; with stdin/stdout redirected it prints help instead, keeping scripted runs deterministic). The recipe workflow is Excel-first by the owner's design: `recipes template` writes a workbook pre-filled with the live recipes (plus a Products reference sheet), the owner edits it in Excel (SKUs are the join key), and `recipes import` validates and upserts **by recipe name** (create new / replace an existing one's lines; server recipes absent from the file are left untouched; identical recipes are skipped, so re-import is idempotent). Workbook format: one flat `Recipes` sheet, one row per input line, header columns (Name/Kind/Output SKU/Output Qty) only on a recipe's first row, a fully blank row ends a recipe. Being a console app, it is **directly runnable/testable in this environment** (`dotnet run --project "repos/SKC Admin CLI" -- ...`) against the live droplet — unlike the WinForms apps; import/deactivate refuse to prompt when stdin *or stdout* is redirected (pass `--yes`). Known deferred gap: deactivated recipe names can be re-created as duplicates by import (see `/bug-track.md` Open).

(The legacy `SKC POS` and `SKC.DataEngine` projects — the pre-cloud CSV uploader and its local SQLite store — were deleted on 2026-07-17. The live POS lives inside SKC Branch.)

**Commands** (from `repos/`, or open `SKC.slnx` in Visual Studio):
```
dotnet build SKC.slnx
```
No automated tests — see "Testing & verification methodology" below; for client apps specifically, that means a clean build plus directly verifying the API contracts a screen depends on, since GUI click-testing isn't available in this environment.

**Client → API pattern**: each WinForms app has its own static `*ApiClient` class (`CentralApiClient.cs` in SKC Bakery Supplies, `BranchApiClient.cs` in SKC Branch) — a static `HttpClient` with a 30s timeout, case-insensitive JSON deserialization, a hardcoded `ApiBaseUrl` pointing at the droplet with a commented-out localhost line for local dev, and one method per endpoint that throws on non-success status with the response body in the message. **New client apps copy this shape rather than referencing a shared HTTP client project** — this is an intentional convention in the codebase (see `/spec.md` Part C: "No NuGet packages, no project references. Follow the main app's patterns throughout."), not an oversight to fix.

**POS local store (`PosLocalStore.cs` + `PosSyncEngine.cs`, duplicated in both `SKC Branch/` and `SKC Bakery Supplies/`)**: an offline-first SQLite db at `%APPDATA%\<App Name>\pos.db` — catalog/price/stock cache (refreshed on every sync), a durable queue of unsynced sales, and a synced-sales log. `PosSyncEngine.SyncAsync` is push-then-pull (push sales first so the pulled stock snapshot already reflects them) and relies on the server's `(branch_name, client_sale_id)` idempotency, so a crash mid-sync is harmless. Two gotchas baked into it: **money columns are TEXT, not REAL** (Microsoft.Data.Sqlite binds `decimal` as text; REAL affinity would coerce it through a double and lock in drift like `145.70 → 145.6999…`), and `sold_at` is **counter time, not sync time** (offline sales sync late) — preserve both properties in any change. The two copies are intentionally separate per the no-shared-code convention (a shared-library option was considered and explicitly declined in favor of two independent, purpose-built POS screens); a fix to one (e.g. the timezone/DateTimeKind handling) does not automatically propagate to the other and must be ported by hand if it recurs. The only difference between the two copies' `GetSellableCatalog()` queries is the category filter: SKC Branch excludes `RawMaterial`; SKC Bakery Supplies allows only `RawMaterial`/`Miscellaneous`.

**Before committing**: any hardcoded `ApiBaseUrl` pointed at `localhost` for testing must be reverted to the droplet address (`http://100.84.79.35:7290`) before commit — this is a recurring manual step called out in `/spec.md`'s verification checklist.

**`DataGridView` column config must be re-applied after every `DataSource` assignment**, not just the initial load — rebinding regenerates the auto-generated columns, so hidden columns reappear and any `DefaultCellStyle.Format` is dropped. The established pattern is a small `ApplyGridDisplay()` called after each bind (see `SKC Bakery Supplies/ViewProducts.cs`). Money columns use `Format = "N2"`: anything bound to a `NUMERIC(18,4)` column (`unit_cost`, `consumed_cost`, `total_line_cost`, `total_input_cost`) will otherwise render a long decimal tail, since those values come from divisions. `pack_multiplier` is also 18,4 but is deliberately left unformatted — it's a unit conversion factor, not money. **This formatting currently exists only in `SKC Bakery Supplies`** — the no-shared-code convention means `SKC Branch`/`SKC Admin` need it ported by hand (tracked in `/bug-track.md` Open).

**`.Designer.cs` files** may be edited directly (the owner explicitly allowed this on 2026-07-17; the old deny rule in `SKC Bakery Supplies/.claudesettings.json` is gone). Keep them in the dialect the VS Designer's serializer can round-trip — field declarations plus straightforward property/layout statements inside `InitializeComponent`, no logic — so the form stays openable in the Designer. Layout belongs in the `.Designer.cs` half of the pair, not built programmatically in the form's constructor.

## From CLAUDE.md — Testing — WinForms client verification + form layout audit

**Client (WinForms) apps**: `dotnet build SKC.slnx` only verifies compilation, not behavior. There is no way to click-test a WinForms GUI in this environment (no interactive desktop session) — say so explicitly rather than claiming a screen "works." Verify instead by (a) confirming the build is clean, (b) directly `curl`-testing every API contract the screen depends on against the live droplet, and (c) reading the client logic closely for correctness (parameter mapping, error handling, reset-after-submit). Flag GUI-untested screens plainly when reporting work as done.

**Form layout changes** additionally get a geometry audit: `python .claude/tools/designer_layout_audit.py "repos/<Project>" --allow-overlap lstSearch` checks overlaps, bounds, the 96-DPI (7,15) AutoScale baseline, and anchor behavior at +15px client height — or use the `skc-layout-auditor` agent, which runs it plus wiring/dialect checks and emits an HTML wireframe mockup for visual review. Convention: main SKC Branch screens are designed at ClientSize 1280×640 (720p screen, maximized, FixedSingle) on the (7F,15F) baseline so Designer coordinates are literal screen pixels.

## From bug-track.md — under "2026-07-21 SKC Branch POS feature + bug sweep (client-only; committed `SKC@3d82bdd`, NOT yet installed on branch PCs)"

## 2026-07-21 SKC Branch POS feature + bug sweep (client-only; committed `SKC@3d82bdd`, NOT yet installed on branch PCs)

A "runover the POS + list missing features + bug sweep" pass over the SKC Branch POS (`frmPos` / `PosLocalStore` / `PosSyncEngine` / `frmPosDayLog` / `frmBranchSalesHistory`) and its server contracts. Owner scope for the round: build two features (counter UX polish + sync-error surfacing) and fix client-side bugs; **server findings logged in Open, not fixed** (see the deferred-server-findings item there). Every change ported by hand to **both** POS copies (SKC Branch + the SKC Bakery Supplies office counter) per the no-shared-code convention. `dotnet build SKC.slnx` clean (0/0). Reviewed by `skc-code-tester` (no functional bugs found) and `skc-layout-auditor` (geometry/dialect/wiring all clear). **GUI-untested** (no click-test env) and **not deployed** — client apps install manually on branch PCs, so this ships only on the next build push; no `<Version>` bump done.

Features:
- **Inline cart quantity editing.** The cart grid was read-only (changing a qty meant Remove + re-add). Now only the Qty column is editable (`dgvCart.ReadOnly=false` + a constructor loop locking every other column; `EditMode=EditOnKeystrokeOrF2`), with `CellBeginEdit` (blocks discount lines), `CellValidating` (whole number 1..1,000,000 matching `numQty`, else cancel), `CellEndEdit` (recompute `LineTotal`, refresh totals, re-run the warn-but-allow oversell check), and `DataError` (swallow) handlers. Both `frmPos` copies + `.Designer.cs`.
- **Sync-error surfacing.** A whole-batch HTTP failure (403/400/500) used to be swallowed by `PosSyncEngine` as "offline" — stuck OFFLINE badge, silent forever-retry. New `PosSyncHttpException` (thrown from `PushSalesAsync` on non-2xx) is caught distinctly from `OfflineException`; the status bar now shows `SYNC ERROR (HTTP …)` and a once-per-distinct-error modal fires. This also fixes the actual **per-sale IP-drift case** (see the mitigated IP-gate item under Open) — a gate `Rejected` is kept queued for retry instead of moving to the terminal log.

Client bug fixes (both copies):
- **Disposed-form sync crash** — `RunSyncAsync` touched `lblSyncStatus` after its `await`; a timer tick in flight at form close threw `ObjectDisposedException`. Now stops `syncTimer` in `FormClosing` (restarts if the close is cancelled) and guards `IsDisposed/Disposing` after the await.
- **Negative cached-stock display** — `DecrementCachedStock` had no floor → "(stock: -N)" after an offline oversell; now `MAX(Stock - @qty, 0)`.
- **History detail selection race** — rapid arrow-keying in `frmBranchSalesHistory`/`frmPosSalesHistory` let an out-of-order line fetch fill the detail grid for the wrong row; added a monotonic `selectionLoadToken` (discards stale success *and* stale failure).
- **TEXT-affinity price test** — `GetSellableCatalog`'s `WHERE Price > 0` string-compared a TEXT column; now `CAST(Price AS REAL) > 0`. Branch also `COALESCE(Category,'') != 'RawMaterial'` so a NULL category can't silently hide a priced item (its `!=` blacklist would exclude NULL; the office `IN(...)` whitelist already excludes NULL correctly, so it deliberately keeps no COALESCE).
- **Held-Enter insta-confirm** — the complete-sale confirm now defaults to No (`MessageBoxDefaultButton.Button2`).
- Also ported the office copy's **N2 money formatting** to the branch `frmPos` cart, `frmPosDayLog`, and `frmBranchSalesHistory` grids (see the N2 Open entry — its SKC Branch POS-screen part is now done).

Not changed (verified non-issues): the reported double-submit on Complete Sale (the modal confirm blocks re-entry and the cart clears before the only await — refuted), the empty-snapshot catalog guard (acknowledged tradeoff, #6 Fixed), receipts / past-day void (owner constraints).

**One thing needing a physical click-test before/at install** (can't be verified in this env, per the project's own methodology): the qty-edit uses `MessageBox.Show` inside `CellValidating`/`CellEndEdit` — a documented WinForms focus-reentrancy risk area. `skc-code-tester` found nothing in the surrounding code that would concretely trigger it and did not report it as a bug, but flagged it for a manual pass (rapid Tab/Enter through an invalid qty, and edit-then-immediately-click-Complete-Sale) on a real branch PC. If it ever misbehaves, defer the modal out of the validation call stack (`BeginInvoke`) or replace it with the cell `ErrorText` pattern.

## From bug-track.md — under "2026-07-18 audit round (deployed and curl-verified 2026-07-19)"

- **#3 MEDIUM — Submit idempotency.** Production/purchase/delivery minted a new `transaction_id` per click, so a response lost after commit → re-click → duplicate record. Added a server-side dedup guard (after the advisory lock) on `transaction_id` in `POST /api/purchases`/`/api/deliveries`/`/api/production`; clients now reuse the id across a retry (cleared only on confirmed success) and disable the submit button during the await (Delivery.cs/Purchases.cs gained the guard frmProduction.cs already had).
- **#4 LOW — Fractional multiplier consume-but-zero-output.** `frmProduction.cs` now confirms before recording a 0-output batch (kept server-permissive so a deliberate burnt batch stays possible).

## From bug-track.md — under "2026-07-18 audit round (deployed and curl-verified 2026-07-19)"

- **#6 LOW — Empty stock pull wiped the POS catalog cache.** `PosLocalStore.RefreshCatalog` now early-returns on an empty/null snapshot instead of DELETE-ing the cache.

## From bug-track.md — under "2026-07-18 audit round (deployed and curl-verified 2026-07-19)"

- Submit buttons in `Delivery.cs`/`Purchases.cs` weren't disabled during the in-flight submit (a double-click minted two different ids, defeating the dedup) — added the guard `frmProduction.cs` already had.
- `dgvSales_SelectionChanged` fired during the programmatic `DataSource` reload, populating the detail grid for an unselected row and racing rapid clicks — added a `suppressSelectionLoad` re-entrancy guard in **both** the new `frmBranchSalesHistory` and the pre-existing `BranchSalesReport` (same latent bug).
- POS day-log void: a local `MarkSaleVoided` failure *after* a successful server void showed a misleading "are you online?" error — split so the local mark is best-effort and only the network call reports connectivity failure.

## From bug-track.md — under "Fixed"

- **"Edit Ticket" could silently lose an entire delivery with no recovery path.** `btnEdit_Click` in [ViewDeliveries.cs](repos/SKC%20Bakery%20Supplies/ViewDeliveries.cs) used to delete the original ticket *before* opening the pre-filled edit screen; closing that screen without submitting lost the delivery permanently. Fixed by moving the delete into `frmDelivery.btnSubmitDelivery_Click` ([Delivery.cs](repos/SKC%20Bakery%20Supplies/Delivery.cs)) so it only happens at the moment of resubmission — backing out of the editor now leaves the original ticket untouched, and a resubmit failure after a successful delete leaves the draft items in place for a retry instead of clearing them.
- **`SKC POS/Upload Sales.cs` tagged sales with branch name "Ipil"**, which doesn't exist anywhere else in the system (everywhere else uses "Yoho"). Fixed by relabeling the radio button and its submitted value to "Yoho" in code (the Designer field is still internally named `rdoIpil` since `.Designer.cs` files aren't hand-edited per workspace convention, but the displayed text and the string sent now both read "Yoho").

## From bug-track.md — under "Fixed"

- **CLAUDE.md's list of hardcoded branch-name locations was stale.** Added `SKC Bakery Supplies/BranchInventoryReport.cs` to the list in [CLAUDE.md](CLAUDE.md), which also hardcodes the four-branch array and was previously missing from it.
- **SKC POS's "Batch Uploader" and "Upload Sales" screens looked like they updated the live system but don't.** Both write through the legacy, disconnected `SKC.DataEngine.DatabaseManager` SQLite store. Reworded their success dialogs ([Upload Sales.cs](repos/SKC%20POS/Upload%20Sales.cs), [Batch Uploader.cs](repos/SKC%20POS/Batch%20Uploader.cs)) to say plainly that the import is a local-only legacy log and does not sync to `skc-api`, pointing to the real tools (SKC Admin / SKC Bakery Supplies) instead. Did not wire these screens to the live API — that would be a real feature, not a bug fix, and wasn't asked for.

## From bug-track.md — under "Open"

- **The 2026-07-20 N2 money-formatting pass covered `SKC Bakery Supplies` only — `SKC Branch` and `SKC Admin` got none of it.** Commit `b180d12` added `DefaultCellStyle.Format = "N2"` to ten office-app grids; the other two apps had **zero** occurrences of it. This is the documented no-shared-code hazard (see CLAUDE.md: a fix to one app's copy "must be ported by hand"), caught the same day by grepping `Format = "N2"` per project. Severity varies by what the grid is bound to, so port by value rather than blanket-applying:
  - **Real, visible today: `SKC Branch/frmProductionHistory.cs:32`** binds the DTO list directly and shows `TotalInputCost`, which is `production_batches.total_input_cost NUMERIC(18,4)` *and* division-derived from FIFO consumption — so it genuinely renders a long decimal tail. **Still open — this is the one worth fixing first.**
  - **Cosmetic only:** these bind values already 2 dp on the wire (`pos_sales.total_amount`, `inventory.price`, `LineTotal` = price × qty, all `NUMERIC(18,2)` with no division), so they look correct without a format. **The SKC Branch POS screens were done 2026-07-21** as a side-effect of the POS sweep (branch `frmPos` cart, `frmPosDayLog`, `frmBranchSalesHistory` now carry N2, matching the office copies). **Still unformatted:** `SKC Branch/frmSalesReport` and all of `SKC Admin` — cosmetic, low priority.
  - Anything bound to `unit_cost`, `consumed_cost`, `total_line_cost` or `total_input_cost` (all `NUMERIC(18,4)`) is the long-tail risk; `pack_multiplier` is also 18,4 but is deliberately left unformatted — it's a unit conversion factor, not money, and its precision is meaningful (owner deferred a separate decision on it 2026-07-20).

- **`POST /api/production` silently replaces an explicit `OutputQty = 0` with the recipe's default yield — a genuinely failed (burnt) batch cannot be recorded as zero-output.** Found 2026-07-21 by `skc-code-tester` reviewing the webapp's Phase 5 production entry. `Program.cs` treats `OutputQty <= 0` as "use recipe default × multiplier" (the sentinel and the legitimate value share the encoding), so a baker recording an actual yield of 0 gets a full successful batch recorded instead, with no warning. **WinForms `frmProduction` has the same silent discard** — its zero-output confirm fires, the user says yes, and the server then substitutes the default anyway (the confirm only behaves "correctly" when the *default itself* rounds to 0). The **webapp blocks it honestly** as of Phase 5: an explicitly typed 0 with a nonzero default is refused with a message explaining the server behaviour and pointing at a stock adjustment as the workaround. Proper fix is server-side: a nullable `OutputQty` (null = use default, 0 = record zero) — a wire-compatible change since absent JSON binds to null; the WinForms client would then need `-1`-vs-`0` disambiguation or the same nullable move.

## From bug-track.md — under "Decided (convention, not a bug)"

- **Re-apply grid column config after *every* `DataSource` assignment, not just the initial load.** Rebinding a `DataGridView` regenerates its auto-generated columns, so hidden columns reappear and any `DefaultCellStyle.Format` is dropped. `ViewProducts.cs` had this latent bug — columns were configured in `_Load` but the grid is rebound on every search — and it was fixed in `b180d12` by extracting `ApplyGridDisplay()` and calling it after each bind. **Checked the rest of the codebase the same day: no other form has it.** Every other multi-rebind form (`PurchasesReport.cs`, `BranchSalesReport.cs`, `frmPosSalesHistory.cs`, `frmBranchSalesHistory.cs`, `frmProduction.cs`) already re-applies its column config inline after each assignment. Recorded here so new forms follow the pattern, not as an open defect.

## From bug-track.md — under "Decided (convention, not a bug)"

- **A branch-IP-gate rejection ("not authorized to submit sales for this branch") was treated identically to a permanent bad-data rejection by the POS sync engines — MITIGATED CLIENT-SIDE 2026-07-21 (code, not yet shipped).** Introduced by the 2026-07-19 branch IP-gating rollout (see below). `PosSyncEngine`/`PosLocalStore` (both POS copies) marked any rejected sale as terminal — removed from the pending queue, never auto-retried — on the reasoning that a rejected sale "would fail identically on every retry." True for bad data (unknown SKU, negative qty), but **not** for an IP-gate rejection: if a Yoho device's Tailscale IP ever drifted from `branchIps` (PC swap, reinstall, re-auth), every sale in that window was silently, permanently dropped server-side. **Fix applied 2026-07-21 exactly as the original last line prescribed:** the sync engine now distinguishes a gate rejection (per-sale `Status=Rejected` with detail matching "not authorized to submit sales") from all other Rejected reasons, keeps it in `pending_sales` for the next retry instead of moving it to the terminal `sale_log`, and surfaces it as a `SYNC ERROR` banner + once-per-error modal rather than a silent drop. When the device is re-authorized the queued sales sync normally (they were never committed server-side, so no double-count). Note the matching is on the server's exact per-sale gate string (`Program.cs:1416`) — if that message is reworded, update the `.Contains("not authorized to submit sales")` check in both `PosSyncEngine.cs` copies. **Builds clean, GUI-untested, ships on the next branch-PC install.** A cleaner server-side signal (a distinct status code instead of a string match) remains a future option (see the new Open item below).

## From bug-track.md — under "2026-07-20 SKC Admin CLI audit — deferred items"

- **L1 — `inventory.brand` is nullable in the schema but `AdminProduct.Brand` is a non-nullable `string`** ([Dtos.cs](repos/SKC%20Admin%20CLI/Dtos.cs)). A NULL brand deserializes to `null` and flows into `ws.Cell(row, 2).Value` in `ProductsWorkbook.WriteTemplate`. Not reachable today (the office app always sets a brand), so this is defense-in-depth. Same latent issue exists in the SKC Admin WinForms app's DTO copy.

## From bug-track.md — under "2026-07-19 branch IP-gating rollout (Yoho)"

- The one remaining finding from review round 1 (IP-gate rejections and the POS sync engine's retry semantics) is logged separately above under "Open" — owner decision was to leave it and document, not change client code.

## From bug-track.md — under "Noticed in passing, not part of the original audit"

- ~~`dotnet build` on the client solution surfaces `NU1903: Package 'SQLitePCLRaw.lib.e_sqlite3' 2.1.11 has a known high severity vulnerability`, pulled in via the legacy `SKC.DataEngine`/`SKC POS` SQLite dependency.~~ **Resolved 2026-07-17** by deleting the legacy `SKC POS` and `SKC.DataEngine` projects outright (owner decision — they were unused; the live POS inside SKC Branch already uses the fixed SQLitePCLRaw 2.1.12). The solution file moved from `SKC POS/SKC POS.slnx` to `repos/SKC.slnx`, and SKC Bakery Supplies' vestigial project references to both were removed. The NU1903 warning no longer appears in the build.
