\# Branch Delivery Acceptance — Phase 1 (Status-Only)



\## Context



Per the SKC Digital First System doc, deliveries from SKC Bakery Supplies should count toward branch inventory once received. Today, submitting a delivery in the central app immediately FIFO-deducts central inventory and is final — there is no branch-side confirmation. This phase introduces the smallest useful step toward the full flow: a delivery becomes \*\*InTransit\*\* on submit, and a new branch-side WinForms program ("SKC Branch") lists pending deliveries for its branch and \*\*accepts\*\* them, recording who and when.



Scope decisions (confirmed with user):

\- \*\*Status-only\*\*: accepting flips status + records `accepted\_by`/`accepted\_at`. No per-branch inventory ledger yet (later phase).

\- \*\*New separate WinForms project\*\* for the branch app.

\- \*\*Branch identity\*\*: picked once on first launch, saved to a local config file.

\- Server code lives at `C:\\Users\\jpsm0\\source\\skc-api` (ASP.NET Core minimal API, net8.0, Dapper + Npgsql, Postgres 15 on the droplet via docker-compose).



Key structural fact: `POST /api/deliveries` inserts \*\*one `delivery\_logs` row per FIFO lot chunk\*\*, all sharing one `transaction\_id`. Status lives on each row but is applied/read at \*\*ticket grain\*\* (all rows of a transaction\_id move together).



\---



\## Part A — Server (`C:\\Users\\jpsm0\\source\\skc-api`)



\### A1. New `migrations/002\_delivery\_status.sql` (idempotent, matches 001's style)



```sql

\-- Backfill inside the column-add guard: rows existing at migration time predate

\-- the workflow and become 'Accepted'; new rows default to 'InTransit'. Re-runs

\-- can't flip genuine InTransit rows because the UPDATE only runs on first add.

DO $$

BEGIN

&#x20;   IF NOT EXISTS (SELECT 1 FROM information\_schema.columns

&#x20;                  WHERE table\_name = 'delivery\_logs' AND column\_name = 'status') THEN

&#x20;       ALTER TABLE delivery\_logs ADD COLUMN status VARCHAR(20) NOT NULL DEFAULT 'InTransit';

&#x20;       UPDATE delivery\_logs SET status = 'Accepted';

&#x20;   END IF;

END $$;



ALTER TABLE delivery\_logs ADD COLUMN IF NOT EXISTS accepted\_by VARCHAR(100);

ALTER TABLE delivery\_logs ADD COLUMN IF NOT EXISTS accepted\_at TIMESTAMP WITHOUT TIME ZONE;



DO $$

BEGIN

&#x20;   IF NOT EXISTS (SELECT 1 FROM pg\_constraint WHERE conname = 'chk\_delivery\_status') THEN

&#x20;       ALTER TABLE delivery\_logs

&#x20;           ADD CONSTRAINT chk\_delivery\_status CHECK (status IN ('InTransit', 'Accepted'));

&#x20;   END IF;

END $$;



CREATE INDEX IF NOT EXISTS idx\_deliveries\_pending ON delivery\_logs(to\_branch, status);

```



Historical rows keep `accepted\_by`/`accepted\_at` NULL (unknown).



\### A2. Update `init\_schema.sql`

Add the same three columns + CHECK constraint to the `delivery\_logs` CREATE TABLE (lines 42–58) and `idx\_deliveries\_pending` next to the existing indexes, so fresh installs match.



\### A3. `Program.cs` changes



1\. \*\*New `GET /api/deliveries/pending?branch=X`\*\* (after `/api/deliveries/daily`, \~line 231): ticket-grain summary — `GROUP BY transaction\_id, date, to\_branch` with `SUM(qty)`, `SUM(total\_line\_cost)`, `MIN(status) AS Status`, filtered `WHERE to\_branch = @branch AND status = 'InTransit'`, ordered by date.



2\. \*\*New `POST /api/deliveries/{transactionId}/accept`\*\* with body `AcceptDeliveryDto { Branch, AcceptedBy }`:

&#x20;  - 400 if `AcceptedBy` blank.

&#x20;  - Transaction + `SELECT to\_branch, status ... FOR UPDATE` on the ticket's rows (serializes double-accepts).

&#x20;  - 404 if no rows; 400 if `to\_branch != dto.Branch`; \*\*409\*\* if any row already `Accepted`.

&#x20;  - Else `UPDATE delivery\_logs SET status='Accepted', accepted\_by=@AcceptedBy, accepted\_at=CURRENT\_TIMESTAMP WHERE transaction\_id=@transactionId`, commit, 200.



3\. \*\*Guard `DELETE /api/deliveries/{id}`\*\* (\~line 203, after the advisory lock): if any row of the ticket is `Accepted`, throw `"Cannot delete: this ticket has already been accepted by the branch."` (reuses existing rollback + `Results.Problem` flow).



4\. \*\*`GET /api/deliveries/tickets`\*\* (lines 183–189): add `MIN(status) AS Status` to the SELECT so the central app sees ticket status.



5\. \*\*DTOs\*\* (bottom of file): add `Status` to server-side `DeliveryTicketSummary` (\~line 494); new `AcceptDeliveryDto`.



`POST /api/deliveries` itself needs \*\*no change\*\* — the column default marks new rows InTransit.



