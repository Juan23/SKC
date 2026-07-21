using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKC_Bakery_Supplies
{
    public class SyncOutcome
    {
        public bool Online { get; set; }
        public int Pushed { get; set; }
        public int Shortfalls { get; set; }
        public int Rejected { get; set; }
        public bool CatalogRefreshed { get; set; }
        public List<string> Warnings { get; } = new();
        // Set when the push reached the server but the whole batch was rejected at the HTTP layer
        // (403 IP-gate, 400, 500). Null on a clean sync or a genuine-offline cycle.
        public string? HttpError { get; set; }
    }

    // Push-then-pull sync for the office POS. Push first so the pulled stock snapshot already
    // reflects the sales we just sent. Everything is idempotent server-side, so a crash
    // between push and ResolvePendingSale just means one harmless AlreadySynced next run.
    public static class PosSyncEngine
    {
        private static bool syncing;

        public static DateTime? LastSyncAt { get; private set; }
        public static bool LastSyncOnline { get; private set; }
        // Non-null while the last push was reached-but-rejected at the HTTP layer (e.g. a 403
        // IP-gate rejection). The status bar shows this instead of OFFLINE. Cleared on any sync
        // where the push gets through (or there was nothing to push and the pull succeeded).
        public static string? LastSyncError { get; private set; }

        public static async Task<SyncOutcome> SyncAsync(string branch)
        {
            var outcome = new SyncOutcome();

            // A 60s timer tick can land while a slow manual sync is still running;
            // overlapping pushes of the same queue would be wasteful (though safe).
            if (syncing) return outcome;
            syncing = true;

            try
            {
                // ---- push ----
                var pending = PosLocalStore.GetPendingSales(branch);
                if (pending.Count > 0)
                {
                    List<SaleSyncResult> results;
                    try
                    {
                        results = await CentralApiClient.PushSalesAsync(pending);
                    }
                    catch (PosSyncHttpException ex)
                    {
                        // Reached the server but the batch was rejected (403 IP-gate, 400, 500).
                        // NOT offline - keep the queue but flag the error so the POS shows SYNC
                        // ERROR instead of retrying silently forever under an OFFLINE badge.
                        LastSyncOnline = false;
                        LastSyncError = ex.ShortMessage;
                        outcome.HttpError = ex.ShortMessage;
                        return outcome;
                    }
                    catch
                    {
                        // Offline (or server unreachable): queue stays intact, try again later.
                        // Clear any prior HTTP-error flag - the current reality is just offline.
                        LastSyncOnline = false;
                        LastSyncError = null;
                        return outcome;
                    }

                    // Push reached the server and returned per-sale verdicts: the batch was accepted.
                    LastSyncError = null;

                    foreach (var r in results)
                    {
                        // A per-sale IP-gate rejection comes back as Status=Rejected in a 200 body
                        // (the gate is checked per-sale, not per-batch), NOT as an HTTP error. Unlike
                        // a bad-data rejection it is NOT permanent - the device's Tailscale IP can be
                        // re-authorized - so it must NOT be moved to the terminal log where the sale
                        // (and its revenue) would be silently lost. Keep it queued for the next retry
                        // and surface it as a sync error. All other Rejected reasons stay terminal.
                        if (r.Status == "Rejected" && r.Detail != null &&
                            r.Detail.Contains("not authorized to submit sales", StringComparison.OrdinalIgnoreCase))
                        {
                            LastSyncOnline = false;
                            LastSyncError = "device not authorized";
                            outcome.HttpError = r.Detail;
                            continue; // leave it in pending_sales
                        }

                        PosLocalStore.ResolvePendingSale(r.ClientSaleId,
                            r.Status == "AlreadySynced" ? "Synced" : r.Status, r.Detail);

                        switch (r.Status)
                        {
                            case "Synced":
                            case "AlreadySynced":
                                outcome.Pushed++;
                                break;
                            case "SyncedWithShortfall":
                                outcome.Pushed++;
                                outcome.Shortfalls++;
                                outcome.Warnings.Add($"Sale {r.ClientSaleId}: {r.Detail}");
                                break;
                            case "Rejected":
                                outcome.Rejected++;
                                outcome.Warnings.Add($"Sale {r.ClientSaleId} REJECTED: {r.Detail}");
                                break;
                        }
                    }
                }

                // ---- pull ----
                try
                {
                    var stock = await CentralApiClient.GetBranchInventoryAsync(branch);
                    PosLocalStore.RefreshCatalog(stock);
                    outcome.CatalogRefreshed = true;
                }
                catch
                {
                    // Push may have succeeded while the pull failed; still counts as online
                    // if anything got through.
                    LastSyncOnline = outcome.Pushed > 0;
                    outcome.Online = LastSyncOnline;
                    if (outcome.Online) LastSyncAt = DateTime.Now;
                    return outcome;
                }

                outcome.Online = true;
                LastSyncOnline = true;
                // Don't clear a gate rejection raised in the push loop above (a sale is still
                // queued, unauthorized); only a fully clean cycle clears the error banner.
                if (outcome.HttpError == null) LastSyncError = null;
                LastSyncAt = DateTime.Now;
                return outcome;
            }
            finally
            {
                syncing = false;
            }
        }
    }
}