\## Part B — Central app (`C:\\Users\\jpsm0\\source\\repos\\SKC Bakery Supplies`)



\- \[DeliveryLog.cs:28](SKC Bakery Supplies/DeliveryLog.cs): add `public string Status { get; set; }` to `DeliveryTicketSummary`. The ViewDeliveries grid auto-generates columns and `GetDeliveryTicketsAsync` deserializes case-insensitively, so the Status column appears with no other changes.

\- \[ViewDeliveries.cs](SKC Bakery Supplies/ViewDeliveries.cs): ensure the delete handler surfaces the server's "already accepted" rejection in a clean MessageBox (wrap in try/catch if not already).

\- No changes to `Delivery.cs` — submit flow and printed slip unchanged (the slip's "Received By" signature line remains the paper trail).



\## Part C — New project `C:\\Users\\jpsm0\\source\\repos\\SKC Branch\\`



No NuGet packages, no project references. Follow the main app's patterns throughout.



1\. \*\*`SKC Branch.csproj`\*\* — clone of the main app's csproj minus PDFsharp: WinExe, `net10.0-windows`, `RootNamespace` `SKC\_Branch`, UseWindowsForms, Nullable, ImplicitUsings.

2\. \*\*Register in solution\*\*: add `<Project Path="../SKC Branch/SKC Branch.csproj" />` to `C:\\Users\\jpsm0\\source\\repos\\SKC POS\\SKC POS.slnx`.

3\. \*\*`BranchConfig.cs`\*\* — persists `{ "BranchName": "Yoho" }` at `%APPDATA%\\SKC Branch\\config.json`; static `Load()` (null if missing/corrupt) and `Save()`.

4\. \*\*`BranchApiClient.cs`\*\* — copy the `CentralApiClient.cs` shape (static class, static HttpClient, 30s timeout, case-insensitive JSON, `ApiBaseUrl = "http://100.84.79.35:7290"` with commented localhost line). Methods: `GetAllProductsAsync` (for SKU→name display), `GetPendingDeliveriesAsync(branch)`, `GetDeliveryDetailsAsync(txnId)`, `AcceptDeliveryAsync(txnId, branch, acceptedBy)` (distinct message on 409), `CheckHealthAsync`.

5\. \*\*`Dtos.cs`\*\* — local copies: product (SKU/Brand/BaseName), `DeliveryTicketSummary` (with Status), `DeliveryLog`.

6\. \*\*`Program.cs`\*\* — load config; if null, show `frmBranchPicker` modally (cancel = exit) and save; then `Application.Run(new frmMain(branchName))`.

7\. \*\*`frmBranchPicker`\*\* — DropDownList combo with `{ "Yoho", "Gaisano", "Liloy", "Labason" }` — \*\*must match \[Delivery.cs:45](SKC Bakery Supplies/Delivery.cs:45) strings exactly\*\* (server comparison is case-sensitive). OK disabled until a branch is chosen.

8\. \*\*`frmMain`\*\* — title shows branch name; top grid = pending tickets (refresh button), bottom grid = lines of selected ticket with product names resolved from the cached catalog (same `FirstOrDefault` pattern as ViewDeliveries.cs); \*\*Accept\*\* button → small name-prompt dialog (who is accepting) → confirm → `AcceptDeliveryAsync` → refresh. On 409, show "already accepted" and refresh.



\## Order of work



1\. Migration 002 + `init\_schema.sql`

2\. `Program.cs` endpoints/DTOs → build skc-api

3\. Local API smoke test (curl)

4\. Central app `Status` property → build

5\. SKC Branch project + slnx registration → build

6\. End-to-end walkthrough

7\. Deploy



\## Verification



1\. In skc-api: `docker compose up -d db`, apply `init\_schema.sql` then migration 002 via psql — \*\*run 002 twice\*\* to prove idempotency; pre-existing rows must read `Accepted`.

2\. `dotnet run` in skc-api. Point both apps' `ApiBaseUrl` at \*\*`http://localhost:53756`\*\* (note: the existing commented line says 53755, but that's the \*\*https\*\* port — http is 53756).

3\. Central app: submit a delivery to Yoho → slip prints as before; ViewDeliveries shows `InTransit`.

4\. curl checks: `pending?branch=Yoho` → 1 ticket; `branch=Gaisano` → empty; accept with wrong branch → 400; blank AcceptedBy → 400; unknown id → 404.

5\. Branch app: first launch → picker → config.json written; relaunch skips picker; ticket visible with product names; accept → pending list empties; DB rows all `Accepted` with `accepted\_by`/`accepted\_at` set.

6\. Edge cases: re-accept via curl → 409; central delete of an accepted ticket → clean error message; delete of a fresh InTransit ticket still restocks lots; a multi-lot ticket (multiple rows, one transaction\_id) shows once in pending and all rows flip on accept.

7\. Revert both `ApiBaseUrl`s to the droplet address before committing.



\## Deployment



1\. Run migration 002 on the droplet via psql \*\*before\*\* deploying the new API (old API keeps working — its INSERTs don't name the new columns).

2\. `docker compose build api \&\& docker compose up -d api`.

3\. Publish the branch app; install on one Tailscale-connected PC per branch; first-launch picker sets identity.

4\. Note: deliveries submitted between migration and branch-app install accumulate as InTransit — branches accept the backlog on day one.



\## Risks / notes



\- \*\*Ticket-grain consistency\*\*: mitigated by all-rows UPDATE under `FOR UPDATE`, `MIN(status)` aggregation, and the CHECK constraint. New rows can't join old transaction\_ids (client mints fresh `DEL-` ids).

\- \*\*Concurrent accept/delete\*\*: both lock; loser gets 409/404 respectively.

\- \*\*Migration lock\*\*: `ADD COLUMN` takes a brief ACCESS EXCLUSIVE lock — run in a quiet window.

\- \*\*Branch strings\*\* now live in three places (Delivery.cs, frmBranchPicker, DB rows); exact-match required. A `branches` table is a phase-2 cleanup.

\- \*\*No auth\*\*: `Branch` and `AcceptedBy` are client-asserted, same trust model as the existing `Requester`; Tailscale is the perimeter. QR staff identity comes later per the Notion doc.

---

# Branch Inventory Credit — Phase 2

## Context

Phase 1 gave branches a way to digitally accept a delivery, but acceptance was status-only — it flipped `delivery_logs.status` to `Accepted` and nothing else. There was still no branch-side stock; all inventory lived in one central FIFO ledger (`inventory_lots`, always `branch_name = 'Office'`). This phase closes that gap: **accepting a delivery credits real stock to the receiving branch**, the prerequisite for a later baking module to deduct against actual branch inventory.

`inventory_lots` was already designed with a `branch_name` column and a `UNIQUE (branch_name, lot_id)` constraint plus an `idx_lots_branch_sku(branch_name, sku)` index — it had just never been used for anything but `'Office'`. **No schema migration was required** for this phase; it was purely new logic reusing existing structure.

## Part A — Server (`skc-api`)

- `POST /api/deliveries/{transactionId}/accept`: widened the `FOR UPDATE` locking SELECT to also pull `sku, qty, total_line_cost`, then — after the existing validation and before the final status UPDATE — credits the receiving branch: acquires a per-branch advisory lock (`hashtext('inventory-write:' || @Branch)`, parameterized), computes `nextLotId` scoped to that branch, and inserts one `inventory_lots` row per delivery_logs line (`branch_name = dto.Branch`, `unit_cost = total_line_cost / qty`, `purchase_transaction_id = NULL`). All inside the same transaction as the status flip, so a failure rolls back both.
- `GET /api/inventory`: added `AND l.branch_name = 'Office'` to the `CurrentStock` subquery — previously unscoped, which would have silently blended every branch's stock into the Office figure once branch lots existed.
- New `GET /api/inventory/branch/{branch}`: same shape as `/api/inventory`, scoped to the given branch's lots.

## Part B — Central app (`SKC Bakery Supplies`)

- `CentralApiClient.GetBranchInventoryAsync(branch)` → `List<BakeryProduct>`, GET `/api/inventory/branch/{branch}`.
- New `BranchInventoryReport.cs`/`.Designer.cs` (`frmBranchInventoryReport`): branch dropdown + grid (live snapshot, not a ticket log), plus a print button following `ViewProducts.cs`'s count-sheet pattern.
- `Home.cs`: launch button added **in code** (not the Designer) to avoid hand-editing `Home.Designer.cs`, per the workspace's `.claudesettings.json` convention.

## Part C — SKC Branch app

- `Dtos.cs`: new `BranchStockItem { SKU, Brand, BaseName, CurrentStock }`.
- `BranchApiClient.GetMyStockAsync(branch)` → `List<BranchStockItem>`, GET `/api/inventory/branch/{branch}`.
- New `frmBranchStock.cs`/`.Designer.cs`: a small separate form (not a `TabControl` inside `frmMain`) showing the branch's own stock.
- `frmMain.cs`: "My Stock" button added in code (same Designer-avoidance convention as above).

## Verification (performed against live droplet data)

Purchased test stock, delivered a single-lot ticket to Yoho and a FIFO-split ticket (2 origin lots, different unit costs) to Gaisano, accepted both, and confirmed: branch lots created with correct `branch_name`/`sku`/`qty`/`unit_cost`; `/api/inventory/branch/{branch}` sums correctly across multiple lots of the same SKU; `/api/inventory` (Office) unaffected; re-accept still 409s with no double-credit; delete-after-accept still blocked; wrong-branch/unknown-ticket/blank-name still rejected (400/404/400). Both client apps build cleanly; the new WinForms screens themselves were not click-tested (no GUI session available in the session that implemented this).

Shipped as `skc-api@18778d1` and `repos@291436f`.

---

# Post-Phase-2: Bug fix, branch adjustments, delivery amend, versioning

## Context

Phase 2's per-branch lots exposed a latent data-integrity bug and surfaced three feature asks. Split into two deploys: the bug fix shipped and was verified alone first, then the features shipped together.

## Deploy 1 — Lot-scoping bug fix (`skc-api@e775ca5`)

`lot_id` is only unique per branch (`UNIQUE (branch_name, lot_id)`), and Phase 2 started each branch's lot sequence at 1 — so Office/Yoho/Gaisano all have a lot 1. The delivery FIFO consumption and the adjust endpoint filtered/decremented lots by `sku`/`lot_id` alone, so an Office write could read and decrement another branch's same-numbered lots. Since inventory only ever comes from Office, scoped every affected read/write to `branch_name = 'Office'`. Verified against prod: over-delivery fails cleanly without draining branch stock; normal delivery/accept leaves other branches untouched.

## Deploy 2 — Features

**B. Branch adjustments.** Generalized `POST /api/inventory/{sku}/adjust` with an optional `Branch` (default `Office`), branch-scoping every lot read/write, the advisory lock, and the `inventory_adjustments` insert; `GET /api/inventory/adjustments` gains an optional branch filter and returns `branch_name`. Central app: **Adjust Stock** button on the Branch Inventory Report opens the (now branch-aware) `frmAdjustInventory`. Access boundary is UI-only (the SKC Branch exe has no such button) until QR auth exists.

**C. Versioning + publish.** `<Version>` 2.1.0 in both csprojs, shown in each app's title bar (from `Assembly.GetName().Version`). Authored a single-file self-contained publish profile for SKC Branch (Office already had one); tracked the previously-untracked `SKC Branch/frmMain.resx`.

**D. Delivery amend.** No true in-place edit — instead, **Edit** on View Deliveries (enabled only for InTransit tickets) deletes the ticket (restocking Office) and reopens Create Delivery pre-filled from its lines (FIFO-split rows collapsed back to one draft line per SKU), so the admin adjusts and re-submits without retyping. Submitting mints a fresh ticket id, so a branch holding a stale screen gets a 404 on Accept — the SKC Branch app maps that to a distinct "changed by the office, please review" prompt + refresh. Edit/Delete disable once accepted; blocked-delete now shows the plain server message instead of raw ProblemDetails JSON.

## Verification

Against the live droplet: branch adjust (raise + FIFO shrink) scoped correctly, other branches unaffected; adjustment history filters by branch with no leakage; stale-accept 404 confirmed; SKC Branch publishes to a 111 MB single-file exe stamped 2.1.0. The amend/adjust WinForms dialogs were not click-tested (no GUI session); the API contracts they rely on were verified directly.

Shipped as `skc-api@e775ca5` + `skc-api@4287e9f` and `repos@211a5e7`.

---

# IP allowlist hardening

## Context

Every write endpoint was open to anyone on the Tailscale network — there's no auth anywhere in the system. The user's laptop and the SKC Bakery Supplies office PC both have stable Tailscale addresses (`100.108.218.24` and `100.66.61.24`), so a lightweight server-side IP check closes the "anyone on the tailnet can call the API as Office" gap immediately, without building a real auth system.

## What shipped (`skc-api@16dd8b5`)

A `HashSet<string>` of the two trusted IPs + an `IsTrustedOfficeCaller(HttpContext)` helper (handles the IPv4-mapped-IPv6 case). Applied to every office-only write: `POST/DELETE /api/purchases`, `POST/DELETE /api/deliveries`, `POST/PUT/PATCH /api/inventory`, `POST /api/inventory/{sku}/adjust`. Rejected calls get a 403 ProblemDetails response. Deliberately **not** applied to any `GET` or to `POST /api/deliveries/{id}/accept` (branch-initiated, and branch PCs aren't on Tailscale yet) — those get bound later via a parallel `branch_name → ip` map once branch IPs are known.

## Verification

From the droplet's own loopback (untrusted): writes 403, reads and `/accept` unaffected. From the owner's laptop (trusted): writes succeed. Committed and pushed alone as `skc-api@16dd8b5`.

---

# Baking/Decorating Foundation

## Context

Two production routes needed modeling: **Baking** (raw materials → finished baked goods and pre-decoration goods like chiffon, one batch yields multiple units) and **Decorating** (1–3 pre-decoration goods + raw materials → a finished sellable good, e.g. black forest cake). Every branch has its own baker/decorator working on that branch's own stock, and the owner wants to track who baked/decorated what. Recipes are one shared company-wide list, editable only by the owner. Purchases arrive by sack/bulk but inventory tracks in grams.

Branches also turned out to ship finished (non-decorated) baked goods to each other sometimes (e.g. Yoho → Liloy) — decorated goods never ship (they don't travel well). That's a bigger change to the existing Office-only delivery endpoint, so it was scoped out of this pass and recorded as a gap (see spec-status.md) rather than implemented blind.

## What shipped

**Server (`skc-api@de47b80`, migration `003_production_module.sql`)**
- `inventory.category` (`RawMaterial`/`BakedGood`/`DecoratedGood`, default `RawMaterial`). Wired up the schema's dormant `uom`/`pack_multiplier` columns via `PUT /api/inventory/{sku}/classification`.
- Global `recipes` + `recipe_lines` (owner-gated CRUD via a new `IsOwnerCaller` helper matching only `100.108.218.24`; reads open for branches).
- Branch-scoped `production_batches` + `production_consumed`. `POST /api/production` FIFO-consumes a recipe's inputs from the branch's own lots (same per-branch advisory-lock pattern as every other write) and credits one output lot with rolled-up cost. Baking and decorating share this one endpoint — a decorating recipe just has a `BakedGood` among its inputs. Not IP-gated yet (like `/accept`).
- Found and fixed a Postgres bug during verification: nullable `DateTime?` query params in an `(@start IS NULL OR ...)` clause confuse Postgres's type inference (unlike a nullable `string?` used the same way elsewhere) — needed an explicit `::timestamp` cast on *both* sides of the `IS NULL OR` check, not just the comparison side.

**New app: SKC Admin (`repos@e8c0c4e`)**
- Owner-only WinForms app (no shared references), enforced server-side not client-side. Products tab (category/UoM) + Recipes tab (CRUD with a line editor).

**SKC Branch (`repos@461d455`, v2.2.0)**
- "Bake / Decorate" screen: recipe picker (filtered by kind), batch multiplier, editable actual output qty, staff name, a consumption preview, submit. A history dialog lists past batches.

**SKC Bakery Supplies (`repos@7d284ce`, v2.2.0)**
- Purchases screen: "Buy by pack" toggle appears when a product has a pack conversion set; converts pack-count/cost-per-pack to base units before adding the line.

## Verification

Live against the droplet: recipe/classification writes 403 from an untrusted IP, succeed from the owner's laptop; a 3× baking batch correctly consumed 3 units of a raw material and credited 6 units of a `BakedGood`; a decorating batch consuming that `BakedGood` + a raw material produced a `DecoratedGood` correctly; other branches' stock untouched; production history endpoint verified after the DateTime-cast fix. Client apps build cleanly; SKC Admin's forms, SKC Branch's production screen, and the Purchases toggle were **not click-tested in a GUI session** — the API contracts they depend on were verified directly.

## Deferred (see spec-status.md Gaps for detail)

Branch→branch shipping of baked goods (needs a `from_branch` delivery generalization + branch Tailscale + a branch create-delivery UI), production endpoint IP-binding, report/dashboard views, recipe versioning.

Shipped as `skc-api@de47b80`, `repos@e8c0c4e`, `repos@461d455`, `repos@7d284ce`.

---

# POS — Offline-First Sales in SKC Branch

## Context

Flow step 4's second half ("adds for-sale items for POS") and the biggest remaining step-2 gap (nothing consumes branch stock through normal operations) land together here: a POS screen **inside the SKC Branch app** (user decision — branches run one app for deliveries, production, and sales) that records counter sales offline-first and syncs them to skc-api, where they FIFO-deduct the branch's own `inventory_lots`.

The repo's existing "SKC POS" project is **not** a POS — it's the legacy Aronium-CSV uploader (local SQLite log only). It stays untouched; once this phase is live at branches, it and the Aronium export ritual retire naturally.

Decisions confirmed with user:
- **Lives in SKC Branch** — one app per branch PC. Only the POS portion is offline-first; deliveries/production remain online-only.
- **Offline-first, cloud-sync**: sales are written to a local SQLite queue and synced when online. A sale must never be blocked by connectivity.
- **Sellable = `inventory.price > 0`.** The dormant `price NUMERIC(18,2)` column (already in the live schema and already returned by both inventory GETs) becomes the single company-wide selling price. Category can't determine sellability (candles are `RawMaterial` by taxonomy but sellable; chiffon is a `BakedGood` but an unsellable intermediary) — the owner simply prices what's sellable and leaves raw materials/intermediaries at 0. No new flag, no migration for pricing.
- **One central price list**, owner-managed in SKC Admin, synced down to the POS cache; offline sales use the last-synced price.
- **Oversell: warn but allow.** If cached stock < requested qty, the cashier gets a loud warning ("record your baking/decorating") but the sale completes; the server records the uncovered portion as a shortfall at sync time. Items with no price set are simply *not findable* — that's the hard "no such item" forcing function; stock-level enforcement stays soft because production recording requires connectivity and sales must not stop.
- **Discount** = a generic negative line on the sale (matches their Aronium habit), no inventory effect.
- **No receipts, no barcode scanners.** Type-ahead name search, same pattern as the office app's Delivery screen.
- **Staff identity**: free-text name (retained for the session), same trust model as `AcceptedBy` — QR login (step 5) slots in later.
- **Office app gets a Branch Sales Report screen** in this same phase.
- **Testing**: the owner's laptop (`100.108.218.24`) stands in as the **Yoho** branch (real branch Tailscale onboarding deferred — store visits required).

## Part A — Server (`skc-api`)

### A1. Migration `migrations/004_pos_sales.sql` (idempotent) + same tables in `init_schema.sql`

```sql
CREATE TABLE IF NOT EXISTS pos_sales (
    id SERIAL PRIMARY KEY,
    branch_name VARCHAR(100) NOT NULL,
    local_id INTEGER NOT NULL,                 -- server-assigned MAX+1 under the branch advisory lock (human-friendly display id)
    client_sale_id VARCHAR(100) NOT NULL,      -- minted offline by the POS (GUID) — the idempotency key
    staff_name VARCHAR(100) NOT NULL,
    sold_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,  -- counter time, not sync time (offline sales sync late)
    total_amount NUMERIC(18, 2) NOT NULL,
    created_at TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT uq_branch_pos_sale UNIQUE (branch_name, client_sale_id)
);
CREATE INDEX IF NOT EXISTS idx_pos_sales_branch_date ON pos_sales(branch_name, sold_at);

CREATE TABLE IF NOT EXISTS pos_sale_lines (
    id SERIAL PRIMARY KEY,
    branch_name VARCHAR(100) NOT NULL,
    client_sale_id VARCHAR(100) NOT NULL,
    sku VARCHAR(100) REFERENCES inventory(sku) ON UPDATE CASCADE,  -- NULL for discount lines
    description VARCHAR(255) NOT NULL,         -- display-name snapshot at sale time / 'Discount'
    qty INTEGER NOT NULL,                      -- 1 for discount lines
    unit_price NUMERIC(18, 2) NOT NULL,        -- negative for discount lines
    line_total NUMERIC(18, 2) NOT NULL,
    shortfall_qty INTEGER NOT NULL DEFAULT 0,  -- portion FIFO couldn't cover at sync time (oversell audit)
    consumed_cost NUMERIC(18, 4) NOT NULL DEFAULT 0  -- rolled-up FIFO cost actually consumed (margin reporting later)
);
CREATE INDEX IF NOT EXISTS idx_pos_sale_lines_sale ON pos_sale_lines(branch_name, client_sale_id);
```

Why `client_sale_id` (GUID) instead of the pure `(branch_name, local_id)` idempotency pattern of `purchase_logs`/`delivery_logs`: a wiped/reinstalled local db would restart a local autoincrement at 1 and collide with already-synced rows, silently dropping new sales as "already synced." A GUID survives reinstall. `local_id` is still assigned **server-side** (MAX+1 under the advisory lock, the established pattern) purely as a readable id for reports.

### A2. `PUT /api/inventory/{sku}/price` — owner-gated (`IsOwnerCaller`, like recipes)

Body `{ Price }`; reject negative. 404 unknown SKU. Sets `inventory.price` + `last_updated`. (The office app's Add Item continues to set price at creation — office-gated; the owner's Admin console is the authoritative editor thereafter.)

### A3. `POST /api/sales` — the sync endpoint. **Not IP-gated yet** (same posture as `/accept` and `/production`; joins the future `branch_name → ip` map, which for now would read `Yoho → 100.108.218.24`)

Body: `List<PosSaleDto>` (`ClientSaleId`, `Branch`, `StaffName`, `SoldAt`, `TotalAmount`, `Lines[]`). Per sale, in its own transaction (one bad sale must not block the rest of the batch):
1. `pg_advisory_xact_lock(hashtext('inventory-write:' || @Branch))` — same lock as accept/production/adjust, so all branch stock writes serialize.
2. If `(branch_name, client_sale_id)` already exists → report **AlreadySynced**, skip (this is the retry path, not an error).
3. Validate: non-discount lines must reference an active SKU with `qty > 0`; sale `total_amount >= 0` (bounds the negative discount line); reject the individual sale with a per-sale error status otherwise.
4. Assign `local_id` = branch-scoped MAX+1.
5. For each non-discount line: FIFO-consume from the branch's lots (`FOR UPDATE`, oldest first, **typed** `QueryAsync<T>` — aliased-column gotcha). If lots run dry, consume what exists and record the rest as `shortfall_qty` — **a sale is never rejected for stock**. Discount lines skip inventory entirely.
6. Insert header + lines, commit.

Response: per-sale results `[{ ClientSaleId, Status: "Synced" | "AlreadySynced" | "SyncedWithShortfall" | "Rejected", Detail }]` so the client can drain its queue item-by-item.

### A4. `GET /api/sales?branch=&start=&end=` + `GET /api/sales/{branch}/{clientSaleId}`

Summary list (LocalId, ClientSaleId, StaffName, SoldAt, TotalAmount, HasShortfall) and line detail for one sale. Make `start`/`end` **required** `DateTime` (like `/api/purchases/tickets`) to sidestep the nullable-`DateTime?` `::timestamp` inference bug documented from the production-history work.

## Part B — SKC Admin

- Products tab: show Price + derived "Sellable" (price > 0); a Set Price action calling `AdminApiClient.SetPriceAsync(sku, price)` → `PUT /api/inventory/{sku}/price`.

## Part C — SKC Branch (bump to 2.3.0)

New packages in `SKC Branch.csproj`: `Microsoft.Data.Sqlite` + `Dapper` (same versions as SKC.DataEngine: 10.0.8 / 2.1.79). This is a **deliberate deviation** from the no-packages convention — offline-first genuinely requires a local store; still no project references (the local-store class is written fresh, not shared from SKC.DataEngine).

1. **`PosLocalStore.cs`** — SQLite at `%APPDATA%\SKC Branch\pos.db`:
   - `catalog_cache` (sku PK, brand, base_name, price, category, stock, last_synced) — only rows with price > 0 are surfaced in the POS search.
   - `pending_sales` + `pending_sale_lines` — the outbound queue (client_sale_id GUID PK).
   - `sale_log` — local copy of synced sales for the day view.
   - Cached `stock` is decremented locally on each completed sale (so intra-day offline sales inform the oversell warning) and overwritten by the authoritative pull on every sync.
2. **`PosSyncEngine.cs`** — a WinForms `Timer` (60s) + manual "Sync now": (a) push `pending_sales` via `BranchApiClient.PushSalesAsync`; remove on Synced/AlreadySynced, surface SyncedWithShortfall as a non-modal notice, park Rejected sales with their reason for manual review; (b) pull `GET /api/inventory/branch/{branch}` into `catalog_cache`. Tracks online/offline state for the status strip.
3. **`frmPos.cs`** — the sale screen: staff-name box (retained for the session); type-ahead search + floating ListBox (copy `txtProductSearch_TextChanged` from the office Delivery.cs) over sellable cached items showing name/price/stock; cart grid; qty entry; **Discount** button → prompts an amount, adds a negative line; running total; cash-tendered → change display (no printing); **Complete Sale** → validate total ≥ 0 → write to local queue, decrement cached stock, kick an immediate sync attempt. Oversell path: warning dialog ("Stock shows only N — record your baking/decorating; this sale will be flagged") with proceed/cancel. Status strip: Online/Offline, pending-sale count, last sync time.
4. **`frmPosDayLog.cs`** — today's sales (pending + synced, flagged distinctly), running total. The poor man's Z-report.
5. **`frmMain.cs`** — "POS" button added **in code** (Designer-avoidance convention). Soften the two startup modals: catalog/pending load failures become status text instead of blocking MessageBoxes, so an offline launch lands in a usable app whose POS works from cache.
6. **`BranchApiClient.cs`** — `PushSalesAsync(List<PosSaleDto>)`; extend the branch-inventory DTO with `Price`/`Category` (endpoint already returns them).
7. **`Dtos.cs`** — `PosSaleDto`, `PosSaleLineDto`, `SaleSyncResultDto`.

First run requires connectivity once (initial catalog pull); after that the POS is fully usable offline.

## Part D — Central app (SKC Bakery Supplies, bump to 2.3.0)

- `CentralApiClient.GetSalesAsync(branch, start, end)` + `GetSaleLinesAsync(...)`.
- New `frmBranchSalesReport` (same shape as Branch Inventory Report): branch dropdown + date range → summary grid; selecting a row shows its lines; shortfall-flagged sales visually marked. Home button added in code.

## Order of work

1. Migration 004 + `init_schema.sql`
2. skc-api: price endpoint, `POST /api/sales`, `GET /api/sales*` → build
3. Local smoke test (docker db + `dotnet run`, ApiBaseUrl → `http://localhost:53756`)
4. SKC Admin price UI → build
5. SKC Branch: local store, sync engine, POS screens, frmMain button → build
6. Central app sales report → build
7. End-to-end vs local API **including offline simulation** (stop the API mid-session, keep selling, restart, watch the queue drain)
8. Deploy: migration on droplet **before** API image (old API never touches the new tables); then live verification as Yoho from the laptop

## Verification

1. Run 004 twice against local db — idempotent.
2. Price: set via owner IP succeeds; from droplet loopback → 403; negative price → 400. Price 0 SKU absent from POS search; priced SKU appears.
3. Sync happy path: sale → lots FIFO-decremented oldest-first (verify across a multi-lot SKU); `local_id` increments per branch; day log and office report agree.
4. Idempotency: re-push the same `client_sale_id` → AlreadySynced, **no double-deduction**.
5. Oversell: sell qty > stock → SyncedWithShortfall, lots drained to exactly 0, `shortfall_qty` recorded, office report flags it.
6. Discount: sale with a negative line → inventory untouched for that line; sale with total < 0 → Rejected.
7. Unknown/inactive SKU in a line → that sale Rejected, rest of batch still processes.
8. Offline: kill API → complete several sales → relaunch app (queue survives — SQLite durability) → start API → queue drains in order.
9. Concurrency: sale sync racing a production batch / delivery accept on the same branch serializes on the advisory lock.
10. Live as Yoho: price a test SKU via Admin from the laptop, sell it, verify Yoho lots and the office report against the droplet.
11. GUI screens (POS, day log, Admin price, sales report) are **not click-testable here** — flag plainly; verify their API contracts via curl and read the wiring closely.
12. Revert any localhost `ApiBaseUrl` to `http://100.84.79.35:7290` before commit.

## Deployment

1. `psql` migration 004 on the droplet, then `docker compose build api && docker compose up -d api`.
2. SKC Branch 2.3.0 + SKC Admin exes republished. **Real branches can't use the POS until their PCs join Tailscale** (they can't reach the API at all) — the laptop-as-Yoho install is the pilot; store-visit rollout follows.
3. Owner prices the sellable catalog in SKC Admin before branch rollout (unpriced = invisible to POS).

## Risks / notes

- **Pending-sale loss**: an unsynced local queue dies with the disk/db — accepted (same exposure as the paper it replaces); synced sales are server-safe. GUIDs make a reinstall collision-proof.
- **Stale cache**: offline sales use last-synced price (user-confirmed) and last-known stock minus local sales — the shortfall mechanism absorbs the drift.
- **Clock skew**: `sold_at` is branch-PC time; reports order by it. Acceptable for a single-operator system.
- **No cash-drawer sessions / Z-report** in v1 — day log only; deferred with the other reporting work.
- **Discount is unbounded** below only by total ≥ 0 — no per-line cap; staff-trust model unchanged.
- **POS writes not IP-gated** until the branch map exists (deliberate, consistent with accept/production).
- **`foodpanda_sku`** stays dormant — noted as a possible future POS/delivery-platform tie-in, out of scope.


---

# Branch End-of-Day Sales Report (2026-07-20)

## Context

Staff need to close out the counter at end of day: a printed sheet showing the day's sales, totalled and signed off, so the till can be reconciled and someone can be held accountable at inventory time. Until now the only aid was the POS day log on screen (`frmPosDayLog`), which is not printable and covers today only.

Decisions confirmed with user:
- **This is a management/Z-report, NOT a customer receipt.** The standing "no receipt printing — ever" rule is a **BIR (Bureau of Internal Revenue) compliance constraint**: printing customer receipts from an unregistered POS would expose the business to penalties. Customer receipts stay pen-and-paper; BIR-registered receipting is far future. An internal report carries no such exposure, so it is fine.
- **Date range, defaulting to today**, so any past day can be reprinted.
- **Also export for Excel.** The owner is building an Excel workbook as the analytics companion to the C# apps; per-item summarising happens there via SUMIFS, not in the app.
- **Export must be line-level.** The owner initially assumed the existing `GET /api/sales` could feed a per-item SUMIFS — it can't; it returns sale-level rows with no SKU or qty. Hence the new endpoint below. The *printed* report stays sale-level; only the export goes deeper.
- **CSV, not .xlsx** — opens straight into Excel, no NuGet dependency, consistent with the client apps' minimal-dependency convention.
- **Offline fallback for today only.** Closing time is exactly when a branch may be offline, so a today-only range falls back to the local POS store rather than refusing to print, banner-flagged on screen and on paper. Past ranges have nothing to fall back to (the local store keeps a day log, not history) and say so.

## Part A — Server (`skc-api`)

### A1. `GET /api/sales/lines?branch=&start=&end=` (`skc-api@6cb7dda`)

Flat line-level sales over a date range — one row per item per sale, carrying its parent sale's `local_id`/`sold_at`/`staff_name`/`voided`. A join of `pos_sales` + `pos_sale_lines`, branch-scoped on both sides. Read-only, **no migration**.

Voided sales are returned and flagged rather than filtered, so the export accounts for the same rows the printed report does. No route conflict with `GET /api/sales/{branch}/{clientSaleId}` — that one has three segments, this has two.

## Part B — SKC Branch

New `frmSalesReport` (+ `.Designer.cs`), opened from a **"Sales Report / Print..."** button on the POS right rail. The rail's totals block shifted down 35–40px to make room.

- Date range defaulting to today, plus a **Today** button. Per-sale grid: no., date/time, cashier, total, flag (`VOIDED`/`SHORTFALL`/`REJECTED`/`UNSYNCED`), with voided/rejected greyed and shortfall/unsynced tinted.
- **Print** — `PrintDocument` + preview, same house pattern as the office app's report screens. Renders the per-sale list then a summary block (sales counted, gross total, voided excluded, shortfall/rejected/unsynced counts) and **"Counted by / Verified by"** signature lines.
- **Save for Excel** — CSV via the new endpoint. UTF-8 with BOM (Excel mis-decodes non-ASCII otherwise); invariant `0.00` with no thousands separator (a `1,234.00` would split into two columns); fields quoted only when they contain a comma/quote/newline.
- `BranchApiClient.GetBranchSaleLinesRangeAsync` + `BranchSaleLineExport` DTO.

Three non-obvious properties worth preserving on any edit:
1. **Print and export both work off the range that was actually loaded**, never the live date pickers — otherwise moving a picker without pressing Load prints one range's rows under another range's heading. A "Dates changed — press Load" warning shows when the two diverge.
2. **The print footer reservation is `165 + 20n`**, where `n` counts the optional summary lines actually present. The variable part of the reservation matches the variable part of the content 1:1, so the last signature line lands a constant ~36 above the bottom margin for any flag combination. A flat reservation overflows as soon as a range contains both a voided and a shortfall sale.
3. **CSV export has no offline fallback** — item lines for already-synced sales exist only server-side.

## Verification

Endpoint curl-tested against the live droplet after deploy: real multi-line data (including a discount line with `sku: null` and a negative price, shortfall quantities, and voided sales flagged `true`), branch scoping (`Liloy` → `[]`), clean 400s on missing/garbage params, and the pre-existing 3-segment sales route still resolving (no shadowing).

Both repos build clean; `designer_layout_audit.py` ALL CLEAR for SKC Branch. Reviewed by `skc-code-tester`, which found three real bugs — all fixed, then re-verified: stale printed header, stale offline flag blocking valid exports, and the footer-overflow case in (2) above.

**The forms are not GUI click-tested** — no interactive desktop session in this environment, per this workspace's standard client-verification limits.

## Risks / notes

- **Local fallback is honest but partial**: it can't see sales rung up on another device at the same branch, nor voids performed elsewhere. Banner-flagged on both screen and paper for exactly this reason.
- **Async-UI latency**: `btnPrint`/`btnExportCsv` and the date pickers stay enabled during Load's in-flight `await`, so for up to the 10s HTTP timeout a print could use the previous range's rows. Self-correcting once the load completes; a pre-existing pattern in this codebase, not introduced here.
- **Distribution**: reaching Yoho needs a manual reinstall — no auto-update. Client `<Version>` still `2.4.0` at time of writing; bump before redistributing.
